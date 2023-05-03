using AslPrescriptionApi.Models.ASL;

namespace AslPrescriptionApi.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AslPrescriptionApi.Models.AslPrescriptionApiDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AslPrescriptionApi.Models.AslPrescriptionApiDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            context.AslUsercoDbSet.AddOrUpdate(x => x.AslUsercoId,
              new AslUserco() { AslUsercoId = 1, COMPID = 1, USERID = 10001, USERNM = "Alchemy Software(Piash)", DEPTNM = "Admin", OPTP = "AslSuperadmin", ADDRESS = "Goal pahar,Suborna, 203/b,Chittagong", MOBNO = "8804444444444", EMAILID = "superadmin01@gmail.com", LOGINBY = "EMAIL", LOGINID = "superadmin01@gmail.com", LOGINPW = "123", TIMEFR = "00:01", TIMETO = "23:59", STATUS = "A" },
              new AslUserco() { AslUsercoId = 2, COMPID = 2, USERID = 10002, USERNM = "Alchemy Software(Shimul)", DEPTNM = "Admin", OPTP = "AslSuperadmin", ADDRESS = "Goal pahar, 203/b,Chittagong", MOBNO = "8801775222222", EMAILID = "superadmin02@gmail.com", LOGINBY = "EMAIL", LOGINID = "superadmin02@gmail.com", LOGINPW = "123", TIMEFR = "00:01", TIMETO = "23:59", STATUS = "A" }
          );
            context.SaveChanges();
        }
    }
}
