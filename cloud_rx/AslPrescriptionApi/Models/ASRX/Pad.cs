using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AslPrescriptionApi.Models.ASRX
{
    [Table("RX_PAD")]
    public class Pad
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 ID { get; set; }

        [Key, Column(Order = 1)]
        public Int64 COMPID { get; set; }



        public string HL1 { get; set; }
        public string HL2 { get; set; }
        public string HL3 { get; set; }
        public string HL4 { get; set; }
        public string HL5 { get; set; }


        public string HM1 { get; set; }
        public string HM2 { get; set; }
        public string HM3 { get; set; }


        public string HR1 { get; set; }
        public string HR2 { get; set; }
        public string HR3 { get; set; }
        public string HR4 { get; set; }
        public string HR5 { get; set; }


        public string FL1 { get; set; }
        public string FL2 { get; set; }
        public string FL3 { get; set; }
        public string FL4 { get; set; }


        public string FM1 { get; set; }
        public string FM2 { get; set; }
        public string FM3 { get; set; }



        public string FR1 { get; set; }
        public string FR2 { get; set; }
        public string FR3 { get; set; }
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