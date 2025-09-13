using System;
using Platform.RegularExpressions.Transformer.HasuraSQLSimplifier;

var minifier = new HasuraSQLMinifierTransformer();
var simplifier = new HasuraSQLSimplifierTransformer();

var testSql = @"SELECT
  col1,   col2
FROM
  table1
WHERE
  col1 = 'value'
  AND col2 > 10";

Console.WriteLine("Original SQL:");
Console.WriteLine(testSql);
Console.WriteLine("\nSimplified:");
Console.WriteLine(simplifier.Transform(testSql));
Console.WriteLine("\nMinified:");
Console.WriteLine(minifier.Transform(testSql));