using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AslPrescriptionApi.Models.ASRX
{
    [Table("RX_PRESCRIBE")]
    public class Prescribe
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 ID { get; set; }

        [Key, Column(Order = 1)]
        public Int64 COMPID { get; set; }

        [Key, Column(Order = 2)]
        [StringLength(9, MinimumLength = 9)]
        public string TRANSNO { get; set; }

        [Key, Column(Order = 3)]
        [StringLength(11, MinimumLength = 11)]
        public string TRANSSL { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime TRANSDT { get; set; }
        public Int64 RXPID { get; set; }




        [StringLength(3, MinimumLength = 3)]
        public string PARTSL { get; set; }

        [StringLength(6, MinimumLength = 6)]
        public string ENTRYTP { get; set; }

        public string TABLEID { get; set; }
        public Int64? GCATID { get; set; }
        public Int64? GHEADID { get; set; }



        
        public string RESULT { get; set; }
        public Int64? DOSEID { get; set; }

        [StringLength(5, MinimumLength = 3)]
        public string DDMMTP { get; set; }
        public Int64? DDMMQTY { get; set; }

        



        public string USERPC { get; set; }
        public Int64 INSUSERID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? INSTIME { get; set; }
        public string INSIPNO { get; set; }
        public string INSLTUDE { get; set; }
        public Int64 UPDUSERID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? UPDTIME { get; set; }
        public string UPDIPNO { get; set; }
        public string UPDLTUDE { get; set; }
    }
}