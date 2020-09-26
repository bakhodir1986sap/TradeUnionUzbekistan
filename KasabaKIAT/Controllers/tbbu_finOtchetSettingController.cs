using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KasabaKIAT.Models;
using PagedList.Mvc;
using PagedList;

namespace KasabaKIAT.Controllers
{
    public class finOtchetSetting
    {
            public int otchet_part	    {get; set;}
            public string code	            {get; set;}
            public string debit_side	    {get; set;}
            public string credit_side	    {get; set;}
            public string account_code	    {get; set;}
            public int? data_type	        {get; set;}
            public string data_desc	        {get; set;}
            public string department_base { get; set; }
    }

    public class tbbu_finOtchetSettingController : Controller
    {
        private KasabaKIATEntities db = new KasabaKIATEntities();

        // GET: tbbu_finOtchetSetting
        public ActionResult Index(int? page)
        {
            var list = new List<finOtchetSetting>();

            foreach (var obj in db.tbbu_finOtchetSetting)
            {
                var model = new finOtchetSetting();
                model.otchet_part	    = obj.OTCHET_PART	   ;  
                model.code	            = obj.CODE	           ;
                model.debit_side	    = obj.DEBIT_SIDE	   ;
                model.credit_side	    = obj.CREDIT_SIDE	   ;
                model.account_code	    = obj.ACCOUNT_CODE	   ;
                model.data_type	        = obj.DATA_TYPE	       ;
                model.data_desc	        = obj.DATA_DESC	       ;
                model.department_base = obj.DEPARTMENT_BASE;

                list.Add(model);
            }

            ViewBag.datasource = list;

            return View();
        }

        // GET: tbbu_finOtchetSetting/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbbu_finOtchetSetting tbbu_finOtchetSetting = await db.tbbu_finOtchetSetting.FindAsync(id);
            if (tbbu_finOtchetSetting == null)
            {
                return HttpNotFound();
            }
            return View(tbbu_finOtchetSetting);
        }

        // GET: tbbu_finOtchetSetting/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: tbbu_finOtchetSetting/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "REC_ID,OTCHET_PART,CODE,DEBIT_SIDE,CREDIT_SIDE,ACCOUNT_CODE,DATA_TYPE,DATA_DESC,DEPARTMENT_BASE,USR_ID")] tbbu_finOtchetSetting tbbu_finOtchetSetting)
        {
            if (ModelState.IsValid)
            {
                db.tbbu_finOtchetSetting.Add(tbbu_finOtchetSetting);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(tbbu_finOtchetSetting);
        }

        // GET: tbbu_finOtchetSetting/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbbu_finOtchetSetting tbbu_finOtchetSetting = await db.tbbu_finOtchetSetting.FindAsync(id);
            if (tbbu_finOtchetSetting == null)
            {
                return HttpNotFound();
            }
            return View(tbbu_finOtchetSetting);
        }

        // POST: tbbu_finOtchetSetting/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "REC_ID,OTCHET_PART,CODE,DEBIT_SIDE,CREDIT_SIDE,ACCOUNT_CODE,DATA_TYPE,DATA_DESC,DEPARTMENT_BASE,USR_ID")] tbbu_finOtchetSetting tbbu_finOtchetSetting)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbbu_finOtchetSetting).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(tbbu_finOtchetSetting);
        }

        // GET: tbbu_finOtchetSetting/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbbu_finOtchetSetting tbbu_finOtchetSetting = await db.tbbu_finOtchetSetting.FindAsync(id);
            if (tbbu_finOtchetSetting == null)
            {
                return HttpNotFound();
            }
            return View(tbbu_finOtchetSetting);
        }

        // POST: tbbu_finOtchetSetting/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            tbbu_finOtchetSetting tbbu_finOtchetSetting = await db.tbbu_finOtchetSetting.FindAsync(id);
            db.tbbu_finOtchetSetting.Remove(tbbu_finOtchetSetting);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
