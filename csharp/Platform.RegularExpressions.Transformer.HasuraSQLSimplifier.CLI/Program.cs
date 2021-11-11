using System;
using Platform.Collections.Arrays;

namespace Platform.RegularExpressions.Transformer.HasuraSQLSimplifier.CLI
{
    class Program
    {
        private const string DefaultSourceFileExtension = ".sql";
        private const string DefaultTargetFileExtension = ".simplified.sql";

        /// <summary>
        /// <para>
        /// Main the args.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="args">
        /// <para>The args.</para>
        /// <para></para>
        /// </param>
        static void Main(string[] args)
        {
            var sourceFileExtension = GetSourceFileExtension(args);
            var targetFileExtension = GetTargetFileExtension(args);
            var simplifier = new HasuraSQLSimplifierTransformer();
            var transformer = IsDebugModeRequested(args) ? new LoggingFileTransformer(simplifier, sourceFileExtension, targetFileExtension) : new FileTransformer(simplifier, sourceFileExtension, targetFileExtension);
            new TransformerCLI(transformer).Run(args);
        }

        /// <summary>
        /// <para>
        /// Gets the source file extension using the specified args.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="args">
        /// <para>The args.</para>
        /// <para></para>
        /// </param>
        /// <returns>
        /// <para>The string</para>
        /// <para></para>
        /// </returns>
        static string GetSourceFileExtension(string[] args) => args.TryGetElement(2, out string sourceFileExtension) ? sourceFileExtension : DefaultSourceFileExtension;

        /// <summary>
        /// <para>
        /// Gets the target file extension using the specified args.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="args">
        /// <para>The args.</para>
        /// <para></para>
        /// </param>
        /// <returns>
        /// <para>The string</para>
        /// <para></para>
        /// </returns>
        static string GetTargetFileExtension(string[] args) => args.TryGetElement(3, out string targetFileExtension) ? targetFileExtension : DefaultTargetFileExtension;

        /// <summary>
        /// <para>
        /// Determines whether is debug mode requested.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="args">
        /// <para>The args.</para>
        /// <para></para>
        /// </param>
        /// <returns>
        /// <para>The bool</para>
        /// <para></para>
        /// </returns>
        static private bool IsDebugModeRequested(string[] args) => args.TryGetElement(4, out string debugArgument) ? string.Equals(debugArgument, "debug", StringComparison.OrdinalIgnoreCase) : false;
    }
}
