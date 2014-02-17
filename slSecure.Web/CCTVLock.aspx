<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CCTVLock.aspx.cs" Inherits="slSecure.Web.CCTVLock" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
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
					 <SCRIPT id="cctvframe"  LANGUAGE="JavaScript" SRC="http://117.56.89.19:80/axis-cgi/mjpg.cgi?ch=<%=Request.QueryString["ch"] %>&imagesize=CIF"></SCRIPT>
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
