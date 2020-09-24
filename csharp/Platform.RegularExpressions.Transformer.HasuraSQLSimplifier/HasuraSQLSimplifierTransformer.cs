using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.RegularExpressions.Transformer.HasuraSQLSimplifier
{
    public class HasuraSQLSimplifierTransformer : TextTransformer
    {
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

        public HasuraSQLSimplifierTransformer()
            : base(DefaultRules)
        {
        }
    }
}
