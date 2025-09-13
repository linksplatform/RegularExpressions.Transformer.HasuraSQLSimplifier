using Platform.RegularExpressions.Transformer.HasuraSQLSimplifier;
using System;

namespace Experiments
{
    class TestFunctionIssue
    {
        static void Main(string[] args)
        {
            var transformer = new HasuraSQLSimplifierTransformer();
            
            // Test case from the issue
            var testInput = "bool_or('true')";
            var result = transformer.Transform(testInput);
            
            Console.WriteLine($"Input:  {testInput}");
            Console.WriteLine($"Output: {result}");
            Console.WriteLine($"Expected: {testInput}"); // Function calls should remain unchanged
            Console.WriteLine($"Issue: {result != testInput}");
            
            // Additional test cases
            string[] testCases = {
                "bool_or('true')",
                "count('items')",
                "sum('values')",
                "bool_and('false')",
                "('quoted_string')",  // This should be simplified to 'quoted_string'
                "max('column')",
                "min('column')"
            };
            
            Console.WriteLine("\n=== All Test Cases ===");
            foreach (var testCase in testCases)
            {
                var output = transformer.Transform(testCase);
                Console.WriteLine($"{testCase} -> {output}");
            }
        }
    }
}