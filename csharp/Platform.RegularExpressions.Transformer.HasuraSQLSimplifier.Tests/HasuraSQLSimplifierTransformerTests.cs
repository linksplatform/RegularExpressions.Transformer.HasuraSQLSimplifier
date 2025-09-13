using Xunit;

namespace Platform.RegularExpressions.Transformer.HasuraSQLSimplifier.Tests
{
    public class HasuraSQLSimplifierTransformerTests
    {
        [Fact]
        public void EmptyLineTest()
        {
            // This test can help to test basic problems with regular expressions like incorrect syntax
            var transformer = new HasuraSQLSimplifierTransformer();
            var actualResult = transformer.Transform("");
            Assert.Equal("", actualResult);
        }

        [Fact]
        public void FunctionCallsPreservedTest()
        {
            var transformer = new HasuraSQLSimplifierTransformer();

            // Test that function calls with quoted parameters are preserved
            Assert.Equal("bool_or('true')", transformer.Transform("bool_or('true')"));
            Assert.Equal("count('items')", transformer.Transform("count('items')"));
            Assert.Equal("sum('values')", transformer.Transform("sum('values')"));
            Assert.Equal("bool_and('false')", transformer.Transform("bool_and('false')"));
            Assert.Equal("max('column')", transformer.Transform("max('column')"));
            Assert.Equal("min('column')", transformer.Transform("min('column')"));
            
            // Test that standalone quoted strings in parentheses are still simplified
            Assert.Equal("'quoted_string'", transformer.Transform("('quoted_string')"));
            Assert.Equal("'describe'", transformer.Transform("('describe')"));
            
            // Test mixed cases - function calls preserved, standalone quoted strings simplified
            Assert.Equal("SELECT bool_or('active') FROM table WHERE 'condition'", 
                        transformer.Transform("SELECT bool_or('active') FROM table WHERE ('condition')"));
        }

        [Fact]
        public void BasicRequestTest()
        {
            var original = @"SELECT
  coalesce(json_agg(""root""), '[]') AS ""root""
FROM
  (
    SELECT
      row_to_json(
        (
          SELECT
            ""_2_e""
          FROM
            (
              SELECT
                ""_1_root.base"".""id"" AS ""id""
            ) AS ""_2_e""
        )
      ) AS ""root""
    FROM
      (
        SELECT
          *
        FROM
          ""public"".""nodes""
        WHERE
          (
            (
              (""public"".""nodes"".""type"") = (('auth_token') :: text)
            )
            AND (
              EXISTS (
                SELECT
                  1
                FROM
                  ""public"".""nodes"" AS ""_0__be_0_nodes""
                WHERE
                  (
                    (
                      (
                        (""_0__be_0_nodes"".""_source_id"") = (""public"".""nodes"".""_id"")
                      )
                      AND ('true')
                    )
                    AND (
                      (
                        (
                          ((""_0__be_0_nodes"".""type"") = (('describe') :: text))
                          AND ('true')
                        )
                        AND (
                          (
                            (
                              (""_0__be_0_nodes"".""target_id"") = (('X-Hasura-User-Id') :: text)
                            )
                            AND ('true')
                          )
                          AND ('true')
                        )
                      )
                      AND (
                        ('true')
                        AND ('true')
                      )
                    )
                  )
              )
            )
          )
      ) AS ""_1_root.base""
    LIMIT
      1
  ) AS ""_3_root""";

            var expected = @"SELECT
  coalesce(json_agg(""root""), '[]') AS ""root""
FROM
  (
    SELECT
      row_to_json(
        (
          SELECT
            ""_2_e""
          FROM
            (
              SELECT
                ""_1_root.base"".""id"" AS ""id""
            ) AS ""_2_e""
        )
      ) AS ""root""
    FROM
      (
        SELECT
          *
        FROM
          ""public"".""nodes""
        WHERE
          ""public"".""nodes"".""type"" = 'auth_token'::text
            AND EXISTS (
                SELECT
                  1
                FROM
                  ""public"".""nodes"" AS ""_0__be_0_nodes""
                WHERE
                  ""_0__be_0_nodes"".""_source_id"" = ""public"".""nodes"".""_id""
                    AND ""_0__be_0_nodes"".""type"" = 'describe'::text
                        AND ""_0__be_0_nodes"".""target_id"" = 'X-Hasura-User-Id'::text
              )
      ) AS ""_1_root.base""
    LIMIT 1
  ) AS ""_3_root""";
            var transformer = new HasuraSQLSimplifierTransformer();
            var actual = transformer.Transform(original);
            Assert.Equal(expected, actual);
        }
    }
}
