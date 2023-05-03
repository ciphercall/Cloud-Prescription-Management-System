using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using AslPrescriptionApi.Models;
using AslPrescriptionApi.Models.ASRX;
using AslPrescriptionApi.Models.DTO;

namespace AslPrescriptionApi.Controllers.ASRX
{
    public class PadController : AppController
    {
         //Datetime formet
        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);
        TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
        public DateTime td;

        AslPrescriptionApiDbContext db = new AslPrescriptionApiDbContext();
        //Get Ip ADDRESS,Time & user PC Name
        public string strHostName;
        public IPHostEntry ipHostInfo;
        public IPAddress ipAddress;

        public PadController()
        {
            strHostName = System.Net.Dns.GetHostName();
            ipHostInfo = Dns.Resolve(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[0];
            //td = DateTime.Now;
            td = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);

            ViewData["HighLight_Menu_BillingForm"] = "Heigh Light Menu";
        }





        // GET: Pad
        public ActionResult Index()
        {
            return View();
        }


         //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(PadDTO model)
        {
            var checkData = (from m in db.RxPadDbSet where m.COMPID == model.COMPID select m).ToList();
            if (checkData.Count == 1)
            {
                foreach (Pad obj in checkData)
                {
                    obj.HL1 = model.HL1;
                    obj.HL2 = model.HL2;
                    obj.HL3 = model.HL3;
                    obj.HL4 = model.HL4;
                    obj.HL5 = model.HL5;

                    obj.HM1 = model.HM1;
                    obj.HM2 = model.HM2;
                    obj.HM3 = model.HM3;

                    obj.HR1 = model.HR1;
                    obj.HR2 = model.HR2;
                    obj.HR3 = model.HR3;
                    obj.HR4 = model.HR4;
                    obj.HR5 = model.HR5;

                    obj.FL1 = model.FL1;
                    obj.FL2 = model.FL2;
                    obj.FL3 = model.FL3;
                    obj.FL4 = model.FL4;

                    obj.FM1 = model.FM1;
                    obj.FM2 = model.FM2;
                    obj.FM3 = model.FM3;

                    obj.FR1 = model.FR1;
                    obj.FR2 = model.FR2;
                    obj.FR3 = model.FR3;
                    obj.FR4 = model.FR4;

                    obj.USERPC = strHostName;
                    obj.UPDIPNO = ipAddress.ToString();
                    obj.UPDTIME = Convert.ToDateTime(td);
                    obj.UPDUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                }
                db.SaveChanges();
            }
            else
            {
                Pad pad = new Pad();
                pad.COMPID = model.COMPID;
                pad.HL1 = model.HL1;
                pad.HL2 = model.HL2;
                pad.HL3 = model.HL3;
                pad.HL4 = model.HL4;
                pad.HL5 = model.HL5;

                pad.HM1 = model.HM1;
                pad.HM2 = model.HM2;
                pad.HM3 = model.HM3;

                pad.HR1 = model.HR1;
                pad.HR2 = model.HR2;
                pad.HR3 = model.HR3;
                pad.HR4 = model.HR4;
                pad.HR5 = model.HR5;

                pad.FL1 = model.FL1;
                pad.FL2 = model.FL2;
                pad.FL3 = model.FL3;
                pad.FL4 = model.FL4;

                pad.FM1 = model.FM1;
                pad.FM2 = model.FM2;
                pad.FM3 = model.FM3;

                pad.FR1 = model.FR1;
                pad.FR2 = model.FR2;
                pad.FR3 = model.FR3;
                pad.FR4 = model.FR4;

                pad.USERPC = strHostName;
                pad.INSIPNO = ipAddress.ToString();
                pad.INSTIME = Convert.ToDateTime(td);
                pad.INSUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);

                db.RxPadDbSet.Add(pad);
                db.SaveChanges();
            }

            TempData["PadModel"] = model;
            return RedirectToAction("GetPadForm");
        }


        public ActionResult GetPadForm()
        {
            var model = (PadDTO)TempData["PadModel"];
            return View(model);
        }

    }
}