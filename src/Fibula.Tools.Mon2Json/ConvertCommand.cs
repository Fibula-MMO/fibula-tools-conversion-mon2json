// -----------------------------------------------------------------
// <copyright file="ConvertCommand.cs" company="The Fibula Project">
// Copyright (c) | The Fibula Project.
// https://github.com/orgs/fibula-mmo/people
//
// All Rights Reserved.
//
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// -----------------------------------------------------------------

namespace Fibula.Tools.Mon2Json.Standalone
{
    using System.CommandLine;

    /// <summary>
    /// Class that represents the convert command.
    /// </summary>
    public class ConvertCommand : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertCommand"/> class.
        /// </summary>
        public ConvertCommand()
            : base(name: "convert", description: "Converts CipSoft .mon monster files to Fibula's .json monster files.")
        {
        }
    }
}
