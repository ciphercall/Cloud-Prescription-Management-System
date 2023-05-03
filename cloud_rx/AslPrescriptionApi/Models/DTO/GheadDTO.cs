﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AslPrescriptionApi.Models.DTO
{
    public class GheadDTO
    {
        public Int64 ID { get; set; }
        public Int64 COMPID { get; set; }


        [Required(ErrorMessage = "General Category Name can not be empty!")]
        public Int64? GCATID { get; set; }
        public string GCATNM { get; set; }


        public Int64? GHEADID { get; set; }
        public string GHEADEN { get; set; }
        public string GHEADBG { get; set; }
        public string SHOWTP { get; set; }


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
    }
}