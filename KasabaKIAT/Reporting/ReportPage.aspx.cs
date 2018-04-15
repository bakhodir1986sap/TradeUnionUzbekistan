using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Collections;


namespace KasabaKIAT.Reporting
{
    public partial class ReportPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ShowReport();
        }


        private void ShowReport()
        {
            try
            {
                string urlReportServer = "http://localhost:80/Reports_SQLEXPRESS";
                rptViewer.ProcessingMode = ProcessingMode.Remote; // ProcessingMode will be Either Remote or Local
                rptViewer.ServerReport.ReportServerUrl = new Uri(urlReportServer); //Set the ReportServer Url
                rptViewer.ServerReport.ReportPath = "/StartSSRS/OblastRayonOtrasl"; //Passing the Report Path                

                //Creating an ArrayList for combine the Parameters which will be passed into SSRS Report
                ArrayList reportParam = new ArrayList();
                reportParam = ReportDefaultPatam();

                ReportParameter[] param = new ReportParameter[reportParam.Count];
                for (int k = 0; k < reportParam.Count; k++)
                {
                    param[k] = (ReportParameter)reportParam[k];
                }
                // pass crendentitilas
                rptViewer.ServerReport.ReportServerCredentials =
                  new CustomReportCredentials("BAKHODIR", "baxa", "");

                //pass parmeters to report
                rptViewer.ServerReport.SetParameters(param); //Set Report Parameters
                rptViewer.ServerReport.Refresh();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ArrayList ReportDefaultPatam()
        {
            ArrayList arrLstDefaultParam = new ArrayList();
            arrLstDefaultParam.Add(CreateReportParameter("BEGIN_DATE", "2018-03-01"));
            arrLstDefaultParam.Add(CreateReportParameter("END_DATE", "2018-04-01"));
            return arrLstDefaultParam;
        }

        private ReportParameter CreateReportParameter(string paramName, string pramValue)
        {
            ReportParameter aParam = new ReportParameter(paramName, pramValue);
            return aParam;
        }
    }
}