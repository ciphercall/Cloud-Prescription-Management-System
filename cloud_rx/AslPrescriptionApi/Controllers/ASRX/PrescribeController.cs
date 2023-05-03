using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using AslPrescriptionApi.Models;
using AslPrescriptionApi.Models.ASL;
using AslPrescriptionApi.Models.DTO;
using WebGrease.Css.Ast;

namespace AslPrescriptionApi.Controllers.ASRX
{
    public class PrescribeController : AppController
    {

        AslPrescriptionApiDbContext db = new AslPrescriptionApiDbContext();

        //Get Ip ADDRESS,Time & user PC Name
        public string strHostName;
        public IPHostEntry ipHostInfo;
        public IPAddress ipAddress;

        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);
        TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
        public DateTime td;
        public PrescribeController()
        {
            strHostName = System.Net.Dns.GetHostName();
            ipHostInfo = Dns.Resolve(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[0];
            td = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);

            ViewData["HighLight_Menu_BillingForm"] = "Heigh Light Menu";
        }







        public ASL_LOG aslLog = new ASL_LOG();

        public void Insert_LogData(PrescMst_PrescribeDTO model)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslLogDbSet where n.COMPID == model.COMPID && n.USERID == model.INSUSERID select n.LOGSLNO).Max());
            if (maxSerialNo == 0)
            {
                aslLog.LOGSLNO = Convert.ToInt64("1");
            }
            else
            {
                aslLog.LOGSLNO = maxSerialNo + 1;
            }


            aslLog.COMPID = Convert.ToInt64(model.COMPID);
            aslLog.USERID = model.INSUSERID;
            aslLog.LOGTYPE = "INSERT";
            aslLog.LOGSLNO = aslLog.LOGSLNO;
            aslLog.LOGDATE = Convert.ToDateTime(date);
            aslLog.LOGTIME = Convert.ToString(time);
            aslLog.LOGIPNO = model.INSIPNO;
            aslLog.LOGLTUDE = model.INSLTUDE;
            aslLog.TABLEID = "RX_PRESCMST";
            aslLog.LOGDATA = Convert.ToString("Prescription page input. Transaction Date: " + model.TRANSDT + ",\nMemo No:  " + model.TRANSNO + ",\nPatient name: " + model.RXPNM + ",\nPatient Type: " + model.RXPTP + ".");
            aslLog.USERPC = model.USERPC;

            db.AslLogDbSet.Add(aslLog);
            db.SaveChanges();
        }

        public void Update_LogData(PrescMst_PrescribeDTO model)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslLogDbSet where n.COMPID == model.COMPID && n.USERID == model.INSUSERID select n.LOGSLNO).Max());
            if (maxSerialNo == 0)
            {
                aslLog.LOGSLNO = Convert.ToInt64("1");
            }
            else
            {
                aslLog.LOGSLNO = maxSerialNo + 1;
            }


            aslLog.COMPID = Convert.ToInt64(model.COMPID);
            aslLog.USERID = model.INSUSERID;
            aslLog.LOGTYPE = "UPDATE";
            aslLog.LOGSLNO = aslLog.LOGSLNO;
            aslLog.LOGDATE = Convert.ToDateTime(date);
            aslLog.LOGTIME = Convert.ToString(time);
            aslLog.LOGIPNO = ipAddress.ToString(); 
            aslLog.LOGLTUDE = model.INSLTUDE;
            aslLog.TABLEID = "RX_PRESCMST";
            aslLog.LOGDATA = Convert.ToString("Prescription page update. Transaction Date: " + model.TRANSDT + ",\nMemo No:  " + model.TRANSNO + ",\nPatient name: " + model.RXPNM + ",\nPatient Type: " + model.RXPTP + ".");
            aslLog.USERPC = strHostName; 

            db.AslLogDbSet.Add(aslLog);
            db.SaveChanges();
        }













        // GET: Prescribe (Create) 
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Index(PrescMst_PrescribeDTO model)
        {
            var check_PrescribeMaster_data = (from n in db.RxPrescMstDbSet where n.COMPID == model.COMPID && n.TRANSNO == model.TRANSNO select n).ToList();
            if (check_PrescribeMaster_data.Count == 1)
            {
                foreach (var data in check_PrescribeMaster_data)
                {
                    data.RXPTP = Convert.ToString(model.RXPTP);
                    if (model.NXTDT != null)
                    {
                        data.NXTDT = Convert.ToDateTime(model.NXTDT);
                    }
                    else
                    {
                        data.NXTDT = null;
                    }
                    if (model.AMOUNT != null)
                    {
                        data.AMOUNT = Convert.ToInt64(model.AMOUNT);
                    }
                    else
                    {
                        data.AMOUNT = 0;
                    }
                    data.REMARKS = Convert.ToString(model.REMARKS);
                   
                    data.USERPC = strHostName;
                    data.INSIPNO = ipAddress.ToString();
                    data.INSLTUDE = Convert.ToString(model.INSLTUDE);
                    data.INSTIME = Convert.ToDateTime(td);
                    data.INSUSERID = Convert.ToInt64(model.INSUSERID);
                }
                db.SaveChanges();
            }

            TempData["Prescribe_Report_Model"] = model;
            return RedirectToAction("GetPrescribeForm");
        }










        //AutoComplete Master Part
        public JsonResult TagSearch(string character, string type)
        {
            List<string> result = new List<string>();
            var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());

            string date = td.ToString("dd-MMM-yyyy");
            DateTime transDate = DateTime.Parse(date);
            if (type == "NEW")
            {
               var tags = from p in db.RxPatientDbSet
                           where p.COMPID == compid && p.TRANSDT == transDate
                           select new { p.RXPIDM };

                foreach (var tag in tags)
                {
                    result.Add(tag.RXPIDM.ToString());
                }
            }
            else if (type == "OLD")
            {
                var tags = from p in db.RxPatientDbSet
                           where p.COMPID == compid && p.TRANSDT != transDate
                           select new { p.RXPIDM };

                foreach (var tag in tags)
                {
                    result.Add(tag.RXPIDM.ToString());
                }
            }
            
            return this.Json(result.Where(t => t.StartsWith(character)), JsonRequestBehavior.AllowGet);
        }


        //AutoComplete  Master Part
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult keyword(string changedText, string patientType)
        {
            var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
            String itemId = "";

            // string changedText = Convert.ToString(changedText1);

            List<string> patientIDM_List = new List<string>();
            var tags = from p in db.RxPatientDbSet
                       where p.COMPID == compid
                       select new { p.RXPIDM, p.COMPID };
            foreach (var tag in tags)
            {
                patientIDM_List.Add(tag.RXPIDM.ToString());
            }

            var rt = patientIDM_List.Where(t => t.StartsWith(changedText)).ToList();

            int lenChangedtxt = changedText.Length;

            Int64 cont = rt.Count();
            Int64 k = 0;
            string status = "";
            if (changedText != "" && cont != 0)
            {
                while (status != "no")
                {
                    k = 0;
                    foreach (var n in rt)
                    {
                        string ss = Convert.ToString(n);
                        string subss = "";
                        if (ss.Length >= lenChangedtxt)
                        {
                            subss = ss.Substring(0, lenChangedtxt);
                            subss = subss.ToUpper();
                        }
                        else
                        {
                            subss = "";
                        }


                        if (subss == changedText.ToUpper())
                        {
                            k++;
                        }
                        if (k == cont)
                        {
                            status = "yes";
                            lenChangedtxt++;
                            if (ss.Length > lenChangedtxt - 1)
                            {
                                changedText = changedText + ss[lenChangedtxt - 1];
                            }

                        }
                        else
                        {
                            status = "no";
                            //lenChangedtxt--;
                        }

                    }

                }
                if (lenChangedtxt == 1)
                {
                    itemId = changedText.Substring(0, lenChangedtxt);
                }

                else
                {
                    itemId = changedText.Substring(0, lenChangedtxt - 1);
                }
            }
            else
            {
                itemId = changedText;
            }

            String patientName = "", patientID = "";
            Regex regex = new Regex("^[0-9]+$");//check its int or string?
            if (regex.IsMatch(itemId))
            {
                //string value is a number
                Int64 Number = Convert.ToInt64(itemId);
                
                string date = td.ToString("dd-MMM-yyyy");
                DateTime transDate = DateTime.Parse(date);
                if (patientType == "NEW")
                {
                    var query = db.RxPatientDbSet.Where(n => n.RXPIDM == Number && n.COMPID == compid && n.TRANSDT == transDate).Select(n => new { n.RXPNM, n.RXPID });
                    foreach (var n in query)
                    {
                        patientName = Convert.ToString(n.RXPNM);
                        patientID = Convert.ToString(n.RXPID);
                    }
                }
                else if (patientType == "OLD")
                {
                    var query = db.RxPatientDbSet.Where(n => n.RXPIDM == Number && n.COMPID == compid && n.TRANSDT != transDate).Select(n => new { n.RXPNM, n.RXPID });
                    foreach (var n in query)
                    {
                        patientName = Convert.ToString(n.RXPNM);
                        patientID = Convert.ToString(n.RXPID);
                    }
                }
            }


            var result = new { PATIENTIDM = itemId, PATIENTNM = patientName, PATIENTID = patientID };
            return Json(result, JsonRequestBehavior.AllowGet);
        }










        //AutoComplete 1st part Tab
        public JsonResult TagSearch_Diagnosis(string term)
        {
            List<string> result = new List<string>();
            var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
            Int64 gCatId = Convert.ToInt64(compid + "02");
            var tags = from p in db.RxGheadDbSet
                       where p.COMPID == compid && p.GCATID == gCatId
                       select new { p.GHEADEN };
            foreach (var tag in tags)
            {
                result.Add(tag.GHEADEN.ToString());
            }
            return this.Json(result.Where(t => t.StartsWith(term)), JsonRequestBehavior.AllowGet);
        }


        //AutoComplete  1st part Tab
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult keyword_Diagnosis(string changedText)
        {
            var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
            Int64 gCatId = Convert.ToInt64(compid + "02");

            String itemId = "";

            // string changedText = Convert.ToString(changedText1);

            List<string> List = new List<string>();
            var tags = from p in db.RxGheadDbSet
                       where p.COMPID == compid && p.GCATID == gCatId
                       select new { p.GHEADEN, p.COMPID };
            foreach (var tag in tags)
            {
                List.Add(tag.GHEADEN.ToString());
            }
            var rt = List.Where(t => t.StartsWith(changedText)).ToList();

            int lenChangedtxt = changedText.Length;

            Int64 cont = rt.Count();
            Int64 k = 0;
            string status = "";
            if (changedText != "" && cont != 0)
            {
                while (status != "no")
                {
                    k = 0;
                    foreach (var n in rt)
                    {
                        string ss = Convert.ToString(n);
                        string subss = "";
                        if (ss.Length >= lenChangedtxt)
                        {
                            subss = ss.Substring(0, lenChangedtxt);
                            subss = subss.ToUpper();
                        }
                        else
                        {
                            subss = "";
                        }


                        if (subss == changedText.ToUpper())
                        {
                            k++;
                        }
                        if (k == cont)
                        {
                            status = "yes";
                            lenChangedtxt++;
                            if (ss.Length > lenChangedtxt - 1)
                            {
                                changedText = changedText + ss[lenChangedtxt - 1];
                            }

                        }
                        else
                        {
                            status = "no";
                            //lenChangedtxt--;
                        }

                    }

                }
                if (lenChangedtxt == 1)
                {
                    itemId = changedText.Substring(0, lenChangedtxt);
                }

                else
                {
                    itemId = changedText.Substring(0, lenChangedtxt - 1);
                }
            }
            else
            {
                itemId = changedText;
            }

            String GheadID = "";
            var query = db.RxGheadDbSet.Where(n => n.GHEADEN == itemId && n.COMPID == compid && n.GCATID == gCatId).Select(n => new { n.GHEADID, n.GHEADEN });
            foreach (var n in query)
            {
                GheadID = Convert.ToString(n.GHEADID);
            }

            var result = new { GHEADEN = itemId, GHEADID = GheadID };
            return Json(result, JsonRequestBehavior.AllowGet);

        }











        //AutoComplete 2nd part Tab
        public JsonResult TagSearch_Particulars(string term)
        {
            List<string> result = new List<string>();
            var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
            Int64 gCatId = Convert.ToInt64(compid + "03");
            var tags = from p in db.RxGheadDbSet
                       where p.COMPID == compid && p.GCATID == gCatId
                       select new { p.GHEADEN };
            foreach (var tag in tags)
            {
                result.Add(tag.GHEADEN.ToString());
            }
            return this.Json(result.Where(t => t.StartsWith(term)), JsonRequestBehavior.AllowGet);
        }


        //AutoComplete  2nd part Tab
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult keyword_HeadParticulars(string changedText)
        {
            var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
            Int64 gCatId = Convert.ToInt64(compid + "03");

            String itemId = "";

            // string changedText = Convert.ToString(changedText1);

            List<string> List = new List<string>();
            var tags = from p in db.RxGheadDbSet
                       where p.COMPID == compid && p.GCATID == gCatId
                       select new { p.GHEADEN, p.COMPID };
            foreach (var tag in tags)
            {
                List.Add(tag.GHEADEN.ToString());
            }

            var rt = List.Where(t => t.StartsWith(changedText)).ToList();

            int lenChangedtxt = changedText.Length;

            Int64 cont = rt.Count();
            Int64 k = 0;
            string status = "";
            if (changedText != "" && cont != 0)
            {
                while (status != "no")
                {
                    k = 0;
                    foreach (var n in rt)
                    {
                        string ss = Convert.ToString(n);
                        string subss = "";
                        if (ss.Length >= lenChangedtxt)
                        {
                            subss = ss.Substring(0, lenChangedtxt);
                            subss = subss.ToUpper();
                        }
                        else
                        {
                            subss = "";
                        }


                        if (subss == changedText.ToUpper())
                        {
                            k++;
                        }
                        if (k == cont)
                        {
                            status = "yes";
                            lenChangedtxt++;
                            if (ss.Length > lenChangedtxt - 1)
                            {
                                changedText = changedText + ss[lenChangedtxt - 1];
                            }
                        }
                        else
                        {
                            status = "no";
                            //lenChangedtxt--;
                        }
                    }
                }
                if (lenChangedtxt == 1)
                {
                    itemId = changedText.Substring(0, lenChangedtxt);
                }
                else
                {
                    itemId = changedText.Substring(0, lenChangedtxt - 1);
                }
            }
            else
            {
                itemId = changedText;
            }

            String GheadID = "";
            var query = db.RxGheadDbSet.Where(n => n.GHEADEN == itemId && n.COMPID == compid && n.GCATID == gCatId).Select(n => new { n.GHEADID, n.GHEADEN });
            foreach (var n in query)
            {
                GheadID = Convert.ToString(n.GHEADID);
            }

            var result = new { GHEADEN = itemId, GHEADID = GheadID };
            return Json(result, JsonRequestBehavior.AllowGet);
        }












        //AutoComplete 3rd part Tab
        public JsonResult TagSearch_Investigations(string term)
        {
            List<string> result = new List<string>();
            var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
            var tags = from p in db.RxTestDbSet
                       where p.COMPID == compid
                       select new { p.TESTNM, p.TESTID, p.TCATID };
            foreach (var tag in tags)
            {
                result.Add(tag.TESTNM.ToString());
            }
            return this.Json(result.Where(t => t.StartsWith(term)), JsonRequestBehavior.AllowGet);
        }


        //AutoComplete  3rd part Tab
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult keyword_Investigations(string changedText)
        {
            var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
            String itemId = "";

            // string changedText = Convert.ToString(changedText1);

            List<string> List = new List<string>();
            var tags = from p in db.RxTestDbSet
                       where p.COMPID == compid
                       select new { p.TESTNM, p.COMPID };
            foreach (var tag in tags)
            {
                List.Add(tag.TESTNM.ToString());
            }
            var rt = List.Where(t => t.StartsWith(changedText)).ToList();

            int lenChangedtxt = changedText.Length;

            Int64 cont = rt.Count();
            Int64 k = 0;
            string status = "";
            if (changedText != "" && cont != 0)
            {
                while (status != "no")
                {
                    k = 0;
                    foreach (var n in rt)
                    {
                        string ss = Convert.ToString(n);
                        string subss = "";
                        if (ss.Length >= lenChangedtxt)
                        {
                            subss = ss.Substring(0, lenChangedtxt);
                            subss = subss.ToUpper();
                        }
                        else
                        {
                            subss = "";
                        }


                        if (subss == changedText.ToUpper())
                        {
                            k++;
                        }
                        if (k == cont)
                        {
                            status = "yes";
                            lenChangedtxt++;
                            if (ss.Length > lenChangedtxt - 1)
                            {
                                changedText = changedText + ss[lenChangedtxt - 1];
                            }
                        }
                        else
                        {
                            status = "no";
                            //lenChangedtxt--;
                        }
                    }
                }
                if (lenChangedtxt == 1)
                {
                    itemId = changedText.Substring(0, lenChangedtxt);
                }
                else
                {
                    itemId = changedText.Substring(0, lenChangedtxt - 1);
                }
            }
            else
            {
                itemId = changedText;
            }

            String Testid = "", TCategoryid = "";
            var query = db.RxTestDbSet.Where(n => n.TESTNM == itemId && n.COMPID == compid).Select(n => new { n.TESTNM, n.TESTID, n.TCATID });
            foreach (var n in query)
            {
                TCategoryid = Convert.ToString(n.TCATID);
                Testid = Convert.ToString(n.TESTID);
            }

            var result = new { GHEADEN = itemId, GHEADID = Testid, GCATID = TCategoryid };
            return Json(result, JsonRequestBehavior.AllowGet);
        }












        //AutoComplete 4th part Tab
        public JsonResult TagSearch_Medicine(string term)
        {
            List<string> result = new List<string>();
            var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
            var tags = from p in db.RxMediCareDbSet
                       where p.COMPID == compid
                       select new { p.MEDINM };
            foreach (var tag in tags)
            {
                result.Add(tag.MEDINM.ToString());
            }
            return this.Json(result.Where(t => t.StartsWith(term)), JsonRequestBehavior.AllowGet);
        }


        //AutoComplete  4th part Tab
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult keyword_Medicine(string changedText)
        {
            var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
            String itemId = "";

            // string changedText = Convert.ToString(changedText1);

            List<string> List = new List<string>();
            var tags = from p in db.RxMediCareDbSet
                       where p.COMPID == compid
                       select new { p.MEDINM, p.COMPID };
            foreach (var tag in tags)
            {
                List.Add(tag.MEDINM.ToString());
            }
            var rt = List.Where(t => t.StartsWith(changedText)).ToList();

            int lenChangedtxt = changedText.Length;

            Int64 cont = rt.Count();
            Int64 k = 0;
            string status = "";
            if (changedText != "" && cont != 0)
            {
                while (status != "no")
                {
                    k = 0;
                    foreach (var n in rt)
                    {
                        string ss = Convert.ToString(n);
                        string subss = "";
                        if (ss.Length >= lenChangedtxt)
                        {
                            subss = ss.Substring(0, lenChangedtxt);
                            subss = subss.ToUpper();
                        }
                        else
                        {
                            subss = "";
                        }


                        if (subss == changedText.ToUpper())
                        {
                            k++;
                        }
                        if (k == cont)
                        {
                            status = "yes";
                            lenChangedtxt++;
                            if (ss.Length > lenChangedtxt - 1)
                            {
                                changedText = changedText + ss[lenChangedtxt - 1];
                            }
                        }
                        else
                        {
                            status = "no";
                            //lenChangedtxt--;
                        }
                    }
                }
                if (lenChangedtxt == 1)
                {
                    itemId = changedText.Substring(0, lenChangedtxt);
                }
                else
                {
                    itemId = changedText.Substring(0, lenChangedtxt - 1);
                }
            }
            else
            {
                itemId = changedText;
            }

            String MediId = "", MCategoryid = "";
            var query = db.RxMediCareDbSet.Where(n => n.MEDINM == itemId && n.COMPID == compid).Select(n => new { n.MEDINM, n.MEDIID, n.MCATID });
            foreach (var n in query)
            {
                MCategoryid = Convert.ToString(n.MCATID);
                MediId = Convert.ToString(n.MEDIID);
            }

            var result = new { MEDINM = itemId, MEDIID = MediId, MCATID = MCategoryid };
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        //AutoComplete 4th part Tab
        public JsonResult TagSearch_Dose(string term)
        {
            List<string> result = new List<string>();
            var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
            Int64 gCatId = Convert.ToInt64(compid + "01");
            var tags = from p in db.RxGheadDbSet
                       where p.COMPID == compid && p.GCATID == gCatId
                       select new { p.GHEADEN };
            foreach (var tag in tags)
            {
                result.Add(tag.GHEADEN.ToString());
            }
            return this.Json(result.Where(t => t.StartsWith(term)), JsonRequestBehavior.AllowGet);
        }


        //AutoComplete  4th part Tab
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult keyword_Dose(string changedText)
        {
            var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
            Int64 gCatId = Convert.ToInt64(compid + "01");
            String itemId = "";

            // string changedText = Convert.ToString(changedText1);

            List<string> List = new List<string>();
            var tags = from p in db.RxGheadDbSet
                       where p.COMPID == compid && p.GCATID == gCatId
                       select new { p.GHEADEN, p.COMPID };
            foreach (var tag in tags)
            {
                List.Add(tag.GHEADEN.ToString());
            }
            var rt = List.Where(t => t.StartsWith(changedText)).ToList();

            int lenChangedtxt = changedText.Length;

            Int64 cont = rt.Count();
            Int64 k = 0;
            string status = "";
            if (changedText != "" && cont != 0)
            {
                while (status != "no")
                {
                    k = 0;
                    foreach (var n in rt)
                    {
                        string ss = Convert.ToString(n);
                        string subss = "";
                        if (ss.Length >= lenChangedtxt)
                        {
                            subss = ss.Substring(0, lenChangedtxt);
                            subss = subss.ToUpper();
                        }
                        else
                        {
                            subss = "";
                        }

                        if (subss == changedText.ToUpper())
                        {
                            k++;
                        }
                        if (k == cont)
                        {
                            status = "yes";
                            lenChangedtxt++;
                            if (ss.Length > lenChangedtxt - 1)
                            {
                                changedText = changedText + ss[lenChangedtxt - 1];
                            }
                        }
                        else
                        {
                            status = "no";
                        }
                    }
                }
                if (lenChangedtxt == 1)
                {
                    itemId = changedText.Substring(0, lenChangedtxt);
                }
                else
                {
                    itemId = changedText.Substring(0, lenChangedtxt - 1);
                }
            }
            else
            {
                itemId = changedText;
            }

            String GheadID = "";
            var query = db.RxGheadDbSet.Where(n => n.GHEADEN == itemId && n.COMPID == compid && n.GCATID == gCatId).Select(n => new { n.GHEADID, n.GHEADEN });
            foreach (var n in query)
            {
                GheadID = Convert.ToString(n.GHEADID);
            }

            var result = new { GHEADEN = itemId, GHEADID = GheadID };
            return Json(result, JsonRequestBehavior.AllowGet);

        }










        //AutoComplete 5th part Tab
        public JsonResult TagSearch_Advice(string term)
        {
            List<string> result = new List<string>();
            var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
            Int64 gCatId = Convert.ToInt64(compid + "04");
            var tags = from p in db.RxGheadDbSet
                       where p.COMPID == compid && p.GCATID == gCatId
                       select new { p.GHEADEN };
            foreach (var tag in tags)
            {
                result.Add(tag.GHEADEN.ToString());
            }
            return this.Json(result.Where(t => t.StartsWith(term)), JsonRequestBehavior.AllowGet);
        }


        //AutoComplete  5th part Tab
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult keyword_Advice(string changedText)
        {
            var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
            Int64 gCatId = Convert.ToInt64(compid + "04");
            String itemId = "";

            // string changedText = Convert.ToString(changedText1);

            List<string> List = new List<string>();
            var tags = from p in db.RxGheadDbSet
                       where p.COMPID == compid && p.GCATID == gCatId
                       select new { p.GHEADEN, p.COMPID };
            foreach (var tag in tags)
            {
                List.Add(tag.GHEADEN.ToString());
            }
            var rt = List.Where(t => t.StartsWith(changedText)).ToList();

            int lenChangedtxt = changedText.Length;

            Int64 cont = rt.Count();
            Int64 k = 0;
            string status = "";
            if (changedText != "" && cont != 0)
            {
                while (status != "no")
                {
                    k = 0;
                    foreach (var n in rt)
                    {
                        string ss = Convert.ToString(n);
                        string subss = "";
                        if (ss.Length >= lenChangedtxt)
                        {
                            subss = ss.Substring(0, lenChangedtxt);
                            subss = subss.ToUpper();
                        }
                        else
                        {
                            subss = "";
                        }

                        if (subss == changedText.ToUpper())
                        {
                            k++;
                        }
                        if (k == cont)
                        {
                            status = "yes";
                            lenChangedtxt++;
                            if (ss.Length > lenChangedtxt - 1)
                            {
                                changedText = changedText + ss[lenChangedtxt - 1];
                            }
                        }
                        else
                        {
                            status = "no";
                        }
                    }
                }
                if (lenChangedtxt == 1)
                {
                    itemId = changedText.Substring(0, lenChangedtxt);
                }
                else
                {
                    itemId = changedText.Substring(0, lenChangedtxt - 1);
                }
            }
            else
            {
                itemId = changedText;
            }

            String GheadID = "";
            var query = db.RxGheadDbSet.Where(n => n.GHEADEN == itemId && n.COMPID == compid && n.GCATID == gCatId).Select(n => new { n.GHEADID, n.GHEADEN });
            foreach (var n in query)
            {
                GheadID = Convert.ToString(n.GHEADID);
            }

            var result = new { GHEADEN = itemId, GHEADID = GheadID };
            return Json(result, JsonRequestBehavior.AllowGet);

        }










        // GET: Prescribe (Update) 
        public ActionResult Update()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Update(PrescMst_PrescribeDTO model)
        {
            var check_PrescribeMaster_data = (from n in db.RxPrescMstDbSet where n.COMPID == model.COMPID && n.TRANSNO == model.TRANSNO select n).ToList();
            if (check_PrescribeMaster_data.Count == 1)
            {
                foreach (var data in check_PrescribeMaster_data)
                {
                    data.RXPTP = Convert.ToString(model.RXPTP);
                    if (model.NXTDT != null)
                    {
                        data.NXTDT = Convert.ToDateTime(model.NXTDT);
                    }
                    else
                    {
                        data.NXTDT = null;
                    }
                    if (model.AMOUNT != null)
                    {
                        data.AMOUNT = Convert.ToInt64(model.AMOUNT);
                    }
                    else
                    {
                        data.AMOUNT = 0;
                    }
                    data.REMARKS = Convert.ToString(model.REMARKS);
                    
                    data.USERPC = strHostName;
                    data.UPDIPNO = ipAddress.ToString();
                    data.UPDLTUDE = Convert.ToString(model.INSLTUDE);
                    data.UPDTIME = Convert.ToDateTime(td);
                    data.UPDUSERID = Convert.ToInt64(model.INSUSERID);
                }
                db.SaveChanges();

                if (model.UpdateLogData != null)
                {
                    Update_LogData(model);
                }

            }

            TempData["Prescribe_Report_Model"] = model;
            return RedirectToAction("GetPrescribeForm");
        }

        public ActionResult GetPrescribeForm()
        {
            var model = (PrescMst_PrescribeDTO)TempData["Prescribe_Report_Model"];
            return View(model);
        }

        




        //AutoComplete Master Part (Update)
        public JsonResult TagSearch_update(string term, string changedDropDown, string companyid)
        {
            if (changedDropDown == "")
            {
                return this.Json(null, JsonRequestBehavior.AllowGet);
            }
            else
            {
                DateTime date = Convert.ToDateTime(changedDropDown);
                Int64 compid = Convert.ToInt64(companyid);

                List<string> result = new List<string>();

                var findPaitentID = from m in db.RxPrescMstDbSet
                                    where m.COMPID == compid && m.TRANSDT == date
                                    select new { m.TRANSNO };
                foreach (var pid in findPaitentID)
                {
                    result.Add(pid.TRANSNO.ToString());
                }
                return this.Json(result.Where(t => t.StartsWith(term)), JsonRequestBehavior.AllowGet);
            }
        }


        //AutoComplete  Master Part (Update)
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult keyword_Update(string changedText, string transdate, string companyid)
        {
            if (transdate == "")
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Int64 compid = Convert.ToInt16(companyid);
                String itemId = "";
                DateTime date = Convert.ToDateTime(transdate);

                List<string> transNOList = new List<string>();
                var tags = from p in db.RxPrescMstDbSet
                           where p.COMPID == compid && date == p.TRANSDT
                           select new { p.TRANSNO };
                foreach (var tag in tags)
                {
                    transNOList.Add(tag.TRANSNO.ToString());
                }

                var rt = transNOList.Where(t => t.StartsWith(changedText)).ToList();

                int lenChangedtxt = changedText.Length;

                Int64 cont = rt.Count();
                Int64 k = 0;
                string status = "";
                if (changedText != "" && cont != 0)
                {
                    while (status != "no")
                    {
                        k = 0;
                        foreach (var n in rt)
                        {
                            string ss = Convert.ToString(n);
                            string subss = "";
                            if (ss.Length >= lenChangedtxt)
                            {
                                subss = ss.Substring(0, lenChangedtxt);
                                subss = subss.ToUpper();
                            }
                            else
                            {
                                subss = "";
                            }


                            if (subss == changedText.ToUpper())
                            {
                                k++;
                            }
                            if (k == cont)
                            {
                                status = "yes";
                                lenChangedtxt++;
                                if (ss.Length > lenChangedtxt - 1)
                                {
                                    changedText = changedText + ss[lenChangedtxt - 1];
                                }

                            }
                            else
                            {
                                status = "no";
                                //lenChangedtxt--;
                            }

                        }

                    }
                    if (lenChangedtxt == 1)
                    {
                        itemId = changedText.Substring(0, lenChangedtxt);
                    }

                    else
                    {
                        itemId = changedText.Substring(0, lenChangedtxt - 1);
                    }
                }
                else
                {
                    itemId = changedText;
                }

                String patientName = "", patientIDM = "", PatientType = "", Nextdate = "", Remarks = "";
                Int64 pid = 0, amount = 0;
                Regex regex = new Regex("^[0-9]+$");//check its int or string?
                if (regex.IsMatch(itemId))
                {
                    //string value is a number
                    string Number = Convert.ToString(itemId);
                    var findPrescribeMasterData = db.RxPrescMstDbSet.Where(n => n.TRANSNO == Number && n.COMPID == compid).Select(n => new { n.RXPID, n.RXPTP, n.NXTDT, n.REMARKS,n.AMOUNT });
                    foreach (var n in findPrescribeMasterData)
                    {
                        PatientType = Convert.ToString(n.RXPTP);
                        if (n.NXTDT != null)
                        {
                            string date1 = Convert.ToString(n.NXTDT);
                            DateTime MyDateTime = DateTime.Parse(date1);
                            Nextdate = MyDateTime.ToString("dd-MMM-yyyy");
                        }

                        Remarks = Convert.ToString(n.REMARKS);
                        pid = Convert.ToInt64(n.RXPID);
                        if (n.AMOUNT != null)
                        {
                            amount = Convert.ToInt16(n.AMOUNT);
                        }
                    }

                    var query = db.RxPatientDbSet.Where(n => n.RXPID == pid && n.COMPID == compid).Select(n => new { n.RXPNM, n.RXPIDM });
                    foreach (var n in query)
                    {
                        patientName = Convert.ToString(n.RXPNM);
                        patientIDM = Convert.ToString(n.RXPIDM);
                    }
                }
                var result = new
                {
                    TRANSNO = itemId,
                    PATIENTNM = patientName,
                    PATIENTID = pid,
                    PATIENTIDM = patientIDM,
                    RXPTP = PatientType,
                    NXTDT = Nextdate,
                    REMARKS = Remarks,
                    AMOUNT = amount,
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
    }
}