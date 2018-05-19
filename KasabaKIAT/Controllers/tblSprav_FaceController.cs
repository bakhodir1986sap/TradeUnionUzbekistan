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
    public class tblSprav_FaceController : Controller
    {
        private KasabaKIATEntities db = new KasabaKIATEntities();

        // GET: tblSprav_Face
        public ActionResult Index()
        {
            var tblSprav_Face = db.tblSprav_Face.Include(t => t.users).Take(50);
            return View(tblSprav_Face.ToList());
        }

        // GET: tblSprav_Face/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSprav_Face tblSprav_Face = db.tblSprav_Face.Find(id);
            if (tblSprav_Face == null)
            {
                return HttpNotFound();
            }
            return View(tblSprav_Face);
        }

        // GET: tblSprav_Face/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.users, "id", "email");
            return View();
        }

        // POST: tblSprav_Face/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name_Os,UserId")] tblSprav_Face tblSprav_Face)
        {
            if (ModelState.IsValid)
            {
                db.tblSprav_Face.Add(tblSprav_Face);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.users, "id", "email", tblSprav_Face.UserId);
            return View(tblSprav_Face);
        }

        // GET: tblSprav_Face/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSprav_Face tblSprav_Face = db.tblSprav_Face.Find(id);
            if (tblSprav_Face == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.users, "id", "email", tblSprav_Face.UserId);
            return View(tblSprav_Face);
        }

        // POST: tblSprav_Face/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name_Os,UserId")] tblSprav_Face tblSprav_Face)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblSprav_Face).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.users, "id", "email", tblSprav_Face.UserId);
            return View(tblSprav_Face);
        }

        // GET: tblSprav_Face/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSprav_Face tblSprav_Face = db.tblSprav_Face.Find(id);
            if (tblSprav_Face == null)
            {
                return HttpNotFound();
            }
            return View(tblSprav_Face);
        }

        // POST: tblSprav_Face/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            tblSprav_Face tblSprav_Face = db.tblSprav_Face.Find(id);
            db.tblSprav_Face.Remove(tblSprav_Face);
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
