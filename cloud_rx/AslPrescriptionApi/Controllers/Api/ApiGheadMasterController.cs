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
    public class ApiGheadMasterController : ApiController
    {
        AslPrescriptionApiDbContext db = new AslPrescriptionApiDbContext();

        //Datetime formet
        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);
        TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
        public DateTime td;

        //Get Ip ADDRESS,Time & user PC Name
        public string strHostName;
        public IPHostEntry ipHostInfo;
        public IPAddress ipAddress;

        public ApiGheadMasterController()
        {
            strHostName = System.Net.Dns.GetHostName();
            ipHostInfo = Dns.Resolve(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[0];
            td = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
        }




        [System.Web.Http.Route("api/ApiGheadMasterController/GetGheadMasterInfo")]
        [System.Web.Http.HttpGet]
        public IEnumerable<GheadMstDTO> GetCategoryData(string Compid)
        {
            Int64 compid = Convert.ToInt64(Compid);
            var find_GridData = (from t1 in db.RxGheadMstDbSet
                                 where t1.COMPID == compid
                                 select new
                                 {
                                     t1.ID,
                                     t1.COMPID,
                                     t1.GCATID,
                                     t1.GCATNM,

                                     t1.INSIPNO,
                                     t1.INSLTUDE,
                                     t1.INSTIME,
                                     t1.INSUSERID,
                                 }).ToList();

            if (find_GridData.Count == 0)
            {
                yield return new GheadMstDTO
                {
                    count = 1, //no data found
                };
            }
            else
            {
                foreach (var s in find_GridData)
                {
                    yield return new GheadMstDTO
                    {
                        ID = s.ID,
                        COMPID = Convert.ToInt64(s.COMPID),
                        GCATID = Convert.ToInt64(s.GCATID),
                        GCATNM = s.GCATNM,
                        INSUSERID = s.INSUSERID,
                        INSLTUDE = s.INSLTUDE,
                        INSTIME = s.INSTIME,
                        INSIPNO = s.INSIPNO,
                    };
                }
            }
        }









        [Route("api/ApiGheadMasterController/grid/addData")]
        [HttpPost]
        public HttpResponseMessage AddData(GheadMstDTO model)
        {
            GheadMst gheadMst = new GheadMst();

            var check_data = (from n in db.RxGheadMstDbSet where n.COMPID == model.COMPID && n.GCATNM == model.GCATNM select n).ToList();
            if (check_data.Count == 0)
            {
                var find_data = (from n in db.RxGheadMstDbSet where n.COMPID == model.COMPID select n.GCATID).ToList();
                if (find_data.Count == 0)
                {
                    gheadMst.GCATID = Convert.ToInt64(Convert.ToString(model.COMPID) + "01");
                }
                else
                {
                    Int64 find_Max_GCATID = Convert.ToInt64((from n in db.RxGheadMstDbSet where n.COMPID == model.COMPID select n.GCATID).Max());
                    gheadMst.GCATID = find_Max_GCATID + 1;
                }

                gheadMst.COMPID = model.COMPID;
                gheadMst.GCATNM = model.GCATNM;
                gheadMst.USERPC = strHostName;
                gheadMst.INSIPNO = ipAddress.ToString();
                gheadMst.INSTIME = Convert.ToDateTime(td);
                gheadMst.INSUSERID = model.INSUSERID;
                gheadMst.INSLTUDE = Convert.ToString(model.INSLTUDE);

                db.RxGheadMstDbSet.Add(gheadMst);
                db.SaveChanges();

                model.ID = gheadMst.ID;
                model.COMPID = gheadMst.COMPID;
                model.GCATID = Convert.ToInt64(gheadMst.GCATID);
                model.GCATNM = gheadMst.GCATNM;
                model.USERPC = gheadMst.USERPC;
                model.INSIPNO = gheadMst.INSIPNO;
                model.INSTIME = gheadMst.INSTIME;
                model.INSUSERID = gheadMst.INSUSERID;
                model.INSLTUDE = Convert.ToString(gheadMst.INSLTUDE);

                //Log data save from gheadMst Tabel
                GheadController gheadController = new GheadController();
                gheadController.Insert_GheadMaster_LogData(model);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                return response;
            }
            else
            {
                model.GCATID = 0;
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                return response;
            }
        }






        [Route("api/ApiGheadMasterController/grid/UpdateData")]
        [HttpPost]
        public HttpResponseMessage UpdateData(GheadMstDTO model)
        {
            var check_data = (from n in db.RxGheadMstDbSet where n.COMPID == model.COMPID && n.GCATNM == model.GCATNM select n).ToList();
            if (check_data.Count == 0)
            {
                var data_find = (from n in db.RxGheadMstDbSet where n.ID == model.ID && n.COMPID == model.COMPID && n.GCATID == model.GCATID select n).ToList();

                foreach (var item in data_find)
                {
                    item.ID = model.ID;
                    item.COMPID = model.COMPID;
                    item.GCATID = Convert.ToInt64(model.GCATID);
                    item.GCATNM = model.GCATNM;

                    item.USERPC = strHostName;
                    item.UPDIPNO = ipAddress.ToString();
                    item.UPDLTUDE = Convert.ToString(model.INSLTUDE);
                    item.UPDTIME = Convert.ToDateTime(td);
                    item.UPDUSERID = Convert.ToInt16(model.INSUSERID);

                }
                db.SaveChanges();

                //Log data save from gheadMst Tabel
                GheadController gheadController = new GheadController();
                gheadController.update_GheadMaster_LogData(model);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                return response;
            }
            else
            {
                model.GCATID = 0;
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                return response;
            }
        }










        [Route("api/ApiGheadMasterController/grid/DeleteData")]
        [HttpPost]
        // DELETE api/<controller>/5
        public HttpResponseMessage DeleteData(GheadMstDTO model)
        {
            GheadMst deleteModel = new GheadMst();
            deleteModel.ID = model.ID;
            deleteModel.COMPID = model.COMPID;
            deleteModel.GCATID = Convert.ToInt64(model.GCATID);

            var findChildData = (from n in db.RxGheadDbSet
                                 where
                                     n.GCATID == deleteModel.GCATID && n.COMPID == deleteModel.COMPID
                                 select n).ToList();

            GheadMstDTO gheadMstObj = new GheadMstDTO();

            if (findChildData.Count != 0)
            {
                gheadMstObj.GetChildDataForDeleteMasterCategory = 1; // True (child data found)
            }
            else
            {
                deleteModel = db.RxGheadMstDbSet.Find(deleteModel.ID, deleteModel.COMPID, deleteModel.GCATID);
                db.RxGheadMstDbSet.Remove(deleteModel);
                db.SaveChanges();

                //Log data save from GheadMst Tabel
                GheadController gheadController = new GheadController();
                gheadController.Delete_GheadMaster_LogData(model);
                gheadController.Delete_GheadMaster_LogDelete(model);
            }
            return Request.CreateResponse(HttpStatusCode.OK, gheadMstObj);
        }
    }
}
