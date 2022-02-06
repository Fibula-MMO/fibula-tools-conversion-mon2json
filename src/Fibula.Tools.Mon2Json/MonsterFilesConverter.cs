// -----------------------------------------------------------------
// <copyright file="MonsterFilesConverter.cs" company="The Fibula Project">
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
    using System;
    using System.IO;
    using Fibula.Parsing.CipFiles;
    using Fibula.Tools.Mon2Json.Extensions;
    using Fibula.Utilities.Validation;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;

    /// <summary>
    /// Class that implements the converter from .mon to .json monster files.
    /// </summary>
    public class MonsterFilesConverter
    {
        private MonsterFilesConverterOptions options;

        /// <summary>
        /// Initializes a new instance of the <see cref="MonsterFilesConverter"/> class.
        /// </summary>
        /// <param name="options">The options for this converter.</param>
        public MonsterFilesConverter(IOptions<MonsterFilesConverterOptions> options)
        {
            this.options = options.Value;
        }

        /// <summary>
        /// Performs the conversion of .mon files to .json files.
        /// </summary>
        /// <param name="monDirPath">Optional. The path to the directory containing the .mon files. Defaults to the initialization <see cref="options"/> value if not supplied.</param>
        /// <param name="jsonDirPath">Optional. The path to the directory that will contain the .json files. Defaults to the initialization <see cref="options"/> value if not supplied.</param>
        /// <param name="overwriteFiles">Optional. A value indicating whether to overwrite files at the destination folder. Defaults to the initialization <see cref="options"/> value if not supplied.</param>
        public void Convert(string monDirPath = null, string jsonDirPath = null, bool? overwriteFiles = null)
        {
            this.options.MonsterFilesDirectory = monDirPath ?? this.options.MonsterFilesDirectory;
            this.options.OutputDirectory = jsonDirPath ?? this.options.OutputDirectory;
            this.options.OverwriteFiles = overwriteFiles ?? this.options.OverwriteFiles;

            DataAnnotationsValidator.ValidateObjectRecursive(this.options);

            var monDirectoryInfo = new DirectoryInfo(monDirPath);
            var jsonDirectoryInfo = new DirectoryInfo(jsonDirPath);

            if (!monDirectoryInfo.Exists)
            {
                throw new ArgumentException($"The specified directory for .mon files: [{monDirectoryInfo.FullName}] does not exist.");
            }

            if (!jsonDirectoryInfo.Exists)
            {
                jsonDirectoryInfo.Create();

                if (!jsonDirectoryInfo.Exists)
                {
                    throw new ArgumentException($"The specified output directory: [{jsonDirectoryInfo.FullName}] does not exist and could not be created.");
                }
            }

            // Now that both directories exist, start processing files serially.
            foreach (var monFileInfo in monDirectoryInfo.GetFiles($"*.mon"))
            {
                var parsedMonsterModel = CipFileParser.ParseMonsterFile(monFileInfo);

                if (parsedMonsterModel == null)
                {
                    continue;
                }

                var targetModel = parsedMonsterModel.ToSerializableModel();
                var convertedFilePath = Path.Combine(jsonDirectoryInfo.FullName, Path.GetTempFileName());
                var tempFileSream = File.Create(convertedFilePath);

                using (var fw = new StreamWriter(tempFileSream))
                {
                    var serializedMonster = JsonConvert.SerializeObject(targetModel, Formatting.Indented);

                    fw.Write(serializedMonster);
                    fw.Flush();
                    fw.Close();
                }

                File.Move(convertedFilePath, Path.Combine(jsonDirectoryInfo.FullName, monFileInfo.Name).Replace(".mon", ".json"), overwriteFiles ?? false);
            }
        }
    }
}
