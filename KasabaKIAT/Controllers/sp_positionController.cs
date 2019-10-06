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
    public class sp_positionController : Controller
    {
        private KasabaKIATEntities db = new KasabaKIATEntities();

        // GET: sp_position
        public ActionResult Index()
        {
            var sp_position = db.sp_position.Include(s => s.user).Take(50);
            return View(sp_position.ToList());
        }

        // GET: sp_position/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_position sp_position = db.sp_position.Find(id);
            if (sp_position == null)
            {
                return HttpNotFound();
            }
            return View(sp_position);
        }

        // GET: sp_position/Create
        public ActionResult Create()
        {
            ViewBag.s_user_id = new SelectList(db.users, "id", "email");
            return View();
        }

        // POST: sp_position/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,version,s_comment,s_create,s_status,name_ru,name_uz,name_uzl,s_user_id,NATIVE_ID")] sp_position sp_position)
        {
            if (ModelState.IsValid)
            {
                db.sp_position.Add(sp_position);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.s_user_id = new SelectList(db.users, "id", "email", sp_position.s_user_id);
            return View(sp_position);
        }

        // GET: sp_position/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_position sp_position = db.sp_position.Find(id);
            if (sp_position == null)
            {
                return HttpNotFound();
            }
            ViewBag.s_user_id = new SelectList(db.users, "id", "email", sp_position.s_user_id);
            return View(sp_position);
        }

        // POST: sp_position/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,version,s_comment,s_create,s_status,name_ru,name_uz,name_uzl,s_user_id,NATIVE_ID")] sp_position sp_position)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sp_position).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.s_user_id = new SelectList(db.users, "id", "email", sp_position.s_user_id);
            return View(sp_position);
        }

        // GET: sp_position/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_position sp_position = db.sp_position.Find(id);
            if (sp_position == null)
            {
                return HttpNotFound();
            }
            return View(sp_position);
        }

        // POST: sp_position/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            sp_position sp_position = db.sp_position.Find(id);
            db.sp_position.Remove(sp_position);
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
