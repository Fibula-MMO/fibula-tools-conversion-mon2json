// -----------------------------------------------------------------
// <copyright file="MonsterStrategyModel.cs" company="The Fibula Project">
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
    /// Class that represents a monster strategy.
    /// </summary>
    [JsonObject]
    internal class MonsterStrategyModel
    {
        /// <summary>
        /// Gets or sets the way target changes within a strategy.
        /// </summary>
        [JsonProperty("changeTarget", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ChangeTargetInStrategyModel ChangeTarget { get; set; }

        /// <summary>
        /// Gets or sets the way fleeing works within a strategy.
        /// </summary>
        [JsonProperty("flee", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public FleeInStrategyModel Flee { get; set; }
    }
}
