// -----------------------------------------------------------------
// <copyright file="MonsterModel.cs" company="The Fibula Project">
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
    /// Class to represent and serialize the monster.
    /// </summary>
    [JsonObject]
    internal sealed class MonsterModel
    {
        /// <summary>
        /// Gets or sets an article to prefix the name of the monster with.
        /// </summary>
        [JsonProperty("article", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Article { get; set; }

        /// <summary>
        /// Gets or sets the name of the monster.
        /// </summary>
        [JsonProperty("name", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of blood that this monster has.
        /// </summary>
        [JsonProperty("blood", DefaultValueHandling = DefaultValueHandling.Include)]
        public string Blood { get; set; }

        /// <summary>
        /// Gets or sets the experience that this monster yields when dying.
        /// </summary>
        [JsonProperty("experienceYield", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public uint? ExperienceYield { get; set; }

        /// <summary>
        /// Gets or sets the definition for the way this monster looks.
        /// </summary>
        [JsonProperty("look", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public MonsterLookModel Look { get; set; }

        /// <summary>
        /// Gets or sets the corpse that this monster leaves when dying.
        /// </summary>
        [JsonProperty("corpse", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Corpse { get; set; }

        /// <summary>
        /// Gets or sets the stats for this monster.
        /// </summary>
        [JsonProperty("stats", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public MonsterStatsModel Stats { get; set; }

        /// <summary>
        /// Gets or sets the flags that the monster has.
        /// </summary>
        [JsonProperty("flags", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string[] Flags { get; set; }

        /// <summary>
        /// Gets or sets the definition for how this monster does combat.
        /// </summary>
        [JsonProperty("combat", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public MonsterCombatModel Combat { get; set; }

        /// <summary>
        /// Gets or sets the way that the inventory for this monster is composed.
        /// </summary>
        [JsonProperty("inventory", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public MonsterInventoryModel[] Inventory { get; set; }

        /// <summary>
        /// Gets or sets the definition for how this monster speaks.
        /// </summary>
        [JsonProperty("phrases", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string[] Phrases { get; set; }
    }
}
