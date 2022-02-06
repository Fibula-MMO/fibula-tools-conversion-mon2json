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
    }
}
