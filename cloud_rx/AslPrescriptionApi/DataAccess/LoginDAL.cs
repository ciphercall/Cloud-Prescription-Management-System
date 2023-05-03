using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using AslPrescriptionApi.Models;

namespace AslPrescriptionApi.DataAccess
{
    public class LoginDAL
    {

        static SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["AslPrescriptionApiDbContext"].ToString());

     

        internal static bool AdminIsValid(string username, string password)
        {
            bool authenticated = false;

            string query = string.Format("SELECT * FROM ASL_USERCO WHERE LOGINID = '{0}' AND LOGINPW = '{1}'", username, password);

            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();

            SqlDataReader sdr = cmd.ExecuteReader();
            authenticated = sdr.HasRows;
            conn.Close();
            return (authenticated);
        }
    }
}