// -----------------------------------------------------------------
// <copyright file="MonsterTargetAreaAbilityModel.cs" company="The Fibula Project">
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
    internal class MonsterTargetAreaAbilityModel : MonsterAbilityModel
    {
        /// <summary>
        /// Gets the type of ability.
        /// </summary>
        public override string Type => "targetArea";

        /// <summary>
        /// Gets or sets the range of the ability.
        /// </summary>
        [JsonProperty("range", DefaultValueHandling = DefaultValueHandling.Include)]
        public byte Range { get; set; }

        /// <summary>
        /// Gets or sets the radius of a self area ability.
        /// </summary>
        [JsonProperty("radius", DefaultValueHandling = DefaultValueHandling.Include)]
        public byte Radius { get; set; }

        /// <summary>
        /// Gets or sets the effect displayed over the caster.
        /// </summary>
        [JsonProperty("casterEffect", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string CasterEffect { get; set; }

        /// <summary>
        /// Gets or sets the effect displayed over the area.
        /// </summary>
        [JsonProperty("areaEffect", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string AreaEffect { get; set; }

        /// <summary>
        /// Gets or sets the projectile effect for the ability.
        /// </summary>
        [JsonProperty("projectileEffect", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ProjectileEffect { get; set; }
    }
}
