// -----------------------------------------------------------------
// <copyright file="MonsterCombatModel.cs" company="The Fibula Project">
// Copyright (c) | The Fibula Project.
// https://github.com/orgs/fibula-mmo/people
//
// All Rights Reserved.
//
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// -----------------------------------------------------------------

namespace Fibula.Tools.Mon2Json.Models
{
    using Newtonsoft.Json;

    /// <summary>
    /// Class that represents the definition for how this monster does combat.
    /// </summary>
    [JsonObject]
    internal class MonsterCombatModel
    {
        /// <summary>
        /// Gets or sets the base attack power of this monster.
        /// </summary>
        [JsonProperty("baseAttackPower", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ushort BaseAttackPower { get; set; }

        /// <summary>
        /// Gets or sets the base defense power of this monster.
        /// </summary>
        [JsonProperty("baseDefensePower", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ushort BaseDefensePower { get; set; }

        /// <summary>
        /// Gets or sets the base armor of this monster.
        /// </summary>
        [JsonProperty("baseArmor", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ushort BaseArmor { get; set; }

        /// <summary>
        /// Gets or sets the distance that this monster fights at.
        /// </summary>
        [JsonProperty("distance", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public byte Distance { get; set; }

        /// <summary>
        /// Gets or sets the immunities of the monster.
        /// </summary>
        [JsonProperty("immunities", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public MonsterImmunityModel[] Immunities { get; set; }

        /// <summary>
        /// Gets or sets the skills of the monster.
        /// </summary>
        [JsonProperty("skills", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public MonsterSkillModel[] Skills { get; set; }

        /// <summary>
        /// Gets or sets the combat strategy for the monster.
        /// </summary>
        [JsonProperty("strategy", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public MonsterStrategyModel Strategy { get; set; }

        /// <summary>
        /// Gets or sets the abilities of the monster.
        /// </summary>
        [JsonProperty("abilities", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public MonsterAbilityModel[] Abilities { get; set; }
    }
}
