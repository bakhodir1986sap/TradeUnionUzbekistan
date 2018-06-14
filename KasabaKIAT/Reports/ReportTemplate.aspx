<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportTemplate.aspx.cs" Inherits="KasabaKIAT.Reports.ReportTemplate" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <%--<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js">--%>
    <script>
        $(document).ready(function(){

 if ($.browser.webkit)
 {
    $($(":hidden[id*='DatePickers']").val().split(",")).each(function(i, item){
         var h = $("table[id*='ParametersGrid'] span").filter(function(i) {
             var v = "[" + $(this).text() + "]";
             return (v != null && v.indexOf(item) >= 0);
          }).parent("td").next("td").find("input").datepicker({
           showOn: "button"
           ,buttonImage: '/Reserved.ReportViewerWebControl.axd?OpType=Resource&Name=Microsoft.Reporting.WebForms.calendar.gif'
           ,buttonImageOnly: true
           ,dateFormat: 'dd/mm/yy'
           ,changeMonth: true
           ,changeYear: true
           });
     });
  }

});
    </script>
</head>
<body style="margin: 0px; padding: 0px;">
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">               
            </asp:ScriptManager>
            <asp:HiddenField ID="DatePickers" runat="server" />
            <rsweb:ReportViewer id="rvSiteMapping" runat ="server" ShowPrintButton="false"  Width="99.9%" Height="100%" AsyncRendering="true" ZoomMode="Percent" KeepSessionAlive="true" SizeToReportContent="false" >
            </rsweb:ReportViewer>  
        </div>
    </form>
</body>
</html>
