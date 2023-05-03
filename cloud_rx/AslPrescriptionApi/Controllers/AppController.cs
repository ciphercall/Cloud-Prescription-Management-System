using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AslPrescriptionApi.Models;

namespace AslPrescriptionApi.Controllers
{
    public class AppController : Controller
    {
        //
        // GET: /App/
        AslPrescriptionApiDbContext db = new AslPrescriptionApiDbContext();

        public AslPrescriptionApiDbContext DataContext
        {
            get { return db; }
        }

        public AppController()
        {

        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
           
            try
            {
                var userid = Convert.ToInt64(Session["loggedUserID"]);
                var comid = Convert.ToInt64(Session["loggedCompID"]);

                //ASL
                ViewData["validUserForm"] = from c in db.AslRoleDbSet
                                       where (c.USERID == userid && c.COMPID == comid && c.STATUS == "A" && c.MENUTP=="F" && c.MODULEID=="01")
                                       select c;

                ViewData["validUserReports"] = from c in db.AslRoleDbSet
                                            where (c.USERID == userid && c.COMPID == comid && c.STATUS == "A" && c.MENUTP == "R" && c.MODULEID=="01")
                                            select c;



                //Prescription
                ViewData["validBillingForm"] = (from c in db.AslRoleDbSet
                                                where (c.USERID == userid && c.COMPID == comid && c.STATUS == "A" && c.MENUTP == "F" && c.MODULEID == "02")
                                                select c).OrderBy(x => x.SERIAL);

                ViewData["validBillingReports"] = (from c in db.AslRoleDbSet
                                                   where (c.USERID == userid && c.COMPID == comid && c.STATUS == "A" && c.MENUTP == "R" && c.MODULEID == "02")
                                                   select c).OrderBy(x => x.SERIAL);



                ////Restaurant
                //ViewData["validRMSForm"] = (from c in db.AslRoleDbSet
                //                                where (c.USERID == userid && c.COMPID == comid && c.STATUS == "A" && c.MENUTP == "F" && c.MODULEID == "03")
                //                                select c).OrderBy(x => x.SERIAL);

                //ViewData["validRMSReports"] = (from c in db.AslRoleDbSet
                //                                   where (c.USERID == userid && c.COMPID == comid && c.STATUS == "A" && c.MENUTP == "R" && c.MODULEID == "03")
                //                                   select c).OrderBy(x => x.SERIAL);



                ////GL
                //ViewData["validAccountForm"] = (from c in db.AslRoleDbSet
                //                            where (c.USERID == userid && c.COMPID == comid && c.STATUS == "A" && c.MENUTP == "F" && c.MODULEID == "04")
                //                            select c).OrderBy(x=>x.SERIAL);

                //ViewData["validAccountReports"] = (from c in db.AslRoleDbSet
                //                               where (c.USERID == userid && c.COMPID == comid && c.STATUS == "A" && c.MENUTP == "R" && c.MODULEID == "04")
                //                               select c).OrderBy(x=>x.SERIAL);



                //Promotion
                ViewData["validPromotionForm"] = (from c in db.AslRoleDbSet
                                                  where (c.USERID == userid && c.COMPID == comid && c.STATUS == "A" && c.MENUTP == "F" && c.MODULEID == "03")
                                                  select c).OrderBy(x => x.SERIAL);

                var findCompanyName = from m in db.AslCompanyDbSet where m.COMPID == comid select new { m.COMPNM };
                foreach (var name in findCompanyName)
                {
                    ViewData["CompanyName"] = name.COMPNM;
                }

            }
            catch
            {

            }
        }

    }
}
