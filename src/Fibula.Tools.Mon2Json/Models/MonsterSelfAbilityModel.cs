// -----------------------------------------------------------------
// <copyright file="MonsterSelfAbilityModel.cs" company="The Fibula Project">
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
    internal class MonsterSelfAbilityModel : MonsterAbilityModel
    {
        /// <summary>
        /// Gets the type of ability.
        /// </summary>
        public override string Type => "self";

        /// <summary>
        /// Gets or sets the effect displayed over the caster.
        /// </summary>
        [JsonProperty("casterEffect", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string CasterEffect { get; set; }
    }
}
