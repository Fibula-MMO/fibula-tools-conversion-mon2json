// -----------------------------------------------------------------
// <copyright file="MonsterAbilityActionModel.cs" company="The Fibula Project">
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
    /// Class that represents an action for a monster ability.
    /// </summary>
    [JsonObject]
    internal class MonsterAbilityActionModel
    {
        /// <summary>
        /// Gets or sets the type of an action.
        /// </summary>
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the base value for a damage action.
        /// </summary>
        [JsonProperty("base", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Base { get; set; }

        /// <summary>
        /// Gets or sets the variation value for a damage action.
        /// </summary>
        [JsonProperty("variation", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Variation { get; set; }

        /// <summary>
        /// Gets or sets the duration on a speed change action.
        /// </summary>
        [JsonProperty("duration", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public uint Duration { get; set; }

        /// <summary>
        /// Gets or sets the kind of field in a magic field action.
        /// </summary>
        [JsonProperty("kind", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Kind { get; set; }

        /// <summary>
        /// Gets or sets the referenced monster file in a summon action.
        /// </summary>
        [JsonProperty("monsterFile", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string MonsterFile { get; set; }

        /// <summary>
        /// Gets or sets the maximum summon count in a summon action.
        /// </summary>
        [JsonProperty("maximumCount", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public byte MaximumCount { get; set; }

        /// <summary>
        /// Gets or sets the monster look in a look change action.
        /// </summary>
        [JsonProperty("targetLook", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public MonsterLookModel TargetLook { get; set; }

        /// <summary>
        /// Gets or sets the effect of the action.
        /// </summary>
        [JsonProperty("effect", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Effect { get; set; }
    }
}
