using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using AslPrescriptionApi.Models.ASL;
using AslPrescriptionApi.Models.ASRX;

namespace AslPrescriptionApi.Models
{
    public class AslPrescriptionApiDbContext :DbContext
    {
        public AslPrescriptionApiDbContext()
            : base("name=AslPrescriptionApiDbContext")
        {
            //base.Configuration.ProxyCreationEnabled = false;
        }


        public DbSet<AslCompany> AslCompanyDbSet { get; set; }
        public DbSet<AslUserco> AslUsercoDbSet { get; set; }
        public DbSet<ASL_LOG> AslLogDbSet { get; set; }
        public DbSet<ASL_DELETE> AslDeleteDbSet { get; set; }
        public DbSet<ASL_MENUMST> AslMenumstDbSet { get; set; }
        public DbSet<ASL_MENU> AslMenuDbSet { get; set; }
        public DbSet<ASL_ROLE> AslRoleDbSet { get; set; }


        public DbSet<Refer> RxReferDbSet { get; set; }
        public DbSet<Patient> RxPatientDbSet { get; set; }
        public DbSet<Test> RxTestDbSet { get; set; }
        public DbSet<TestMst> RxTestMstDbSet { get; set; }
        public DbSet<Pharma> RxPharmaDbSet { get; set; }
        public DbSet<MediMst> RxMediMstDbSet { get; set; }
        public DbSet<MediCare> RxMediCareDbSet { get; set; }
        public DbSet<GheadMst> RxGheadMstDbSet { get; set; }
        public DbSet<Ghead> RxGheadDbSet { get; set; }
        public DbSet<PrescMst> RxPrescMstDbSet { get; set; }
        public DbSet<Prescribe> RxPrescribeDbSet { get; set; }
        public DbSet<Pad> RxPadDbSet { get; set; }




        //Upload Excel File Data module
        public DbSet<ASL_PCONTACTS> UploadContactDbSet { get; set; }
        public DbSet<ASL_PGROUPS> UploadGroupDbSet { get; set; }
        public DbSet<ASL_PEMAIL> SendLogEmailDbSet { get; set; }
        public DbSet<ASL_PSMS> SendLogSMSDbSet { get; set; }


        //Promotional
        public DbSet<ASL_PCalendarImage> CalendarImageDbSet { get; set; }
        public DbSet<ASL_SchedularCalendar> SchedularCalendarDbSet { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}