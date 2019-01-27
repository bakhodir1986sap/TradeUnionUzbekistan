<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportTemplate.aspx.cs" Inherits="KasabaKIAT.Reports.ReportTemplate" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script src="/Scripts/jquery-1.10.2.min.js" ></script>
    <script src="/Scripts/jquery-ui-1.8.17.custom.min.js"></script>

    <script>
        $(document).ready(function(){

        var parameterRow = $("#ParametersRowrvSiteMapping");
        var innerTable = $(parameterRow).find("table").find("table");
        var span = innerTable.find("span:contains('Дата начало')");
            if (span) {
                var innerRow = $(span).parent().parent().parent();
                var innerCell = innerRow.find("td").eq(1);
                var textFrom = innerCell.find("input[type=text]");

                textFrom.attr('type', 'date');
            }

        var span2 = innerTable.find("span:contains('Дата конца')");
            if (span2) {
                var innerRow2 = $(span2).parent().parent().parent();
                var innerCell2 = innerRow2.find("td").eq(4);
                var textFrom2 = innerCell2.find("input[type=text]");

                textFrom2.attr('type', 'date');
            }

        $("#rvSiteMapping_ctl04_ctl00").val('Показать');

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
