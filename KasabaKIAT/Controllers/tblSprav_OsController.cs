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
    public class tblSprav_OsController : Controller
    {
        private KasabaKIATEntities db = new KasabaKIATEntities();

        // GET: tblSprav_Os
        public ActionResult Index()
        {
            var tblSprav_Os = db.tblSprav_Os.Include(t => t.users).Take(50);
            return View(tblSprav_Os.ToList());
        }

        // GET: tblSprav_Os/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSprav_Os tblSprav_Os = db.tblSprav_Os.Find(id);
            if (tblSprav_Os == null)
            {
                return HttpNotFound();
            }
            return View(tblSprav_Os);
        }

        // GET: tblSprav_Os/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.users, "id", "email");
            return View();
        }

        // POST: tblSprav_Os/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name_Os,UserId")] tblSprav_Os tblSprav_Os)
        {
            if (ModelState.IsValid)
            {
                db.tblSprav_Os.Add(tblSprav_Os);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.users, "id", "email", tblSprav_Os.UserId);
            return View(tblSprav_Os);
        }

        // GET: tblSprav_Os/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSprav_Os tblSprav_Os = db.tblSprav_Os.Find(id);
            if (tblSprav_Os == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.users, "id", "email", tblSprav_Os.UserId);
            return View(tblSprav_Os);
        }

        // POST: tblSprav_Os/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name_Os,UserId")] tblSprav_Os tblSprav_Os)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblSprav_Os).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.users, "id", "email", tblSprav_Os.UserId);
            return View(tblSprav_Os);
        }

        // GET: tblSprav_Os/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSprav_Os tblSprav_Os = db.tblSprav_Os.Find(id);
            if (tblSprav_Os == null)
            {
                return HttpNotFound();
            }
            return View(tblSprav_Os);
        }

        // POST: tblSprav_Os/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            tblSprav_Os tblSprav_Os = db.tblSprav_Os.Find(id);
            db.tblSprav_Os.Remove(tblSprav_Os);
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
