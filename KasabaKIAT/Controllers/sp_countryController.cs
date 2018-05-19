using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KasabaKIAT.Models;

namespace KasabaKIAT.Controllers
{
    public class sp_countryController : Controller
    {
        private KasabaKIATEntities db = new KasabaKIATEntities();

        // GET: sp_country
        public ActionResult Index()
        {
            var sp_country = db.sp_country.Include(s => s.users).Take(50);
            return View(sp_country.ToList());
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
