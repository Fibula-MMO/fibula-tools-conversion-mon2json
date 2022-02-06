// -----------------------------------------------------------------
// <copyright file="MonsterTargetDirectionAbilityModel.cs" company="The Fibula Project">
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
    internal class MonsterTargetDirectionAbilityModel : MonsterAbilityModel
    {
        /// <summary>
        /// Gets the type of ability.
        /// </summary>
        public override string Type => "targetDirection";

        /// <summary>
        /// Gets or sets the length of the area on a target direction ability.
        /// </summary>
        [JsonProperty("length", DefaultValueHandling = DefaultValueHandling.Include)]
        public byte Length { get; set; }

        /// <summary>
        /// Gets or sets the spread of the area on a target direction ability.
        /// </summary>
        [JsonProperty("spread", DefaultValueHandling = DefaultValueHandling.Include)]
        public byte Spread { get; set; }

        /// <summary>
        /// Gets or sets the effect displayed over the area.
        /// </summary>
        [JsonProperty("areaEffect", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string AreaEffect { get; set; }
    }
}
