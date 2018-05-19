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
    public class sp_nationController : Controller
    {
        private KasabaKIATEntities db = new KasabaKIATEntities();

        // GET: sp_nation
        public ActionResult Index()
        {
            var sp_nation = db.sp_nation.Include(s => s.users).Take(50);
            return View(sp_nation.ToList());
        }

        // GET: sp_nation/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_nation sp_nation = db.sp_nation.Find(id);
            if (sp_nation == null)
            {
                return HttpNotFound();
            }
            return View(sp_nation);
        }

        // GET: sp_nation/Create
        public ActionResult Create()
        {
            ViewBag.s_user_id = new SelectList(db.users, "id", "email");
            return View();
        }

        // POST: sp_nation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,version,s_comment,s_create,s_status,name_ru,name_uz,name_uzl,s_user_id,NATIVE_ID")] sp_nation sp_nation)
        {
            if (ModelState.IsValid)
            {
                db.sp_nation.Add(sp_nation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.s_user_id = new SelectList(db.users, "id", "email", sp_nation.s_user_id);
            return View(sp_nation);
        }

        // GET: sp_nation/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_nation sp_nation = db.sp_nation.Find(id);
            if (sp_nation == null)
            {
                return HttpNotFound();
            }
            ViewBag.s_user_id = new SelectList(db.users, "id", "email", sp_nation.s_user_id);
            return View(sp_nation);
        }

        // POST: sp_nation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,version,s_comment,s_create,s_status,name_ru,name_uz,name_uzl,s_user_id,NATIVE_ID")] sp_nation sp_nation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sp_nation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.s_user_id = new SelectList(db.users, "id", "email", sp_nation.s_user_id);
            return View(sp_nation);
        }

        // GET: sp_nation/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_nation sp_nation = db.sp_nation.Find(id);
            if (sp_nation == null)
            {
                return HttpNotFound();
            }
            return View(sp_nation);
        }

        // POST: sp_nation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            sp_nation sp_nation = db.sp_nation.Find(id);
            db.sp_nation.Remove(sp_nation);
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
