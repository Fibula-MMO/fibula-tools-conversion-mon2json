// -----------------------------------------------------------------
// <copyright file="MonsterAbilityModel.cs" company="The Fibula Project">
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
    /// Class that represents an ability of the monster.
    /// </summary>
    [JsonObject]
    internal abstract class MonsterAbilityModel
    {
        /// <summary>
        /// Gets the type of ability.
        /// </summary>
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Include)]
        public abstract string Type { get; }

        /// <summary>
        /// Gets or sets the chance of this ability being cast at each evaluation.
        /// </summary>
        [JsonProperty("chance", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public decimal Chance { get; set; }

        /// <summary>
        /// Gets or sets the actions of the ability.
        /// </summary>
        [JsonProperty("actions", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public MonsterAbilityActionModel[] Actions { get; set; }
    }
}
