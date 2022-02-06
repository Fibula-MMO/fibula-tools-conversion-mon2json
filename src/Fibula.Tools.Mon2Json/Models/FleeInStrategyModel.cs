// -----------------------------------------------------------------
// <copyright file="FleeInStrategyModel.cs" company="The Fibula Project">
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
    /// Class that represents how fleeing works within a strategy.
    /// </summary>
    [JsonObject]
    internal class FleeInStrategyModel
    {
        /// <summary>
        /// Gets or sets a threshold for the monster hitpoints under which the monster flees.
        /// </summary>
        [JsonProperty("hitpointThreshold", DefaultValueHandling = DefaultValueHandling.Include)]
        public uint HitpointThreshold { get; set; }
    }
}
