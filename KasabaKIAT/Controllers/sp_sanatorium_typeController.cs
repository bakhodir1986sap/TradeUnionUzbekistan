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
    public class sp_sanatorium_typeController : Controller
    {
        private KasabaKIATEntities db = new KasabaKIATEntities();

        // GET: sp_sanatorium_type
        public ActionResult Index()
        {
            var sp_sanatorium_type = db.sp_sanatorium_type.Include(s => s.users).Take(50);
            return View(sp_sanatorium_type.ToList());
        }

        // GET: sp_sanatorium_type/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_sanatorium_type sp_sanatorium_type = db.sp_sanatorium_type.Find(id);
            if (sp_sanatorium_type == null)
            {
                return HttpNotFound();
            }
            return View(sp_sanatorium_type);
        }

        // GET: sp_sanatorium_type/Create
        public ActionResult Create()
        {
            ViewBag.s_user_id = new SelectList(db.users, "id", "email");
            return View();
        }

        // POST: sp_sanatorium_type/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,version,s_comment,s_create,s_status,name_ru,name_uz,name_uzl,s_user_id,NATIVE_ID")] sp_sanatorium_type sp_sanatorium_type)
        {
            if (ModelState.IsValid)
            {
                db.sp_sanatorium_type.Add(sp_sanatorium_type);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.s_user_id = new SelectList(db.users, "id", "email", sp_sanatorium_type.s_user_id);
            return View(sp_sanatorium_type);
        }

        // GET: sp_sanatorium_type/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_sanatorium_type sp_sanatorium_type = db.sp_sanatorium_type.Find(id);
            if (sp_sanatorium_type == null)
            {
                return HttpNotFound();
            }
            ViewBag.s_user_id = new SelectList(db.users, "id", "email", sp_sanatorium_type.s_user_id);
            return View(sp_sanatorium_type);
        }

        // POST: sp_sanatorium_type/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,version,s_comment,s_create,s_status,name_ru,name_uz,name_uzl,s_user_id,NATIVE_ID")] sp_sanatorium_type sp_sanatorium_type)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sp_sanatorium_type).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.s_user_id = new SelectList(db.users, "id", "email", sp_sanatorium_type.s_user_id);
            return View(sp_sanatorium_type);
        }

        // GET: sp_sanatorium_type/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_sanatorium_type sp_sanatorium_type = db.sp_sanatorium_type.Find(id);
            if (sp_sanatorium_type == null)
            {
                return HttpNotFound();
            }
            return View(sp_sanatorium_type);
        }

        // POST: sp_sanatorium_type/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            sp_sanatorium_type sp_sanatorium_type = db.sp_sanatorium_type.Find(id);
            db.sp_sanatorium_type.Remove(sp_sanatorium_type);
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
