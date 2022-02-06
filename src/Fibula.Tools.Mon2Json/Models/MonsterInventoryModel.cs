// -----------------------------------------------------------------
// <copyright file="MonsterInventoryModel.cs" company="The Fibula Project">
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
    /// Class that represents the way that the inventory for this monster is composed.
    /// </summary>
    [JsonObject]
    internal class MonsterInventoryModel
    {
        /// <summary>
        /// Gets or sets the id of the item to add as inventory.
        /// </summary>
        [JsonProperty("id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the item to add as inventory.
        /// </summary>
        [JsonProperty("name", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of the item to add to the inventory.
        /// </summary>
        [JsonProperty("maximumCount", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ushort MaximumCount { get; set; }

        /// <summary>
        /// Gets or sets the chance to add the item to the inventory.
        /// </summary>
        [JsonProperty("chance", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public decimal Chance { get; set; }
    }
}
