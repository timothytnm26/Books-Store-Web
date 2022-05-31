using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
namespace Project8.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<Project8.Models.Data.BSDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Project8.Models.Data.BSDBContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
