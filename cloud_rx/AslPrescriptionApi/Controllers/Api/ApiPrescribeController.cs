using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AslPrescriptionApi.Controllers.ASRX;
using AslPrescriptionApi.Models;
using AslPrescriptionApi.Models.ASRX;
using AslPrescriptionApi.Models.DTO;

namespace AslPrescriptionApi.Controllers.Api
{
    public class ApiPrescribeController : ApiController
    {
        AslPrescriptionApiDbContext db = new AslPrescriptionApiDbContext();

        //Datetime formet
        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);
        TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
        public DateTime td;

        //Get Ip ADDRESS,Time & user PC Name
        public string strHostName;
        public IPHostEntry ipHostInfo;
        public IPAddress ipAddress;

        public ApiPrescribeController()
        {
            strHostName = System.Net.Dns.GetHostName();
            ipHostInfo = Dns.Resolve(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[0];
            td = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
        }






        [System.Web.Http.Route("api/ApiPrescribeController/Get1stPart")]
        [System.Web.Http.HttpGet]
        public IEnumerable<PrescMst_PrescribeDTO> Get1StPartData(string Compid, string TrnsNo)
        {
            Int64 compid = Convert.ToInt64(Compid);

            //          --1ST PART QUERY
            //SELECT RES1ST, ID ,COMPID, TRANSNO,TRANSSL,ENTRYTP FROM(
            //SELECT GHEADEN RES1ST,RX_PRESCRIBE.ID, RX_PRESCRIBE.COMPID, TRANSNO,TRANSSL,ENTRYTP FROM RX_PRESCRIBE INNER JOIN RX_GHEAD ON RX_PRESCRIBE.GHEADID = RX_GHEAD.GHEADID
            //WHERE RX_PRESCRIBE.COMPID = 101 AND RX_PRESCRIBE.COMPID = RX_GHEAD.COMPID AND ENTRYTP = 'RECORD' 
            //AND RX_PRESCRIBE.TRANSNO = 220815001 AND PARTSL = '1ST' 
            //UNION
            //SELECT RESULT RES1ST ,ID ,COMPID, TRANSNO,TRANSSL,ENTRYTP FROM RX_PRESCRIBE 
            //WHERE COMPID = 101 AND ENTRYTP = 'MANUAL' AND RX_PRESCRIBE.TRANSNO = 220815001 AND PARTSL = '1ST' 
            //) A order by ID ASC 

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["AslPrescriptionApiDbContext"].ToString());
            string query = string.Format(
                " SELECT RES1ST, ID ,COMPID, TRANSNO,TRANSSL,ENTRYTP FROM( " +
"SELECT GHEADEN RES1ST,RX_PRESCRIBE.ID, RX_PRESCRIBE.COMPID, TRANSNO, TRANSSL, ENTRYTP FROM RX_PRESCRIBE INNER JOIN RX_GHEAD ON RX_PRESCRIBE.GHEADID = RX_GHEAD.GHEADID " +
"WHERE RX_PRESCRIBE.COMPID = '{0}' AND RX_PRESCRIBE.COMPID = RX_GHEAD.COMPID AND ENTRYTP = 'RECORD' " +
"AND RX_PRESCRIBE.TRANSNO = '{1}' AND PARTSL = '1ST' " +
"UNION " +
"SELECT RESULT RES1ST ,ID ,COMPID, TRANSNO, TRANSSL, ENTRYTP FROM RX_PRESCRIBE " +
"WHERE COMPID ='{0}'AND ENTRYTP = 'MANUAL' AND RX_PRESCRIBE.TRANSNO = '{1}' AND PARTSL = '1ST' " +
") A order by ID ASC ",
                compid, TrnsNo);
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, conn);
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable ds = new DataTable();
            da.Fill(ds);


