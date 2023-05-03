using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Http;
using AslPrescriptionApi.Controllers.ASRX;
using AslPrescriptionApi.Models;
using AslPrescriptionApi.Models.ASL;
using AslPrescriptionApi.Models.ASRX;
using AslPrescriptionApi.Models.DTO;

namespace AslPrescriptionApi.Controllers.Api
{
    public class ApiTestController : ApiController
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

        public ApiTestController()
        {
            strHostName = System.Net.Dns.GetHostName();
            ipHostInfo = Dns.Resolve(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[0];
            td = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
        }







        [Route("api/GetCategoryList_TagSearch")]
        [HttpGet]
        public IEnumerable<TestMstDTO> GetCategoryList(string character, string compid)
        {
            using (var db = new AslPrescriptionApiDbContext())
            {
                //var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
                Int64 companyID = Convert.ToInt64(compid);
                return String.IsNullOrEmpty(character) ?
                    db.RxTestMstDbSet.AsEnumerable().Select(b => new TestMstDTO()).ToList() :
                    db.RxTestMstDbSet.Where(p => p.TCATNM.StartsWith(character) && p.COMPID == companyID).Select(
                          x => new
                          {
                              TCatName = x.TCATNM,
                              TCatId = x.TCATID,
                          })
                .AsEnumerable().Select(a => new TestMstDTO
                {
                    TCATNM = a.TCatName,
                    TCATID = Convert.ToInt64(a.TCatId),
                }).ToList();
            }
        }









        [System.Web.Http.Route("api/CategoryNameChanged")]
        [System.Web.Http.HttpGet]
        public IEnumerable<TestMstDTO> Dynamicautocomplete(string changedText, string compid)
        {
            using (var db = new AslPrescriptionApiDbContext())
            {
                Int64 companyID = Convert.ToInt64(compid);
                String name = "";
                var rt = db.RxTestMstDbSet.Where(n => n.TCATNM.StartsWith(changedText) &&
                                                         n.COMPID == companyID).Select(n => new
                {
                    headname = n.TCATNM

                }).ToList();

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
                            string ss = Convert.ToString(n.headname);
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
                        name = changedText.Substring(0, lenChangedtxt);
                    }

                    else
                    {
                        name = changedText.Substring(0, lenChangedtxt - 1);
                    }
                }
                else
                {
                    name = changedText;
                }

                var findid = (from n in db.RxTestMstDbSet where n.TCATNM == name && n.COMPID == companyID select n).ToList();
                if (findid.Count != 0)
                {
                    return db.RxTestMstDbSet.Where(p => p.TCATNM == name && p.COMPID == companyID).Select(
                        x => new
                        {
                            TCatname = x.TCATNM,
                            TCatId = x.TCATID,
                        }).AsEnumerable().Select(a => new TestMstDTO
                        {
                            TCATNM = a.TCatname,
                            TCATID = Convert.ToInt64(a.TCatId),
                        }).ToList();
                }
                else
                {
                    return db.RxTestMstDbSet.AsEnumerable().Select(a => new TestMstDTO
                    {
                        TCATNM = name,
                        TCATID = 0
                    }).ToList();
                }
            }
        }








        [System.Web.Http.Route("api/ApiTestController/GetDataOrAddCategory")]
        [System.Web.Http.HttpGet]
        public IEnumerable<TestMstDTO> GetData_Or_AddCategory(string Compid, string TCatID, string TCatName, string Insltude, string loggedUserID, string Insert_Permission)
        {
            if (TCatName == null)
            {
                yield return new TestMstDTO
                {
                    TCATNM = null,
                };
            }
            else
            {
                Int64 compid = Convert.ToInt64(Compid);
                Int64 TCategoryID = Convert.ToInt64(TCatID);
                Int64 LoggedUserID = Convert.ToInt64(loggedUserID);

                var find_Data = (from n in db.RxTestMstDbSet where n.COMPID == compid && n.TCATNM == TCatName && n.TCATID == TCategoryID select n).ToList();
                if (find_Data.Count == 0)
                {
                    if (Insert_Permission == "I")
                    {
                        yield return new TestMstDTO
                        {
                            Insert = "I",
                        };
                    }
                    else
                    {
                        Int64 find_Max_TCATID = 0;
                        try
                        {
                            find_Max_TCATID = Convert.ToInt64((from n in db.RxTestMstDbSet where n.COMPID == compid select n.TCATID).Max());
                        }
                        catch
                        {
                            find_Max_TCATID = 0;
                        }

                        if (find_Max_TCATID == 0)
                        {
                            TestMst obj = new TestMst
                            {
                                COMPID = compid,
                                TCATID = Convert.ToInt64(Convert.ToString(compid) + "01"),
                                TCATNM = TCatName,

                                USERPC = strHostName,
                                INSIPNO = ipAddress.ToString(),
                                INSTIME = Convert.ToDateTime(td),
                                INSUSERID = LoggedUserID,
                                INSLTUDE = Convert.ToString(Insltude),
                            };
                            db.RxTestMstDbSet.Add(obj);
                            db.SaveChanges();

                            //Log data save in TestMst Tabel
                            TestController testController = new TestController();
                            testController.Insert_TestMaster_LogData(obj);

                            yield return new TestMstDTO
                            {
                                TCATID = Convert.ToInt64(Convert.ToString(compid) + "01"),
                                TCATNM = TCatName,
                                InsertNewDataShowMessage = "its used for passing the new id creation message",
                            };
                        }
                        else
                        {
                            TestMst obj = new TestMst
                            {
                                COMPID = compid,
                                TCATID = find_Max_TCATID + 1,
                                TCATNM = TCatName,

                                USERPC = strHostName,
                                INSIPNO = ipAddress.ToString(),
                                INSTIME = Convert.ToDateTime(td),
                                INSUSERID = LoggedUserID,
                                INSLTUDE = Convert.ToString(Insltude),
                            };

                            db.RxTestMstDbSet.Add(obj);
                            db.SaveChanges();

                            //Log data save in TestMst Tabel
                            TestController testController = new TestController();
                            testController.Insert_TestMaster_LogData(obj);

                            yield return new TestMstDTO
                            {
                                TCATID = find_Max_TCATID + 1,
                                TCATNM = TCatName,
                                InsertNewDataShowMessage = "its used for passing the new id creation message",
                            };
                        }
                    }               
                }
                else
                {
                    var find_GridData = (from t1 in db.RxTestDbSet
                                         where t1.TCATID == TCategoryID && t1.COMPID == compid
                                         select new
                                         {
                                             t1.ID,
                                             t1.COMPID,
                                             t1.TCATID,
                                             t1.TESTID,
                                             t1.TESTNM,

                                             t1.INSIPNO,
                                             t1.INSLTUDE,
                                             t1.INSTIME,
                                             t1.INSUSERID,
                                         }).ToList();

                    if (find_GridData.Count == 0)
                    {
                        yield return new TestMstDTO
                        {
                            TCATID = TCategoryID,
                            TESTID = 0,
                            TCATNM = TCatName,
                        };
                    }
                    else
                    {
                        foreach (var item in find_GridData)
                        {
                            yield return new TestMstDTO
                            {
                                ID = item.ID,
                                COMPID = Convert.ToInt64(item.COMPID),
                                TCATID = item.TCATID,
                                TCATNM = TCatName,
                                TESTID = item.TESTID,
                                TESTNM = item.TESTNM,

                                INSUSERID = item.INSUSERID,
                                INSLTUDE = item.INSLTUDE,
                                INSTIME = item.INSTIME,
                                INSIPNO = item.INSIPNO,
                            };
                        }
                    }
                }
            }

        }











        [Route("api/ApiTestController/grid/TestChild")]
        [HttpPost]
        public HttpResponseMessage AddChildData(TestMstDTO model)
        {
            if (model.TCATNM == null || model.TCATNM == "")
            {
                model.TESTID = 0;
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                return response;
            }


            var check_data = (from n in db.RxTestDbSet where n.COMPID == model.COMPID && n.TCATID == model.TCATID && n.TESTNM == model.TESTNM select n).ToList();
            if (check_data.Count == 0)
            {
                var find_data = (from n in db.RxTestDbSet where n.COMPID == model.COMPID && n.TCATID == model.TCATID select n).ToList();
                if (find_data.Count == 0)
                {
                    Test testData = new Test();
                    testData.COMPID = model.COMPID;
                    testData.TCATID = Convert.ToInt64(model.TCATID);
                    testData.TESTID = Convert.ToInt64(Convert.ToString(model.TCATID) + "001");
                    testData.TESTNM = model.TESTNM;
                    testData.USERPC = strHostName;
                    testData.INSIPNO = ipAddress.ToString();
                    testData.INSTIME = Convert.ToDateTime(td);
                    testData.INSUSERID = model.INSUSERID;
                    testData.INSLTUDE = Convert.ToString(model.INSLTUDE);

                    db.RxTestDbSet.Add(testData);
                    db.SaveChanges();

                    model.ID = testData.ID;
                    model.COMPID = model.COMPID;
                    model.TCATID = testData.TCATID;
                    model.TESTID = Convert.ToInt64(testData.TESTID);
                    model.TESTNM = testData.TESTNM;
                    model.USERPC = testData.USERPC;
                    model.INSIPNO = testData.INSIPNO;
                    model.INSTIME = testData.INSTIME;
                    model.INSUSERID = testData.INSUSERID;
                    model.INSLTUDE = Convert.ToString(testData.INSLTUDE);
                }
                else
                {
                    Int64 find_Max_TESTID = Convert.ToInt64((from n in db.RxTestDbSet where n.COMPID == model.COMPID && n.TCATID == model.TCATID select n.TESTID).Max());

                    Test testData = new Test();
                    testData.COMPID = model.COMPID;
                    testData.TCATID = Convert.ToInt64(model.TCATID);
                    testData.TESTID = find_Max_TESTID + 1;
                    testData.TESTNM = model.TESTNM;
                    testData.USERPC = strHostName;
                    testData.INSIPNO = ipAddress.ToString();
                    testData.INSTIME = Convert.ToDateTime(td);
                    testData.INSUSERID = model.INSUSERID;
                    testData.INSLTUDE = Convert.ToString(model.INSLTUDE);

                    db.RxTestDbSet.Add(testData);
                    db.SaveChanges();

                    model.ID = testData.ID;
                    model.COMPID = model.COMPID;
                    model.TCATID = testData.TCATID;
                    model.TESTID = Convert.ToInt64(testData.TESTID);
                    model.TESTNM = testData.TESTNM;
                    model.USERPC = testData.USERPC;
                    model.INSIPNO = testData.INSIPNO;
                    model.INSTIME = testData.INSTIME;
                    model.INSUSERID = testData.INSUSERID;
                    model.INSLTUDE = Convert.ToString(testData.INSLTUDE);
                }

                //Log data save in Test Tabel
                TestController testController = new TestController();
                testController.Insert_Test_LogData(model);


                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                return response;
            }
            else
            {
                model.TESTID = 0;
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                return response;
            }
        }











        [Route("api/ApiTestController/grid/UpdateChildData")]
        [HttpPost]
        public HttpResponseMessage UpdateData(TestMstDTO model)
        {
            var check_data = (from n in db.RxTestDbSet where n.COMPID == model.COMPID && n.TCATID == model.TCATID && n.TESTNM == model.TESTNM select n).ToList();
            if (check_data.Count == 0)
            {
                var data_find = (from n in db.RxTestDbSet where n.ID == model.ID && n.COMPID == model.COMPID && n.TCATID == model.TCATID && n.TESTID == model.TESTID select n).ToList();

                foreach (var item in data_find)
                {
                    item.ID = model.ID;
                    item.COMPID = model.COMPID;
                    item.TCATID = Convert.ToInt64(model.TCATID);
                    item.TESTID = Convert.ToInt64(model.TESTID);
                    item.TESTNM = model.TESTNM;

                    item.USERPC = strHostName;
                    item.UPDIPNO = ipAddress.ToString();
                    item.UPDLTUDE = Convert.ToString(model.INSLTUDE);
                    item.UPDTIME = Convert.ToDateTime(td);
                    item.UPDUSERID = Convert.ToInt16(model.INSUSERID);

                }
                db.SaveChanges();

                //Log data save in Test Tabel
                TestController testController = new TestController();
                testController.update_Test_LogData(model);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                return response;
            }
            else
            {
                model.TESTID = 0;
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                return response;
            }
        }











        [Route("api/ApiTestController/grid/DeleteChildData")]
        [HttpPost]
        // DELETE api/<controller>/5
        public HttpResponseMessage DeleteData(TestMstDTO model)
        {
            Test deleteModel = new Test();
            deleteModel.ID = model.ID;
            deleteModel.COMPID = model.COMPID;
            deleteModel.TCATID = Convert.ToInt64(model.TCATID);
            deleteModel.TESTID = Convert.ToInt64(model.TESTID);

            deleteModel = db.RxTestDbSet.Find(deleteModel.ID, deleteModel.COMPID, deleteModel.TCATID, deleteModel.TESTID);
            db.RxTestDbSet.Remove(deleteModel);
            db.SaveChanges();

            //Log data save in Test Tabel
            TestController testController = new TestController();
            testController.Delete_Test_LogData(model);
            testController.Delete_Test_LogDelete(model);

            Test testObj = new Test();
            return Request.CreateResponse(HttpStatusCode.OK, testObj);
        }

    }
}
