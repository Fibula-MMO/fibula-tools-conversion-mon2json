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

        /// <summary>
        /// Gets or sets the chance to choose the nearest target when selecting one.
        /// </summary>
        [JsonProperty("closest", DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal Closest { get; set; }

        /// <summary>
        /// Gets or sets the chance to choose the weakest target when selecting one.
        /// </summary>
        [JsonProperty("weakest", DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal Weakest { get; set; }

        /// <summary>
        /// Gets or sets the chance to choose the target who has dealt the most damage when selecting one.
        /// </summary>
        [JsonProperty("strongest", DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal Strongest { get; set; }

        /// <summary>
        /// Gets or sets the chance to choose the a random target when selecting one.
        /// </summary>
        [JsonProperty("random", DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal Random { get; set; }
    }
}
