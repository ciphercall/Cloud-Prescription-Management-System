using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AslPrescriptionApi.Models.DTO
{
    public class PadDTO
    {
        public Int64 COMPID { get; set; }


        [RegularExpression(@"^(.{0,40}\S)?$", ErrorMessage = "String can be no more than 40 Characters in length including spaces")]
        public string HL1 { get; set; }

        [RegularExpression(@"^(.{0,40}\S)?$", ErrorMessage = "String can be no more than 40 Characters in length including spaces")]
        public string HL2 { get; set; }

        [RegularExpression(@"^(.{0,40}\S)?$", ErrorMessage = "String can be no more than 40 Characters in length including spaces")]
        public string HL3 { get; set; }

        [RegularExpression(@"^(.{0,40}\S)?$", ErrorMessage = "String can be no more than 40 Characters in length including spaces")]
        public string HL4 { get; set; }

        [RegularExpression(@"^(.{0,40}\S)?$", ErrorMessage = "String can be no more than 40 Characters in length including spaces")]
        public string HL5 { get; set; }



        [RegularExpression(@"^(.{0,40}\S)?$", ErrorMessage = "String can be no more than 40 Characters in length including spaces")]
        public string HM1 { get; set; }

        [RegularExpression(@"^(.{0,40}\S)?$", ErrorMessage = "String can be no more than 40 Characters in length including spaces")]
        public string HM2 { get; set; }

        [RegularExpression(@"^(.{0,40}\S)?$", ErrorMessage = "String can be no more than 40 Characters in length including spaces")]
        public string HM3 { get; set; }




        [RegularExpression(@"^(.{0,40}\S)?$", ErrorMessage = "String can be no more than 40 Characters in length including spaces")]
        public string HR1 { get; set; }

        [RegularExpression(@"^(.{0,40}\S)?$", ErrorMessage = "String can be no more than 40 Characters in length including spaces")]
        public string HR2 { get; set; }

        [RegularExpression(@"^(.{0,40}\S)?$", ErrorMessage = "String can be no more than 40 Characters in length including spaces")]
        public string HR3 { get; set; }

        [RegularExpression(@"^(.{0,40}\S)?$", ErrorMessage = "String can be no more than 40 Characters in length including spaces")]
        public string HR4 { get; set; }

        [RegularExpression(@"^(.{0,40}\S)?$", ErrorMessage = "String can be no more than 40 Characters in length including spaces")]
        public string HR5 { get; set; }



        [RegularExpression(@"^(.{0,40}\S)?$", ErrorMessage = "String can be no more than 40 Characters in length including spaces")]
        public string FL1 { get; set; }

        [RegularExpression(@"^(.{0,40}\S)?$", ErrorMessage = "String can be no more than 40 Characters in length including spaces")]
        public string FL2 { get; set; }

        [RegularExpression(@"^(.{0,40}\S)?$", ErrorMessage = "String can be no more than 40 Characters in length including spaces")]
        public string FL3 { get; set; }

        [RegularExpression(@"^(.{0,40}\S)?$", ErrorMessage = "String can be no more than 40 Characters in length including spaces")]
        public string FL4 { get; set; }



        [RegularExpression(@"^(.{0,40}\S)?$", ErrorMessage = "String can be no more than 40 Characters in length including spaces")]
        public string FM1 { get; set; }

        [RegularExpression(@"^(.{0,40}\S)?$", ErrorMessage = "String can be no more than 40 Characters in length including spaces")]
        public string FM2 { get; set; }

        [RegularExpression(@"^(.{0,40}\S)?$", ErrorMessage = "String can be no more than 40 Characters in length including spaces")]
        public string FM3 { get; set; }



        [RegularExpression(@"^(.{0,40}\S)?$", ErrorMessage = "String can be no more than 40 Characters in length including spaces")]
        public string FR1 { get; set; }

        [RegularExpression(@"^(.{0,40}\S)?$", ErrorMessage = "String can be no more than 40 Characters in length including spaces")]
        public string FR2 { get; set; }

        [RegularExpression(@"^(.{0,40}\S)?$", ErrorMessage = "String can be no more than 40 Characters in length including spaces")]
        public string FR3 { get; set; }

        [RegularExpression(@"^(.{0,40}\S)?$", ErrorMessage = "String can be no more than 40 Characters in length including spaces")]
        public string FR4 { get; set; }




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