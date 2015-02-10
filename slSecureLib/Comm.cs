using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace slSecureLib
{
    public static class Comm
    {
        #region +++++ 共用變數 +++++
        public static DateTime strTime;
        public static DateTime endTime;
        #endregion

        #region +++ 自訂函式  +++
        public static bool checkTimes(string sTime, string eTime)
        {
            if (sTime != "") strTime = Convert.ToDateTime(sTime).Date;
            bool flag = true;
            if (sTime != "" && eTime != "")
            {
                strTime = Convert.ToDateTime(sTime).Date;
                //endTime = Convert.ToDateTime(eTime).Date;
                endTime = Convert.ToDateTime((Convert.ToDateTime(eTime)).ToShortDateString() + " 23:59:59"); 
                if (strTime > endTime) flag = false;
            }
            else if ((sTime == "") && (eTime != ""))
            {
                flag = false;
            }
            else if ((sTime != "" && eTime == "") || (sTime == eTime))
            {
                strTime = Convert.ToDateTime(sTime).Date;
                //endTime = strTime.AddDays(1);
                endTime = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " 23:59:59"); 
            }

            return flag;
        }

        #endregion

    }
}
