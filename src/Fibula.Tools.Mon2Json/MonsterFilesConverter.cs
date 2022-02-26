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
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Fibula.Data.Contracts.Abstractions;
    using Fibula.Definitions.Data.Entities;
    using Fibula.Parsing.CipFiles;
    using Fibula.Tools.Mon2Json.Extensions;
    using Fibula.Tools.Mon2Json.Models;
    using Fibula.Utilities.Validation;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;

    /// <summary>
    /// Class that implements the converter from .mon to .json monster files.
    /// </summary>
    public class MonsterFilesConverter
    {
        private const string MonExtension = ".mon";
        private const string JsonExtension = ".json";

        private readonly MonsterFilesConverterOptions options;
        private readonly IItemTypesLoader itemTypesLoader;

        /// <summary>
        /// Initializes a new instance of the <see cref="MonsterFilesConverter"/> class.
        /// </summary>
        /// <param name="options">The options for this converter.</param>
        /// <param name="itemTypesLoader">A reference to an item types loader to load monster inventory names.</param>
        public MonsterFilesConverter(IOptions<MonsterFilesConverterOptions> options, IItemTypesLoader itemTypesLoader)
        {
            options.ThrowIfNull(nameof(options));
            itemTypesLoader.ThrowIfNull(nameof(itemTypesLoader));

            this.options = options.Value;
            this.itemTypesLoader = itemTypesLoader;
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

            var itemDictionary = this.itemTypesLoader.LoadTypes();

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

            var existingFiles = monDirectoryInfo.GetFiles($"*{MonExtension}");
            var modelMap = new Dictionary<string, MonsterModel>();
            var raceMap = new Dictionary<uint, string>();

            // Now that both directories exist, start processing files serially.
            foreach (var monFileInfo in existingFiles)
            {
                var parsedMonsterModel = CipFileParser.ParseMonsterFile(monFileInfo);

                if (parsedMonsterModel == null)
                {
                    continue;
                }

                var targetModel = parsedMonsterModel.ToSerializableModel();
                var fileNameWithoutExt = monFileInfo.Name.Replace(MonExtension, string.Empty);

                modelMap.Add(fileNameWithoutExt, targetModel);
                raceMap.Add(parsedMonsterModel.RaceId, fileNameWithoutExt);
            }

            // Ammend item names and summon references
            foreach (var convertedModel in modelMap.Values)
            {
                this.AmmendInventoryItemNames(itemDictionary, convertedModel);
                this.AmmendSummonReferences(raceMap, convertedModel);
            }

            // Output converted models
            foreach (var (fileNameWithoutExt, convertedModel) in modelMap)
            {
                var convertedFilePath = Path.Combine(jsonDirectoryInfo.FullName, Path.GetTempFileName());
                var targetFilePath = $"{Path.Combine(jsonDirectoryInfo.FullName, fileNameWithoutExt)}{JsonExtension}";
                var tempFileSream = File.Create(convertedFilePath);

                using (var fw = new StreamWriter(tempFileSream))
                {
                    var serializedMonster = JsonConvert.SerializeObject(convertedModel, Formatting.Indented);

                    fw.Write(serializedMonster);
                    fw.Flush();
                    fw.Close();
                }

                File.Move(convertedFilePath, targetFilePath, overwriteFiles ?? false);
            }
        }

        private void AmmendSummonReferences(Dictionary<uint, string> raceMap, MonsterModel convertedModel)
        {
            // HACK: hardcoded ability action type here.
            foreach (var abilityActionModel in convertedModel.Combat.Abilities.SelectMany(a => a.Actions.Where(action => action.Type == "summon")))
            {
                if (!uint.TryParse(abilityActionModel.MonsterFile, out var referencedMonsterRaceId) || !raceMap.ContainsKey(referencedMonsterRaceId))
                {
                    // TODO: logging?
                    continue;
                }

                abilityActionModel.MonsterFile = raceMap[referencedMonsterRaceId];
            }
        }

        private void AmmendInventoryItemNames(IDictionary<string, ItemTypeEntity> itemDictionary, MonsterModel targetModel)
        {
            foreach (var inventoryModel in targetModel.Inventory)
            {
                if (!itemDictionary.ContainsKey(inventoryModel.Id))
                {
                    // TODO: logging?
                    continue;
                }

                var match = Regex.Match(itemDictionary[inventoryModel.Id].Name, "^(?'article'an?)?\\s?(?'itemName'.*)$");

                match.Groups.TryGetValue("article", out _);
                match.Groups.TryGetValue("itemName", out Group nameGrp);

                inventoryModel.Name = nameGrp.Value;
            }
        }
    }
}
