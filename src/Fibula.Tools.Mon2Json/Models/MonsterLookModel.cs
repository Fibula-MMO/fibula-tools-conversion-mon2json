// -----------------------------------------------------------------
// <copyright file="MonsterLookModel.cs" company="The Fibula Project">
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
    /// Class that represents the way this monster looks.
    /// </summary>
    [JsonObject]
    internal class MonsterLookModel
    {
        /// <summary>
        /// Gets or sets the type of look.
        /// </summary>
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the item or race, depending on the look type.
        /// </summary>
        [JsonProperty("id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ushort? Id { get; set; }

        /// <summary>
        /// Gets or sets the look for the head.
        /// </summary>
        [JsonProperty("head", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public byte? Head { get; set; }

        /// <summary>
        /// Gets or sets the look for the body.
        /// </summary>
        [JsonProperty("body", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public byte? Body { get; set; }

        /// <summary>
        /// Gets or sets the look for the legs.
        /// </summary>
        [JsonProperty("legs", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public byte? Legs { get; set; }

        /// <summary>
        /// Gets or sets the look for the feet.
        /// </summary>
        [JsonProperty("feet", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public byte? Feet { get; set; }
    }
}
