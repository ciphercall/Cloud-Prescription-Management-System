using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AslPrescriptionApi.Models.ASRX
{
    [Table("RX_PATIENT")]
    public class Patient
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 ID { get; set; }

        [Key, Column(Order = 1)]
        public Int64 COMPID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime TRANSDT { get; set; }

        [Key, Column(Order = 2)]
        public Int64 TRANSYY { get; set; }
        public Int64 RXPIDM { get; set; }

        [Key, Column(Order = 3)]
        public Int64 RXPID { get; set; }
        public string RXPNM { get; set; }
        public string ADDRESS { get; set; }

        [StringLength(1, MinimumLength = 1)]
        public string GENDER { get; set; }
        public Int64 AGE { get; set; }
        public string MOBNO1 { get; set; }
        public string MOBNO2 { get; set; }
        public Int64 REFERID { get; set; }
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