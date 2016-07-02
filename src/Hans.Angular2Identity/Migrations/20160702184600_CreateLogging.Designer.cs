using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Hans.Angular2Identity.Infrastructure;

namespace Hans.Angular2Identity.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20160702184600_CreateLogging")]
    partial class CreateLogging
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rc2-20901")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Hans.Angular2Identity.Infrastructure.Domains.Logging", b =>
            {
                b.Property<int>("Id");

                b.Property<string>("Message");

                b.Property<string>("StackTrace");

                b.Property<DateTimeOffset?>("DateCreated");
            });
        }
    }
}
