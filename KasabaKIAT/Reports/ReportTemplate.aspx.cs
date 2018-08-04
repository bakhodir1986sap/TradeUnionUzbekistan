using Microsoft.Reporting.WebForms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace KasabaKIAT.Reports
{
    public partial class ReportTemplate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    var reportFolder = System.Configuration.ConfigurationManager.AppSettings["SSRSReportsFolderFed"].ToString();
                    var reportServerUrl = System.Configuration.ConfigurationManager.AppSettings["SSRSPath"].ToString();

                    rvSiteMapping.Height = Unit.Pixel(Convert.ToInt32(Request["Height"]) - 58);
                    rvSiteMapping.ProcessingMode = ProcessingMode.Remote;

                    rvSiteMapping.ServerReport.ReportServerUrl = new Uri(reportServerUrl); // Add the Reporting Server URL 
                    rvSiteMapping.ServerReport.ReportPath = String.Format("/{0}/{1}", Request["RoleId"].ToString(), Request["ReportName"].ToString());

                    List<ReportParameter> paramList = new List<ReportParameter>();

                    paramList.Add(new ReportParameter("departmentBase", Request["DepartmentBase"].ToString(), false));

                    rvSiteMapping.ServerReport.SetParameters(paramList);

                    rvSiteMapping.ServerReport.Refresh();
                }
                catch (Exception)
                {

                }
            }
        }

        protected override void OnPreRender( EventArgs e)
        {
            base.OnPreRender(e);
            DatePickers.Value = string.Join(",", PerformQuery());
        }

        private IEnumerable GetDateParameters()
        {
            // I'm assuming report view control id as reportViewer
            foreach (ReportParameterInfo info in rvSiteMapping.ServerReport.GetParameters())
            {
                if (info.DataType == ParameterDataType.DateTime)
                {
                    yield return string.Format("[{0}]", info.Prompt);
                }
            }
        }


        public string[] PerformQuery()
        {
            IEnumerable query = GetDateParameters();
            string[] array = query.Cast<string>().ToArray();
            return array;
        }
    }
}