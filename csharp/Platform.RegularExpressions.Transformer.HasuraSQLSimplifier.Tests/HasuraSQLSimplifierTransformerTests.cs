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
