using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
    public class ApiPharmaController : ApiController
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

        public ApiPharmaController()
        {
            strHostName = System.Net.Dns.GetHostName();
            ipHostInfo = Dns.Resolve(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[0];
            td = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
        }




        [System.Web.Http.Route("api/ApiPharmaController/GetPharma")]
        [System.Web.Http.HttpGet]
        public IEnumerable<PharmaDTO> GetCategoryData(string Compid)
        {
            Int64 compid = Convert.ToInt64(Compid);
            var find_GridData = (from t1 in db.RxPharmaDbSet
                                 where t1.COMPID == compid
                                 select new
                                 {
                                     t1.ID,
                                     t1.COMPID,
                                     t1.PHARMAID,
                                     t1.PHARMANM,
                                     t1.STATUS,

                                     t1.INSIPNO,
                                     t1.INSLTUDE,
                                     t1.INSTIME,
                                     t1.INSUSERID,
                                 }).ToList();

            if (find_GridData.Count == 0)
            {
                yield return new PharmaDTO
                {
                    count = 1, //no data found
                };
            }
            else
            {
                foreach (var s in find_GridData)
                {
                    string status = "";
                    if (s.STATUS == "A")
                    {
                        status = "Active";
                    }
                    else
                    {
                        status = "Inactive";
                    }
                    yield return new PharmaDTO
                    {
                        ID = s.ID,
                        COMPID = Convert.ToInt64(s.COMPID),
                        PHARMAID = s.PHARMAID,
                        PHARMANM = s.PHARMANM,
                        STATUS = status,//s.STATUS,
                        INSUSERID = s.INSUSERID,
                        INSLTUDE = s.INSLTUDE,
                        INSTIME = s.INSTIME,
                        INSIPNO = s.INSIPNO,
                    };
                }
            }
        }











        [Route("api/ApiPharmaController/grid/addData")]
        [HttpPost]
        public HttpResponseMessage AddData(PharmaDTO model)
        {
            Pharma pharma = new Pharma();

            var check_data = (from n in db.RxPharmaDbSet where n.COMPID == model.COMPID && n.PHARMANM == model.PHARMANM select n).ToList();
            if (check_data.Count == 0)
            {
                var find_data = (from n in db.RxPharmaDbSet where n.COMPID == model.COMPID select n.PHARMAID).ToList();
                if (find_data.Count == 0)
                {
                    pharma.PHARMAID = Convert.ToInt64(Convert.ToString(model.COMPID) + "001");
                }
                else
                {
                    Int64 find_Max_PharmaID = Convert.ToInt64((from n in db.RxPharmaDbSet where n.COMPID == model.COMPID select n.PHARMAID).Max());
                    pharma.PHARMAID = find_Max_PharmaID + 1;
                }

                pharma.COMPID = model.COMPID;
                pharma.PHARMANM = model.PHARMANM;
                pharma.STATUS = model.STATUS;
                pharma.USERPC = strHostName;
                pharma.INSIPNO = ipAddress.ToString();
                pharma.INSTIME = Convert.ToDateTime(td);
                pharma.INSUSERID = model.INSUSERID;
                pharma.INSLTUDE = Convert.ToString(model.INSLTUDE);

                db.RxPharmaDbSet.Add(pharma);
                db.SaveChanges();

                model.ID = pharma.ID;
                model.COMPID = pharma.COMPID;
                model.PHARMAID = Convert.ToInt64(pharma.PHARMAID);
                model.PHARMANM = pharma.PHARMANM;
                model.STATUS = pharma.STATUS;
                model.USERPC = pharma.USERPC;
                model.INSIPNO = pharma.INSIPNO;
                model.INSTIME = pharma.INSTIME;
                model.INSUSERID = pharma.INSUSERID;
                model.INSLTUDE = Convert.ToString(pharma.INSLTUDE);

                //Log data save in Test Tabel
                PharmaController pharmaController = new PharmaController();
                pharmaController.Insert_Pharma_LogData(model);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                return response;
            }
            else
            {
                model.PHARMAID = 0;
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                return response;
            }
        }










        [Route("api/ApiPharmaController/grid/UpdateData")]
        [HttpPost]
        public HttpResponseMessage UpdateData(PharmaDTO model)
        {
            string status = model.STATUS;
            if (status == "Active")
            {
                model.STATUS = "A";
            }
            else if (status == "Inactive")
            {
                model.STATUS = "I";
            }

            var check_data = (from n in db.RxPharmaDbSet where n.COMPID == model.COMPID && n.PHARMANM == model.PHARMANM && model.STATUS==n.STATUS select n).ToList();
            if (check_data.Count == 0)
            {
                var data_find = (from n in db.RxPharmaDbSet where n.ID == model.ID && n.COMPID == model.COMPID && n.PHARMAID == model.PHARMAID select n).ToList();

                foreach (var item in data_find)
                {
                    item.ID = model.ID;
                    item.COMPID = model.COMPID;
                    item.PHARMAID = Convert.ToInt64(model.PHARMAID);
                    item.PHARMANM = model.PHARMANM;
                    item.STATUS = model.STATUS;

                    item.USERPC = strHostName;
                    item.UPDIPNO = ipAddress.ToString();
                    item.UPDLTUDE = Convert.ToString(model.INSLTUDE);
                    item.UPDTIME = Convert.ToDateTime(td);
                    item.UPDUSERID = Convert.ToInt16(model.INSUSERID);

                }
                db.SaveChanges();

                //Log data save in Test Tabel
                PharmaController pharmaController = new PharmaController();
                pharmaController.update_Pharma_LogData(model);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                return response;
            }
            else
            {
                model.PHARMAID = 0;
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                return response;
            }
        }







        


        [Route("api/ApiPharmaController/grid/DeleteData")]
        [HttpPost]
        // DELETE api/<controller>/5
        public HttpResponseMessage DeleteData(PharmaDTO model)
        {
            Pharma deleteModel = new Pharma();
            deleteModel.ID = model.ID;
            deleteModel.COMPID = model.COMPID;
            deleteModel.PHARMAID = Convert.ToInt64(model.PHARMAID);
            
            deleteModel = db.RxPharmaDbSet.Find(deleteModel.ID, deleteModel.COMPID, deleteModel.PHARMAID);
            db.RxPharmaDbSet.Remove(deleteModel);
            db.SaveChanges();

            //Log data save from Pharma Table
            PharmaController pharmaController = new PharmaController();
            pharmaController.Delete_pharma_LogData(model);
            pharmaController.Delete_pharma_LogDelete(model);

            PharmaDTO pharmaObj = new PharmaDTO();
            return Request.CreateResponse(HttpStatusCode.OK, pharmaObj);
        }
    }
}
