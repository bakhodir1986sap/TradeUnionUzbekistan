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
    public class tblSprav_PrController : Controller
    {
        private KasabaKIATEntities db = new KasabaKIATEntities();

        // GET: tblSprav_Pr
        public ActionResult Index()
        {
            var tblSprav_Pr = db.tblSprav_Pr.Include(t => t.users);
            return View(tblSprav_Pr.ToList());
        }

        // GET: tblSprav_Pr/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSprav_Pr tblSprav_Pr = db.tblSprav_Pr.Find(id);
            if (tblSprav_Pr == null)
            {
                return HttpNotFound();
            }
            return View(tblSprav_Pr);
        }

        // GET: tblSprav_Pr/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.users, "id", "email");
            return View();
        }

        // POST: tblSprav_Pr/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name_Proc,Num,UserId")] tblSprav_Pr tblSprav_Pr)
        {
            if (ModelState.IsValid)
            {
                db.tblSprav_Pr.Add(tblSprav_Pr);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.users, "id", "email", tblSprav_Pr.UserId);
            return View(tblSprav_Pr);
        }

        // GET: tblSprav_Pr/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSprav_Pr tblSprav_Pr = db.tblSprav_Pr.Find(id);
            if (tblSprav_Pr == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.users, "id", "email", tblSprav_Pr.UserId);
            return View(tblSprav_Pr);
        }

        // POST: tblSprav_Pr/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name_Proc,Num,UserId")] tblSprav_Pr tblSprav_Pr)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblSprav_Pr).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.users, "id", "email", tblSprav_Pr.UserId);
            return View(tblSprav_Pr);
        }

        // GET: tblSprav_Pr/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSprav_Pr tblSprav_Pr = db.tblSprav_Pr.Find(id);
            if (tblSprav_Pr == null)
            {
                return HttpNotFound();
            }
            return View(tblSprav_Pr);
        }

        // POST: tblSprav_Pr/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            tblSprav_Pr tblSprav_Pr = db.tblSprav_Pr.Find(id);
            db.tblSprav_Pr.Remove(tblSprav_Pr);
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
