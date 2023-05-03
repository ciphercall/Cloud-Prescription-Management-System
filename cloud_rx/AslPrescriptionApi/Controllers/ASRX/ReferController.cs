using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AslPrescriptionApi.Models;
using AslPrescriptionApi.Models.ASL;
using AslPrescriptionApi.Models.ASRX;
using AslPrescriptionApi.Models.DTO;

namespace AslPrescriptionApi.Controllers.ASRX
{
    public class ReferController : AppController
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

        public ReferController()
        {
            //if (System.Web.HttpContext.Current.Request.Cookies["UI"] != null)
            //{

            //}
            //else
            //{
            //    Index();
            //}
            //HttpCookie decodedCookie1 = CookieSecurityProvider.Decrypt(System.Web.HttpContext.Current.Request.Cookies["CI"]);
            //HttpCookie decodedCookie2 = CookieSecurityProvider.Decrypt(System.Web.HttpContext.Current.Request.Cookies["UI"]);
            //HttpCookie decodedCookie3 = CookieSecurityProvider.Decrypt(System.Web.HttpContext.Current.Request.Cookies["UT"]);
            //HttpCookie decodedCookie4 = CookieSecurityProvider.Decrypt(System.Web.HttpContext.Current.Request.Cookies["UN"]);
            //HttpCookie decodedCookie5 = CookieSecurityProvider.Decrypt(System.Web.HttpContext.Current.Request.Cookies["US"]);
            //HttpCookie decodedCookie6 = CookieSecurityProvider.Decrypt(System.Web.HttpContext.Current.Request.Cookies["CS"]);

            strHostName = System.Net.Dns.GetHostName();
            ipHostInfo = Dns.Resolve(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[0];
            //td = DateTime.Now;
            td = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);

            ViewData["HighLight_Menu_BillingForm"] = "Heigh Light Menu";
        }





        public ASL_LOG aslLog = new ASL_LOG();

