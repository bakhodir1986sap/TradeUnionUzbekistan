using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KasabaKIAT.Models;
using PagedList.Mvc;
using PagedList;

namespace KasabaKIAT.Controllers
{
    public class countries
    {
        public long id { get; set; }

        public string code { get; set; }

        public string name_ru { get; set; }

        public string name_uz { get; set; }

        public string name_uzl { get; set; }

        public string create_date { get; set; }
    }

    public class sp_countryController : Controller
    {
        private KasabaKIATEntities db = new KasabaKIATEntities();

        // GET: sp_country
        public ActionResult Index(int? page)
        {
            return View();
        }

        public JsonResult UrlDatasource()
        {
            var list = new List<countries>();

            foreach (var obj in db.sp_country.ToList())
            {
                var model = new countries();
                model.id = obj.id;
                model.code = obj.code;
                model.create_date = obj.s_create.Value.ToString("dd.MM.yyyy");
                model.name_ru = obj.name_ru;
                model.name_uz = obj.name_uz;
                model.name_uzl = obj.name_uzl;

                list.Add(model);
            }

            if (db.sp_country.Count() == 0) Json(new { result = db.sp_country.ToList(), count = 0 });

            return Json(new { result = list, count = list.Count }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Insert([System.Web.Http.FromBody]branches value)
        {
            //Insert record in database .

            var objInsert = new sp_country();
            objInsert.version = 0;
            objInsert.code = value.code;
            objInsert.name_ru = value.name_ru;
            objInsert.name_uz = value.name_uz;
            objInsert.name_uzl = value.name_uzl;
            objInsert.s_create = DateTime.Now;

            db.sp_country.Add(objInsert);
            db.SaveChanges();

            return Json(value, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update([System.Web.Http.FromBody]branches value)
        {
            //update record in database .
            if (value == null) return null;

            var ObjUpdate = db.sp_country.First(z => z.id == value.id);

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

            var ObjDelete = db.sp_country.First(z => z.id == key);

            db.sp_country.Remove(ObjDelete);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: sp_country/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_country sp_country = db.sp_country.Find(id);
            if (sp_country == null)
            {
                return HttpNotFound();
            }
            return View(sp_country);
        }

        // GET: sp_country/Create
        public ActionResult Create()
        {
            ViewBag.s_user_id = new SelectList(db.users, "id", "email");
            return View();
        }

        // POST: sp_country/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,version,code,s_comment,s_create,s_status,name_ru,name_uz,name_uzl,name_short_ru,name_short_uz,name_short_uzl,s_user_id,NATIVE_ID")] sp_country sp_country)
        {
            if (ModelState.IsValid)
            {
                db.sp_country.Add(sp_country);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.s_user_id = new SelectList(db.users, "id", "email", sp_country.s_user_id);
            return View(sp_country);
        }

        // GET: sp_country/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_country sp_country = db.sp_country.Find(id);
            if (sp_country == null)
            {
                return HttpNotFound();
            }
            ViewBag.s_user_id = new SelectList(db.users, "id", "email", sp_country.s_user_id);
            return View(sp_country);
        }

        // POST: sp_country/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,version,code,s_comment,s_create,s_status,name_ru,name_uz,name_uzl,name_short_ru,name_short_uz,name_short_uzl,s_user_id,NATIVE_ID")] sp_country sp_country)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sp_country).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.s_user_id = new SelectList(db.users, "id", "email", sp_country.s_user_id);
            return View(sp_country);
        }

        // GET: sp_country/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_country sp_country = db.sp_country.Find(id);
            if (sp_country == null)
            {
                return HttpNotFound();
            }
            return View(sp_country);
        }

        // POST: sp_country/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            sp_country sp_country = db.sp_country.Find(id);
            db.sp_country.Remove(sp_country);
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
