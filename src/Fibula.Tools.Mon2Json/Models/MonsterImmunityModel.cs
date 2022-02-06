// -----------------------------------------------------------------
// <copyright file="MonsterImmunityModel.cs" company="The Fibula Project">
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
    /// Class that represents a monster's immunity.
    /// </summary>
    [JsonObject]
    internal class MonsterImmunityModel
    {
        /// <summary>
        /// Gets or sets the type of immunity.
        /// </summary>
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the modifier for the immunity in the monster.
        /// </summary>
        [JsonProperty("modifier", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public decimal Modifier { get; set; }
    }
}
