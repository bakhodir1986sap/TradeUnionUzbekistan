using KasabaKIAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Structure
{
    public enum KIAT_STRUCT
    {
        Federation, //Department
        Oblsovprof, //Department
        TumanBranch,
        PPO,
        UnKnown,
    }

    public enum KIAT_OBJECT_TYPE
    {
         UnKnown
        ,Bkut
        ,Department
        ,Bkuq      //Qo'mita
        ,Workshop  //Tsex
        ,Camp      
        ,Sanatorium      
        ,GroupDepartment //GRUPPA
    }

    /// <summary>
    /// 
    /// </summary>
    public class StructureOfKiat
    {
        /// <summary>
        /// 
        /// </summary>
        public string federation  = "17";

        /// <summary>
        /// 
        /// </summary>
        public string federationOrgs = "171";

        /// <summary>
        /// 
        /// </summary>
        public string regionalOrg = "172";

        /// <summary>
        /// 
        /// </summary>
        public string branchOrg   = "173";

        /// <summary>
        /// 
        /// </summary>
        public string Sanatorium  = "174";

        /// <summary>
        /// 
        /// </summary>
        public string Camp        = "175";

        //Oblsovprof begins with 172XX - XX region code f.e. 17203 - Andijan OblSovprof TABLE in DB - tbku_StrokaProfsoyza

        //branch ogranization 173XXYYYYY - XX branch code TABLE in DB sp_branch, YYYYY - tuman code from SOATO Reference leave 17 from begin.

        //BKUT 173XXYYYYYZZZZ - XX branch code TABLE in DB sp_branch,  YYYYY - tuman code from SOATO Reference leave 17 from begin.  ZZZZ - order number for example - 173Д1032030024 

        //Federasiya 17 u LIKE bilan hammani department codelari bo'yicha qidirishi mumkin
        //Oblsovprof poiskni Organization dagi tashkilotlarni department codi bo'yicha qidiradi == bilan
        //Tuman - rayon o'zidan tagidagi tashkilotralni xam kiritsin  dagi tashkilotlarni department codi bo'yicha qidiradi == bilan
        //PPO - zanjirni oxirgi bo'limi

        /// <summary>
        /// 
        /// </summary>
        /// <param name="departmentbaseCode"></param>
        /// <returns></returns>
        public bool IsRegionOblsovprof(string departmentbaseCode)
        {
            if (!string.IsNullOrEmpty(departmentbaseCode))
                if (departmentbaseCode.StartsWith("172") && departmentbaseCode.Length == 5)
                {
                    var oblCode = string.Format("{0}{1}", "17", departmentbaseCode.Substring(3, 2));

                    using (KasabaKIATEntities db = new KasabaKIATEntities())
                    {
                        var kod = db.tbsp_soato_new.Where(z => z.KODI == oblCode).FirstOrDefault();

                        if (kod != null)
                        {
                            return true;
                        }
                    }

                }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="departmentbaseCode"></param>
        /// <returns></returns>
        public bool IsTumanBranch(string departmentbaseCode)
        {
            if (!string.IsNullOrEmpty(departmentbaseCode))
                if (departmentbaseCode.StartsWith("173") && departmentbaseCode.Length == 10)
                {
                    var tumanCode = string.Format("{0}{1}", "17", departmentbaseCode.Substring(5, 5));

                    using (KasabaKIATEntities db = new KasabaKIATEntities())
                    {
                        var branchcd = departmentbaseCode.Substring(3, 2);

                        var kod = db.sp_branch.Where(z => z.code == branchcd).FirstOrDefault();

                        var kod2 = db.tbsp_soato_new.Where(z => z.KODI == tumanCode).FirstOrDefault();

                        if ((kod != null) && (kod2 != null))
                        {
                            return true;
                        }
                    }
                }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="departmentbaseCode"></param>
        /// <returns></returns>
        public bool IsFederation(string departmentbaseCode)
        {
            if (string.IsNullOrEmpty(departmentbaseCode))
            {
                return false;
            }
            else
            {
                if (departmentbaseCode.Equals("17"))
                {
                    return true;
                }                
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="departmentbaseCode"></param>
        /// <returns></returns>
        public bool IsPervichka(string departmentbaseCode)
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="departmentbaseCode"></param>
        /// <returns></returns>
        public KIAT_STRUCT GetWhoIsThisCode(string departmentbaseCode)
        {
            if (string.IsNullOrEmpty(departmentbaseCode))
            {
                return KIAT_STRUCT.UnKnown;
            }

            if (departmentbaseCode.Equals("17"))
            {
                return KIAT_STRUCT.Federation;
            }
            else if (departmentbaseCode.StartsWith("172") && departmentbaseCode.Length == 5)
            {
                var oblCode = string.Format("{0}{1}", "17", departmentbaseCode.Substring(3, 2));

                using (KasabaKIATEntities db = new KasabaKIATEntities())
                {
                    var kod = db.tbsp_soato_new.Where(z => z.KODI == oblCode).FirstOrDefault();

                    if (kod != null)
                    {
                        return KIAT_STRUCT.Oblsovprof;
                    }
                }
                    
            }
            else if (departmentbaseCode.StartsWith("173") && departmentbaseCode.Length == 10)
            {
                var tumanCode = string.Format("{0}{1}", "17", departmentbaseCode.Substring(5, 5));

                using (KasabaKIATEntities db = new KasabaKIATEntities())
                {
                    var branchcd = departmentbaseCode.Substring(3, 2);

                    var kod = db.sp_branch.Where(z => z.code == branchcd).FirstOrDefault();

                    var kod2 = db.tbsp_soato_new.Where(z => z.KODI == tumanCode).FirstOrDefault();

                    if ((kod != null) && (kod2 != null))
                    {
                        return KIAT_STRUCT.TumanBranch;
                    }
                }
            }
            else if (departmentbaseCode.StartsWith("173") && departmentbaseCode.Length == 14)
            {
                var tumanCode = string.Format("{0}{1}", "17", departmentbaseCode.Substring(5, 5));

                using (KasabaKIATEntities db = new KasabaKIATEntities())
                {
                    var branchcd = departmentbaseCode.Substring(3, 2);

                    var kod = db.sp_branch.Where(z => z.code == branchcd).FirstOrDefault();

                    var kod2 = db.tbsp_soato_new.Where(z => z.KODI == tumanCode).FirstOrDefault();

                    if ((kod != null) && (kod2 != null))
                    {
                        return KIAT_STRUCT.PPO;
                    }
                }
            }

            return KIAT_STRUCT.UnKnown;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="departmentbaseCode"></param>
        /// <returns></returns>
        public KIAT_OBJECT_TYPE GetWhoIsThisCodeN(string departmentbaseCode)
        {
            if (string.IsNullOrEmpty(departmentbaseCode))
            {
                return KIAT_OBJECT_TYPE.UnKnown;
            }
            else if (departmentbaseCode.Equals("17"))
            {
                return KIAT_OBJECT_TYPE.Department;
            }
            else if (departmentbaseCode.Equals("172"))
            {
                return KIAT_OBJECT_TYPE.Department;
            }
            else if (departmentbaseCode.Equals("174"))
            {
                return KIAT_OBJECT_TYPE.Sanatorium;
            }
            else if (departmentbaseCode.Equals("175"))
            {
                return KIAT_OBJECT_TYPE.Camp;
            }
            else if (departmentbaseCode.StartsWith("172") && departmentbaseCode.Length == 5)
            {
                return KIAT_OBJECT_TYPE.Department;
            }
            else if (departmentbaseCode.StartsWith("173") && departmentbaseCode.Length == 10)
            {
                return KIAT_OBJECT_TYPE.Department;
            }
            else if (departmentbaseCode.Contains("Ц"))
            {
                return KIAT_OBJECT_TYPE.Workshop;
            }
            else if (departmentbaseCode.Contains("Г"))
            {
                return KIAT_OBJECT_TYPE.GroupDepartment;
            }
            else
            {
                return KIAT_OBJECT_TYPE.Bkut;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="departmentbaseCode"></param>
        /// <param name="data_depCode"></param>
        /// <returns></returns>
        public bool IsAllowToWatch(string departmentbaseCode, string data_depCode)
        {
            return data_depCode.StartsWith(departmentbaseCode) || data_depCode.Equals(departmentbaseCode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="departmentbaseCode"></param>
        /// <param name="data_depCode"></param>
        /// <returns></returns>
        public bool IsParent(string departmentbaseCode, string data_depCode)
        {
            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var kod = db.t_departmentbase.Where(z => z.code == departmentbaseCode && z.parent_code == data_depCode).FirstOrDefault();

                if (kod != null)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="departmentbaseCode"></param>
        /// <returns></returns>
        public string GetBrachCode(string departmentbaseCode)
        {
            if (!string.IsNullOrEmpty(departmentbaseCode))
             if (departmentbaseCode.StartsWith("173") && departmentbaseCode.Length >= 5)
             {
                 return departmentbaseCode.Substring(3, 2);
             }

            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="departmentbaseCode"></param>
        /// <returns></returns>
        public string GetTumanCode(string departmentbaseCode)
        {
            if (!string.IsNullOrEmpty(departmentbaseCode))
            {
                if (departmentbaseCode.StartsWith("173") && departmentbaseCode.Length == 10)
                {
                    var tumanCode = string.Format("{0}{1}", "17", departmentbaseCode.Substring(5, 5));

                    return tumanCode;
                }
                else if (departmentbaseCode.StartsWith("173") && departmentbaseCode.Length == 14)
                {
                    var tumanCode = string.Format("{0}{1}", "17", departmentbaseCode.Substring(5, 5));

                    return tumanCode;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="departmentbaseCode"></param>
        /// <returns></returns>
        public string GetOrderNumber(string departmentbaseCode)
        {
            if (departmentbaseCode.StartsWith("173") && departmentbaseCode.Length == 14)
            {
                var orderNumber = string.Format("{0}{1}", "17", departmentbaseCode.Substring(10, 4));

                return orderNumber;
            }

            return "0000";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string GetAsos(KIAT_STRUCT str)
        {
            string asos = string.Empty;

            if (KIAT_STRUCT.Federation == str)
            {
                asos = federationOrgs;
            }
            else if (KIAT_STRUCT.Oblsovprof == str)
            {
                asos = regionalOrg;
            }
            else if (KIAT_STRUCT.TumanBranch == str)
            {
                asos = branchOrg;
            }
            else if (KIAT_STRUCT.PPO == str)
            {
                asos = branchOrg;
            }

            return asos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="branchCode"></param>
        /// <param name="tumanCode"></param>
        /// <returns></returns>
        private string TransformPoluFabrikat(KIAT_STRUCT str, string branchCode = "", string tumanCode = "")
        {
            var asos = GetAsos(str);

            if (KIAT_STRUCT.Federation == str)
            {
                //
            }
            else if (KIAT_STRUCT.Oblsovprof == str)
            {
                //
            }
            else if (KIAT_STRUCT.TumanBranch == str)
            {
                if (!string.IsNullOrEmpty(branchCode) && !string.IsNullOrEmpty(tumanCode))
                {
                    if ((tumanCode.Length == 7) && (branchCode.Length == 2))
                    {
                        var tumanCode1 = tumanCode.Substring(2);

                        asos = string.Format("{0}{1}{2}", asos, branchCode, tumanCode1);
                    }
                    else
                    {
                        throw new ApplicationException("branchCode & tumanCode have invalid length");
                    }
                }
                else
                {
                    throw new ApplicationException("branchCode & tumanCode is empty");
                }
            }
            else if (KIAT_STRUCT.PPO == str)
            {
                if (!string.IsNullOrEmpty(branchCode) && !string.IsNullOrEmpty(tumanCode))
                {
                    if ((tumanCode.Length == 7) && (branchCode.Length == 2))
                    {
                        var tumanCode1 = tumanCode.Substring(2);

                        asos = string.Format("{0}{1}{2}", asos, branchCode, tumanCode1);
                    }
                    else
                    {
                        throw new ApplicationException("branchCode & tumanCode have invalid length");
                    }
                }
                else
                {
                    throw new ApplicationException("branchCode & tumanCode is empty");
                }
            }

            return asos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="branchCode"></param>
        /// <param name="tumanCode"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        private string GetNextOrder(KIAT_STRUCT str, string branchCode = "", string tumanCode = "", long ID = 0)
        {
            var uzak = TransformPoluFabrikat(str, branchCode, tumanCode);

            if (KIAT_STRUCT.TumanBranch == str)
            {
                return uzak;
            }
            else if (KIAT_STRUCT.PPO == str)
            {
                var IdLast = 3000 + ID;

                return string.Format("{0}{1}", uzak, IdLast.ToString());
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="branchCode"></param>
        /// <param name="tumanCode"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string GenerateDepartmentCode(KIAT_STRUCT str, string branchCode = "", string tumanCode = "", long ID = 0)
        {
            var departmentCode = GetNextOrder(str, branchCode, tumanCode, ID);

            return departmentCode;
        }

        /// <summary>
        /// For BKUTS
        /// </summary>
        /// <param name="nodeCode"></param>
        /// <param name="branchCode"></param>
        /// <param name="tumanCode"></param>
        /// <returns></returns>
        public string GenerateBkutDepCode(string nodeCode, string branchCode, string tumanCode)
        {
            return "";
        }

        /// <summary>
        /// 173 И1 06401 0132 Ц 036
        /// </summary>
        /// <param name="nodeCode">173</param>
        /// <param name="branchCode">И1</param>
        /// <param name="tumanCode">06401</param>
        /// <returns></returns>
        public string GenerateWorkshopDepCode(string nodeCode, string branchCode, string tumanCode)
        {
            return "";
        }

        /// <summary>
        /// 173 С1 06    0152 Г 062
        /// 173 И1 06401 0175 Г 001
        /// </summary>
        /// <param name="nodeCode"></param>
        /// <param name="branchCode"></param>
        /// <param name="tumanCode"></param>
        /// <returns></returns>
        public string GenerateGroupDepartmentDepCode(string nodeCode, string branchCode, string tumanCode)
        {
            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="departmentCode"></param>
        /// <returns></returns>
        public string GetParentOrganizationCode(string departmentCode)
        {
            if (string.IsNullOrEmpty(departmentCode))
            {
                return null;
            }

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var org = db.tblSprav_Org.Where(z => z.OWNER_CODE == departmentCode);

                if (!org.Any())
                {
                    if (departmentCode.Length == 5)
                    {
                        if (departmentCode.Substring(0, 3).Equals("172"))
                        {
                            return "17";
                        }
                    }
                    else if (departmentCode.Length == 2)
                    {
                        if (departmentCode.Equals("17"))
                        {
                            return "17";
                        }
                    }

                }

                var organization = org.First();


                if (organization.ParentID == null || organization.ParentID == -1)
                {
                    return organization.DepartmentBase;
                }
                else
                {
                    var parOrg = db.tblSprav_Org.Where(z => z.ID == organization.ParentID);

                    if (!parOrg.Any()) return organization.DepartmentBase;

                    return parOrg.First().OWNER_CODE;
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="departmentCode"></param>
        /// <returns></returns>
        public string GetParentOrganizationName(string departmentCode)
        {
            //var departmentcd = GetParentOrganizationCode(departmentCode);

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                var org = db.tblSprav_Org.Where(z => z.OWNER_CODE == departmentCode);

                if (org.Any())
                {
                    var orgs = org.First();

                    return orgs.Name_Org;
                }

                var depts = db.t_departmentbase.Where(z => z.code == departmentCode);

                if (depts.Any())
                {
                    var deppp = depts.First();

                    if (!string.IsNullOrEmpty(deppp.name_ru))
                    {
                        return deppp.name_ru;
                    }
                    else if (!string.IsNullOrEmpty(deppp.dep_name))
                    {
                        return deppp.dep_name;
                    }
                    else if (!string.IsNullOrEmpty(deppp.bkut_name))
                    {
                        return deppp.bkut_name;
                    }
                }
            }

            return string.Empty;
        }

    }
}