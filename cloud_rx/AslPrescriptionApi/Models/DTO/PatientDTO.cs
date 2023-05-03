using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AslPrescriptionApi.Models.DTO
{
    public class PatientDTO
    {

        public Int64 ID { get; set; }
        public Int64 COMPID { get; set; }
        public DateTime TRANSDT { get; set; }
        public Int64 TRANSYY { get; set; }
        public Int64 RXPIDM { get; set; }
        public Int64 RXPID { get; set; }

        [Required(ErrorMessage = "Patient Name can not be empty!")]
        public string RXPNM { get; set; }

        [Required(ErrorMessage = "Address can not be empty!")]
        public string ADDRESS { get; set; }

        [Required(ErrorMessage = "Gender field required!")]
        public string GENDER { get; set; }

        [Required(ErrorMessage = "Age field required!")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Age must be a number")]
        public Int64 AGE { get; set; }

        [Required(ErrorMessage = "Mobile Number field required!")]
        [RegularExpression(@"^(8{2})([0-9]{11})", ErrorMessage = "Insert a valid phone Number like 8801900000000")]
        public string MOBNO1 { get; set; }

        [RegularExpression(@"^(8{2})([0-9]{11})", ErrorMessage = "Insert a valid phone Number like 8801900000000")]
        public string MOBNO2 { get; set; }

        public Int64? REFERID { get; set; }
        public string REMARKS { get; set; }






        public string USERPC { get; set; }
        public Int64 INSUSERID { get; set; }

        //[Display(Name = "Insert Time")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? INSTIME { get; set; }

        public string INSIPNO { get; set; }
        public string INSLTUDE { get; set; }
        public Int64 UPDUSERID { get; set; }

        //[Display(Name = "Update Time")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? UPDTIME { get; set; }

        public string UPDIPNO { get; set; }
        public string UPDLTUDE { get; set; }
    }
}