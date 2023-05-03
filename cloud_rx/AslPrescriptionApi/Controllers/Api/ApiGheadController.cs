using System;
using System.Collections.Generic;
using System.Data;
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
    public class ApiGheadController : ApiController
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

        public ApiGheadController()
        {
            strHostName = System.Net.Dns.GetHostName();
            ipHostInfo = Dns.Resolve(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[0];
            td = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
        }







        [System.Web.Http.Route("api/ApiGheadController/GetCategoryWiseHeadInfo")]
        [System.Web.Http.HttpGet]
        public IEnumerable<GheadDTO> GetHeadData(string Compid, string GCatid)
        {
            Int64 compid = Convert.ToInt64(Compid);
            Int64 GeneralCategoryId = Convert.ToInt64(GCatid);
            var find_GridData = (from t1 in db.RxGheadDbSet
                                 where t1.COMPID == compid && t1.GCATID == GeneralCategoryId
                                 select new
                                 {
                                     t1.ID,
                                     t1.COMPID,
                                     t1.GCATID,
                                     t1.GHEADID,
                                     t1.GHEADEN,
                                     t1.GHEADBG,
                                     t1.SHOWTP,

                                     t1.INSIPNO,
                                     t1.INSLTUDE,
                                     t1.INSTIME,
                                     t1.INSUSERID,
                                 }).ToList();

            if (find_GridData.Count == 0)
            {
                yield return new GheadDTO
                {
                    count = 1, //no data found
                };
            }
            else
            {
                foreach (var s in find_GridData)
                {
                    string showType = "";
                    if (s.SHOWTP == "EN")
                    {
                        showType = "English";
                    }
                    else if (s.SHOWTP == "BA")
                    {
                        showType = "Bangla";
                    }
                    yield return new GheadDTO
                    {
                        ID = s.ID,
                        COMPID = Convert.ToInt64(s.COMPID),
                        GCATID = Convert.ToInt64(s.GCATID),
                        GHEADID = Convert.ToInt64(s.GHEADID),
                        GHEADEN = s.GHEADEN,
                        GHEADBG = s.GHEADBG,
                        SHOWTP = showType,
                        INSUSERID = s.INSUSERID,
                        INSLTUDE = s.INSLTUDE,
                        INSTIME = s.INSTIME,
                        INSIPNO = s.INSIPNO,
                    };
                }
            }
        }







        [Route("api/ApiGheadController/grid/addData")]
        [HttpPost]
        public HttpResponseMessage AddData(GheadDTO model)
        {
            Ghead ghead = new Ghead();

            var check_data = (from n in db.RxGheadDbSet where n.COMPID == model.COMPID && n.GCATID == model.GCATID && n.GHEADEN == model.GHEADEN select n).ToList();
            if (check_data.Count == 0)
            {
                var find_data = (from n in db.RxGheadDbSet where n.COMPID == model.COMPID && n.GCATID == model.GCATID select n.GCATID).ToList();
                if (find_data.Count == 0)
                {
                    ghead.GHEADID = Convert.ToInt64(Convert.ToString(model.GCATID) + "001");
                }
                else
                {
                    Int64 find_Max_GHEADID = Convert.ToInt64((from n in db.RxGheadDbSet where n.COMPID == model.COMPID && n.GCATID == model.GCATID select n.GHEADID).Max());
                    ghead.GHEADID = find_Max_GHEADID + 1;
                }

                ghead.COMPID = model.COMPID;
                ghead.GCATID = Convert.ToInt64(model.GCATID);
                ghead.GHEADEN = model.GHEADEN;
                ghead.GHEADBG = model.GHEADBG;
                ghead.SHOWTP = model.SHOWTP;

                ghead.USERPC = strHostName;
                ghead.INSIPNO = ipAddress.ToString();
                ghead.INSTIME = Convert.ToDateTime(td);
                ghead.INSUSERID = model.INSUSERID;
                ghead.INSLTUDE = Convert.ToString(model.INSLTUDE);

                db.RxGheadDbSet.Add(ghead);
                db.SaveChanges();

                model.ID = ghead.ID;
                model.COMPID = ghead.COMPID;
                model.GCATID = Convert.ToInt64(ghead.GCATID);
                model.GHEADID = Convert.ToInt64(ghead.GHEADID);
                model.GHEADEN = ghead.GHEADEN;
                model.GHEADBG = ghead.GHEADBG;
                model.SHOWTP = ghead.SHOWTP;
                model.USERPC = ghead.USERPC;
                model.INSIPNO = ghead.INSIPNO;
                model.INSTIME = ghead.INSTIME;
                model.INSUSERID = ghead.INSUSERID;
                model.INSLTUDE = Convert.ToString(ghead.INSLTUDE);

                //Log data save from gheadMst Tabel
                GheadController gheadController = new GheadController();
                gheadController.Insert_Ghead_LogData(model);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                return response;
            }
            else
            {
                model.GHEADID = 0;
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                return response;
            }
        }













        [Route("api/ApiGheadController/grid/UpdateData")]
        [HttpPost]
        public HttpResponseMessage UpdateData(GheadDTO model)
        {
            string showtype = "";
            if (model.SHOWTP == "English")
            {
                showtype = "EN";
            }
            else if (model.SHOWTP == "Bangla")
            {
                showtype = "BA";
            }

            var check_data = (from n in db.RxGheadDbSet where n.COMPID == model.COMPID && n.GCATID == model.GCATID && n.GHEADEN == model.GHEADEN && n.SHOWTP == showtype && n.GHEADBG == model.GHEADBG select n).ToList();
            if (check_data.Count == 0)
            {
                var data_find = (from n in db.RxGheadDbSet where n.ID == model.ID && n.COMPID == model.COMPID && n.GCATID == model.GCATID && n.GHEADID == model.GHEADID select n).ToList();

                foreach (var item in data_find)
                {
                    item.ID = model.ID;
                    item.COMPID = model.COMPID;
                    item.GCATID = Convert.ToInt64(model.GCATID);
                    item.GHEADID = Convert.ToInt64(model.GHEADID);
                    item.GHEADEN = model.GHEADEN;
                    item.GHEADBG = model.GHEADBG;
                    item.SHOWTP = showtype;
                    
                    item.USERPC = strHostName;
                    item.UPDIPNO = ipAddress.ToString();
                    item.UPDLTUDE = Convert.ToString(model.INSLTUDE);
                    item.UPDTIME = Convert.ToDateTime(td);
                    item.UPDUSERID = Convert.ToInt16(model.INSUSERID);

                }
                db.SaveChanges();

                //Log data save from gheadMst Tabel
                GheadController gheadController = new GheadController();
                gheadController.update_Ghead_LogData(model);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                return response;
            }
            else
            {
                model.GHEADID = 0;
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                return response;
            }
        }










        [Route("api/ApiGheadController/grid/DeleteData")]
        [HttpPost]
        // DELETE api/<controller>/5
        public HttpResponseMessage DeleteData(GheadDTO model)
        {
            Ghead deleteModel = new Ghead();
            deleteModel.ID = model.ID;
            deleteModel.COMPID = model.COMPID;
            deleteModel.GCATID = Convert.ToInt64(model.GCATID);
            deleteModel.GHEADID = Convert.ToInt64(model.GHEADID);

            deleteModel = db.RxGheadDbSet.Find(deleteModel.ID, deleteModel.COMPID, deleteModel.GCATID, deleteModel.GHEADID);
            db.RxGheadDbSet.Remove(deleteModel);
            db.SaveChanges();

            //Log data save from GheadMst Tabel
            GheadController gheadController = new GheadController();
            gheadController.Delete_Ghead_LogData(model);
            gheadController.Delete_Ghead_LogDelete(model);


            GheadDTO gheadObj = new GheadDTO();
            return Request.CreateResponse(HttpStatusCode.OK, gheadObj);
        }


    }
}
