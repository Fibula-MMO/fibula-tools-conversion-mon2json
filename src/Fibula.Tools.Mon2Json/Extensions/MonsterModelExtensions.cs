// -----------------------------------------------------------------
// <copyright file="MonsterModelExtensions.cs" company="The Fibula Project">
// Copyright (c) | The Fibula Project.
// https://github.com/orgs/fibula-mmo/people
//
// All Rights Reserved.
//
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// -----------------------------------------------------------------

namespace Fibula.Tools.Mon2Json.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fibula.Definitions.Enumerations;
    using Fibula.Parsing.CipFiles.Enumerations;
    using Fibula.Parsing.CipFiles.Extensions;
    using Fibula.Parsing.CipFiles.Models;
    using Fibula.Tools.Mon2Json.Models;
    using Fibula.Utilities.Validation;

    /// <summary>
    /// Static class that extends operations over the monster model and submodels.
    /// </summary>
    public static class MonsterModelExtensions
    {
        /// <summary>
        /// Converts Cip's model for a monster into the serializable model that can print to the json file.
        /// </summary>
        /// <param name="cipModel">The Cip monster model.</param>
        /// <returns>The serializable model.</returns>
        internal static MonsterModel ToSerializableModel(this CipMonster cipModel)
        {
            return new MonsterModel()
            {
                Article = cipModel.Article,
                Name = cipModel.Name,
                Blood = cipModel.BloodType.ToString().ToLower(),
                ExperienceYield = cipModel.Experience,
                Look = cipModel.Outfit.ToMonsterLookModel(),
                Corpse = cipModel.Corpse.ToString(),
                Stats = cipModel.Skills.ToMonsterStatsModel(),
                Flags = cipModel.Flags.AsCreatureFlagStrings(),
                Combat = cipModel.ToMonsterCombatModel(cipModel.Flags),
                Inventory = cipModel.Inventory.AsMonsterInventoryModelArray(),
                Phrases = cipModel.Phrases.ToArray(),
            };
        }

        private static string[] AsCreatureFlagStrings(this IList<CipCreatureFlag> cipFlags)
        {
            return cipFlags.Select(f => f.ToCreatureFlag())
                            .Where(f => f.HasValue)
                            .Select(f => f.Value.ToString())
                            .ToArray();
        }

        private static MonsterInventoryModel[] AsMonsterInventoryModelArray(this IList<(ushort TypeId, byte MaxAmount, ushort DropChance)> inventoryItems)
        {
            return inventoryItems.Select((tuple) => new MonsterInventoryModel()
            {
                Name = "placeholder",
                Id = tuple.TypeId.ToString(),
                MaximumCount = tuple.MaxAmount,
                Chance = Convert.ToDecimal(tuple.DropChance) / 1000,
            }).ToArray();
        }

        private static MonsterCombatModel ToMonsterCombatModel(this CipMonster cipModel, IList<CipCreatureFlag> flags)
        {
            return new MonsterCombatModel()
            {
                BaseAttackPower = cipModel.Attack,
                BaseDefensePower = cipModel.Defense,
                BaseArmor = cipModel.Armor,
                Distance = Convert.ToByte(flags.Contains(CipCreatureFlag.DistanceFighting) ? 4 : 1),
                Immunities = cipModel.ToMonsterImmunitiesModel(),
                Skills = cipModel.Skills.ToMonsterSkillsModel(),
                Abilities = cipModel.Spells.ToMonsterAbilitiesModel(),
                Strategy = new MonsterStrategyModel()
                {
                    Closest = cipModel.Strategy.Closest,
                    Strongest = cipModel.Strategy.Strongest,
                    Weakest = cipModel.Strategy.Weakest,
                    Random = cipModel.Strategy.Random,
                    ChangeTarget = new ChangeTargetInStrategyModel()
                    {
                        Chance = Convert.ToDecimal(cipModel.LoseTarget) / 100,
                    },
                    Flee = new FleeInStrategyModel()
                    {
                        HitpointThreshold = cipModel.FleeThreshold,
                    },
                },
            };
        }

        private static MonsterAbilityModel[] ToMonsterAbilitiesModel(this IList<(CipMonsterSpellCastCondition Condition, CipMonsterSpellEffect Effect, byte Chance)> spellRules)
        {
            var abilities = new List<MonsterAbilityModel>();

            foreach (var spellRule in spellRules)
            {
                var parsedAbility = spellRule.Condition.Type switch
                {
                    CipMonsterSpellCastType.Actor => spellRule.ToSelfMonsterAbilityModel(),
                    CipMonsterSpellCastType.Origin => spellRule.ToSelfAreaMonsterAbilityModel(),
                    CipMonsterSpellCastType.Victim => spellRule.ToTargetMonsterAbilityModel(),
                    CipMonsterSpellCastType.Destination => spellRule.ToTargetAreaMonsterAbilityModel(),
                    CipMonsterSpellCastType.Angle => spellRule.ToTargetDirectionMonsterAbilityModel(),
                    _ => throw new NotSupportedException($"A spellrule with a cast type of {spellRule.Condition.Type} is not supported."),
                };

                abilities.Add(parsedAbility);
            }

            return abilities.ToArray();
        }

        // Actor(int castEffect) -> {action} : chance
        private static MonsterAbilityModel ToSelfMonsterAbilityModel(this (CipMonsterSpellCastCondition Condition, CipMonsterSpellEffect Effect, byte Chance) spellRule)
        {
            const int ExpectedValuesCount = 1;

            spellRule.ThrowIfNull(nameof(spellRule));

            if (spellRule.Condition.Type != CipMonsterSpellCastType.Actor)
            {
                throw new ArgumentException($"Trying to cast a spell rule of the wrong type ({spellRule.Condition.Type}) to type:{nameof(MonsterSelfAbilityModel)}.");
            }

            if (!spellRule.Condition.Values.Any() || spellRule.Condition.Values.Count() != ExpectedValuesCount)
            {
                throw new ArgumentException(
                    $"Unexpected number of values in spell rule condition. Expected {ExpectedValuesCount} but got {spellRule.Condition.Values.Count()}",
                    nameof(spellRule.Condition.Values));
            }

            var values = spellRule.Condition.Values.ToArray();

            return new MonsterSelfAbilityModel()
            {
                Chance = Math.Floor(1 / Convert.ToDecimal(spellRule.Chance) * 1000) / 1000,
                CasterEffect = values[0].AsAnimatedEffect().ToString(),
                Actions = spellRule.Effect.AsMonsterAbilityActionsModelArray(),
            };
        }

        // Origin(int radius, int areaEffect) -> {action} : chance
        private static MonsterAbilityModel ToSelfAreaMonsterAbilityModel(this (CipMonsterSpellCastCondition Condition, CipMonsterSpellEffect Effect, byte Chance) spellRule)
        {
            const int ExpectedValuesCount = 2;

            spellRule.ThrowIfNull(nameof(spellRule));

            if (spellRule.Condition.Type != CipMonsterSpellCastType.Origin)
            {
                throw new ArgumentException($"Trying to cast a spell rule of the wrong type ({spellRule.Condition.Type}) to type:{nameof(MonsterSelfAreaAbilityModel)}.");
            }

            if (!spellRule.Condition.Values.Any() || spellRule.Condition.Values.Count() != ExpectedValuesCount)
            {
                throw new ArgumentException(
                    $"Unexpected number of values in spell rule condition. Expected {ExpectedValuesCount} but got {spellRule.Condition.Values.Count()}",
                    nameof(spellRule.Condition.Values));
            }

            var values = spellRule.Condition.Values.ToArray();

            return new MonsterSelfAreaAbilityModel()
            {
                Chance = Math.Floor(1 / Convert.ToDecimal(spellRule.Chance) * 1000) / 1000,
                Radius = Convert.ToByte(values[0]),
                AreaEffect = values[1].AsAnimatedEffect().ToString(),
                Actions = spellRule.Effect.AsMonsterAbilityActionsModelArray(),
            };
        }

        // Victim(int range, int projectileEffect, int targetEffect) -> {action} : chance
        private static MonsterAbilityModel ToTargetMonsterAbilityModel(this (CipMonsterSpellCastCondition Condition, CipMonsterSpellEffect Effect, byte Chance) spellRule)
        {
            const int ExpectedValuesCount = 3;

            spellRule.ThrowIfNull(nameof(spellRule));

            if (spellRule.Condition.Type != CipMonsterSpellCastType.Victim)
            {
                throw new ArgumentException($"Trying to cast a spell rule of the wrong type ({spellRule.Condition.Type}) to type:{nameof(MonsterTargetAbilityModel)}.");
            }

            if (!spellRule.Condition.Values.Any() || spellRule.Condition.Values.Count() != ExpectedValuesCount)
            {
                throw new ArgumentException(
                    $"Unexpected number of values in spell rule condition. Expected {ExpectedValuesCount} but got {spellRule.Condition.Values.Count()}",
                    nameof(spellRule.Condition.Values));
            }

            var values = spellRule.Condition.Values.ToArray();

            return new MonsterTargetAbilityModel()
            {
                Chance = Math.Floor(1 / Convert.ToDecimal(spellRule.Chance) * 1000) / 1000,
                Range = Convert.ToByte(values[0]),
                ProjectileEffect = values[1].AsProjectileType().ToString(),
                TargetEffect = values[2].AsAnimatedEffect().ToString(),
                Actions = spellRule.Effect.AsMonsterAbilityActionsModelArray(),
            };
        }

        // Destination(int range, int projectileEffect, int radius, int areaEffect) -> {action} : chance
        private static MonsterAbilityModel ToTargetAreaMonsterAbilityModel(this (CipMonsterSpellCastCondition Condition, CipMonsterSpellEffect Effect, byte Chance) spellRule)
        {
            const int ExpectedValuesCount = 4;

            spellRule.ThrowIfNull(nameof(spellRule));

            if (spellRule.Condition.Type != CipMonsterSpellCastType.Destination)
            {
                throw new ArgumentException($"Trying to cast a spell rule of the wrong type ({spellRule.Condition.Type}) to type:{nameof(MonsterTargetAreaAbilityModel)}.");
            }

            if (!spellRule.Condition.Values.Any() || spellRule.Condition.Values.Count() != ExpectedValuesCount)
            {
                throw new ArgumentException(
                    $"Unexpected number of values in spell rule condition. Expected {ExpectedValuesCount} but got {spellRule.Condition.Values.Count()}",
                    nameof(spellRule.Condition.Values));
            }

            var values = spellRule.Condition.Values.ToArray();

            return new MonsterTargetAreaAbilityModel()
            {
                Chance = Math.Floor(1 / Convert.ToDecimal(spellRule.Chance) * 1000) / 1000,
                Range = Convert.ToByte(values[0]),
                ProjectileEffect = values[1].AsProjectileType().ToString(),
                Radius = Convert.ToByte(values[2]),
                AreaEffect = values[3].AsAnimatedEffect().ToString(),
                Actions = spellRule.Effect.AsMonsterAbilityActionsModelArray(),
            };
        }

        // Angle(int spread, int length, int areaEffect) -> {action} : chance
        private static MonsterAbilityModel ToTargetDirectionMonsterAbilityModel(this (CipMonsterSpellCastCondition Condition, CipMonsterSpellEffect Effect, byte Chance) spellRule)
        {
            const int ExpectedValuesCount = 3;

            spellRule.ThrowIfNull(nameof(spellRule));

            if (spellRule.Condition.Type != CipMonsterSpellCastType.Angle)
            {
                throw new ArgumentException($"Trying to cast a spell rule of the wrong type ({spellRule.Condition.Type}) to type:{nameof(MonsterTargetDirectionAbilityModel)}.");
            }

            if (!spellRule.Condition.Values.Any() || spellRule.Condition.Values.Count() != ExpectedValuesCount)
            {
                throw new ArgumentException(
                    $"Unexpected number of values in spell rule condition. Expected {ExpectedValuesCount} but got {spellRule.Condition.Values.Count()}",
                    nameof(spellRule.Condition.Values));
            }

            var values = spellRule.Condition.Values.ToArray();

            return new MonsterTargetDirectionAbilityModel()
            {
                Chance = Math.Floor(1 / Convert.ToDecimal(spellRule.Chance) * 1000) / 1000,
                Spread = Convert.ToByte(values[0]),
                Length = Convert.ToByte(values[1]),
                AreaEffect = values[2].AsAnimatedEffect().ToString(),
                Actions = spellRule.Effect.AsMonsterAbilityActionsModelArray(),
            };
        }

        private static string AsDamageKind(this long fromUint)
        {
            return fromUint switch
            {
                1 => "Physical",
                2 => "Poison",
                4 => "Fire",
                8 => "Energy",
                16 => "Unknown",
                32 => "PoisonCondition",
                64 => "FireCondition",
                128 => "EnergyCondition",
                256 => "LifeDrain",
                512 => "ManaDrain",
                _ => throw new NotSupportedException($"Unsupported damage type code {fromUint}."),
            };
        }

        private static string AsFieldType(this long fromUint)
        {
            return fromUint switch
            {
                1 => "Fire",
                2 => "Poison",
                3 => "Energy",
                _ => throw new NotSupportedException($"Unsupported field type code {fromUint}."),
            };
        }

        private static AnimatedEffect AsAnimatedEffect(this long fromUint)
        {
            return fromUint switch
            {
                0 => AnimatedEffect.None,
                1 => AnimatedEffect.XBlood,
                2 => AnimatedEffect.RingsBlue,
                3 => AnimatedEffect.Puff,
                4 => AnimatedEffect.SparkYellow,
                5 => AnimatedEffect.DamageExplosion,
                6 => AnimatedEffect.DamageMagicMissile,
                7 => AnimatedEffect.AreaFlame,
                8 => AnimatedEffect.RingsYellow,
                9 => AnimatedEffect.RingsGreen,
                10 => AnimatedEffect.XGray,
                11 => AnimatedEffect.BubbleBlue,
                12 => AnimatedEffect.DamageEnergy,
                13 => AnimatedEffect.GlitterBlue,
                14 => AnimatedEffect.GlitterRed,
                15 => AnimatedEffect.GlitterGreen,
                16 => AnimatedEffect.Flame,
                17 => AnimatedEffect.Poison,
                18 => AnimatedEffect.BubbleBlack,
                19 => AnimatedEffect.SoundGreen,
                20 => AnimatedEffect.SoundRed,
                21 => AnimatedEffect.DamageVenomMissile,
                22 => AnimatedEffect.SoundYellow,
                23 => AnimatedEffect.SoundPurple,
                24 => AnimatedEffect.SoundBlue,
                25 => AnimatedEffect.SoundWhite,
                _ => AnimatedEffect.None,
            };
        }

        private static ProjectileType AsProjectileType(this long fromUint)
        {
            return fromUint switch
            {
                0 => ProjectileType.None,
                1 => ProjectileType.Spear,
                2 => ProjectileType.Bolt,
                3 => ProjectileType.Arrow,
                4 => ProjectileType.OrangeOrb,
                5 => ProjectileType.BlueOrb,
                6 => ProjectileType.PoisonArrow,
                7 => ProjectileType.BurstArrow,
                8 => ProjectileType.ThrowingStar,
                9 => ProjectileType.ThrowingKnife,
                10 => ProjectileType.SmallStone,
                11 => ProjectileType.BlackOrb,
                12 => ProjectileType.LargeRock,
                13 => ProjectileType.Snowball,
                14 => ProjectileType.PowerBolt,
                15 => ProjectileType.GreenOrb,
                _ => ProjectileType.None,
            };
        }

        private static MonsterAbilityActionModel[] AsMonsterAbilityActionsModelArray(this CipMonsterSpellEffect spellEffect)
        {
            var parsedAction = spellEffect.Type switch
            {
                CipMonsterSpellEffectType.Healing => spellEffect.ToHealMonsterAbilityActionModel(),
                CipMonsterSpellEffectType.Damage => spellEffect.ToDamageMonsterAbilityActionModel(),
                CipMonsterSpellEffectType.Summon => spellEffect.ToSummonMonsterAbilityActionModel(),
                CipMonsterSpellEffectType.Field => spellEffect.ToMagicFieldMonsterAbilityActionModel(),
                CipMonsterSpellEffectType.Speed => spellEffect.ToSpeedChangeMonsterAbilityActionModel(),
                CipMonsterSpellEffectType.Drunken => spellEffect.ToMakeDrunkMonsterAbilityActionModel(),
                CipMonsterSpellEffectType.Strength => spellEffect.ToChangeSkillsMonsterAbilityActionModel(),
                CipMonsterSpellEffectType.Outfit => spellEffect.ToChangeOutfitMonsterAbilityActionModel(),
                _ => throw new NotSupportedException($"A spell effect with an action type of {spellEffect.Type} is not supported."),
            };

            return new MonsterAbilityActionModel[] { parsedAction };
        }

        // Healing (int base, int variation)
        private static MonsterAbilityActionModel ToHealMonsterAbilityActionModel(this CipMonsterSpellEffect spellEffect)
        {
            const string ActionType = "heal";
            const int ExpectedValuesCount = 2;

            spellEffect.ThrowIfNull(nameof(spellEffect));

            if (spellEffect.Type != CipMonsterSpellEffectType.Healing)
            {
                throw new ArgumentException($"Trying to cast a spell rule of the wrong type ({spellEffect.Type}) to type:{ActionType}.");
            }

            if (!spellEffect.Values.Any() || spellEffect.Values.Count() != ExpectedValuesCount)
            {
                throw new ArgumentException(
                    $"Unexpected number of values in spell rule condition. Expected {ExpectedValuesCount} but got {spellEffect.Values.Count()}",
                    nameof(spellEffect.Values));
            }

            var values = spellEffect.Values.ToArray();

            return new MonsterAbilityActionModel()
            {
                Type = ActionType,
                Base = Convert.ToInt32(values[0]),
                Variation = Convert.ToInt32(values[1]),
            };
        }

        // Damage (int damageKind, int base, int variation)
        private static MonsterAbilityActionModel ToDamageMonsterAbilityActionModel(this CipMonsterSpellEffect spellEffect)
        {
            const string ActionType = "damage";
            const int ExpectedValuesCount = 3;

            spellEffect.ThrowIfNull(nameof(spellEffect));

            if (spellEffect.Type != CipMonsterSpellEffectType.Damage)
            {
                throw new ArgumentException($"Trying to cast a spell rule of the wrong type ({spellEffect.Type}) to type:{ActionType}.");
            }

            if (!spellEffect.Values.Any() || spellEffect.Values.Count() != ExpectedValuesCount)
            {
                throw new ArgumentException(
                    $"Unexpected number of values in spell rule condition. Expected {ExpectedValuesCount} but got {spellEffect.Values.Count()}",
                    nameof(spellEffect.Values));
            }

            var values = spellEffect.Values.ToArray();

            return new MonsterAbilityActionModel()
            {
                Type = ActionType,
                Base = Convert.ToInt32(values[1]),
                Kind = values[0].AsDamageKind(),
                Variation = Convert.ToInt32(values[2]),
            };
        }

        // Summon (int raceId, int maxCount)
        private static MonsterAbilityActionModel ToSummonMonsterAbilityActionModel(this CipMonsterSpellEffect spellEffect)
        {
            const string ActionType = "summon";
            const int ExpectedValuesCount = 2;

            spellEffect.ThrowIfNull(nameof(spellEffect));

            if (spellEffect.Type != CipMonsterSpellEffectType.Summon)
            {
                throw new ArgumentException($"Trying to cast a spell rule of the wrong type ({spellEffect.Type}) to type:{ActionType}.");
            }

            if (!spellEffect.Values.Any() || spellEffect.Values.Count() != ExpectedValuesCount)
            {
                throw new ArgumentException(
                    $"Unexpected number of values in spell rule condition. Expected {ExpectedValuesCount} but got {spellEffect.Values.Count()}",
                    nameof(spellEffect.Values));
            }

            var values = spellEffect.Values.ToArray();

            return new MonsterAbilityActionModel()
            {
                Type = ActionType,
                MonsterFile = $"filename for race {values[0]}",
                MaximumCount = Convert.ToByte(values[1]),
            };
        }

        // Field (int magicFieldKind)
        private static MonsterAbilityActionModel ToMagicFieldMonsterAbilityActionModel(this CipMonsterSpellEffect spellEffect)
        {
            const string ActionType = "magicField";
            const int ExpectedValuesCount = 1;

            spellEffect.ThrowIfNull(nameof(spellEffect));

            if (spellEffect.Type != CipMonsterSpellEffectType.Field)
            {
                throw new ArgumentException($"Trying to cast a spell rule of the wrong type ({spellEffect.Type}) to type:{ActionType}.");
            }

            if (!spellEffect.Values.Any() || spellEffect.Values.Count() != ExpectedValuesCount)
            {
                throw new ArgumentException(
                    $"Unexpected number of values in spell rule condition. Expected {ExpectedValuesCount} but got {spellEffect.Values.Count()}",
                    nameof(spellEffect.Values));
            }

            var values = spellEffect.Values.ToArray();

            return new MonsterAbilityActionModel()
            {
                Type = ActionType,
                Kind = values[0].AsFieldType(),
            };
        }

        // Speed (int base, int variation, int duration)
        private static MonsterAbilityActionModel ToSpeedChangeMonsterAbilityActionModel(this CipMonsterSpellEffect spellEffect)
        {
            const string ActionType = "changeSpeed";
            const int ExpectedValuesCount = 3;

            spellEffect.ThrowIfNull(nameof(spellEffect));

            if (spellEffect.Type != CipMonsterSpellEffectType.Speed)
            {
                throw new ArgumentException($"Trying to cast a spell rule of the wrong type ({spellEffect.Type}) to type:{ActionType}.");
            }

            if (!spellEffect.Values.Any() || spellEffect.Values.Count() != ExpectedValuesCount)
            {
                throw new ArgumentException(
                    $"Unexpected number of values in spell rule condition. Expected {ExpectedValuesCount} but got {spellEffect.Values.Count()}",
                    nameof(spellEffect.Values));
            }

            var values = spellEffect.Values.ToArray();

            return new MonsterAbilityActionModel()
            {
                Type = ActionType,
                Base = Convert.ToInt32(values[0]),
                Variation = Convert.ToInt32(values[1]),
                Duration = Convert.ToUInt32(values[2] * 1000),
            };
        }

        // Drunken (int strength, int effect, int duration)
        private static MonsterAbilityActionModel ToMakeDrunkMonsterAbilityActionModel(this CipMonsterSpellEffect spellEffect)
        {
            const string ActionType = "makeDrunk";
            const int ExpectedValuesCount = 3;

            spellEffect.ThrowIfNull(nameof(spellEffect));

            if (spellEffect.Type != CipMonsterSpellEffectType.Drunken)
            {
                throw new ArgumentException($"Trying to cast a spell rule of the wrong type ({spellEffect.Type}) to type:{ActionType}.");
            }

            if (!spellEffect.Values.Any() || spellEffect.Values.Count() != ExpectedValuesCount)
            {
                throw new ArgumentException(
                    $"Unexpected number of values in spell rule condition. Expected {ExpectedValuesCount} but got {spellEffect.Values.Count()}",
                    nameof(spellEffect.Values));
            }

            var values = spellEffect.Values.ToArray();

            return new MonsterAbilityActionModel()
            {
                Type = ActionType,
                Base = Convert.ToInt32(values[0]),
                Effect = values[1].AsAnimatedEffect().ToString(),
                Duration = Convert.ToUInt32(values[2] * 1000),
            };
        }

        // Strength (int skillId, int basePercent, int variationPercent, int durationInSeconds)
        private static MonsterAbilityActionModel ToChangeSkillsMonsterAbilityActionModel(this CipMonsterSpellEffect spellEffect)
        {
            const string ActionType = "changeSkill";
            const int ExpectedValuesCount = 4;

            spellEffect.ThrowIfNull(nameof(spellEffect));

            if (spellEffect.Type != CipMonsterSpellEffectType.Strength)
            {
                throw new ArgumentException($"Trying to cast a spell rule of the wrong type ({spellEffect.Type}) to type:{ActionType}.");
            }

            if (!spellEffect.Values.Any() || spellEffect.Values.Count() != ExpectedValuesCount)
            {
                throw new ArgumentException(
                    $"Unexpected number of values in spell rule condition. Expected {ExpectedValuesCount} but got {spellEffect.Values.Count()}",
                    nameof(spellEffect.Values));
            }

            var values = spellEffect.Values.ToArray();

            return new MonsterAbilityActionModel()
            {
                Type = ActionType,
                Kind = values[0].ToString(),
                Base = Convert.ToInt32(values[1]),
                Variation = Convert.ToInt32(values[2]),
                Duration = Convert.ToUInt32(values[3] * 1000),
            };
        }

        // Outfit ((0, 0), int duration) for invisible
        // Outfit ((0, int itemdId), int duration) for item
        // Outfit ((int id, 0-0-0-0), int duration) for monster race
        // Outfit ((int id, head-body-legs-feet), int duration) for a full outfit
        private static MonsterAbilityActionModel ToChangeOutfitMonsterAbilityActionModel(this CipMonsterSpellEffect spellEffect)
        {
            const string ActionType = "changeOutfit";
            const int ExpectedValuesCount = 1;

            spellEffect.ThrowIfNull(nameof(spellEffect));

            if (spellEffect.Type != CipMonsterSpellEffectType.Outfit)
            {
                throw new ArgumentException($"Trying to cast a spell rule of the wrong type ({spellEffect.Type}) to type:{ActionType}.");
            }

            if (!spellEffect.Values.Any() || spellEffect.Values.Count() != ExpectedValuesCount)
            {
                throw new ArgumentException(
                    $"Unexpected number of values in spell rule condition. Expected {ExpectedValuesCount} but got {spellEffect.Values.Count()}",
                    nameof(spellEffect.Values));
            }

            var values = spellEffect.Values.ToArray();

            return new MonsterAbilityActionModel()
            {
                Type = ActionType,
                TargetLook = new MonsterLookModel()
                {
                    Type = spellEffect.ChangeToOutfit.Type.ToString().ToLower(),
                    Id = spellEffect.ChangeToOutfit.Id,
                    Head = spellEffect.ChangeToOutfit.Head,
                    Body = spellEffect.ChangeToOutfit.Body,
                    Legs = spellEffect.ChangeToOutfit.Legs,
                    Feet = spellEffect.ChangeToOutfit.Feet,
                },
                Duration = Convert.ToUInt32(values[0] * 1000),
            };
        }

        private static MonsterSkillModel[] ToMonsterSkillsModel(this IList<(string Name, uint DefaultLevel, uint CurrentLevel, uint MaximumLevel, uint TargetCount, uint CountIncreaseFactor, byte IncreaserPerLevel)> skills)
        {
            var skillsMap = new Dictionary<string, string>()
            {
                { "fistfighting", "fist" },
                { "axefighting", "axe" },
                { "clubfighting", "club" },
                { "swordfighting", "sword" },
                { "distancefighting", "ranged" },
                { "shielding", "shield" },
            };

            return skills.Where(s => skillsMap.ContainsKey(s.Name.ToLower()))
                         .Select(s => new MonsterSkillModel()
                         {
                             Type = skillsMap[s.Name.ToLower()],
                             Level = s.CurrentLevel,
                             TargetCount = s.TargetCount,
                             Factor = Convert.ToDecimal(s.CountIncreaseFactor) / 1000,
                             Increase = s.IncreaserPerLevel,
                         })
                         .ToArray();
        }

        private static MonsterImmunityModel[] ToMonsterImmunitiesModel(this CipMonster cipModel)
        {
            var immunities = new List<MonsterImmunityModel>();

            if (cipModel.Flags.Contains(CipCreatureFlag.NoPoison))
            {
                immunities.Add(new MonsterImmunityModel() { Type = "poison", Modifier = 1m });
            }

            if (cipModel.Flags.Contains(CipCreatureFlag.NoBurning))
            {
                immunities.Add(new MonsterImmunityModel() { Type = "fire", Modifier = 1m });
            }

            if (cipModel.Flags.Contains(CipCreatureFlag.NoEnergy))
            {
                immunities.Add(new MonsterImmunityModel() { Type = "energy", Modifier = 1m });
            }

            if (cipModel.Flags.Contains(CipCreatureFlag.NoLifeDrain))
            {
                immunities.Add(new MonsterImmunityModel() { Type = "lifeDrain", Modifier = 1m });
            }

            if (cipModel.Flags.Contains(CipCreatureFlag.NoParalyze))
            {
                immunities.Add(new MonsterImmunityModel() { Type = "paralysis", Modifier = 1m });
            }

            if (cipModel.Flags.Contains(CipCreatureFlag.NoHit))
            {
                immunities.Add(new MonsterImmunityModel() { Type = "physical", Modifier = 1m });
            }

            return immunities.ToArray();
        }

        private static MonsterLookModel ToMonsterLookModel(this CipOutfit cipOutiftModel)
        {
            return cipOutiftModel.Type switch
            {
                CipOutfitType.Race => new MonsterLookModel()
                {
                    Type = "race",
                    Id = Convert.ToUInt16(cipOutiftModel.Id),
                },
                CipOutfitType.Outfit => new MonsterLookModel()
                {
                    Type = "outfit",
                    Id = Convert.ToUInt16(cipOutiftModel.Id),
                    Head = Convert.ToByte(cipOutiftModel.Head),
                    Body = Convert.ToByte(cipOutiftModel.Body),
                    Legs = Convert.ToByte(cipOutiftModel.Legs),
                    Feet = Convert.ToByte(cipOutiftModel.Feet),
                },
                CipOutfitType.Invisible => new MonsterLookModel()
                {
                    Type = "invisible",
                    Id = 0,
                },
                CipOutfitType.Item => new MonsterLookModel()
                {
                    Type = "item",
                    Id = Convert.ToUInt16(cipOutiftModel.Id),
                },
                _ => throw new NotSupportedException($"The look type of {cipOutiftModel.Type} is not supported."),
            };
        }

        private static MonsterStatsModel ToMonsterStatsModel(this IList<(string Name, uint DefaultLevel, uint CurrentLevel, uint MaximumLevel, uint TargetCount, uint CountIncreaseFactor, byte IncreaserPerLevel)> skills)
        {
            var hitpoints = skills.Single(s => s.Name.Equals("HitPoints", StringComparison.OrdinalIgnoreCase));
            var goStrength = skills.Single(s => s.Name.Equals("GoStrength", StringComparison.OrdinalIgnoreCase));
            var carryStrength = skills.Single(s => s.Name.Equals("CarryStrength", StringComparison.OrdinalIgnoreCase));

            return new MonsterStatsModel()
            {
                Hitpoints = hitpoints.DefaultLevel,
                BaseSpeed = goStrength.DefaultLevel,
                CarryStrength = carryStrength.DefaultLevel,
            };
        }
    }
}
