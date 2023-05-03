using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AslPrescriptionApi.Models.DTO;

namespace AslPrescriptionApi.Controllers.ASRX
{
    public class GraphViewController : AppController
    {

        public GraphViewController()
        {
            ViewData["HighLight_Menu_DashBoard"] = "High Light DashBoard";
        }



        // GET: GraphView
        public ActionResult Index()
        {
            return View();
        }

       
        public ActionResult IndexPost(Int64 cid, Int64 patientID, Int64 patient_idm, string patientName,string patientType,string prescribeDate, string prescribeMemoNo,string NextDate, string remarks)
        {
            PrescMst_PrescribeDTO model = new PrescMst_PrescribeDTO();
            model.COMPID = cid;
            model.RXPID = patientID;
            model.RXPIDM = patient_idm;
            model.RXPNM =patientName;
            model.RXPTP =patientType;
            model.TRANSDT = Convert.ToString(prescribeDate);
            model.TRANSNO = prescribeMemoNo;
            model.NXTDT = Convert.ToString(NextDate);
            model.REMARKS =remarks;
            TempData["Prescribe_Report_Model"] = model;
            return RedirectToAction("GetPrescribeForm", "Prescribe");
        }
    }
}