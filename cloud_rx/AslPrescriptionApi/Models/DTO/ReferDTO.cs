using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AslPrescriptionApi.Models.DTO
{
    public class ReferDTO
    {
        public Int64 ID { get; set; }
        public Int64 COMPID { get; set; }
        public Int64 REFERID { get; set; }

        [Required(ErrorMessage = "Refer Name can not be empty!")]
        public string REFERNM { get; set; }

        [Required(ErrorMessage = "Address can not be empty!")]
        public string ADDRESS { get; set; }

        [Required(ErrorMessage = "Mobile Number field required!")]
        [RegularExpression(@"^(8{2})([0-9]{11})", ErrorMessage = "Insert a valid phone Number like 8801900000000")]
        public string MOBNO1 { get; set; }

        [RegularExpression(@"^(8{2})([0-9]{11})", ErrorMessage = "Insert a valid phone Number like 8801900000000")]
        public string MOBNO2 { get; set; }

        [Display(Name = "Email Address")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid Email Address")]
        public string EMAILID { get; set; }

        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "Refer (%) must be a Number.")]
        [Range(typeof(Decimal), "0.00", "100000000000000.00", ErrorMessage = "Refer (%) must be a decimal/number between {1} and {2}.")]
        public decimal REFPCNT { get; set; }

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

        // [Display(Name = "Update Time")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? UPDTIME { get; set; }

        public string UPDIPNO { get; set; }
        public string UPDLTUDE { get; set; }
    }
}