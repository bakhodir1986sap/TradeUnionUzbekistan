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
    public class tblSprav_MFOController : Controller
    {
        private KasabaKIATEntities db = new KasabaKIATEntities();

        // GET: tblSprav_MFO
        public ActionResult Index(int? page)
        {
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(db.tblSprav_MFO.ToList().ToPagedList(pageNumber, pageSize));
        }

        // GET: tblSprav_MFO/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSprav_MFO tblSprav_MFO = db.tblSprav_MFO.Find(id);
            if (tblSprav_MFO == null)
            {
                return HttpNotFound();
            }
            return View(tblSprav_MFO);
        }

        // GET: tblSprav_MFO/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.users, "id", "email");
            return View();
        }

        // POST: tblSprav_MFO/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Code_MFO,Name_MFO,Adres_MFO,Data_Act,Data_Cls,UserId")] tblSprav_MFO tblSprav_MFO)
        {
            if (ModelState.IsValid)
            {
                db.tblSprav_MFO.Add(tblSprav_MFO);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.users, "id", "email", tblSprav_MFO.UserId);
            return View(tblSprav_MFO);
        }

        // GET: tblSprav_MFO/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSprav_MFO tblSprav_MFO = db.tblSprav_MFO.Find(id);
            if (tblSprav_MFO == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.users, "id", "email", tblSprav_MFO.UserId);
            return View(tblSprav_MFO);
        }

        // POST: tblSprav_MFO/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Code_MFO,Name_MFO,Adres_MFO,Data_Act,Data_Cls,UserId")] tblSprav_MFO tblSprav_MFO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblSprav_MFO).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.users, "id", "email", tblSprav_MFO.UserId);
            return View(tblSprav_MFO);
        }

        // GET: tblSprav_MFO/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSprav_MFO tblSprav_MFO = db.tblSprav_MFO.Find(id);
            if (tblSprav_MFO == null)
            {
                return HttpNotFound();
            }
            return View(tblSprav_MFO);
        }

        // POST: tblSprav_MFO/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            tblSprav_MFO tblSprav_MFO = db.tblSprav_MFO.Find(id);
            db.tblSprav_MFO.Remove(tblSprav_MFO);
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
