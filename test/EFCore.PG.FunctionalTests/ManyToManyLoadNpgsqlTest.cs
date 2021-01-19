using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.TestModels.ManyToManyModel;
using Microsoft.EntityFrameworkCore.TestUtilities;
using Npgsql.EntityFrameworkCore.PostgreSQL.TestUtilities;
using Xunit;

namespace Npgsql.EntityFrameworkCore.PostgreSQL
{
    public class ManyToManyLoadNpgsqlTest : ManyToManyLoadTestBase<ManyToManyLoadNpgsqlTest.ManyToManyLoadNpgsqlFixture>
    {
        public ManyToManyLoadNpgsqlTest(ManyToManyLoadNpgsqlFixture fixture)
            : base(fixture)
        {
        }

        [ConditionalTheory(Skip = "https://github.com/dotnet/efcore/pull/23823")]
        public override Task Load_collection_using_Query_with_filtered_Include_and_projection(bool async)
            => base.Load_collection_using_Query_with_filtered_Include_and_projection(async);

        public class ManyToManyLoadNpgsqlFixture : ManyToManyLoadFixtureBase
        {
            public TestSqlLoggerFactory TestSqlLoggerFactory => (TestSqlLoggerFactory)ListLoggerFactory;
            protected override ITestStoreFactory TestStoreFactory => NpgsqlTestStoreFactory.Instance;

            protected override void OnModelCreating(ModelBuilder modelBuilder, DbContext context)
            {
                base.OnModelCreating(modelBuilder, context);

                modelBuilder
                    .Entity<JoinOneSelfPayload>()
                    .Property(e => e.Payload)
                    .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");

                modelBuilder
                    .SharedTypeEntity<Dictionary<string, object>>("JoinOneToThreePayloadFullShared")
                    .IndexerProperty<string>("Payload")
                    .HasDefaultValue("Generated");

                modelBuilder
                    .Entity<JoinOneToThreePayloadFull>()
                    .Property(e => e.Payload)
                    .HasDefaultValue("Generated");
            }
        }
    }
}
