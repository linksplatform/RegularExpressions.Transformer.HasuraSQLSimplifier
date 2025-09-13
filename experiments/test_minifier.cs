using System;
using Platform.RegularExpressions.Transformer.HasuraSQLSimplifier;

class TestMinifier 
{
    static void Main()
    {
        var transformer = new HasuraSQLMinifierTransformer();
        
        var test1 = @"SELECT*FROM table WHERE col1IS NOT NULL AND col2BETWEEN 1AND 10";
        var result1 = transformer.Transform(test1);
        Console.WriteLine($"Input: {test1}");
        Console.WriteLine($"Output: {result1}");
        Console.WriteLine($"Expected: SELECT * FROM table WHERE col1 IS NOT NULL AND col2 BETWEEN 1 AND 10");
        Console.WriteLine();
        
        var test2 = @"SELECT   col1,   col2
FROM     table1
WHERE    col1  =  'value'
AND      col2  >  10";
        var result2 = transformer.Transform(test2);
        Console.WriteLine($"Input: {test2}");
        Console.WriteLine($"Output: {result2}");
        Console.WriteLine($"Expected: SELECT col1, col2 FROM table1 WHERE col1 = 'value' AND col2 > 10");
    }
}