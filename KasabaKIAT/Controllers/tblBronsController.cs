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
    public class tblBronsController : Controller
    {
        private KasabaKIATEntities db = new KasabaKIATEntities();

        // GET: tblBrons
        public ActionResult Index()
        {
            var tblBron = db.tblBron.Include(t => t.tblSprav_San).Include(t => t.users);
            return View(tblBron.ToList());
        }

        // GET: tblBrons/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblBron tblBron = db.tblBron.Find(id);
            if (tblBron == null)
            {
                return HttpNotFound();
            }
            return View(tblBron);
        }

        // GET: tblBrons/Create
        public ActionResult Create()
        {
            ViewBag.ID_San = new SelectList(db.tblSprav_San, "ID", "Name_San");
            ViewBag.UserId = new SelectList(db.users, "id", "email");
            return View();
        }

        // POST: tblBrons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ID_San,Num_Put,Prem,Uchet,ExpireDate,UserId,DepartmentBase,TIME_STAMP")] tblBron tblBron)
        {
            if (ModelState.IsValid)
            {
                db.tblBron.Add(tblBron);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID_San = new SelectList(db.tblSprav_San, "ID", "Name_San", tblBron.ID_San);
            ViewBag.UserId = new SelectList(db.users, "id", "email", tblBron.UserId);
            return View(tblBron);
        }

        // GET: tblBrons/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblBron tblBron = db.tblBron.Find(id);
            if (tblBron == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_San = new SelectList(db.tblSprav_San, "ID", "Name_San", tblBron.ID_San);
            ViewBag.UserId = new SelectList(db.users, "id", "email", tblBron.UserId);
            return View(tblBron);
        }

        // POST: tblBrons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ID_San,Num_Put,Prem,Uchet,ExpireDate,UserId,DepartmentBase,TIME_STAMP")] tblBron tblBron)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblBron).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_San = new SelectList(db.tblSprav_San, "ID", "Name_San", tblBron.ID_San);
            ViewBag.UserId = new SelectList(db.users, "id", "email", tblBron.UserId);
            return View(tblBron);
        }

        // GET: tblBrons/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblBron tblBron = db.tblBron.Find(id);
            if (tblBron == null)
            {
                return HttpNotFound();
            }
            return View(tblBron);
        }

        // POST: tblBrons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            tblBron tblBron = db.tblBron.Find(id);
            db.tblBron.Remove(tblBron);
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
