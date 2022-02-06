// -----------------------------------------------------------------
// <copyright file="MonsterSkillModel.cs" company="The Fibula Project">
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
    /// Class that represents a skill in a monster.
    /// </summary>
    [JsonObject]
    internal class MonsterSkillModel
    {
        /// <summary>
        /// Gets or sets the type of skill.
        /// </summary>
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the skill level.
        /// </summary>
        [JsonProperty("level", DefaultValueHandling = DefaultValueHandling.Include)]
        public uint Level { get; set; }

        /// <summary>
        /// Gets or sets the target count for advancing this skill.
        /// </summary>
        [JsonProperty("targetCount", DefaultValueHandling = DefaultValueHandling.Include)]
        public uint TargetCount { get; set; }

        /// <summary>
        /// Gets or sets the factor by which the next target count is calculated.
        /// </summary>
        [JsonProperty("factor", DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal Factor { get; set; }

        /// <summary>
        /// Gets or sets the amount of levels by which the skill increases when reaching the target count.
        /// </summary>
        [JsonProperty("increase", DefaultValueHandling = DefaultValueHandling.Include)]
        public uint Increase { get; set; }
    }
}
