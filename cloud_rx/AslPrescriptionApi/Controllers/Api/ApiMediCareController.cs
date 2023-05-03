using System;
using System.Collections.Generic;
using System.Drawing;
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
    public class ApiMediCareController : ApiController
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

        public ApiMediCareController()
        {
            strHostName = System.Net.Dns.GetHostName();
            ipHostInfo = Dns.Resolve(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[0];
            td = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
        }









        [System.Web.Http.Route("api/ApiMediCareController/GetMediCareData")]
        [System.Web.Http.HttpGet]
        public IEnumerable<MediCareDTO> GetMediCareData(string Compid, string MCatid)
        {
            Int64 compid = Convert.ToInt64(Compid);
            Int64 MedicalCategoryId = Convert.ToInt64(MCatid);
            var find_GridData = (from mediCare in db.RxMediCareDbSet 
                                 join pharma in db.RxPharmaDbSet on mediCare.COMPID equals  pharma.COMPID
                                 //join ghead in db.RxGheadDbSet on mediCare.COMPID equals ghead.COMPID
                                 where mediCare.COMPID == compid && mediCare.MCATID == MedicalCategoryId &&
                                 mediCare.PHARMAID == pharma.PHARMAID //&& mediCare.GHEADID==ghead.GHEADID
                                 select new
                                 {
                                     mediCare.ID,
                                     mediCare.COMPID,
                                     mediCare.MCATID,
                                     mediCare.MEDIID,
                                     mediCare.MEDINM,
                                     mediCare.PHARMAID,
                                     mediCare.GHEADID,
                                     pharma.PHARMANM,
                                     //ghead.GHEADEN,

                                     mediCare.INSIPNO,
                                     mediCare.INSLTUDE,
                                     mediCare.INSTIME,
                                     mediCare.INSUSERID,
                                 }).OrderBy(e=>e.ID).ToList();

            if (find_GridData.Count == 0)
            {
                yield return new MediCareDTO
                {
                    count = 1, //no data found
                };
            }
            else
            {
                foreach (var s in find_GridData)
                {
                    String doseName = "";
                    var findDoseName =
                        (from m in db.RxGheadDbSet
                            where m.COMPID == compid && m.GHEADID == s.GHEADID
                            select new {m.GHEADEN}).ToList();
                    if (findDoseName.Count != 0)
                    {
                        foreach (var x in findDoseName)
                        {
                            doseName = x.GHEADEN;
                        }
                    }
                    

                    yield return new MediCareDTO
                    {
                        ID = s.ID,
                        COMPID = Convert.ToInt64(s.COMPID),
                        MCATID = Convert.ToInt64(s.MCATID),
                        MEDIID = Convert.ToInt64(s.MEDIID),
                        MEDINM = s.MEDINM,
                        PHARMAID = Convert.ToInt64(s.PHARMAID),
                        PHARMANM= s.PHARMANM,
                        GHEADID = Convert.ToInt64(s.GHEADID),
                        GHEADEN = doseName,
                        INSUSERID = s.INSUSERID,
                        INSLTUDE = s.INSLTUDE,
                        INSTIME = s.INSTIME,
                        INSIPNO = s.INSIPNO,
                    };
                }
            }
        }









        [Route("api/ApiMediCareController/grid/addData")]
        [HttpPost]
        public HttpResponseMessage AddData(MediCareDTO model)
        {
            MediCare mediCare = new MediCare();

            var check_data = (from n in db.RxMediCareDbSet where n.COMPID == model.COMPID && n.MCATID == model.MCATID && n.MEDINM == model.MEDINM 
                                  && n.PHARMAID == model.PHARMAID && n.GHEADID == model.GHEADID select n).ToList();
            if (check_data.Count == 0)
            {
                var find_data = (from n in db.RxMediCareDbSet where n.COMPID == model.COMPID && n.MCATID == model.MCATID select n).ToList();
                if (find_data.Count == 0)
                {
                    mediCare.MEDIID = Convert.ToInt64(Convert.ToString(model.MCATID) + "001");
                }
                else
                {
                    Int64 find_Max_MEDIID = Convert.ToInt64((from n in db.RxMediCareDbSet where n.COMPID == model.COMPID && n.MCATID == model.MCATID select n.MEDIID).Max());
                    mediCare.MEDIID = find_Max_MEDIID + 1;
                }

                mediCare.COMPID = model.COMPID;
                mediCare.MCATID = Convert.ToInt64(model.MCATID);
                mediCare.MEDINM = model.MEDINM;
                mediCare.PHARMAID = model.PHARMAID;
                mediCare.GHEADID = model.GHEADID;

                mediCare.USERPC = strHostName;
                mediCare.INSIPNO = ipAddress.ToString();
                mediCare.INSTIME = Convert.ToDateTime(td);
                mediCare.INSUSERID = model.INSUSERID;
                mediCare.INSLTUDE = Convert.ToString(model.INSLTUDE);

                db.RxMediCareDbSet.Add(mediCare);
                db.SaveChanges();

                model.ID = mediCare.ID;
                model.COMPID = mediCare.COMPID;
                model.MCATID = Convert.ToInt64(mediCare.MCATID);
                model.MEDIID = Convert.ToInt64(mediCare.MEDIID);
                model.MEDINM = mediCare.MEDINM;
                model.PHARMAID = Convert.ToInt64(mediCare.PHARMAID);
                model.GHEADID = Convert.ToInt64(mediCare.GHEADID);
                model.USERPC = mediCare.USERPC;
                model.INSIPNO = mediCare.INSIPNO;
                model.INSTIME = mediCare.INSTIME;
                model.INSUSERID = mediCare.INSUSERID;
                model.INSLTUDE = Convert.ToString(mediCare.INSLTUDE);

                //Log data save from gheadMst Tabel
                MediController mediController = new MediController();
                mediController.Insert_MediCare_LogData(model);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                return response;
            }
            else
            {
                model.MEDIID = 0;
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                return response;
            }
        }











        [Route("api/ApiMediCareController/grid/UpdateData")]
        [HttpPost]
        public HttpResponseMessage UpdateData(MediCareDTO model)
        {
            var check_data = (from n in db.RxMediCareDbSet
                              where n.COMPID == model.COMPID && n.MCATID == model.MCATID && n.MEDINM == model.MEDINM
                                  && n.PHARMAID == model.PHARMAID && n.GHEADID == model.GHEADID
                              select n).ToList();
            if (check_data.Count == 0)
            {
                var data_find = (from n in db.RxMediCareDbSet where n.ID == model.ID && n.COMPID == model.COMPID && n.MCATID == model.MCATID && n.MEDIID == model.MEDIID select n).ToList();

                foreach (var item in data_find)
                {
                    item.ID = model.ID;
                    item.COMPID = model.COMPID;
                    item.MCATID = Convert.ToInt64(model.MCATID);
                    item.MEDIID = Convert.ToInt64(model.MEDIID);
                    item.MEDINM = model.MEDINM;
                    item.PHARMAID = model.PHARMAID;
                    item.GHEADID = model.GHEADID;

                    item.USERPC = strHostName;
                    item.UPDIPNO = ipAddress.ToString();
                    item.UPDLTUDE = Convert.ToString(model.INSLTUDE);
                    item.UPDTIME = Convert.ToDateTime(td);
                    item.UPDUSERID = Convert.ToInt16(model.INSUSERID);

                }
                db.SaveChanges();

                //Log data save from gheadMst Tabel
                MediController mediController = new MediController();
                mediController.update_MediCare_LogData(model);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                return response;
            }
            else
            {
                model.MEDIID = 0;
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                return response;
            }
        }









        [Route("api/ApiMediCareController/grid/DeleteData")]
        [HttpPost]
        // DELETE api/<controller>/5
        public HttpResponseMessage DeleteData(MediCareDTO model)
        {
            MediCare deleteModel = new MediCare();
            deleteModel.ID = model.ID;
            deleteModel.COMPID = model.COMPID;
            deleteModel.MCATID = Convert.ToInt64(model.MCATID);
            deleteModel.MEDIID = Convert.ToInt64(model.MEDIID);

            deleteModel = db.RxMediCareDbSet.Find(deleteModel.ID, deleteModel.COMPID, deleteModel.MCATID, deleteModel.MEDIID);
            db.RxMediCareDbSet.Remove(deleteModel);
            db.SaveChanges();

            //Log data save from GheadMst Tabel
            MediController mediController = new MediController();
            mediController.Delete_mediCare_LogData(model);
            mediController.Delete_mediCare_LogDelete(model);


            MediCareDTO mediCareObj = new MediCareDTO();
            return Request.CreateResponse(HttpStatusCode.OK, mediCareObj);
        }
    }
}