        // SAVE ALL INFORMATION from PST_REFER TO Asl_lOG Database Table.
        public void Insert_Refer_LogData(ReferDTO model)
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
            aslLog.TABLEID = "RX_REFER";
            aslLog.LOGDATA = Convert.ToString("Refer Name: " + model.REFERNM + ",\nAddress: " + model.ADDRESS + ",\nMobile No: " + model.MOBNO1 + ",\nOther Number: " + model.MOBNO2 + ",\nEmail ID: " + model.EMAILID + ",\nREFER (%): " + model.REFPCNT + ",\nRemarks: " + model.REMARKS + ".");
            aslLog.USERPC = model.USERPC;
            db.AslLogDbSet.Add(aslLog);
        }

        // Edit ALL INFORMATION from PST_REFER TO Asl_lOG Database Table.
        public void Update_Refer_LogData(ReferDTO model)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslLogDbSet where n.COMPID == model.COMPID && n.USERID == model.UPDUSERID select n.LOGSLNO).Max());
            if (maxSerialNo == 0)
            {
                aslLog.LOGSLNO = Convert.ToInt64("1");
            }
            else
            {
                aslLog.LOGSLNO = maxSerialNo + 1;
            }

            aslLog.COMPID = Convert.ToInt64(model.COMPID);
            aslLog.USERID = model.UPDUSERID;
            aslLog.LOGTYPE = "UPDATE";
            aslLog.LOGSLNO = aslLog.LOGSLNO;
            aslLog.LOGDATE = Convert.ToDateTime(date);
            aslLog.LOGTIME = Convert.ToString(time);
            aslLog.LOGIPNO = model.UPDIPNO;
            aslLog.LOGLTUDE = model.UPDLTUDE;
            aslLog.TABLEID = "RX_REFER";
            aslLog.LOGDATA = Convert.ToString("Refer Name: " + model.REFERNM + ",\nAddress: " + model.ADDRESS + ",\nMobile No: " + model.MOBNO1 + ",\nOther Number: " + model.MOBNO2 + ",\nEmail ID: " + model.EMAILID + ",\nREFER (%): " + model.REFPCNT + ",\nRemarks: " + model.REMARKS + ".");
            aslLog.USERPC = model.USERPC;
            db.AslLogDbSet.Add(aslLog);
        }


        // Delete ALL INFORMATION from PST_REFER TO Asl_lOG Database Table.
        public void Delete_Refer_LogData(Refer model)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslLogDbSet where n.COMPID == model.COMPID && n.USERID == model.UPDUSERID select n.LOGSLNO).Max());
            if (maxSerialNo == 0)
            {
                aslLog.LOGSLNO = Convert.ToInt64("1");
            }
            else
            {
                aslLog.LOGSLNO = maxSerialNo + 1;
            }

            aslLog.COMPID = Convert.ToInt64(model.COMPID);
            aslLog.USERID = model.UPDUSERID;
            aslLog.LOGTYPE = "DELETE";
            aslLog.LOGSLNO = aslLog.LOGSLNO;
            aslLog.LOGDATE = Convert.ToDateTime(date);
            aslLog.LOGTIME = Convert.ToString(time);
            aslLog.LOGIPNO = model.UPDIPNO;
            aslLog.LOGLTUDE = model.UPDLTUDE;
            aslLog.TABLEID = "RX_REFER";
            aslLog.LOGDATA = Convert.ToString("Refer Name: " + model.REFERNM + ",\nAddress: " + model.ADDRESS + ",\nMobile No: " + model.MOBNO1 + ",\nOther Number: " + model.MOBNO2 + ",\nEmail ID: " + model.EMAILID + ",\nREFER (%): " + model.REFPCNT + ",\nRemarks: " + model.REMARKS + ".");
            aslLog.USERPC = model.USERPC;
            db.AslLogDbSet.Add(aslLog);
        }


        public ASL_DELETE AslDelete = new ASL_DELETE();

        // Delete ALL INFORMATION from PST_REFER TO ASL_DELETE Database Table.
        public void Delete_Refer_DELETE(Refer model)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslDeleteDbSet where n.COMPID == model.COMPID && n.USERID == model.UPDUSERID select n.DELSLNO).Max());
            if (maxSerialNo == 0)
            {
                AslDelete.DELSLNO = Convert.ToInt64("1");
            }
            else
            {
                AslDelete.DELSLNO = maxSerialNo + 1;
            }

            AslDelete.COMPID = Convert.ToInt64(model.COMPID);
            AslDelete.USERID = model.UPDUSERID;
            AslDelete.DELSLNO = AslDelete.DELSLNO;
            AslDelete.DELDATE = Convert.ToString(date);
            AslDelete.DELTIME = Convert.ToString(time);
            AslDelete.DELIPNO = model.UPDIPNO;
            AslDelete.DELLTUDE = model.UPDLTUDE;
            AslDelete.TABLEID = "RX_REFER";
            AslDelete.DELDATA = Convert.ToString("Refer Name: " + model.REFERNM + ",\nAddress: " + model.ADDRESS + ",\nMobile No: " + model.MOBNO1 + ",\nOther Number: " + model.MOBNO2 + ",\nEmail ID: " + model.EMAILID + ",\nREFER (%): " + model.REFPCNT + ",\nRemarks: " + model.REMARKS + ".");
            AslDelete.USERPC = model.USERPC;
            db.AslDeleteDbSet.Add(AslDelete);
        }





        //Get 
        public ActionResult Create()
        {
            return View();
        }



        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ReferDTO model)
        {
            if (ModelState.IsValid)
            {
                model.USERPC = strHostName;
                model.INSIPNO = ipAddress.ToString();
                model.INSTIME = Convert.ToDateTime(td);
                model.INSUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);

                Int64 maxData=0;
                try
                {
                     maxData = Convert.ToInt64((from n in db.RxReferDbSet where model.COMPID == n.COMPID select n.REFERID).Max());
                }
                catch
                {
                    maxData = 0;
                }
                
                Int64 R = Convert.ToInt64(model.COMPID + "204" + "9999");

                if (maxData == 0)
                {
                    model.REFERID = Convert.ToInt64(model.COMPID + "204" + "0001");
                  
                }
                else if (maxData <= R)
                {
                    model.REFERID = maxData + 1;
                   
                }
                else
                {
                    TempData["ReferCreationMessage"] = "Not possible entry! ";
                    return RedirectToAction("Create");
                }


                Refer refer =new Refer();
                refer.COMPID = model.COMPID;
                refer.REFERID = model.REFERID;
                refer.REFERNM = model.REFERNM;
                refer.ADDRESS = model.ADDRESS;
                refer.MOBNO1 = model.MOBNO1;
                refer.MOBNO2 = model.MOBNO2;
                refer.EMAILID = model.EMAILID;
                refer.REFPCNT = model.REFPCNT;
                refer.REMARKS = model.REMARKS;

                refer.USERPC = model.USERPC;
                refer.INSUSERID = model.INSUSERID;
                refer.INSIPNO = model.INSIPNO;
                refer.INSLTUDE = model.INSLTUDE;
                refer.INSTIME = model.INSTIME;

                Insert_Refer_LogData(model);
                db.RxReferDbSet.Add(refer);
                db.SaveChanges();


                //var searchGL_AcChart =
                //    (from m in db.GlAcchartDbSet
                //        where m.COMPID == model.COMPID && m.HEADTP == 2 && m.ACCOUNTCD == model.REFERID
                //        select m).ToList();
                //if (searchGL_AcChart.Count == 0)
                //{
                //    GL_ACCHART glAcchart =new GL_ACCHART();
                //    glAcchart.COMPID = model.COMPID;
                //    glAcchart.HEADTP = 2;
                //    glAcchart.HEADCD = Convert.ToInt64(model.COMPID + "204");
                //    glAcchart.ACCOUNTCD = model.REFERID;
                //    glAcchart.ACCOUNTNM = model.REFERNM;
                //    glAcchart.USERPC = strHostName;
                //    glAcchart.INSIPNO = ipAddress.ToString();
                //    glAcchart.INSTIME = Convert.ToDateTime(td);
                //    glAcchart.INSLTUDE = model.INSLTUDE;
                //    glAcchart.INSUSERID = model.INSUSERID;
                    
                //    db.GlAcchartDbSet.Add(glAcchart);
                //    db.SaveChanges();
                //}



                TempData["ReferCreationMessage"] = "Refer Name: '" + model.REFERNM + "' successfully registered! ";
                return RedirectToAction("Create");
            }
            return RedirectToAction("Create");
        }








        //Get
        public ActionResult Update()
        {
            return View();
        }


        //post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(ReferDTO model)
        {
            if (model.ID != null && model.COMPID != null && model.ADDRESS != null &&
                model.MOBNO1 != null && model.REFERNM!=null && model.REFERID!=null)
            {
                model.USERPC = strHostName;
                model.UPDIPNO = ipAddress.ToString();
                model.UPDTIME = Convert.ToDateTime(td);
                model.UPDUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);

                Update_Refer_LogData(model);
                db.SaveChanges();

                Refer refer = new Refer();
                refer.ID = model.ID;
                refer.COMPID = model.COMPID;
                refer.REFERID = model.REFERID;
                refer.REFERNM = model.REFERNM;
                refer.ADDRESS = model.ADDRESS;
                refer.MOBNO1 = model.MOBNO1;
                refer.MOBNO2 = model.MOBNO2;
                refer.EMAILID = model.EMAILID;
                refer.REFPCNT = model.REFPCNT;
                refer.REMARKS = model.REMARKS;

                refer.USERPC = model.USERPC;
                refer.INSUSERID = model.INSUSERID;
                refer.INSIPNO = model.INSIPNO;
                refer.INSLTUDE = model.INSLTUDE;
                refer.INSTIME = model.INSTIME;
                refer.UPDIPNO = model.UPDIPNO;
                refer.UPDTIME = model.UPDTIME;
                refer.UPDUSERID = model.UPDUSERID;
                refer.UPDLTUDE = model.UPDLTUDE;

                db.Entry(refer).State = EntityState.Modified;
                db.SaveChanges();

                ////Gl_AcChart field Update
                //var searchGL_AcChart =
                //  (from m in db.GlAcchartDbSet
                //   where m.COMPID == model.COMPID && m.HEADTP == 2 && m.ACCOUNTCD == model.REFERID
                //   select m).ToList();
                //if (searchGL_AcChart.Count != 0)
                //{
                //    foreach (GL_ACCHART glAcchart in searchGL_AcChart)
                //    {
                //        glAcchart.ACCOUNTNM = model.REFERNM;
                //        glAcchart.USERPC = strHostName;
                //        glAcchart.UPDIPNO = ipAddress.ToString();
                //        glAcchart.UPDTIME = Convert.ToDateTime(td);
                //        glAcchart.UPDLTUDE = model.UPDLTUDE;
                //        glAcchart.UPDUSERID = model.UPDUSERID;
                        
                //    }
                //    db.SaveChanges();
                //}

                TempData["ReferUpdateMessage"] = "Refer Name: '" + model.REFERNM + "' successfully updated!";
                return RedirectToAction("Update");
            }
            else
            {
                TempData["selectReferName"] = "Please select refer name first !";
                return RedirectToAction("Update");
            }
        }



        //AutoComplete 
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetReferInformation(Int64 changedDropDown)
        {
            var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
            String refernm = "", address = "", mobileNo1 = "", mobileNo2 = "", emailId = "", remarks = "", inserttime = "", insertIpno = "", insltude = "", refpcnt="";
            Int64 Pst_Refer_Id = 0, companyID = 0, insertUserId = 0;
         

            var rt = db.RxReferDbSet.Where(n => n.REFERID == changedDropDown &&
                                                         n.COMPID == compid).Select(n => new
                                                         {
                                                             n.ID,
                                                             n.COMPID,
                                                             n.REFERNM,
                                                             n.ADDRESS,
                                                             n.MOBNO1,
                                                             n.MOBNO2,
                                                             n.EMAILID,
                                                             n.REFPCNT,
                                                             n.REMARKS,
                                                             insuserid = n.INSUSERID,
                                                             instime = n.INSTIME,
                                                             insipno = n.INSIPNO,
                                                             insltude = n.INSLTUDE

                                                         });
            foreach (var n in rt)
            {
                Pst_Refer_Id = n.ID;
                companyID = Convert.ToInt64(n.COMPID);
                refernm = n.REFERNM;
                address = n.ADDRESS;
                mobileNo1 = n.MOBNO1;
                mobileNo2 = n.MOBNO2;
                emailId = n.EMAILID;
                refpcnt = Convert.ToString(n.REFPCNT);
                remarks = n.REMARKS;

                insertUserId = n.insuserid;
                inserttime = Convert.ToString(n.instime);
                insertIpno = Convert.ToString(n.insipno);
                insltude = Convert.ToString(n.insltude);
            }

            var result = new
            {
                pst_Refer_Id = Pst_Refer_Id,
                COMPID = companyID,
                REFERNM = refernm,
                ADDRESS = address,
                MOBNO1 = mobileNo1,
                MOBNO2 = mobileNo2,
                EMAILID = emailId,
                REFPCNT = refpcnt,
                REMARKS = remarks,
                INSUSERID = insertUserId,
                INSTIME = inserttime,
                INSIPNO = insertIpno,
                INSLTUDE = insltude
            };
            return Json(result, JsonRequestBehavior.AllowGet);

        }













        //Get
        public ActionResult Delete()
        {
            return View();
        }



        //post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ReferDTO model)
        {
            if (ModelState.IsValid)
            {
                Refer deleteModel = new Refer();
                deleteModel.ID = model.ID;
                deleteModel.COMPID = model.COMPID;
                deleteModel.REFERID = model.REFERID;

                deleteModel = db.RxReferDbSet.Find(deleteModel.ID,deleteModel.COMPID,deleteModel.REFERID);

                deleteModel.USERPC = strHostName;
                deleteModel.UPDIPNO = ipAddress.ToString();
                deleteModel.UPDTIME = Convert.ToDateTime(td);
                deleteModel.UPDUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                deleteModel.UPDLTUDE = model.UPDLTUDE;

                Delete_Refer_DELETE(deleteModel);
                Delete_Refer_LogData(deleteModel);

                db.RxReferDbSet.Remove(deleteModel);
                db.SaveChanges();


                ////Gl_AcChart field Update
                //var searchGL_AcChart =
                //  (from m in db.GlAcchartDbSet
                //   where m.COMPID == model.COMPID && m.HEADTP == 2 && m.ACCOUNTCD == model.REFERID
                //   select m).ToList();
                //if (searchGL_AcChart.Count != 0)
                //{
                //    foreach (GL_ACCHART remove in searchGL_AcChart)
                //    {
                //        db.GlAcchartDbSet.Remove(remove);

                //    }
                //    db.SaveChanges();
                //}

                TempData["ReferDeleteMessage"] = "Refer Name: '" + model.REFERNM + "' successfully deleted!";
                return RedirectToAction("Delete");
            }
            return RedirectToAction("Delete");
        }





        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
	}
}