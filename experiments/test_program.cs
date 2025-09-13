using System;
using Platform.RegularExpressions.Transformer.HasuraSQLSimplifier;

class Program
{
    static void Main()
    {
        var transformer = new HasuraSQLSimplifierTransformer();
        
        Console.WriteLine("=== HTML Tag Removal Tests ===");
        
        // Test 1: Basic span tag
        string test1 = @"SELECT <span class=""highlight"">column_name</span> FROM table";
        Console.WriteLine($"Input:  {test1}");
        Console.WriteLine($"Output: {transformer.Transform(test1)}");
        Console.WriteLine();
        
        // Test 2: Basic code tag (new functionality)
        string test2 = @"SELECT <code>column_name</code> FROM table";
        Console.WriteLine($"Input:  {test2}");
        Console.WriteLine($"Output: {transformer.Transform(test2)}");
        Console.WriteLine();
        
        // Test 3: Span with whitespace trimming
        string test3 = @"SELECT <span class=""highlight"">  column_name  </span> FROM table";
        Console.WriteLine($"Input:  {test3}");
        Console.WriteLine($"Output: {transformer.Transform(test3)}");
        Console.WriteLine();
        
        // Test 4: Code with whitespace trimming (new functionality)
        string test4 = @"SELECT <code>  column_name  </code> FROM table";
        Console.WriteLine($"Input:  {test4}");
        Console.WriteLine($"Output: {transformer.Transform(test4)}");
        Console.WriteLine();
        
        // Test 5: Mixed tags
        string test5 = @"SELECT <span class=""table"">table_name</span>.<code>  column_id  </code> FROM database";
        Console.WriteLine($"Input:  {test5}");
        Console.WriteLine($"Output: {transformer.Transform(test5)}");
        Console.WriteLine();
    }
}