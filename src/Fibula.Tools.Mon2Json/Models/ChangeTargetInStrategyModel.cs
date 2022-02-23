// -----------------------------------------------------------------
// <copyright file="ChangeTargetInStrategyModel.cs" company="The Fibula Project">
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
    /// Class that represents how changing target works within the stategy.
    /// </summary>
    [JsonObject]
    internal class ChangeTargetInStrategyModel
    {
        /// <summary>
        /// Gets or sets the chance to change target within a strategy.
        /// </summary>
        [JsonProperty("chance", DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal Chance { get; set; }

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
