// -----------------------------------------------------------------
// <copyright file="Program.cs" company="The Fibula Project">
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
    using System;
    using System.CommandLine;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Fibula.Plugins.ItemLoaders.CipObjectsFile;
    using Fibula.Utilities.CLI;
    using Fibula.Utilities.CLI.Streams;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Serilog;

    /// <summary>
    /// Class that represents a standalone Fibula server instance.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The cancellation token source for the entire application.
        /// </summary>
        private static readonly CancellationTokenSource MasterCancellationTokenSource = new();

        /// <summary>
        /// The main entry point for the program.
        /// </summary>
        /// <param name="args">The arguments for this program.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("logsettings.json")
                .Build();

            var host = new HostBuilder()
                .UseConsoleLifetime()
                .ConfigureHostConfiguration(hostConfig =>
                {
                    hostConfig.SetBasePath(Directory.GetCurrentDirectory());
                    hostConfig.AddJsonFile("hostsettings.json", optional: true, reloadOnChange: true);
                })
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    configApp.SetBasePath(Directory.GetCurrentDirectory());
                    configApp.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                    configApp.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
                })
                .ConfigureServices((hostingContext, serviceCollection) =>
                {
                    serviceCollection.AddInputStreamListener(Console.OpenStandardInput());

                    // Chose a type of item types (catalog) loader:
                    serviceCollection.AddObjectsFileItemTypeLoader(hostingContext.Configuration);

                    serviceCollection.AddInteractiveMonsterFilesConverter();
                })
                .UseSerilog((context, services, loggerConfig) =>
                {
                    loggerConfig.ReadFrom.Configuration(configuration).Enrich.FromLogContext();
                })
                .Build();

            TaskScheduler.UnobservedTaskException += HandleUnobservedTaskException;

            var inputListener = host.Services.GetService(typeof(InputStreamReaderListener)) as IInputListener;
            var converter = host.Services.GetService(typeof(MonsterFilesConverter)) as MonsterFilesConverter;

            var monDirArg = new Argument<string>("from", description: "The directory that contains the .mon files.");
            var outDirArg = new Argument<string>("to", description: "The directory in which to put the converted files.");
            var overwriteOp = new Option<bool>("overwrite", description: "A value indicating whether to overwrite files.", getDefaultValue: () => false);
            var convertCommand = new Command("convert", "Converts CipSoft's .mon monster files to Fibula's .json monster files.");

            convertCommand.AddArgument(monDirArg);
            convertCommand.AddArgument(outDirArg);
            convertCommand.AddOption(overwriteOp);

            convertCommand.SetHandler(
                (string monDir, string outDir, bool overwrite) =>
                {
                    converter.Convert(monDir, outDir, overwrite);

                    Console.WriteLine("Command completed.");
                },
                monDirArg,
                outDirArg,
                overwriteOp);

            var rootCommand = new RootCommand("A tool to convert or verify monster files.");

            rootCommand.AddAlias("mon2json");
            rootCommand.AddCommand(convertCommand);

            inputListener.NewLineRead += (line) =>
            {
                rootCommand.Invoke(line);
            };

            await host.RunAsync(Program.MasterCancellationTokenSource.Token).ConfigureAwait(false);
        }

        /// <summary>
        /// Handles an unobserved task exception.
        /// </summary>
        /// <param name="sender">The sender of the exception.</param>
        /// <param name="e">The exception arguments.</param>
        private static void HandleUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            // TODO: log this somewhere...
        }
    }
}
