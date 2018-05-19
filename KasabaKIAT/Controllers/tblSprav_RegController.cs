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
    public class tblSprav_RegController : Controller
    {
        private KasabaKIATEntities db = new KasabaKIATEntities();

        // GET: tblSprav_Reg
        public ActionResult Index()
        {
            var tblSprav_Reg = db.tblSprav_Reg.Include(t => t.users).Take(50);
            return View(tblSprav_Reg.ToList());
        }

        // GET: tblSprav_Reg/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSprav_Reg tblSprav_Reg = db.tblSprav_Reg.Find(id);
            if (tblSprav_Reg == null)
            {
                return HttpNotFound();
            }
            return View(tblSprav_Reg);
        }

        // GET: tblSprav_Reg/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.users, "id", "email");
            return View();
        }

        // POST: tblSprav_Reg/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,NameReg,CodeReg,UserId,DepartmentBase,VilId,SOATO_CODE")] tblSprav_Reg tblSprav_Reg)
        {
            if (ModelState.IsValid)
            {
                db.tblSprav_Reg.Add(tblSprav_Reg);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.users, "id", "email", tblSprav_Reg.UserId);
            return View(tblSprav_Reg);
        }

        // GET: tblSprav_Reg/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSprav_Reg tblSprav_Reg = db.tblSprav_Reg.Find(id);
            if (tblSprav_Reg == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.users, "id", "email", tblSprav_Reg.UserId);
            return View(tblSprav_Reg);
        }

        // POST: tblSprav_Reg/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,NameReg,CodeReg,UserId,DepartmentBase,VilId,SOATO_CODE")] tblSprav_Reg tblSprav_Reg)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblSprav_Reg).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.users, "id", "email", tblSprav_Reg.UserId);
            return View(tblSprav_Reg);
        }

        // GET: tblSprav_Reg/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSprav_Reg tblSprav_Reg = db.tblSprav_Reg.Find(id);
            if (tblSprav_Reg == null)
            {
                return HttpNotFound();
            }
            return View(tblSprav_Reg);
        }

        // POST: tblSprav_Reg/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            tblSprav_Reg tblSprav_Reg = db.tblSprav_Reg.Find(id);
            db.tblSprav_Reg.Remove(tblSprav_Reg);
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
