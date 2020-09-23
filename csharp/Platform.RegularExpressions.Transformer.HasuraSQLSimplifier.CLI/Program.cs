using Platform.Collections.Arrays;
using System;

namespace Platform.RegularExpressions.Transformer.HasuraSQLSimplifier.CLI
{
    class Program
    {
        private const string DefaultSourceFileExtension = ".sql";
        private const string DefaultTargetFileExtension = ".simplified.sql";

        static void Main(string[] args)
        {
            var sourceFileExtension = GetSourceFileExtension(args);
            var targetFileExtension = GetTargetFileExtension(args);
            var simplifier = new HasuraSQLSimplifierTransformer();
            var transformer = IsDebugModeRequested(args) ? new LoggingFileTransformer(simplifier, sourceFileExtension, targetFileExtension) : new FileTransformer(simplifier, sourceFileExtension, targetFileExtension);
            new TransformerCLI(transformer).Run(args);
        }

        static string GetSourceFileExtension(string[] args) => args.TryGetElement(2, out string sourceFileExtension) ? sourceFileExtension : DefaultSourceFileExtension;

        static string GetTargetFileExtension(string[] args) => args.TryGetElement(3, out string targetFileExtension) ? targetFileExtension : DefaultTargetFileExtension;

        static private bool IsDebugModeRequested(string[] args) => args.TryGetElement(4, out string debugArgument) ? string.Equals(debugArgument, "debug", StringComparison.OrdinalIgnoreCase) : false;
    }
}
