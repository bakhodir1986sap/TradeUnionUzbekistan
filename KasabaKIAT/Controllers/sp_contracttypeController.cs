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
    public class sp_contracttypeController : Controller
    {
        private KasabaKIATEntities db = new KasabaKIATEntities();

        // GET: sp_contracttype
        public ActionResult Index()
        {
            var sp_contracttype = db.sp_contracttype.Include(s => s.users).Take(50);
            return View(sp_contracttype.ToList());
        }

        // GET: sp_contracttype/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_contracttype sp_contracttype = db.sp_contracttype.Find(id);
            if (sp_contracttype == null)
            {
                return HttpNotFound();
            }
            return View(sp_contracttype);
        }

        // GET: sp_contracttype/Create
        public ActionResult Create()
        {
            ViewBag.s_user_id = new SelectList(db.users, "id", "email");
            return View();
        }

        // POST: sp_contracttype/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,version,code,s_comment,s_create,s_status,name_ru,name_uz,name_uzl,s_user_id,NATIVE_ID")] sp_contracttype sp_contracttype)
        {
            if (ModelState.IsValid)
            {
                db.sp_contracttype.Add(sp_contracttype);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.s_user_id = new SelectList(db.users, "id", "email", sp_contracttype.s_user_id);
            return View(sp_contracttype);
        }

        // GET: sp_contracttype/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_contracttype sp_contracttype = db.sp_contracttype.Find(id);
            if (sp_contracttype == null)
            {
                return HttpNotFound();
            }
            ViewBag.s_user_id = new SelectList(db.users, "id", "email", sp_contracttype.s_user_id);
            return View(sp_contracttype);
        }

        // POST: sp_contracttype/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,version,code,s_comment,s_create,s_status,name_ru,name_uz,name_uzl,s_user_id,NATIVE_ID")] sp_contracttype sp_contracttype)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sp_contracttype).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.s_user_id = new SelectList(db.users, "id", "email", sp_contracttype.s_user_id);
            return View(sp_contracttype);
        }

        // GET: sp_contracttype/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_contracttype sp_contracttype = db.sp_contracttype.Find(id);
            if (sp_contracttype == null)
            {
                return HttpNotFound();
            }
            return View(sp_contracttype);
        }

        // POST: sp_contracttype/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            sp_contracttype sp_contracttype = db.sp_contracttype.Find(id);
            db.sp_contracttype.Remove(sp_contracttype);
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
