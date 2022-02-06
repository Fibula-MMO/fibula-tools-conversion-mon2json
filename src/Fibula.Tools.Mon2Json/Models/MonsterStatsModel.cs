// -----------------------------------------------------------------
// <copyright file="MonsterStatsModel.cs" company="The Fibula Project">
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
    /// Class that represents and serializes the creature stats for this monster.
    /// </summary>
    [JsonObject]
    internal class MonsterStatsModel
    {
        /// <summary>
        /// Gets or sets the number of hitpoints that this monster has.
        /// </summary>
        [JsonProperty("hitpoints", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public uint Hitpoints { get; set; }

        /// <summary>
        /// Gets or sets the base speed that this monster has.
        /// </summary>
        [JsonProperty("baseSpeed", DefaultValueHandling = DefaultValueHandling.Include)]
        public uint BaseSpeed { get; set; }

        /// <summary>
        /// Gets or sets the strength to carry stuff that this monster has.
        /// </summary>
        [JsonProperty("carryStrength", DefaultValueHandling = DefaultValueHandling.Include)]
        public uint CarryStrength { get; set; }
    }
}
