using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AslPrescriptionApi.Controllers.ASRX;
using AslPrescriptionApi.Models;
using AslPrescriptionApi.Models.ASRX;
using AslPrescriptionApi.Models.DTO;

namespace AslPrescriptionApi.Controllers.Api
{
    public class ApiTestMasterController : ApiController
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

        public ApiTestMasterController()
        {
            strHostName = System.Net.Dns.GetHostName();
            ipHostInfo = Dns.Resolve(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[0];
            td = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
        }





        [System.Web.Http.Route("api/ApiTestMasterController/GetCategory")]
        [System.Web.Http.HttpGet]
        public IEnumerable<TestMstDTO> GetCategoryData(string Compid, string Insltude, string loggedUserID)
        {
            Int64 compid = Convert.ToInt64(Compid);
            Int64 LoggedUserID = Convert.ToInt64(loggedUserID);
            
            var find_GridData = (from t1 in db.RxTestMstDbSet
                                 where t1.COMPID == compid
                                 select new
                                 {
                                     t1.ID,
                                     t1.COMPID,
                                     t1.TCATID,
                                     t1.TCATNM,

                                     t1.INSIPNO,
                                     t1.INSLTUDE,
                                     t1.INSTIME,
                                     t1.INSUSERID,
                                 }).ToList();

            if (find_GridData.Count == 0)
            {
                yield return new TestMstDTO
                {
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
                        TCATNM = item.TCATNM,

                        INSUSERID = item.INSUSERID,
                        INSLTUDE = item.INSLTUDE,
                        INSTIME = item.INSTIME,
                        INSIPNO = item.INSIPNO,
                    };
                }
            }
        }








        [Route("api/ApiTestMasterController/grid/UpdateData")]
        [HttpPost]
        public HttpResponseMessage UpdateData(TestMstDTO model)
        {
            var check_data = (from n in db.RxTestMstDbSet where n.COMPID == model.COMPID && n.TCATNM == model.TCATNM select n).ToList();
            if (check_data.Count == 0)
            {
                var data_find = (from n in db.RxTestMstDbSet where n.ID == model.ID && n.COMPID == model.COMPID && n.TCATID == model.TCATID select n).ToList();

                foreach (var item in data_find)
                {
                    item.ID = model.ID;
                    item.COMPID = model.COMPID;
                    item.TCATID = Convert.ToInt64(model.TCATID);
                    item.TCATNM = model.TCATNM;

                    item.USERPC = strHostName;
                    item.UPDIPNO = ipAddress.ToString();
                    item.UPDLTUDE = Convert.ToString(model.INSLTUDE);
                    item.UPDTIME = Convert.ToDateTime(td);
                    item.UPDUSERID = Convert.ToInt16(model.INSUSERID);

                }
                db.SaveChanges();

                //Log data save in Test Tabel
                TestController testController = new TestController();
                testController.update_TestMaster_LogData(model);

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









        [Route("api/ApiTestMasterController/grid/DeleteData")]
        [HttpPost]
        // DELETE api/<controller>/5
        public HttpResponseMessage DeleteData(TestMstDTO model)
        {
            TestMst deleteModel = new TestMst();
            deleteModel.ID = model.ID;
            deleteModel.COMPID = model.COMPID;
            deleteModel.TCATID = Convert.ToInt64(model.TCATID);

            var findChildData = (from n in db.RxTestDbSet where 
                                     n.TCATID == deleteModel.TCATID && n.COMPID == deleteModel.COMPID select n).ToList();

            TestMstDTO testObj = new TestMstDTO();
            if (findChildData.Count != 0)
            {
                testObj.GetChildDataForDeleteMasterCategory = 1; // True (child data found)
            }
            else
            {
                deleteModel = db.RxTestMstDbSet.Find(deleteModel.ID, deleteModel.COMPID, deleteModel.TCATID);
                db.RxTestMstDbSet.Remove(deleteModel);
                db.SaveChanges();

                //Log data save in Test Tabel
                TestController testController = new TestController();
                testController.Delete_TestMaster_LogData(model);
                testController.Delete_TestMaster_LogDelete(model);       
            }            
            return Request.CreateResponse(HttpStatusCode.OK, testObj);
        }









    }
}
