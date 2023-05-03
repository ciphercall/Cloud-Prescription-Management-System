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
    public class ApiMediMasterController : ApiController
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

        public ApiMediMasterController()
        {
            strHostName = System.Net.Dns.GetHostName();
            ipHostInfo = Dns.Resolve(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[0];
            td = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
        }




        [System.Web.Http.Route("api/ApiMediMasterController/GetMediMasterInfo")]
        [System.Web.Http.HttpGet]
        public IEnumerable<MediMstDTO> GetCategoryData(string Compid)
        {
            Int64 compid = Convert.ToInt64(Compid);
            var find_GridData = (from t1 in db.RxMediMstDbSet
                                 where t1.COMPID == compid
                                 select new
                                 {
                                     t1.ID,
                                     t1.COMPID,
                                     t1.MCATID,
                                     t1.MCATNM,

                                     t1.INSIPNO,
                                     t1.INSLTUDE,
                                     t1.INSTIME,
                                     t1.INSUSERID,
                                 }).ToList();

            if (find_GridData.Count == 0)
            {
                yield return new MediMstDTO
                {
                    count=1, //no data found
                };
            }
            else
            {
                foreach (var s in find_GridData)
                {
                    yield return new MediMstDTO
                    {
                        ID = s.ID,
                        COMPID = Convert.ToInt64(s.COMPID),
                        MCATID = Convert.ToInt64(s.MCATID),
                        MCATNM = s.MCATNM,
                        INSUSERID = s.INSUSERID,
                        INSLTUDE = s.INSLTUDE,
                        INSTIME = s.INSTIME,
                        INSIPNO = s.INSIPNO,
                    };
                }
            }
        }









        [Route("api/ApiMediMasterController/grid/addData")]
        [HttpPost]
        public HttpResponseMessage AddData(MediMstDTO model)
        {
            MediMst mediMst = new MediMst();

            var check_data = (from n in db.RxMediMstDbSet where n.COMPID == model.COMPID && n.MCATNM == model.MCATNM select n).ToList();
            if (check_data.Count == 0)
            {
                var find_data = (from n in db.RxMediMstDbSet where n.COMPID == model.COMPID select n.MCATID).ToList();
                if (find_data.Count == 0)
                {
                    mediMst.MCATID = Convert.ToInt64(Convert.ToString(model.COMPID) + "01");
                }
                else
                {
                    Int64 find_Max_MCATID = Convert.ToInt64((from n in db.RxMediMstDbSet where n.COMPID == model.COMPID select n.MCATID).Max());
                    mediMst.MCATID = find_Max_MCATID + 1;
                }

                mediMst.COMPID = model.COMPID;
                mediMst.MCATNM = model.MCATNM;
                mediMst.USERPC = strHostName;
                mediMst.INSIPNO = ipAddress.ToString();
                mediMst.INSTIME = Convert.ToDateTime(td);
                mediMst.INSUSERID = model.INSUSERID;
                mediMst.INSLTUDE = Convert.ToString(model.INSLTUDE);

                db.RxMediMstDbSet.Add(mediMst);
                db.SaveChanges();

                model.ID = mediMst.ID;
                model.COMPID = mediMst.COMPID;
                model.MCATID = Convert.ToInt64(mediMst.MCATID);
                model.MCATNM = mediMst.MCATNM;
                model.USERPC = mediMst.USERPC;
                model.INSIPNO = mediMst.INSIPNO;
                model.INSTIME = mediMst.INSTIME;
                model.INSUSERID = mediMst.INSUSERID;
                model.INSLTUDE = Convert.ToString(mediMst.INSLTUDE);

                //Log data save from MediMst Tabel
                MediController mediController = new MediController();
                mediController.Insert_mediMaster_LogData(model);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                return response;
            }
            else
            {
                model.MCATID = 0;
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                return response;
            }
        }






        [Route("api/ApiMediMasterController/grid/UpdateData")]
        [HttpPost]
        public HttpResponseMessage UpdateData(MediMstDTO model)
        {
            var check_data = (from n in db.RxMediMstDbSet where n.COMPID == model.COMPID && n.MCATNM == model.MCATNM  select n).ToList();
            if (check_data.Count == 0)
            {
                var data_find = (from n in db.RxMediMstDbSet where n.ID == model.ID && n.COMPID == model.COMPID && n.MCATID == model.MCATID select n).ToList();

                foreach (var item in data_find)
                {
                    item.ID = model.ID;
                    item.COMPID = model.COMPID;
                    item.MCATID = Convert.ToInt64(model.MCATID);
                    item.MCATNM = model.MCATNM;

                    item.USERPC = strHostName;
                    item.UPDIPNO = ipAddress.ToString();
                    item.UPDLTUDE = Convert.ToString(model.INSLTUDE);
                    item.UPDTIME = Convert.ToDateTime(td);
                    item.UPDUSERID = Convert.ToInt16(model.INSUSERID);

                }
                db.SaveChanges();

                //Log data save from MediMst Tabel
                MediController mediController = new MediController();
                mediController.update_mediMaster_LogData(model);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                return response;
            }
            else
            {
                model.MCATID = 0;
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                return response;
            }
        }










        [Route("api/ApiMediMasterController/grid/DeleteData")]
        [HttpPost]
        // DELETE api/<controller>/5
        public HttpResponseMessage DeleteData(MediMstDTO model)
        {
            MediMst deleteModel = new MediMst();
            deleteModel.ID = model.ID;
            deleteModel.COMPID = model.COMPID;
            deleteModel.MCATID = Convert.ToInt64(model.MCATID);

            var findChildData = (from n in db.RxMediCareDbSet
                                 where
                                     n.MCATID == deleteModel.MCATID && n.COMPID == deleteModel.COMPID
                                 select n).ToList();

            MediMstDTO mediMstObj = new MediMstDTO();

            if (findChildData.Count != 0)
            {
                mediMstObj.GetChildDataForDeleteMasterCategory = 1; // True (child data found)
            }
            else
            {
                deleteModel = db.RxMediMstDbSet.Find(deleteModel.ID, deleteModel.COMPID, deleteModel.MCATID);
                db.RxMediMstDbSet.Remove(deleteModel);
                db.SaveChanges();

                //Log data save from MediMst Tabel
                MediController mediController = new MediController();
                mediController.Delete_mediMaster_LogData(model);
                mediController.Delete_mediMaster_LogDelete(model);
            }            
            return Request.CreateResponse(HttpStatusCode.OK, mediMstObj);
        }
    }
}
