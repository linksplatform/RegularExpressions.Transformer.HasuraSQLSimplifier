using System;
using Platform.RegularExpressions.Transformer.HasuraSQLSimplifier;

class TestHTMLRemoval
{
    static void Main(string[] args)
    {
        var transformer = new HasuraSQLSimplifierTransformer();
        
        // Current span tag support
        string spanTest = @"SELECT <span class=""highlight"">column_name</span> FROM table";
        Console.WriteLine("Span test:");
        Console.WriteLine("Input: " + spanTest);
        Console.WriteLine("Output: " + transformer.Transform(spanTest));
        Console.WriteLine();
        
        // Code tag support (should be added)
        string codeTest = @"SELECT <code>column_name</code> FROM table";
        Console.WriteLine("Code test:");
        Console.WriteLine("Input: " + codeTest);
        Console.WriteLine("Output: " + transformer.Transform(codeTest));
        Console.WriteLine();
        
        // Test with inner whitespace
        string whitespaceTest1 = @"SELECT <span class=""highlight"">  column_name  </span> FROM table";
        Console.WriteLine("Whitespace test 1 (span with spaces):");
        Console.WriteLine("Input: " + whitespaceTest1);
        Console.WriteLine("Output: " + transformer.Transform(whitespaceTest1));
        Console.WriteLine();
        
        string whitespaceTest2 = @"SELECT <code>  column_name  </code> FROM table";
        Console.WriteLine("Whitespace test 2 (code with spaces):");
        Console.WriteLine("Input: " + whitespaceTest2);
        Console.WriteLine("Output: " + transformer.Transform(whitespaceTest2));
        Console.WriteLine();
        
        // Test with newlines and tabs
        string whitespaceTest3 = @"SELECT <span class=""highlight"">
            column_name
        </span> FROM table";
        Console.WriteLine("Whitespace test 3 (span with newlines):");
        Console.WriteLine("Input: " + whitespaceTest3);
        Console.WriteLine("Output: " + transformer.Transform(whitespaceTest3));
        Console.WriteLine();
    }
}