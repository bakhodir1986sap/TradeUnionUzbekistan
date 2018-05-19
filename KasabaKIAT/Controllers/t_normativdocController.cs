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
    public class t_normativdocController : Controller
    {
        private KasabaKIATEntities db = new KasabaKIATEntities();

        // GET: t_normativdoc
        public ActionResult Index()
        {
            var t_normativdoc = db.t_normativdoc.Include(t => t.sp_doctype).Include(t => t.users);
            return View(t_normativdoc.ToList());
        }

        // GET: t_normativdoc/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_normativdoc t_normativdoc = db.t_normativdoc.Find(id);
            if (t_normativdoc == null)
            {
                return HttpNotFound();
            }
            return View(t_normativdoc);
        }

        // GET: t_normativdoc/Create
        public ActionResult Create()
        {
            ViewBag.doctype = new SelectList(db.sp_doctype, "id", "keys");
            ViewBag.s_user_id = new SelectList(db.users, "id", "email");
            return View();
        }

        // POST: t_normativdoc/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,version,code,date,s_comment,s_create,s_status,name_ru,name_uz,name_uzl,s_user_id,doctype,url,shortmean,NATIVE_ID")] t_normativdoc t_normativdoc)
        {
            if (ModelState.IsValid)
            {
                db.t_normativdoc.Add(t_normativdoc);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.doctype = new SelectList(db.sp_doctype, "id", "keys", t_normativdoc.doctype);
            ViewBag.s_user_id = new SelectList(db.users, "id", "email", t_normativdoc.s_user_id);
            return View(t_normativdoc);
        }

        // GET: t_normativdoc/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_normativdoc t_normativdoc = db.t_normativdoc.Find(id);
            if (t_normativdoc == null)
            {
                return HttpNotFound();
            }
            ViewBag.doctype = new SelectList(db.sp_doctype, "id", "keys", t_normativdoc.doctype);
            ViewBag.s_user_id = new SelectList(db.users, "id", "email", t_normativdoc.s_user_id);
            return View(t_normativdoc);
        }

        // POST: t_normativdoc/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,version,code,date,s_comment,s_create,s_status,name_ru,name_uz,name_uzl,s_user_id,doctype,url,shortmean,NATIVE_ID")] t_normativdoc t_normativdoc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(t_normativdoc).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.doctype = new SelectList(db.sp_doctype, "id", "keys", t_normativdoc.doctype);
            ViewBag.s_user_id = new SelectList(db.users, "id", "email", t_normativdoc.s_user_id);
            return View(t_normativdoc);
        }

        // GET: t_normativdoc/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_normativdoc t_normativdoc = db.t_normativdoc.Find(id);
            if (t_normativdoc == null)
            {
                return HttpNotFound();
            }
            return View(t_normativdoc);
        }

        // POST: t_normativdoc/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            t_normativdoc t_normativdoc = db.t_normativdoc.Find(id);
            db.t_normativdoc.Remove(t_normativdoc);
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
