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
    public class sp_degreeController : Controller
    {
        private KasabaKIATEntities db = new KasabaKIATEntities();

        // GET: sp_degree
        public ActionResult Index()
        {
            var sp_degree = db.sp_degree.Include(s => s.users);
            return View(sp_degree.ToList());
        }

        // GET: sp_degree/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_degree sp_degree = db.sp_degree.Find(id);
            if (sp_degree == null)
            {
                return HttpNotFound();
            }
            return View(sp_degree);
        }

        // GET: sp_degree/Create
        public ActionResult Create()
        {
            ViewBag.s_user_id = new SelectList(db.users, "id", "email");
            return View();
        }

        // POST: sp_degree/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,version,s_comment,s_create,s_status,name_ru,name_uz,name_uzl,s_user_id,NATIVE_ID")] sp_degree sp_degree)
        {
            if (ModelState.IsValid)
            {
                db.sp_degree.Add(sp_degree);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.s_user_id = new SelectList(db.users, "id", "email", sp_degree.s_user_id);
            return View(sp_degree);
        }

        // GET: sp_degree/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_degree sp_degree = db.sp_degree.Find(id);
            if (sp_degree == null)
            {
                return HttpNotFound();
            }
            ViewBag.s_user_id = new SelectList(db.users, "id", "email", sp_degree.s_user_id);
            return View(sp_degree);
        }

        // POST: sp_degree/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,version,s_comment,s_create,s_status,name_ru,name_uz,name_uzl,s_user_id,NATIVE_ID")] sp_degree sp_degree)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sp_degree).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.s_user_id = new SelectList(db.users, "id", "email", sp_degree.s_user_id);
            return View(sp_degree);
        }

        // GET: sp_degree/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_degree sp_degree = db.sp_degree.Find(id);
            if (sp_degree == null)
            {
                return HttpNotFound();
            }
            return View(sp_degree);
        }

        // POST: sp_degree/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            sp_degree sp_degree = db.sp_degree.Find(id);
            db.sp_degree.Remove(sp_degree);
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