            foreach (DataRow row in ds.Rows)
            {
                string id = row["ID"].ToString();

                if (id == "")
                {
                    yield return new PrescMst_PrescribeDTO
                    {
                        count = 1, //no data found
                    };
                }
                else
                {
                    yield return new PrescMst_PrescribeDTO
                    {
                        ID = Convert.ToInt64(row["ID"].ToString()),
                        COMPID = Convert.ToInt64(row["COMPID"].ToString()),
                        TRANSNO = Convert.ToString(row["TRANSNO"].ToString()),
                        TRANSSL = Convert.ToString(row["TRANSSL"].ToString()),

                        ENTRYTP = row["ENTRYTP"].ToString(),
                        GheadenOrResult = row["RES1ST"].ToString(),
                    };

                }
            }
            conn.Close();
        }




        [System.Web.Http.Route("api/ApiPrescribeController/Get2ndPart")]
        [System.Web.Http.HttpGet]
        public IEnumerable<PrescMst_PrescribeDTO> Get2ndPartData(string Compid, string TrnsNo)
        {
            Int64 compid = Convert.ToInt64(Compid);

            //--2nd PART QUERY
            //SELECT GHEADID,GHEADEN,Result, ID ,COMPID, TRANSNO,TRANSSL,ENTRYTP FROM
            //( SELECT RX_PRESCRIBE.GHEADID, RX_GHEAD.GHEADEN, Result, RX_PRESCRIBE.ID, RX_PRESCRIBE.COMPID, TRANSNO,TRANSSL,ENTRYTP ,PARTSL
            // FROM RX_PRESCRIBE LEFT OUTER JOIN RX_GHEAD ON RX_PRESCRIBE.GHEADID = RX_GHEAD.GHEADID
            // WHERE RX_PRESCRIBE.COMPID = 101 AND RX_PRESCRIBE.TRANSNO = 240815003 AND PARTSL = '2ND' 
            // )A  ORDER BY ID ASC


            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["AslPrescriptionApiDbContext"].ToString());
            string query = string.Format(
                "SELECT GHEADID,GHEADEN,Result, ID ,COMPID, TRANSNO,TRANSSL,ENTRYTP FROM " +
"( SELECT RX_PRESCRIBE.GHEADID, RX_GHEAD.GHEADEN, Result, RX_PRESCRIBE.ID, RX_PRESCRIBE.COMPID, TRANSNO,TRANSSL,ENTRYTP ,PARTSL " +
"FROM RX_PRESCRIBE LEFT OUTER JOIN RX_GHEAD ON RX_PRESCRIBE.GHEADID = RX_GHEAD.GHEADID " +
"WHERE RX_PRESCRIBE.COMPID = '{0}' AND RX_PRESCRIBE.TRANSNO = '{1}' AND PARTSL = '2ND' " +
") A " +
"ORDER BY ID ASC",
                compid, TrnsNo);
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, conn);
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable ds = new DataTable();
            da.Fill(ds);


            foreach (DataRow row in ds.Rows)
            {
                string id = row["ID"].ToString();

                if (id == "")
                {
                    yield return new PrescMst_PrescribeDTO
                    {
                        count = 1, //no data found
                    };
                }
                else
                {
                    yield return new PrescMst_PrescribeDTO
                    {
                        ID = Convert.ToInt64(row["ID"].ToString()),
                        COMPID = Convert.ToInt64(row["COMPID"].ToString()),
                        TRANSNO = Convert.ToString(row["TRANSNO"].ToString()),
                        TRANSSL = Convert.ToString(row["TRANSSL"].ToString()),

                        ENTRYTP = row["ENTRYTP"].ToString(),
                        GHEADEN = row["GHEADEN"].ToString(),
                        RESULT = row["Result"].ToString(),
                    };

                }
            }
            conn.Close();
        }




        [System.Web.Http.Route("api/ApiPrescribeController/Get3rdPart")]
        [System.Web.Http.HttpGet]
        public IEnumerable<PrescMst_PrescribeDTO> Get3rdPartData(string Compid, string TrnsNo)
        {
            Int64 compid = Convert.ToInt64(Compid);


            //          --3RD PART QUERY
            //SELECT RES3RD, ID ,COMPID, TRANSNO,TRANSSL,ENTRYTP FROM(
            //SELECT TESTNM RES3RD,RX_PRESCRIBE.ID, RX_PRESCRIBE.COMPID, TRANSNO,TRANSSL,ENTRYTP FROM 
            //RX_PRESCRIBE INNER JOIN RX_TEST ON RX_PRESCRIBE.GHEADID = RX_TEST.TESTID
            //WHERE RX_PRESCRIBE.COMPID = 101 AND RX_PRESCRIBE.COMPID = RX_TEST.COMPID AND ENTRYTP = 'RECORD' 
            //AND RX_PRESCRIBE.TRANSNO = 250815003 AND PARTSL = '3RD' 
            //UNION
            //SELECT RESULT RES3RD ,ID ,COMPID, TRANSNO,TRANSSL,ENTRYTP FROM RX_PRESCRIBE 
            //WHERE COMPID = 101 AND ENTRYTP = 'MANUAL' AND RX_PRESCRIBE.TRANSNO = 250815003 AND PARTSL = '3RD' 
            //) A order by ID ASC 

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["AslPrescriptionApiDbContext"].ToString());
            string query = string.Format(
                " SELECT RES3RD, ID ,COMPID, TRANSNO,TRANSSL,ENTRYTP FROM( " +
"SELECT TESTNM RES3RD,RX_PRESCRIBE.ID, RX_PRESCRIBE.COMPID, TRANSNO, TRANSSL, ENTRYTP FROM RX_PRESCRIBE INNER JOIN RX_TEST ON RX_PRESCRIBE.GHEADID = RX_TEST.TESTID " +
"WHERE RX_PRESCRIBE.COMPID = '{0}' AND RX_PRESCRIBE.COMPID = RX_TEST.COMPID AND ENTRYTP = 'RECORD' " +
"AND RX_PRESCRIBE.TRANSNO = '{1}' AND PARTSL = '3RD' " +
"UNION " +
"SELECT RESULT RES3RD ,ID ,COMPID, TRANSNO, TRANSSL, ENTRYTP FROM RX_PRESCRIBE " +
"WHERE COMPID ='{0}'AND ENTRYTP = 'MANUAL' AND RX_PRESCRIBE.TRANSNO = '{1}' AND PARTSL = '3RD' " +
") A order by ID ASC ",
                compid, TrnsNo);
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, conn);
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable ds = new DataTable();
            da.Fill(ds);


            foreach (DataRow row in ds.Rows)
            {
                string id = row["ID"].ToString();

                if (id == "")
                {
                    yield return new PrescMst_PrescribeDTO
                    {
                        count = 1, //no data found
                    };
                }
                else
                {
                    yield return new PrescMst_PrescribeDTO
                    {
                        ID = Convert.ToInt64(row["ID"].ToString()),
                        COMPID = Convert.ToInt64(row["COMPID"].ToString()),
                        TRANSNO = Convert.ToString(row["TRANSNO"].ToString()),
                        TRANSSL = Convert.ToString(row["TRANSSL"].ToString()),

                        ENTRYTP = row["ENTRYTP"].ToString(),
                        GheadenOrResult = row["RES3RD"].ToString(),
                    };

                }
            }
            conn.Close();
        }




        [System.Web.Http.Route("api/ApiPrescribeController/Get4thPart")]
        [System.Web.Http.HttpGet]
        public IEnumerable<PrescMst_PrescribeDTO> Get4thPartData(string Compid, string TrnsNo)
        {
            Int64 compid = Convert.ToInt64(Compid);


            //          --4th PART QUERY
            //            SELECT RES4TH,RX_GHEAD.GHEADEN DoseName, A.ID ,A.COMPID, TRANSNO,TRANSSL,ENTRYTP,DDMMTP,DDMMQTY FROM(
            //SELECT MEDINM RES4TH,DOSEID ,RX_PRESCRIBE.ID, RX_PRESCRIBE.COMPID, TRANSNO,TRANSSL,ENTRYTP,DDMMTP,DDMMQTY FROM 
            //RX_PRESCRIBE INNER JOIN RX_MEDICARE ON RX_PRESCRIBE.GHEADID = RX_MEDICARE.MEDIID
            //WHERE RX_PRESCRIBE.COMPID = 101 AND RX_PRESCRIBE.COMPID = RX_MEDICARE.COMPID AND ENTRYTP = 'RECORD' 
            //AND RX_PRESCRIBE.TRANSNO = 260815004 AND PARTSL = '4TH' 
            //UNION
            //SELECT RESULT RES4TH,DOSEID ,ID ,COMPID, TRANSNO,TRANSSL,ENTRYTP,DDMMTP,DDMMQTY FROM RX_PRESCRIBE 
            //WHERE COMPID = 101 AND ENTRYTP = 'MANUAL' AND RX_PRESCRIBE.TRANSNO = 260815004 AND PARTSL = '4TH' 
            //) A LEFT OUTER JOIN RX_GHEAD ON RX_GHEAD.GHEADID = A.DOSEID order by ID ASC

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["AslPrescriptionApiDbContext"].ToString());
            string query = string.Format(
                " SELECT RES4TH,RX_GHEAD.GHEADEN DoseName, A.ID ,A.COMPID, TRANSNO, TRANSSL, ENTRYTP, DDMMTP, DDMMQTY FROM( " +
"SELECT MEDINM RES4TH,DOSEID ,RX_PRESCRIBE.ID, RX_PRESCRIBE.COMPID, TRANSNO,TRANSSL,ENTRYTP,DDMMTP,DDMMQTY FROM " +
"RX_PRESCRIBE INNER JOIN RX_MEDICARE ON RX_PRESCRIBE.GHEADID = RX_MEDICARE.MEDIID " +
"WHERE RX_PRESCRIBE.COMPID = '{0}' AND RX_PRESCRIBE.COMPID = RX_MEDICARE.COMPID AND ENTRYTP = 'RECORD' " +
"AND RX_PRESCRIBE.TRANSNO = '{1}' AND PARTSL = '4TH' " +
"UNION " +
"SELECT RESULT RES4TH,DOSEID ,ID ,COMPID, TRANSNO,TRANSSL,ENTRYTP,DDMMTP,DDMMQTY FROM RX_PRESCRIBE " +
"WHERE COMPID = '{0}' AND ENTRYTP = 'MANUAL' AND RX_PRESCRIBE.TRANSNO = '{1}' AND PARTSL = '4TH' " +
") A  LEFT OUTER JOIN RX_GHEAD ON RX_GHEAD.GHEADID = A.DOSEID order by ID ASC",
                compid, TrnsNo);
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, conn);
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable ds = new DataTable();
            da.Fill(ds);


            foreach (DataRow row in ds.Rows)
            {
                string id = row["ID"].ToString();

                if (id == "")
                {
                    yield return new PrescMst_PrescribeDTO
                    {
                        count = 1, //no data found
                    };
                }
                else
                {
                    yield return new PrescMst_PrescribeDTO
                    {
                        ID = Convert.ToInt64(row["ID"].ToString()),
                        COMPID = Convert.ToInt64(row["COMPID"].ToString()),
                        TRANSNO = Convert.ToString(row["TRANSNO"].ToString()),
                        TRANSSL = Convert.ToString(row["TRANSSL"].ToString()),

                        ENTRYTP = row["ENTRYTP"].ToString(),
                        GheadenOrResult = row["RES4TH"].ToString(),
                        DoseName = row["DoseName"].ToString(),
                        DDMMTP = row["DDMMTP"].ToString(),
                        DDMMQTY = Convert.ToInt64(row["DDMMQTY"].ToString()),
                    };

                }
            }
            conn.Close();
        }




        [System.Web.Http.Route("api/ApiPrescribeController/Get5thPart")]
        [System.Web.Http.HttpGet]
        public IEnumerable<PrescMst_PrescribeDTO> Get5thPartData(string Compid, string TrnsNo)
        {
            Int64 compid = Convert.ToInt64(Compid);


            //          --1ST PART QUERY
            //SELECT RES5TH, ID ,COMPID, TRANSNO,TRANSSL,ENTRYTP FROM(
            //SELECT GHEADEN RES5TH,RX_PRESCRIBE.ID, RX_PRESCRIBE.COMPID, TRANSNO,TRANSSL,ENTRYTP FROM RX_PRESCRIBE INNER JOIN RX_GHEAD ON RX_PRESCRIBE.GHEADID = RX_GHEAD.GHEADID
            //WHERE RX_PRESCRIBE.COMPID = 101 AND RX_PRESCRIBE.COMPID = RX_GHEAD.COMPID AND ENTRYTP = 'RECORD' 
            //AND RX_PRESCRIBE.TRANSNO = 220815001 AND PARTSL = '5TH' 
            //UNION
            //SELECT RESULT RES5TH ,ID ,COMPID, TRANSNO,TRANSSL,ENTRYTP FROM RX_PRESCRIBE 
            //WHERE COMPID = 101 AND ENTRYTP = 'MANUAL' AND RX_PRESCRIBE.TRANSNO = 220815001 AND PARTSL = '5TH' 
            //) A order by ID ASC 

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["AslPrescriptionApiDbContext"].ToString());
            string query = string.Format(
                " SELECT RES5TH, ID ,COMPID, TRANSNO,TRANSSL,ENTRYTP FROM( " +
"SELECT GHEADEN RES5TH,RX_PRESCRIBE.ID, RX_PRESCRIBE.COMPID, TRANSNO, TRANSSL, ENTRYTP FROM RX_PRESCRIBE INNER JOIN RX_GHEAD ON RX_PRESCRIBE.GHEADID = RX_GHEAD.GHEADID " +
"WHERE RX_PRESCRIBE.COMPID = '{0}' AND RX_PRESCRIBE.COMPID = RX_GHEAD.COMPID AND ENTRYTP = 'RECORD' " +
"AND RX_PRESCRIBE.TRANSNO = '{1}' AND PARTSL = '5TH' " +
"UNION " +
"SELECT RESULT RES5TH ,ID ,COMPID, TRANSNO, TRANSSL, ENTRYTP FROM RX_PRESCRIBE " +
"WHERE COMPID ='{0}'AND ENTRYTP = 'MANUAL' AND RX_PRESCRIBE.TRANSNO = '{1}' AND PARTSL = '5TH' " +
") A order by ID ASC ",
                compid, TrnsNo);
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, conn);
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable ds = new DataTable();
            da.Fill(ds);


            foreach (DataRow row in ds.Rows)
            {
                string id = row["ID"].ToString();

                if (id == "")
                {
                    yield return new PrescMst_PrescribeDTO
                    {
                        count = 1, //no data found
                    };
                }
                else
                {
                    yield return new PrescMst_PrescribeDTO
                    {
                        ID = Convert.ToInt64(row["ID"].ToString()),
                        COMPID = Convert.ToInt64(row["COMPID"].ToString()),
                        TRANSNO = Convert.ToString(row["TRANSNO"].ToString()),
                        TRANSSL = Convert.ToString(row["TRANSSL"].ToString()),

                        ENTRYTP = row["ENTRYTP"].ToString(),
                        GheadenOrResult = row["RES5TH"].ToString(),
                    };

                }
            }
            conn.Close();
        }













        [Route("api/ApiPrescribeController/addMasterData")]
        [HttpGet]
        public IEnumerable<PrescMst_PrescribeDTO> AddMasterData(string Compid, string insertUserID, string insltude, string rxpid, string transDate, string transno, string rxpTp, string rxpIDM, string NextDate, string rxpName, string remarks, string amount)
        {
            Int64 COMPID = Convert.ToInt64(Compid);

            PrescMst_PrescribeDTO model = new PrescMst_PrescribeDTO();
            PrescMst prescMst = new PrescMst();
            
            var check_PrescribeMaster_data = (from n in db.RxPrescMstDbSet where n.COMPID == COMPID && n.TRANSNO == transno select n).ToList();
            if (check_PrescribeMaster_data.Count == 0)
            {
                try
                {
                    prescMst.COMPID = COMPID;
                    prescMst.TRANSNO = Convert.ToString(transno);
                    prescMst.TRANSDT = Convert.ToDateTime(transDate);
                    prescMst.RXPTP = Convert.ToString(rxpTp);
                    prescMst.RXPID = Convert.ToInt64(rxpid);
                    if (NextDate != null)
                    {
                        prescMst.NXTDT = Convert.ToDateTime(NextDate);
                    }
                    if (amount != null)
                    {
                        prescMst.AMOUNT = Convert.ToInt64(amount);
                    }
                    else
                    {
                        prescMst.AMOUNT = 0;
                    }
                    prescMst.REMARKS = Convert.ToString(remarks);

                    prescMst.USERPC = strHostName;
                    prescMst.INSIPNO = ipAddress.ToString();
                    prescMst.INSTIME = Convert.ToDateTime(td);
                    prescMst.INSUSERID = Convert.ToInt64(insertUserID);
                    prescMst.INSLTUDE = Convert.ToString(insltude);

                    db.RxPrescMstDbSet.Add(prescMst);
                    db.SaveChanges();

                    model.COMPID = prescMst.COMPID;
                    model.TRANSNO = prescMst.TRANSNO;
                    model.TRANSDT = transDate;
                    model.RXPTP = prescMst.RXPTP;
                    model.RXPNM = rxpName;
                    model.USERPC = prescMst.USERPC;
                    model.INSIPNO = prescMst.INSIPNO;
                    model.INSTIME = prescMst.INSTIME;
                    model.INSUSERID = prescMst.INSUSERID;
                    model.INSLTUDE = Convert.ToString(prescMst.INSLTUDE);

                    //Log data save from Prescribe(Create) Tabel
                    PrescribeController prescribeController = new PrescribeController();
                    prescribeController.Insert_LogData(model);
                }
                catch
                {
                    transno = "0";
                }
               
                yield return new PrescMst_PrescribeDTO
                {
                    TRANSNO = transno,
                };
            }
            else
            {
                try
                {
                    string findTransNO = Convert.ToString(transno);
                    Int64 convertTransNO = Convert.ToInt64(findTransNO.Substring(6, 3));
                    Int64 transNO_increment = convertTransNO + 1;
                    Int64 MAXIMUM = 1000;
                    if (transNO_increment < 10)
                    {
                        transno = Convert.ToString(findTransNO.Substring(0, 6) + "00" + transNO_increment.ToString());
                    }
                    else if ((10 <= transNO_increment) && (transNO_increment <= 99))
                    {
                        transno = Convert.ToString(findTransNO.Substring(0, 6) + "0" + transNO_increment.ToString());
                    }
                    else if (transNO_increment < MAXIMUM)
                    {
                        transno = Convert.ToString(findTransNO.Substring(0, 6) + transNO_increment.ToString());
                    }



                    prescMst.COMPID = COMPID;
                    prescMst.TRANSNO = Convert.ToString(transno);
                    prescMst.TRANSDT = Convert.ToDateTime(transDate);
                    prescMst.RXPTP = Convert.ToString(rxpTp);
                    prescMst.RXPID = Convert.ToInt64(rxpid);
                    if (NextDate != null)
                    {
                        prescMst.NXTDT = Convert.ToDateTime(NextDate);
                    }
                    if (amount != null)
                    {
                        prescMst.AMOUNT = Convert.ToInt64(amount);
                    }
                    else
                    {
                        prescMst.AMOUNT = 0;
                    }
                    
                    prescMst.REMARKS = Convert.ToString(remarks);

                    prescMst.USERPC = strHostName;
                    prescMst.INSIPNO = ipAddress.ToString();
                    prescMst.INSTIME = Convert.ToDateTime(td);
                    prescMst.INSUSERID = Convert.ToInt64(insertUserID);
                    prescMst.INSLTUDE = Convert.ToString(insltude);

                    db.RxPrescMstDbSet.Add(prescMst);
                    db.SaveChanges();

                    model.COMPID = prescMst.COMPID;
                    model.TRANSNO = prescMst.TRANSNO;
                    model.TRANSDT = transDate;
                    model.RXPTP = prescMst.RXPTP;
                    model.RXPNM = rxpName;
                    model.USERPC = prescMst.USERPC;
                    model.INSIPNO = prescMst.INSIPNO;
                    model.INSTIME = prescMst.INSTIME;
                    model.INSUSERID = prescMst.INSUSERID;
                    model.INSLTUDE = Convert.ToString(prescMst.INSLTUDE);

                    //Log data save from Prescribe(Create) Tabel
                    PrescribeController prescribeController = new PrescribeController();
                    prescribeController.Insert_LogData(model);
                }
                catch
                {
                    transno = "0";
                }
               
                yield return new PrescMst_PrescribeDTO
                {
                    TRANSNO = transno,
                };
            }
        }





        [Route("api/ApiPrescribeController/grid/addData")]
        [HttpPost]
        public HttpResponseMessage AddData(PrescMst_PrescribeDTO model)
        {
            Prescribe prescribe = new Prescribe();

            var check_data = (from n in db.RxPrescribeDbSet where n.COMPID == model.COMPID && n.TRANSNO == model.TRANSNO && n.GHEADID == model.GHEADID && n.PARTSL == model.PARTSL select n).ToList();
            if (check_data.Count == 0)
            {
                var find_data = (from n in db.RxPrescribeDbSet where n.COMPID == model.COMPID && n.TRANSNO == model.TRANSNO select n.TRANSSL).ToList();
                if (find_data.Count == 0)
                {
                    model.TRANSSL = Convert.ToString(Convert.ToString(model.TRANSNO) + "01");
                }
                else
                {
                    string find_Max_TRANSSL = Convert.ToString((from n in db.RxPrescribeDbSet where n.COMPID == model.COMPID && n.TRANSNO == model.TRANSNO select n.TRANSSL).Max());
                    string transl_substring_9 = Convert.ToString(find_Max_TRANSSL.Substring(0, 9));
                    Int64 transl_substring_2 = Convert.ToInt64(find_Max_TRANSSL.Substring(9, 2));
                    Int64 increment_Transl = transl_substring_2 + 1;
                    if (increment_Transl < 10)
                    {
                        model.TRANSSL = Convert.ToString(transl_substring_9 + "0" + increment_Transl);
                    }
                    else if ((10 <= increment_Transl) && (increment_Transl <= 99))
                    {
                        model.TRANSSL = Convert.ToString(transl_substring_9 + increment_Transl);
                    }
                    else
                    {
                        model.TRANSSL = "Not possible entry!!!";
                        HttpResponseMessage response_NotpossibleEntry = Request.CreateResponse(HttpStatusCode.Created, model);
                        return response_NotpossibleEntry;
                    }

                }
                prescribe.COMPID = Convert.ToInt64(model.COMPID);
                prescribe.TRANSNO = Convert.ToString(model.TRANSNO);
                prescribe.TRANSSL = Convert.ToString(model.TRANSSL);
                prescribe.TRANSDT = Convert.ToDateTime(model.TRANSDT);
                prescribe.RXPID = Convert.ToInt64(model.RXPID);

                prescribe.PARTSL = model.PARTSL;
                prescribe.ENTRYTP = model.ENTRYTP;
                prescribe.TABLEID = model.TABLEID;

                if (model.PARTSL == "1ST" && model.ENTRYTP == "MANUAL")//1st Part Grid
                {
                    prescribe.RESULT = model.RESULT;
                }
                else if (model.PARTSL == "1ST" && model.ENTRYTP == "RECORD")// 1st part grid
                {
                    prescribe.GCATID = Convert.ToInt64(model.GCATID);
                    prescribe.GHEADID = Convert.ToInt64(model.GHEADID);
                }
                else if (model.PARTSL == "2ND" && model.ENTRYTP == "MANUAL") //2nd part grid
                {
                    prescribe.RESULT = model.RESULT;
                }
                else if (model.PARTSL == "2ND" && model.ENTRYTP == "RECORD") //2nd part grid
                {
                    prescribe.GCATID = Convert.ToInt64(model.GCATID);
                    prescribe.GHEADID = Convert.ToInt64(model.GHEADID);
                    prescribe.RESULT = model.RESULT;
                }
                else if (model.PARTSL == "3RD" && model.ENTRYTP == "MANUAL")//3rd Part Grid
                {
                    prescribe.RESULT = model.RESULT;
                }
                else if (model.PARTSL == "3RD" && model.ENTRYTP == "RECORD")//3rd part grid
                {
                    prescribe.GCATID = Convert.ToInt64(model.GCATID);
                    prescribe.GHEADID = Convert.ToInt64(model.GHEADID);
                }
                else if (model.PARTSL == "4TH" && model.ENTRYTP == "MANUAL")//4th Part Grid
                {
                    prescribe.RESULT = model.RESULT;
                    prescribe.DOSEID = model.DOSEID;
                    prescribe.DDMMTP = model.DDMMTP;
                    prescribe.DDMMQTY = Convert.ToInt64(model.DDMMQTY);
                }
                else if (model.PARTSL == "4TH" && model.ENTRYTP == "RECORD")//4th part grid
                {
                    prescribe.GCATID = Convert.ToInt64(model.GCATID);
                    prescribe.GHEADID = Convert.ToInt64(model.GHEADID);
                    prescribe.DOSEID = model.DOSEID;
                    prescribe.DDMMTP = model.DDMMTP;
                    prescribe.DDMMQTY = Convert.ToInt64(model.DDMMQTY);
                }
                else if (model.PARTSL == "5TH" && model.ENTRYTP == "MANUAL")//5th Part Grid
                {
                    prescribe.RESULT = model.RESULT;
                }
                else if (model.PARTSL == "5TH" && model.ENTRYTP == "RECORD")//5th part grid
                {
                    prescribe.GCATID = Convert.ToInt64(model.GCATID);
                    prescribe.GHEADID = Convert.ToInt64(model.GHEADID);
                }


                prescribe.USERPC = strHostName;
                prescribe.INSIPNO = ipAddress.ToString();
                prescribe.INSTIME = Convert.ToDateTime(td);
                prescribe.INSUSERID = model.INSUSERID;
                prescribe.INSLTUDE = Convert.ToString(model.INSLTUDE);

                db.RxPrescribeDbSet.Add(prescribe);
                db.SaveChanges();

                //model.ID = prescribe.ID;
                //model.COMPID = prescribe.COMPID;
                //model.MCATID = Convert.ToInt64(prescribe.MCATID);
                //model.MCATNM = prescribe.MCATNM;
                //model.USERPC = prescribe.USERPC;
                //model.INSIPNO = prescribe.INSIPNO;
                //model.INSTIME = prescribe.INSTIME;
                //model.INSUSERID = prescribe.INSUSERID;
                //model.INSLTUDE = Convert.ToString(prescribe.INSLTUDE);

                //Log data save from prescribe Tabel
                //PrescribeController prescribeController = new PrescribeController();
                //prescribeController.Insert_prescribe_LogData(model);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                return response;
            }
            else
            {
                model.TRANSSL = "0";
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                return response;
            }
        }





        [Route("api/ApiPrescribeController/grid/DeleteData")]
        [HttpPost]
        public HttpResponseMessage DeleteData(PrescMst_PrescribeDTO model)
        {
            Prescribe deleteModel = new Prescribe();
            deleteModel.ID = model.ID;
            deleteModel.COMPID = model.COMPID;
            deleteModel.TRANSNO = Convert.ToString(model.TRANSNO);
            deleteModel.TRANSSL = Convert.ToString(model.TRANSSL);

            deleteModel = db.RxPrescribeDbSet.Find(deleteModel.ID, deleteModel.COMPID, deleteModel.TRANSNO, deleteModel.TRANSSL);
            db.RxPrescribeDbSet.Remove(deleteModel);
            db.SaveChanges();

            Prescribe testObj = new Prescribe();
            return Request.CreateResponse(HttpStatusCode.OK, testObj);
        }






        [Route("api/ApiPrescribeController/updateMasterData")]
        [HttpGet]
        public IEnumerable<PrescMst_PrescribeDTO> UpdateMasterData(string Compid, string insertUserID, string insltude, string transno, string rxpTp, string NextDate, string remarks, string patientName, string TransDate, string UpLogData, string amount)
        {
            Int64 COMPID = Convert.ToInt64(Compid);

            PrescMst_PrescribeDTO model = new PrescMst_PrescribeDTO();
            var check_PrescribeMaster_data = (from n in db.RxPrescMstDbSet where n.COMPID == COMPID && n.TRANSNO == transno select n).ToList();
            if (check_PrescribeMaster_data.Count == 1)
            {
                foreach (var data in check_PrescribeMaster_data)
                {
                    data.RXPTP = Convert.ToString(rxpTp);
                    if (NextDate != null)
                    {
                        data.NXTDT = Convert.ToDateTime(NextDate);
                    }
                    else
                    {
                        data.NXTDT = null;
                    }
                    data.REMARKS = Convert.ToString(remarks);
                    if (amount != null)
                    {
                        data.AMOUNT = Convert.ToInt64(amount);
                    }
                    else
                    {
                        data.AMOUNT = 0;
                    }

                    data.USERPC = strHostName;
                    if (UpLogData != null)
                    {
                        data.UPDIPNO = ipAddress.ToString();
                        data.UPDLTUDE = Convert.ToString(insltude);
                        data.UPDTIME = Convert.ToDateTime(td);
                        data.UPDUSERID = Convert.ToInt64(insertUserID);
                    }
                    else
                    {
                        data.INSIPNO = ipAddress.ToString();
                        data.INSLTUDE = Convert.ToString(insltude);
                        data.INSTIME = Convert.ToDateTime(td);
                        data.INSUSERID = Convert.ToInt64(insertUserID);
                    }
                }
                db.SaveChanges();

                if (UpLogData != null)
                {
                    model.COMPID = COMPID;
                    model.TRANSNO = transno;
                    model.TRANSDT = TransDate;
                    model.RXPTP = Convert.ToString(rxpTp);
                    model.RXPNM = patientName;
                    model.INSUSERID = Convert.ToInt64(insertUserID);
                    model.INSLTUDE = Convert.ToString(insltude);

                    //Log data save from Prescribe(Create) Tabel
                    PrescribeController prescribeController = new PrescribeController();
                    prescribeController.Update_LogData(model);
                }

                yield return new PrescMst_PrescribeDTO
                {
                    TRANSNO = transno,
                };
            }
            else
            {
                yield return new PrescMst_PrescribeDTO
                {
                    TRANSNO = "0",
                };
            }
        }


    }
}
