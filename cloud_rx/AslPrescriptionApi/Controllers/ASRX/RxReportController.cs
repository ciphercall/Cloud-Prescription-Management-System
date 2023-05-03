using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AslPrescriptionApi.Models;
using AslPrescriptionApi.Models.DTO;

namespace AslPrescriptionApi.Controllers.ASRX
{
    public class RxReportController : AppController
    {
        public RxReportController()
        {
            ViewData["HighLight_Menu_BillingReport"] = "Heigh Light Menu";
        }




        //Prescription Report (Date wise patient information)
        public ActionResult PatientInfo()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PatientInfo(ReportModelDTO model)
        {
            TempData["PatientInfo_model"] = model;
            return RedirectToAction("Get_PatientInfo");
        }
        public ActionResult Get_PatientInfo()
        {
            var model = (ReportModelDTO)TempData["PatientInfo_model"];
            return View(model);
        }






        //Prescription Report (Refer information)
        public ActionResult Get_ReferInfo()
        {
            return View();
        }




        //Prescription Report (Pharmacy information)
        public ActionResult Get_PharmacyInfo()
        {
            return View();
        }




        //Prescription Report (Test information)
        public ActionResult Get_TestInfo()
        {
            return View();
        }



        //Prescription Report (General head information)
        public ActionResult Get_GeneralHeadInfo()
        {
            return View();
        }



        //Prescription Report (Medicine information)
        public ActionResult Get_MedicineInfo()
        {
            return View();
        }





      
        //Prescription Report (REFER WISE PATIENT INFORMATION)
        public ActionResult refer_wise_Patient_information()
        {
            return View();
        }

        [HttpPost]
        public ActionResult refer_wise_Patient_information(ReportModelDTO model)
        {
            TempData["Get_refer_wise_Patient_information_model"] = model;
            return RedirectToAction("Get_ReferWisePatientInformation");
        }
        public ActionResult Get_ReferWisePatientInformation()
        {
            var model = (ReportModelDTO)TempData["Get_refer_wise_Patient_information_model"];
            return View(model);
        }







        //Prescription Report (Patient History)
        public ActionResult patient_History()
        {
            return View();
        }

        [HttpPost]
        public ActionResult patient_History(PatientDTO model)
        {
            TempData["patient_History_model"] = model;
            return RedirectToAction("Get_patient_History");
        }
        public ActionResult Get_patient_History()
        {
            var model = (PatientDTO)TempData["patient_History_model"];
            return View(model);
        }





        //Prescription Report (Date wise Prescribe Amount)
        public ActionResult PrescribeAmount()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PrescribeAmount(ReportModelDTO model)
        {
            TempData["PrescribeAmount_model"] = model;
            return RedirectToAction("Get_PrescribeAmount");
        }
        public ActionResult Get_PrescribeAmount()
        {
            var model = (ReportModelDTO)TempData["PrescribeAmount_model"];
            return View(model);
        }
    }
}