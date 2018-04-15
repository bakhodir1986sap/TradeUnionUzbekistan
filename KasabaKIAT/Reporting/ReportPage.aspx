<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportPage.aspx.cs" Inherits="KasabaKIAT.Reporting.ReportPage" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
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
