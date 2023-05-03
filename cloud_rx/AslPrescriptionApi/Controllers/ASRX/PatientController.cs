using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using AslPrescriptionApi.Models;
using AslPrescriptionApi.Models.ASL;
using AslPrescriptionApi.Models.ASRX;
using AslPrescriptionApi.Models.DTO;

namespace AslPrescriptionApi.Controllers.ASRX
{
    public class PatientController : AppController
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

        public PatientController()
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

        // SAVE ALL INFORMATION from PST_PATIENT TO Asl_lOG Database Table.
        public void Insert_Refer_LogData(PatientDTO model)
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
            aslLog.TABLEID = "RX_PATIENT";

            string referName = "";
            var findReferName = (from m in db.RxReferDbSet where m.COMPID == model.COMPID && m.REFERID == model.REFERID select m);
            foreach (var res in findReferName)
            {
                referName = res.REFERNM.ToString();
            }
            aslLog.LOGDATA = Convert.ToString("Patient ID: " + model.RXPIDM + ",\nPatient Name: " + model.RXPNM + ",\nAddress: " + model.ADDRESS + ",\nGender: " + model.GENDER + ",\nAge: " + model.AGE + ",\nMobile No: " + model.MOBNO1 + ",\nOther Number: " + model.MOBNO2 + ",\nRefer name: " + referName + ",\nRemarks: " + model.REMARKS + ".");
            aslLog.USERPC = model.USERPC;
            db.AslLogDbSet.Add(aslLog);
        }



        // Edit ALL INFORMATION from PST_PATIENT TO Asl_lOG Database Table.
        public void Update_Refer_LogData(PatientDTO model)
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
            aslLog.TABLEID = "RX_PATIENT";

            string referName = "";
            var findReferName = (from m in db.RxReferDbSet where m.COMPID == model.COMPID && m.REFERID == model.REFERID select m);
            foreach (var res in findReferName)
            {
                referName = res.REFERNM.ToString();
            }
            aslLog.LOGDATA = Convert.ToString("Patient ID: " + model.RXPIDM + ",\nPatient Name: " + model.RXPNM + ",\nAddress: " + model.ADDRESS + ",\nGender: " + model.GENDER + ",\nAge: " + model.AGE + ",\nMobile No: " + model.MOBNO1 + ",\nOther Number: " + model.MOBNO2 + ",\nRefer name: " + referName + ",\nRemarks: " + model.REMARKS + ".");
            aslLog.USERPC = model.USERPC;
            db.AslLogDbSet.Add(aslLog);
        }



        // Delete ALL INFORMATION from PST_PATIENT TO Asl_lOG Database Table.
        public void Delete_Refer_LogData(Patient model)
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
            aslLog.TABLEID = "RX_PATIENT";
            string referName = "";
            var findReferName = (from m in db.RxReferDbSet where m.COMPID == model.COMPID && m.REFERID == model.REFERID select m);
            foreach (var res in findReferName)
            {
                referName = res.REFERNM.ToString();
            }
            aslLog.LOGDATA = Convert.ToString("Patient ID: " + model.RXPIDM + ",\nPatient Name: " + model.RXPNM + ",\nAddress: " + model.ADDRESS + ",\nGender: " + model.GENDER + ",\nAge: " + model.AGE + ",\nMobile No: " + model.MOBNO1 + ",\nOther Number: " + model.MOBNO2 + ",\nRefer name: " + referName + ",\nRemarks: " + model.REMARKS + ".");
            aslLog.USERPC = model.USERPC;
            db.AslLogDbSet.Add(aslLog);
        }


        public ASL_DELETE AslDelete = new ASL_DELETE();

        // Delete ALL INFORMATION from PST_PATIENT TO ASL_DELETE Database Table.
        public void Delete_Refer_DELETE(Patient model)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("HH:mm:ss tt"));

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
            AslDelete.TABLEID = "RX_PATIENT";

            string referName = "";
            var findReferName = (from m in db.RxReferDbSet where m.COMPID == model.COMPID && m.REFERID == model.REFERID select m);
            foreach (var res in findReferName)
            {
                referName = res.REFERNM.ToString();
            }
            AslDelete.DELDATA = Convert.ToString("Patient ID: " + model.RXPIDM + ",\nPatient Name: " + model.RXPNM + ",\nAddress: " + model.ADDRESS + ",\nGender: " + model.GENDER + ",\nAge: " + model.AGE + ",\nMobile No: " + model.MOBNO1 + ",\nOther Number: " + model.MOBNO2 + ",\nRefer name: " + referName + ",\nRemarks: " + model.REMARKS + ".");
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
        public ActionResult Create(PatientDTO model, string command)
        {
            if (ModelState.IsValid)
            {
                DateTime patientDate = Convert.ToDateTime(model.USERPC);
                model.TRANSDT = patientDate;

                model.USERPC = strHostName;
                model.INSIPNO = ipAddress.ToString();
                model.INSTIME = Convert.ToDateTime(td);
                model.INSUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);


                string patientYear = Convert.ToString(model.TRANSYY);
                string Year = Convert.ToString(patientYear.Substring(2, 2));
                Int64 R = Convert.ToInt64(Year + "9999");

                var check_Again_SerialID = (from m in db.RxPatientDbSet
                                            where
                                                m.COMPID == model.COMPID && m.TRANSYY == model.TRANSYY && m.RXPID == model.RXPID &&
                                                m.RXPIDM == model.RXPIDM
                                            select m).ToList();

                if (check_Again_SerialID.Count == 0 && model.RXPIDM <= R)
                {

                }
                else if (model.RXPIDM <= R && check_Again_SerialID.Count != 0)
                {
                    model.RXPIDM = model.RXPIDM + 1;
                    model.RXPID = Convert.ToInt64(model.COMPID + "1" + model.RXPIDM);
                }
                else
                {
                    TempData["PatientCreationMessage"] = "Not possible entry! ";
                    return RedirectToAction("Create");
                }


                Patient patient = new Patient();
                patient.COMPID = model.COMPID;
                patient.TRANSDT = model.TRANSDT;
                patient.TRANSYY = model.TRANSYY;
                patient.RXPIDM = model.RXPIDM;
                patient.RXPID = model.RXPID;
                patient.RXPNM = model.RXPNM;
                patient.ADDRESS = model.ADDRESS;
                patient.GENDER = model.GENDER;
                patient.AGE = model.AGE;
                patient.MOBNO1 = model.MOBNO1;
                patient.MOBNO2 = model.MOBNO2;
                if (model.REFERID != null)
                {
                    patient.REFERID = Convert.ToInt64(model.REFERID);
                }
                patient.REMARKS = model.REMARKS;

                patient.USERPC = model.USERPC;
                patient.INSIPNO = model.INSIPNO;
                patient.INSTIME = model.INSTIME;
                patient.INSUSERID = model.INSUSERID;
                patient.INSLTUDE = model.INSLTUDE;

                Insert_Refer_LogData(model);
                db.RxPatientDbSet.Add(patient);
                db.SaveChanges();

                if (command == "Save")
                {
                    TempData["PatientCreationMessage"] = "Patient Name: '" + model.RXPNM + "' successfully registered! ";
                    return RedirectToAction("Create");
                }
                else
                {
                    TempData["PatientCreationMessage"] = model;
                    return RedirectToAction("Index", "Prescribe");
                }
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
        public ActionResult Update(PatientDTO model)
        {
            var result_Find = (from n in db.RxPatientDbSet
                               where model.COMPID == n.COMPID && n.TRANSYY == model.TRANSYY && n.RXPID == model.RXPID
                               select n).Count();

            if (result_Find != 0)
            {
                //db.Entry(model).State = EntityState.Modified;

                model.USERPC = strHostName;
                model.UPDIPNO = ipAddress.ToString();
                model.UPDTIME = Convert.ToDateTime(td);
                model.UPDUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);

                Update_Refer_LogData(model);
                db.SaveChanges();

                Patient patient =new Patient();
                patient.ID = model.ID;
                patient.COMPID = model.COMPID;
                patient.TRANSDT = model.TRANSDT;
                patient.TRANSYY = model.TRANSYY;
                patient.RXPIDM = model.RXPIDM;
                patient.RXPID = model.RXPID;
                patient.RXPNM = model.RXPNM;
                patient.ADDRESS = model.ADDRESS;
                patient.GENDER = model.GENDER;
                patient.AGE = model.AGE;
                patient.MOBNO1 = model.MOBNO1;
                patient.MOBNO2 = model.MOBNO2;
                if (model.REFERID != null)
                {
                    patient.REFERID = Convert.ToInt64(model.REFERID);
                }
                patient.REMARKS = model.REMARKS;

                patient.USERPC = model.USERPC;
                patient.INSIPNO = model.INSIPNO;
                patient.INSTIME = model.INSTIME;
                patient.INSUSERID = model.INSUSERID;
                patient.INSLTUDE = model.INSLTUDE;
                patient.UPDIPNO = model.UPDIPNO;
                patient.UPDTIME = model.UPDTIME;
                patient.UPDUSERID = model.UPDUSERID;
                patient.UPDLTUDE = model.UPDLTUDE;

                db.Entry(patient).State = EntityState.Modified;
                db.SaveChanges();
                
                TempData["PatientUpdateMessage"] = "Patient Name: '" + model.RXPNM + "' successfully updated!";
                return RedirectToAction("Update");
            }
            else
            {
                TempData["selectPatientID"] = "Please select valid Patient ID first !";
                return RedirectToAction("Update");
            }
        }





        //AutoComplete 
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetPatientInformation(Int64 changedtxt)
        {
            var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
            String patientDate = "", patientNm = "", address = "", mobileNo1 = "", mobileNo2 = "", emailId = "", remarks = "", inserttime = "", insertIpno = "", insltude = "", Gender = "", referName = "";
            Int64 Pst_Patient_Id = 0, companyID = 0, insertUserId = 0, ReferID = 0, patientID = 0, age = 0, patientYear = 0;


            var rt = db.RxPatientDbSet.Where(n => n.RXPIDM == changedtxt &&
                                                         n.COMPID == compid).Select(n => new
                                                         {
                                                             n.ID,
                                                             n.COMPID,
                                                             n.RXPID,
                                                             n.TRANSDT,
                                                             n.TRANSYY,
                                                             n.RXPNM,
                                                             n.ADDRESS,
                                                             n.GENDER,
                                                             n.AGE,
                                                             n.MOBNO1,
                                                             n.MOBNO2,
                                                             n.REFERID,
                                                             n.REMARKS,
                                                             insuserid = n.INSUSERID,
                                                             instime = n.INSTIME,
                                                             insipno = n.INSIPNO,
                                                             insltude = n.INSLTUDE

                                                         });
            foreach (var n in rt)
            {
                Pst_Patient_Id = n.ID;
                companyID = Convert.ToInt64(n.COMPID);
                patientID = Convert.ToInt64(n.RXPID);
                patientDate = Convert.ToString(n.TRANSDT);
                patientYear = Convert.ToInt64(n.TRANSYY);
                patientNm = n.RXPNM;
                address = n.ADDRESS;
                Gender = n.GENDER;
                age = n.AGE;
                mobileNo1 = n.MOBNO1;
                mobileNo2 = n.MOBNO2;
                ReferID = Convert.ToInt64(n.REFERID);
                remarks = n.REMARKS;

                insertUserId = n.insuserid;
                inserttime = Convert.ToString(n.instime);
                insertIpno = Convert.ToString(n.insipno);
                insltude = Convert.ToString(n.insltude);
            }


            var find_Refername = db.RxReferDbSet.Where(n => n.REFERID == ReferID &&
                                                                n.COMPID == compid).Select(n => new { n.REFERNM });
            foreach (var m in find_Refername)
            {
                referName = m.REFERNM;
            }

            var result = new
            {
                pst_Patient_Id = Pst_Patient_Id,
                COMPID = companyID,
                PATIENTID = patientID,
                PATIENTDT = patientDate,
                PATIENTYY = patientYear,
                PATIENTNM = patientNm,
                ADDRESS = address,
                GENDER = Gender,
                AGE = age,
                MOBNO1 = mobileNo1,
                MOBNO2 = mobileNo2,
                REFERID = ReferID,
                REFERNM = referName,
                REMARKS = remarks,
                INSUSERID = insertUserId,
                INSTIME = inserttime,
                INSIPNO = insertIpno,
                INSLTUDE = insltude
            };
            return Json(result, JsonRequestBehavior.AllowGet);

        }








        //AutoComplete
        public JsonResult TagSearch(string term)
        {
            List<string> result = new List<string>();
            var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
            var tags = from p in db.RxPatientDbSet
                       where p.COMPID == compid
                       select new { p.RXPIDM };
            foreach (var tag in tags)
            {
                result.Add(tag.RXPIDM.ToString());
            }
            return this.Json(result.Where(t => t.StartsWith(term)), JsonRequestBehavior.AllowGet);
        }




        //AutoComplete 
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult keyword(string changedText)
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

            String patientDate = "", patientNm = "", address = "", mobileNo1 = "", mobileNo2 = "", emailId = "", remarks = "", inserttime = "", insertIpno = "", insltude = "", Gender = "", referName = "";
            Int64 Pst_Patient_Id = 0, companyID = 0, insertUserId = 0, ReferID = 0, patientID = 0, age = 0, patientYear = 0;

            Regex regex = new Regex("^[0-9]+$");//check its int or string?
            if (regex.IsMatch(itemId))
            {
                Int64 pidm = Convert.ToInt64(itemId);
                var findpatientInfo = db.RxPatientDbSet.Where(n => n.RXPIDM == pidm &&
                                                             n.COMPID == compid).Select(n => new
                                                             {
                                                                 n.ID,
                                                                 n.COMPID,
                                                                 n.RXPID,
                                                                 n.TRANSDT,
                                                                 n.TRANSYY,
                                                                 n.RXPNM,
                                                                 n.ADDRESS,
                                                                 n.GENDER,
                                                                 n.AGE,
                                                                 n.MOBNO1,
                                                                 n.MOBNO2,
                                                                 n.REFERID,
                                                                 n.REMARKS,
                                                                 insuserid = n.INSUSERID,
                                                                 instime = n.INSTIME,
                                                                 insipno = n.INSIPNO,
                                                                 insltude = n.INSLTUDE

                                                             });
                foreach (var n in findpatientInfo)
                {
                    Pst_Patient_Id = n.ID;
                    companyID = Convert.ToInt64(n.COMPID);
                    patientID = Convert.ToInt64(n.RXPID);
                    patientDate = Convert.ToString(n.TRANSDT);
                    patientYear = Convert.ToInt64(n.TRANSYY);
                    patientNm = n.RXPNM;
                    address = n.ADDRESS;
                    Gender = n.GENDER;
                    age = n.AGE;
                    mobileNo1 = n.MOBNO1;
                    mobileNo2 = n.MOBNO2;
                    ReferID = Convert.ToInt64(n.REFERID);
                    remarks = n.REMARKS;

                    insertUserId = n.insuserid;
                    inserttime = Convert.ToString(n.instime);
                    insertIpno = Convert.ToString(n.insipno);
                    insltude = Convert.ToString(n.insltude);
                }


                var find_Refername = db.RxReferDbSet.Where(n => n.REFERID == ReferID &&
                                                                    n.COMPID == compid).Select(n => new { n.REFERNM });
                foreach (var m in find_Refername)
                {
                    referName = m.REFERNM;
                }
            }

            var result = new
            {
                PATIENTIDM = itemId,

                pst_Patient_Id = Pst_Patient_Id,
                COMPID = companyID,
                PATIENTID = patientID,
                PATIENTDT = patientDate,
                PATIENTYY = patientYear,
                PATIENTNM = patientNm,
                ADDRESS = address,
                GENDER = Gender,
                AGE = age,
                MOBNO1 = mobileNo1,
                MOBNO2 = mobileNo2,
                REFERID = ReferID,
                REFERNM = referName,
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
        public ActionResult Delete(PatientDTO model)
        {
            var checkPrescriptionData =
                (from m in db.RxPrescMstDbSet where m.COMPID == model.COMPID && m.RXPID == model.RXPID select m).ToList();

            if (checkPrescriptionData.Count != 0)
            {
                TempData["PatientDeleteMessage"] = "Patient ID: '" + model.RXPIDM + ", not deleted. This id already entry in RX page !!!";
                return RedirectToAction("Delete");
            }
            else
            {
                var result_Find = (from n in db.RxPatientDbSet
                                   where model.COMPID == n.COMPID && n.ID == model.ID
                                   select n).Count();
                if (result_Find != 0)
                {
                    Patient deleteModel = new Patient();
                    deleteModel.ID = model.ID;
                    deleteModel.COMPID = model.COMPID;
                    deleteModel.TRANSYY = model.TRANSYY;
                    deleteModel.RXPID = model.RXPID;
                    deleteModel = db.RxPatientDbSet.Find(deleteModel.ID, deleteModel.COMPID, deleteModel.TRANSYY, deleteModel.RXPID);

                    deleteModel.USERPC = strHostName;
                    deleteModel.UPDIPNO = ipAddress.ToString();
                    deleteModel.UPDTIME = Convert.ToDateTime(td);
                    deleteModel.UPDUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                    deleteModel.UPDLTUDE = model.UPDLTUDE;

                    Delete_Refer_DELETE(deleteModel);
                    Delete_Refer_LogData(deleteModel);

                    db.RxPatientDbSet.Remove(deleteModel);
                    db.SaveChanges();
                    TempData["PatientDeleteMessage"] = "Patient Name: '" + model.RXPNM + "' successfully deleted!";
                    return RedirectToAction("Delete");

                }
                else
                {
                    TempData["selectPatientID"] = "Please select valid Patient ID first !";
                    return RedirectToAction("Delete");

                }
            }          
        }





        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}