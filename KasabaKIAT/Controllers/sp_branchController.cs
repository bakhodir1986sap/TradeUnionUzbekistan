using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using KasabaKIAT.Models;

namespace KasabaKIAT.Controllers
{
    public class branches
    {
        public long id { get; set; }

        public string code { get; set; }

        public string name_ru { get; set; }

        public string name_uz { get; set; }

        public string name_uzl { get; set; }

        public string create_date { get; set; }
    }

    public class sp_branchController : Controller
    {
        private KasabaKIATEntities db = new KasabaKIATEntities();

        // GET: sp_branch
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult UrlDatasource()
        {
            var list = new List<branches>();

            foreach (var obj in db.sp_branch.ToList())
            {
                var model = new branches();
                model.id = obj.id;
                model.code = obj.code;
                model.create_date = obj.s_create.Value.ToString("dd.MM.yyyy");
                model.name_ru = obj.name_ru;
                model.name_uz = obj.name_uz;
                model.name_uzl = obj.name_uzl;

                list.Add(model);
            }

            if (db.sp_branch.Count() == 0) Json(new { result = db.sp_branch.ToList(), count = 0 });

            return Json(new { result = list, count = list.Count }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Insert([System.Web.Http.FromBody]branches value)
        {
            //Insert record in database .

            var objInsert = new sp_branch();
            objInsert.version = 0;
            objInsert.code = value.code;
            objInsert.name_ru = value.name_ru;
            objInsert.name_uz = value.name_uz;
            objInsert.name_uzl = value.name_uzl;
            objInsert.s_create = DateTime.Now;

            db.sp_branch.Add(objInsert);
            db.SaveChanges();

            return Json(value, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update([System.Web.Http.FromBody]branches value)
        {
            //update record in database .
            if (value == null) return null;

            var ObjUpdate = db.sp_branch.First(z => z.id == value.id);

            ObjUpdate.version = 0;
            ObjUpdate.code = value.code;
            ObjUpdate.name_ru = value.name_ru;
            ObjUpdate.name_uz = value.name_uz;
            ObjUpdate.name_uzl = value.name_uzl;

            db.SaveChanges();

            return Json(value, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Remove([System.Web.Http.FromBody]long key)
        {
            //Delete record in database 

            if (key == 0) return null;

            var ObjDelete = db.sp_branch.First(z => z.id == key);

            db.sp_branch.Remove(ObjDelete);
            db.SaveChanges();

            return RedirectToAction("Index");
        }


            // GET: sp_branch/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_branch sp_branch = db.sp_branch.Find(id);
            if (sp_branch == null)
            {
                return HttpNotFound();
            }
            return View(sp_branch);
        }

        // GET: sp_branch/Create
        public ActionResult Create()
        {
            ViewBag.s_user_id = new SelectList(db.users, "id", "email");
            return View();
        }

        // POST: sp_branch/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,version,code,s_comment,s_create,s_status,name_ru,name_uz,name_uzl,s_user_id,NATIVE_ID")] sp_branch sp_branch)
        {
            if (ModelState.IsValid)
            {
                db.sp_branch.Add(sp_branch);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.s_user_id = new SelectList(db.users, "id", "email", sp_branch.s_user_id);
            return View(sp_branch);
        }

        // GET: sp_branch/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_branch sp_branch = db.sp_branch.Find(id);
            if (sp_branch == null)
            {
                return HttpNotFound();
            }
            ViewBag.s_user_id = new SelectList(db.users, "id", "email", sp_branch.s_user_id);
            return View(sp_branch);
        }

        // POST: sp_branch/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,version,code,s_comment,s_create,s_status,name_ru,name_uz,name_uzl,s_user_id,NATIVE_ID")] sp_branch sp_branch)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sp_branch).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.s_user_id = new SelectList(db.users, "id", "email", sp_branch.s_user_id);
            return View(sp_branch);
        }

        // GET: sp_branch/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_branch sp_branch = db.sp_branch.Find(id);
            if (sp_branch == null)
            {
                return HttpNotFound();
            }
            return View(sp_branch);
        }

        // POST: sp_branch/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            sp_branch sp_branch = db.sp_branch.Find(id);
            db.sp_branch.Remove(sp_branch);
            db.SaveChanges();
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
