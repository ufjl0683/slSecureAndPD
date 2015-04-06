using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace slSecure.Web
{
    public partial class TRDialog_DownloadForm : System.Web.UI.Page
    {
        #region Declaration
        public string fileName { get; set; }

        public int Get_Item_ID { get; set; }

        public DateTime Start_Date { get; set; }
        public string format_Start_Date { get; set; }

        public DateTime End_Date { get; set; }
        public string format_End_Date { get; set; }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            // Item ID
            this.Get_Item_ID = Convert.ToInt32(Request["id"]);

            // Start Date
            this.Start_Date = Convert.ToDateTime(Request["start-date"]);

            // End Date
            this.End_Date = Convert.ToDateTime(Request["end-date"]);

            // Release Response resources
            Response.Clear();

            // Export to Excel
            Export_CVS();
        }

        private void Export_CVS()
        {
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);
//                    new SqlConnection(@"server=192.192.85.64,1357;
//                                    database=SecureDB;
//                                    uid=secure;
//                                    pwd=secure");

            SqlCommand s_com = new SqlCommand();

            DateTime_Format();

            try
            {
                conn.Open();
                s_com.CommandText =
                                "SELECT Timestamp As '" + "日期時間" + "', " +
                                "ItemID As '" + "機器種類" + "', " +
                                "Memo As '" + "名稱" + "', " +
                                "Value As '" + "數值" + "' " +
                                "FROM [SecureDB].[dbo].[tblAIItem1HourLog]" + " " +
                                "WHERE Timestamp >= '" + this.format_Start_Date + "' " +
                                    "AND Timestamp < '" + this.format_End_Date + "' " +
                                    "AND ItemID = '" + this.Get_Item_ID + "' " +
                                "ORDER BY Timestamp";
                s_com.Connection = conn;

                System.Data.DataTable dt = new System.Data.DataTable();

                SqlDataAdapter sda = new SqlDataAdapter(s_com);
                sda.Fill(dt);
                this.GridView1.DataSource = dt;
                this.GridView1.DataBind();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
               // alert(ex.StackTrace);
                return;
            }

            this.fileName =
                this.Start_Date.ToString("yyyyMMdd") +
                "-" +
                this.End_Date.ToString("yyyyMMdd") +
                "-ItemID_" +
                Request["id"].ToString();

            this.fileName = Server.UrlPathEncode(this.fileName);

            Response.Clear();
            string strContentDisposition = String.Format("{0}; filename=\"{1}\"", "attachment", this.fileName);
            Response.AddHeader("Content-Disposition", strContentDisposition + ".xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write("<meta http-equiv=Content-Type content=text/html;charset=utf-8>");

            //關閉換頁跟排序
            this.GridView1.AllowSorting = false;
            this.GridView1.AllowPaging = false;

            // 連接關閉
            Response.Clear();
            conn.Close();
        }

        private void DateTime_Format()
        {
            this.format_Start_Date = string.Format("{0:yyyy-MM-dd HH:mm:ss:00}", this.Start_Date);
            this.format_End_Date = string.Format("{0:yyyy-MM-dd HH:mm:ss:00}", this.End_Date.AddDays(1));
        }
    }
}