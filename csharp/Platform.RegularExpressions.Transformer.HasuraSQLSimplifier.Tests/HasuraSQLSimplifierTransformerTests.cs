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
        public void SpanTagRemovalTest()
        {
            var transformer = new HasuraSQLSimplifierTransformer();
            
            // Test basic span tag removal
            var input1 = @"SELECT <span class=""highlight"">column_name</span> FROM table";
            var expected1 = @"SELECT column_name FROM table";
            var actual1 = transformer.Transform(input1);
            Assert.Equal(expected1, actual1);
            
            // Test span tag with extra whitespace removal
            var input2 = @"SELECT <span class=""highlight"">  column_name  </span> FROM table";
            var expected2 = @"SELECT column_name FROM table";
            var actual2 = transformer.Transform(input2);
            Assert.Equal(expected2, actual2);
            
            // Test span tag with newlines and tabs
            var input3 = @"SELECT <span class=""highlight"">
                column_name
            </span> FROM table";
            var expected3 = @"SELECT column_name FROM table";
            var actual3 = transformer.Transform(input3);
            Assert.Equal(expected3, actual3);
        }

        [Fact]
        public void CodeTagRemovalTest()
        {
            var transformer = new HasuraSQLSimplifierTransformer();
            
            // Test basic code tag removal
            var input1 = @"SELECT <code>column_name</code> FROM table";
            var expected1 = @"SELECT column_name FROM table";
            var actual1 = transformer.Transform(input1);
            Assert.Equal(expected1, actual1);
            
            // Test code tag with extra whitespace removal
            var input2 = @"SELECT <code>  column_name  </code> FROM table";
            var expected2 = @"SELECT column_name FROM table";
            var actual2 = transformer.Transform(input2);
            Assert.Equal(expected2, actual2);
            
            // Test code tag with newlines and tabs
            var input3 = @"SELECT <code>
                column_name
            </code> FROM table";
            var expected3 = @"SELECT column_name FROM table";
            var actual3 = transformer.Transform(input3);
            Assert.Equal(expected3, actual3);
        }

        [Fact]
        public void MixedHtmlTagsTest()
        {
            var transformer = new HasuraSQLSimplifierTransformer();
            
            // Test mixed span and code tags
            var input = @"SELECT <span class=""highlight"">  table_name  </span>.<code>  column_name  </code> FROM <span class=""table"">another_table</span>";
            var expected = @"SELECT table_name.column_name FROM another_table";
            var actual = transformer.Transform(input);
            Assert.Equal(expected, actual);
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
