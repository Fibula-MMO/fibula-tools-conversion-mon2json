// -----------------------------------------------------------------
// <copyright file="MonsterTargetAbilityModel.cs" company="The Fibula Project">
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
    internal class MonsterTargetAbilityModel : MonsterAbilityModel
    {
        /// <summary>
        /// Gets the type of ability.
        /// </summary>
        public override string Type => "target";

        /// <summary>
        /// Gets or sets the range of the ability.
        /// </summary>
        [JsonProperty("range", DefaultValueHandling = DefaultValueHandling.Include)]
        public byte Range { get; set; }

        /// <summary>
        /// Gets or sets the effect displayed over the target.
        /// </summary>
        [JsonProperty("targetEffect", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string TargetEffect { get; set; }

        /// <summary>
        /// Gets or sets the projectile effect for the ability.
        /// </summary>
        [JsonProperty("projectileEffect", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ProjectileEffect { get; set; }
    }
}
