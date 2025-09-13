using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.RegularExpressions.Transformer.HasuraSQLSimplifier
{
    /// <summary>
    /// <para>
    /// Represents the hasura sql simplifier transformer.
    /// </para>
    /// <para></para>
    /// </summary>
    /// <seealso cref="TextTransformer"/>
    public class HasuraSQLSimplifierTransformer : TextTransformer
    {
        /// <summary>
        /// <para>
        /// The to list.
        /// </para>
        /// <para></para>
        /// </summary>
        public static readonly IList<ISubstitutionRule> DefaultRules = new List<SubstitutionRule>
        {
            // HTML clean up
            (new Regex(@"<span class=""[^""]*"">([^<>]*)<\/span>"), "$1", 0),
            // ('describe')
            // 'describe'
            (new Regex(@"\([\s\n]*('[^']+')[\s\n]*\)"), "$1", int.MaxValue),
            // AND ('true' AND 'true')
            //
            (new Regex(@"[\s\n]*AND[\s\n]*\([\s\n]*'true'[\s\n]*AND[\s\n]*'true'[\s\n]*\)"), "", 0),
            // AND ('true')
            // 
            (new Regex(@"[\s\n]*AND[\s\n]*'true'"), "", 0),
            //  :: 
            // ::
            (new Regex(@"[\s]*::[\s]*"), "::", 0),
            // ('describe'::text)
            // 'describe'::text
            (new Regex(@"\([\s\n]*('[^']+'::text)[\s\n]*\)"), "$1", 0),
            // ("_0__be_0_nodes"."target_id")
            // "_0__be_0_nodes"."target_id"
            (new Regex(@"\([\s\n]*(""[^""]+"")[\s\n]*\.[\s\n]*(""[^""]+"")[\s\n]*\)"), "$1.$2", 0),
            // ("public"."nodes"."_id")
            // "public"."nodes"."_id"
            (new Regex(@"\([\s\n]*(""[^""]+"")[\s\n]*\.[\s\n]*(""[^""]+"")[\s\n]*\.[\s\n]*(""[^""]+"")[\s\n]*\)"), "$1.$2.$3", 0),
            // LIMIT\n\t\t\t1
            // LIMIT 1
            (new Regex(@"(LIMIT)[\s\n]*(\d+)"), "$1 $2", 0),
            // ("_0__be_0_nodes"."type" = 'describe'::text)
            // "_0__be_0_nodes"."type" = 'describe'::text
            (new Regex(@"(\W)\([\s\n]*((?!SELECT)[^\s\n()][^()]*?)[\s\n]*\)"), "$1$2", int.MaxValue),
            // (EXISTS (...))
            // EXISTS (...)
            (new Regex(@"(\W)\([\s\n]*((?!SELECT)[^\s\n()][^()]*\([^()]*\)[^()]*?)[\s\n]*\)"), "$1$2", int.MaxValue),
            // ((EXISTS (...)))
            // (EXISTS (...))
            (new Regex(@"(\W)\([\s\n]*((?!SELECT)[^\s\n()][^()]*\([^()]*\([^()]*\)[^()]*\)[^()]*?)[\s\n]*\)"), "$1$2", int.MaxValue),
        }.Cast<ISubstitutionRule>().ToList();

        /// <summary>
        /// <para>
        /// Initializes a new <see cref="HasuraSQLSimplifierTransformer"/> instance.
        /// </para>
        /// <para></para>
        /// </summary>
        public HasuraSQLSimplifierTransformer()
            : base(DefaultRules)
        {
        }
    }

    /// <summary>
    /// <para>
    /// Represents the hasura sql minifier transformer.
    /// </para>
    /// <para></para>
    /// </summary>
    /// <seealso cref="TextTransformer"/>
    public class HasuraSQLMinifierTransformer : TextTransformer
    {
        /// <summary>
        /// <para>
        /// The minifier rules to compress SQL by removing unnecessary whitespace and formatting.
        /// </para>
        /// <para></para>
        /// </summary>
        public static readonly IList<ISubstitutionRule> MinifierRules = new List<SubstitutionRule>
        {
            // Remove comments starting with --
            (new Regex(@"--.*?(?:\r?\n|$)"), "", 0),
            // Remove C-style comments /* ... */
            (new Regex(@"/\*.*?\*/", RegexOptions.Singleline), "", 0),
            // Replace multiple whitespace characters (including newlines) with single space
            (new Regex(@"\s+"), " ", 0),
            // Remove spaces around parentheses, commas and semicolons but keep comma spacing
            (new Regex(@"\s*([(),;])\s*"), "$1", 0),
            (new Regex(@","), ", ", 0),
            // Remove spaces around dots and double colons
            (new Regex(@"\s*\.\s*"), ".", 0),
            (new Regex(@"\s*::\s*"), "::", 0),
            // Clean up multiple spaces
            (new Regex(@" {2,}"), " ", 0),
            // Trim leading and trailing whitespace
            (new Regex(@"^\s+|\s+$"), "", 0)
        }.Cast<ISubstitutionRule>().ToList();

        /// <summary>
        /// <para>
        /// Initializes a new <see cref="HasuraSQLMinifierTransformer"/> instance.
        /// </para>
        /// <para></para>
        /// </summary>
        public HasuraSQLMinifierTransformer()
            : base(MinifierRules)
        {
        }
    }
}
