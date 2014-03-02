<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CCTVLock.aspx.cs" Inherits="slSecure.Web.CCTVLock" %>

<!doctype html>

<html >
<head runat="server">
<meta charset="utf-8"/>
    <style>
			body {  
				overflow: hidden; 
			}  
		</style>

    <title></title>
</head>
 <body   >
        <center>
            <table border="0" width="100%">
                <!--<tr>
                    <td>即時影像資訊 - 五權西路-文心路</td>
                </tr>
                <tr>
                    <td height=10 bgcolor=#0066CC></td>
                </tr>-->
                <tr >
                    <td   >
					 <script id="cctvframe"  language="JavaScript" SRC="http://117.56.89.19:80/axis-cgi/mjpg.cgi?ch=<%=Request.QueryString["ch"] %>&imagesize=CIF"   >
                        <!--SRC="http://117.56.89.19:80/axis-cgi/mjpg.cgi?ch=<%=Request.QueryString["ch"] %>&imagesize=CIF" -->
         //                 theDate = new Date();
   //var output = "<img src=\"http://117.56.89.19/axis-cgi/mjpg/video.cgi?camera=<%=Request.QueryString["ch"] %>";
   
  // output += "\" width=\"352\" height=\"240\" alt=\"Press Reload if no image is displayed\">";
  // document.write(output);


					 </script>
                      <!--  <script language="javascript" >
                            var ch = QueryString('ch');
                            var s="<"+"scr"+"ipt LANGUAGE='JavaScript' SRC='http://117.56.89.19:80/axis-cgi/mjpg.cgi?ch=";
                            s=s+ch;
                            s = s + "&imagesize=CIF'>";
                            s=s+"<"+"/s"+"cript>";

                            alert(s);
                            document.write(s);
                        </script>-->
						
					</td>
                </tr>                
            </table>
        </center>
    </body></html>
