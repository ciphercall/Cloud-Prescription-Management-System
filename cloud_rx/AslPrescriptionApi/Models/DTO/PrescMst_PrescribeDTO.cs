using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AslPrescriptionApi.Models.DTO
{
    public class PrescMst_PrescribeDTO
    {
        public Int64 ID { get; set; }


        //PrescMst
        public Int64 COMPID { get; set; }
        public string TRANSNO { get; set; }
        public string TRANSDT { get; set; } // DateTime
        public string RXPTP { get; set; }
        public Int64 RXPID { get; set; } // Paitent id (hidden patient ID)
        public Int64 RXPIDM { get; set; } // patient IDM (client side show patient id)
        public string RXPNM { get; set; } // Patient Name
        public string NXTDT { get; set; } // DateTime
        public Int64? AMOUNT { get; set; }
        public string REMARKS { get; set; }




        //Prescribe
        public string TRANSSL { get; set; }

        public string PARTSL { get; set; }
        public string ENTRYTP { get; set; }
        public string TABLEID { get; set; }
        public Int64 GCATID { get; set; }
        public string GCATNM { get; set; } //General Category Name
        public Int64 GHEADID { get; set; }
        public string GHEADEN { get; set; } // Category wise Head Name
        public string GHEADEN1 { get; set; } // Category wise Head Name 1st part
        public string GHEADEN2 { get; set; } // Category wise Head Name 2nd part
        public string GHEADEN3 { get; set; } // Category wise Head Name 3rd part
        public string GHEADEN4 { get; set; } // Category wise Head Name 4th part
        public string GHEADEN5 { get; set; } // Category wise Head Name 5th part

       
        //1st part Dignosis grid
        public string GheadenOrResult { get; set; }

        public string RESULT { get; set; }
        public Int64? DOSEID { get; set; }
        public string DoseName { get; set; } // Dose Name

        [StringLength(5, MinimumLength = 5)]
        public string DDMMTP { get; set; }
        public Int64? DDMMQTY { get; set; }









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



        public string Insert { get; set; }
        public string Update { get; set; }
        public string Delete { get; set; }


        public Int64 count { get; set; }

        // For logdata if user update in prescribe page.
        public string UpdateLogData { get; set; }


        //Get data from patient creation page , this property used only show the submit button.
        public string ShowSubmitButton { get; set; }
    }
}