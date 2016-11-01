using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBoardWPF
{
    public class Hours
    {
        public string hourFr
        {
            get
            {
                return DateTime.Now.ToShortTimeString();
            }
        }
        public string hourVn
        {
            get
            {
                if (Convert.ToBoolean(System.Configuration.ConfigurationSettings.AppSettings["isVnTime"]))
                {
                    DateTime utc = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now, TimeZoneInfo.Local);
                    TimeZoneInfo tst = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                    DateTime tstTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, tst);
                    return tstTime.ToShortTimeString();
                }
                else
                    return null;
            
            }
        }

    }
}
