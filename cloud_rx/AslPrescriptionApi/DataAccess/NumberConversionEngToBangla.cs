using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AslPrescriptionApi.DataAccess
{
    public class NumberConversionEngToBangla
    {
        public static string number(string value)
        {
            Int64[] no = new Int64[10];
            int k;
            string a = "";
            k = value.Length;
            for (int j = 0; j < 8; j++)
            {
                if (k > 0)
                {
                    no[j] = Convert.ToInt64(value.Substring(k - 1, 1));
                }
                else
                {
                    no[j] = 0;
                }
                k = k - 1;
            }

            int y = value.Length;
            for (int x =y-1 ; x<y && x!=-1; x--)
            {
                a = a + BanglaCharacter(no[x]);
            }

            return a;
        }


        private static string BanglaCharacter(Int64 x)
        {
            switch (x)
            {
                case 1:
                    return "১";
                case 2:
                    return "২";
                case 3:
                    return "৩";
                case 4:
                    return "৪";
                case 5:
                    return "৫";
                case 6:
                    return "৬";
                case 7:
                    return "৭";
                case 8:
                    return "৮";
                case 9:
                    return "৯";
                case 0:
                    return "০";
                default:
                    return "";

            }
        }
    }
}