using KasabaKiat.Helpers;
using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KasabaKIAT.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
        public ActionResult ReportTemplate(string ReportName, string ReportDescription, int Width, int Height)
        {
            int UserId = Convert.ToInt32(User.Identity.Name);
            var ustinfo = UserInfo.GetUserInfo(UserId);

            var rptInfo = new ReportInfo
            {
                ReportName = ReportName,
                ReportDescription = ReportDescription,
                ReportURL = String.Format("../../Reports/ReportTemplate.aspx?ReportName={0}&Height={1}&RoleId={2}&DepartmentBase={3}", ReportName, Height, ustinfo.role_id.Value, ustinfo.DepartmentBase),
                Width = Width,
                Height = Height,
                DepartmentBase = ustinfo.DepartmentBase,
                RoleId = ustinfo.role_id.Value
            };

            return View(rptInfo);
        }
    }
}