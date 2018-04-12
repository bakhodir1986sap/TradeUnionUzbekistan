<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportPage.aspx.cs" Inherits="KasabaKIAT.Reporting.ReportPage" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <rsweb:ReportViewer ID="rptViewer" runat="server">
            <ServerReport ReportPath="/StartSSRS/OblastRayonOtrasl" ReportServerUrl="http://HP:80/Reports_SQLEXPRESS" />
        </rsweb:ReportViewer>
    </div>
    </form>
</body>
</html>
