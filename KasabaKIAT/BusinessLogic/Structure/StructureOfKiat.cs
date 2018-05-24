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
        public string federation  = "17";

        public string regionalOrg = "172";

        public string branchOrg   = "173";

        public string Sanatorium  = "174";

        public string Camp        = "175";

        //Oblsovprof begins with 172XX - XX region code f.e. 17203 - Andijan OblSovprof TABLE in DB - tbku_StrokaProfsoyza

        //branch ogranization 173XXYYYYY - XX branch code TABLE in DB sp_branch, YYYYY - tuman code from SOATO Reference leave 17 from begin.

        //BKUT 173XXYYYYYZZZZ - XX branch code TABLE in DB sp_branch,  YYYYY - tuman code from SOATO Reference leave 17 from begin.  ZZZZ - order number for example - 173Д1032030024 

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

        public bool IsFederation(string departmentbaseCode)
        {
            return false;
        }

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
    }
}