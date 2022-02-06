// -----------------------------------------------------------------
// <copyright file="MonsterFilesConverterOptions.cs" company="The Fibula Project">
// Copyright (c) | The Fibula Project.
// https://github.com/orgs/fibula-mmo/people
//
// All Rights Reserved.
//
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// -----------------------------------------------------------------

namespace Fibula.Tools.Mon2Json
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Class that represents options for the <see cref="MonsterFilesConverter"/>.
    /// </summary>
    public class MonsterFilesConverterOptions
    {
        /// <summary>
        /// Gets or sets the directory for the monster (*.mon) files.
        /// </summary>
        [Required(ErrorMessage = "A directory for the monster (*.mon) files must be specified.")]
        public string MonsterFilesDirectory { get; set; }

        /// <summary>
        /// Gets or sets the directory for the monster (*.mon) files.
        /// </summary>
        [Required(ErrorMessage = "An output directory for the resulting (*.json) files must be specified.")]
        public string OutputDirectory { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to overwrite output files with the same name.
        /// </summary>
        public bool OverwriteFiles { get; set; }
    }
}
