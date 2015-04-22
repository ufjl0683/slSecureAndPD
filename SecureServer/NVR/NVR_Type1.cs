using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SecureServer.NVR
{

    // LiLin  NVR
   public  class NVR_Type1:INVR
    {
       public int NVRID
       {
           get;
           set;
       }

       public int ERID
       {
           get;
           set;
       }

       public string IP
       {
           get;
           set;
       }

       public int Port
       {
           get;
           set;
       }

       public string UserName
       {
           get;
           set;
       }

       public string Password
       {
           get;
           set;
       }

       public int PlaneID
       {
           get;
           set;
       }


       public bool SaveRecord(int Chno, DateTime BeginDateTime, DateTime EndDateTime, string SavePathFilename)
       {
         //  throw new NotImplementedException();
           lock (this)
           {
               Console.WriteLine("NVR:{3} call SaveRecordAction,{0}:{1},{2}",BeginDateTime,EndDateTime,SavePathFilename,this.NVRID);
               bool success = SaveRecordAction(Chno, BeginDateTime, EndDateTime, SavePathFilename);
               Console.WriteLine(success);
               return success;
           }
       }


       bool SaveRecordAction(int Chno, DateTime BeginDateTime, DateTime EndDateTime, string SavePathFilename)
       {
             bool IsSuccess=false;
             string url;
           lock (this)
           {
               try
               {
                
                   string state = "";

                   do
                   {
                       url = "http://" + IP + ":" + Port;
                       Console.WriteLine("check backup State:" + url);
                       state = NvrGetBackupState(url);
                       System.Threading.Thread.Sleep(3000);

                   } while (state.Trim() != "1");

                   // private void NvrBackup(string NvrUrl, string startDate, string startTime, string endDate,string endTime, string channel, string desc)
                   string StartDate, StartTime, EndDate, EndTime;
                   StartDate = string.Format("{0:0000}/{1:00}/{2:00}", BeginDateTime.Year, BeginDateTime.Month, BeginDateTime.Day);
                   StartTime = string.Format("{0:00}:{1:00}:00", BeginDateTime.Hour, BeginDateTime.Minute, BeginDateTime.Second);
                   EndDate = string.Format("{0:0000}/{1:00}/{2:00}", EndDateTime.Year, EndDateTime.Month, EndDateTime.Day);
                   EndTime = string.Format("{0:00}:{1:00}:{2:00}", EndDateTime.Hour, EndDateTime.Minute, EndDateTime.Second);
                   string strchno = (System.Convert.ToInt32(Math.Pow(10, Chno - 1))).ToString();
                   Console.WriteLine("backup {0}  chano{1}  from {2} to {3}", url, Chno, StartDate + StartTime, EndDate + EndTime);
                   NvrBackup(url, StartDate, StartTime, EndDate, EndTime, strchno);
                   string downloadfilename = (StartDate + StartTime + "_CAM" + string.Format("{0:00}", Chno) + ".avi").Replace("/", "").Replace(":", "");
                   Console.WriteLine("Begin download file:" + downloadfilename);
                   List<String> downloadlist = null;
                   bool found = false;
                   DateTime dt = DateTime.Now;
                   string DownloadUrl = "";
                   do
                   {
                       Console.WriteLine("Nvr GetBack url:" + url);
                       downloadlist = NvrGetBackup(url);
                       if (downloadlist.Where(n => n.Contains(downloadfilename)).Count() > 0)
                       {
                           found = true;
                           DownloadUrl = downloadlist.Where(n => n.Contains(downloadfilename)).FirstOrDefault();
                           Console.WriteLine("download found!");
                           break;
                       } 

                       System.Threading.Thread.Sleep(3000);
                   } while (!found && DateTime.Now.Subtract(dt) < TimeSpan.FromSeconds(20));

                   if (!found)
                   {
                       Console.WriteLine("download file not found exit!!");
                       return false;
                   }

                   if (DownloadUrl == null)
                       return false;

                   Console.WriteLine("begin download " + DownloadUrl);

                  
                   Download(DownloadUrl, SavePathFilename);


                   #region   Fork Task

                   System.Diagnostics.Process process;

                   string ffmpegArgument;
                   ffmpegArgument = string.Format("-i {0} -vcodec   wmv2   -y  {1}", SavePathFilename, SavePathFilename.Replace("avi", "wmv"));

                   process = Process.Start(AppDomain.CurrentDomain.BaseDirectory + @"\ffmpeg.exe", ffmpegArgument);

                   if (!process.WaitForExit(30000))
                   {
                       try
                       {
                           process.Kill();
                       }
                       catch {
                           Console.WriteLine("ffmpeg.exe");
                           ;}
                       return false;
                   }




                   #endregion





                   IsSuccess = true;

               }
               catch (Exception ex)
               {
                   Console.WriteLine(ex.Message + "," + ex.StackTrace);
                   return false;
               }
               finally
               {
                   System.IO.File.Delete(SavePathFilename);
               }
           }
           return IsSuccess;
       }
       #region NVR BackUp inner program
       private void sendRequest(string url)
       {
           try
           {
               HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
               request.Method = WebRequestMethods.Http.Get;
               HttpRequestCachePolicy noCache = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
               request.CachePolicy = noCache;
               using (WebResponse wr = request.GetResponse())
               {


               }
           }
           catch (Exception ex)
           {


           }
       }
      private void NvrBackup(string NvrUrl, string startDate, string startTime, string endDate,
            string endTime, string channel )
       {
           string url = NvrUrl + "/backup?" +
               "startdate=" + startDate +
               "&starttime=" + startTime +
               "&enddate=" + endDate +
               "&endtime=" + endTime +
               "&stream=2&chflag=" + channel  
                ;


           sendRequest(url);
       }





      private List<string> NvrGetBackup(string NvrUrl)
       {
           List<string> filePaths = new List<string>();
           int fileNum = 1;


           //getResponseXML(http://192.192.85.20:10000/backup?get_ftpfile=1)
           XDocument xdoc = XDocument.Parse( getResponseText(NvrUrl + "/backup?get_ftpfile=1"));


           NvrUrl = NvrUrl + "/Download/";
           var q = (from p in xdoc.Elements("BackUp")
                    select p);
           foreach (var x in q)
           {
               fileNum = Convert.ToInt32(x.Element("filecnt").Value);
           }
           foreach (var x in q)
           {
               for (int i = 1; i <= fileNum; i++)
               {
                   filePaths.Add(NvrUrl + x.Element("file" + i).Value);
               }
           }


           return filePaths;
       }




       private void Download(string remoteUri, string localUri)
       {
           //string remoteUri = "http://192.192.85.20:10000/Download/20141203170000_CAM01.avi";
           //string localUri = "D:\\123.avi"
           WebClient myWebClient=null;

           // Create a new WebClient instance.
           try
           {
                myWebClient = new WebClient();



               Console.WriteLine("Downloading File \"{0}\" from \"{1}\" .......\n\n", localUri, remoteUri);


               // Download the Web resource and save it into the current filesystem folder.
               myWebClient.DownloadFile(remoteUri, localUri);
               Console.WriteLine("Successfully Downloaded File \"{0}\" from \"{1}\"", localUri, remoteUri);
               Console.WriteLine("\nDownloaded file saved in the following file system folder:\n\t");
           }
           catch (Exception ex)
           {
               myWebClient.Dispose();
               throw;
           }
           
        
       }



       private   string NvrGetBackupState(string url)
       {
           url = url + "/connection?cmd=getBackupstate";
           return getResponseText(url);
       }


       //查詢 Nvr 主機狀態CGI指令：「主機url + /backup?get_percent=1」
       //回傳 xml ，找出 progress_percent 節點讀其值，備份完全會變 0 
       private int GetBackupPercent(string url)
       {
           url = url + "/backup?get_percent=1";
           string percent = "";
           XDocument xdoc = XDocument.Parse( getResponseText(url));
           var q = (from p in xdoc.Elements("BackUp")
                    select p);


           foreach (var x in q)
           {
               percent = Convert.ToString(x.Element("progress_percent").Value);
           }


           return Convert.ToInt32(percent);
       }





       private string getResponseText(string url)
       {
           HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
           request.Method = WebRequestMethods.Http.Get;
           HttpWebResponse response = null;
           Stream stream = null;
           StreamReader streamReader = null;
           string finallyStr = "";


           try
           {
               response = (HttpWebResponse)request.GetResponse();
               if (response.StatusCode == HttpStatusCode.OK)
               {
                   stream = response.GetResponseStream();
                   streamReader = new StreamReader(stream);
                   char[] buffer = new char[256];
                   int data = 0;
                   string result = "";
                   //分批讀
                   while (true)
                   {
                       data = streamReader.Read(buffer, 0, buffer.Length);
                       string msg = new string(buffer, 0, data);
                       result = result + msg;
                       if (data == 0)
                           break;
                   }
                   finallyStr = result;
               }
           }
           catch (WebException ex1)
           {
               HttpWebResponse exResponse = (HttpWebResponse)ex1.Response;
               finallyStr = ex1.Message;
           }
           catch (NotSupportedException ex2)
           {
               finallyStr = ex2.Message;
           }
           catch (ProtocolViolationException ex3)
           {
               finallyStr = ex3.Message;
           }
           catch (InvalidOperationException ex4)
           {
               finallyStr = ex4.Message;
           }
           catch (Exception ex)
           {
               finallyStr = ex.Message;
           }
           finally
           {
               if (response != null) response.Close();
               if (streamReader != null) streamReader.Close();
               if (stream != null) stream.Close();
           }


           return finallyStr;
       }
       #endregion
    }
}
