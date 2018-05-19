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
    public class sp_okonxController : Controller
    {
        private KasabaKIATEntities db = new KasabaKIATEntities();

        // GET: sp_okonx
        public ActionResult Index()
        {
            var sp_okonx = db.sp_okonx.Include(s => s.users).Include(s => s.sp_okonx2).Take(50);
            return View(sp_okonx.ToList());
        }

        // GET: sp_okonx/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_okonx sp_okonx = db.sp_okonx.Find(id);
            if (sp_okonx == null)
            {
                return HttpNotFound();
            }
            return View(sp_okonx);
        }

        // GET: sp_okonx/Create
        public ActionResult Create()
        {
            ViewBag.s_user_id = new SelectList(db.users, "id", "email");
            ViewBag.parent_id = new SelectList(db.sp_okonx, "id", "code");
            return View();
        }

        // POST: sp_okonx/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,version,code,s_comment,s_create,s_status,name_ru,name_uz,name_uzl,s_user_id,parent_id,keys,branch_code,NATIVE_ID")] sp_okonx sp_okonx)
        {
            if (ModelState.IsValid)
            {
                db.sp_okonx.Add(sp_okonx);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.s_user_id = new SelectList(db.users, "id", "email", sp_okonx.s_user_id);
            ViewBag.parent_id = new SelectList(db.sp_okonx, "id", "code", sp_okonx.parent_id);
            return View(sp_okonx);
        }

        // GET: sp_okonx/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_okonx sp_okonx = db.sp_okonx.Find(id);
            if (sp_okonx == null)
            {
                return HttpNotFound();
            }
            ViewBag.s_user_id = new SelectList(db.users, "id", "email", sp_okonx.s_user_id);
            ViewBag.parent_id = new SelectList(db.sp_okonx, "id", "code", sp_okonx.parent_id);
            return View(sp_okonx);
        }

        // POST: sp_okonx/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,version,code,s_comment,s_create,s_status,name_ru,name_uz,name_uzl,s_user_id,parent_id,keys,branch_code,NATIVE_ID")] sp_okonx sp_okonx)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sp_okonx).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.s_user_id = new SelectList(db.users, "id", "email", sp_okonx.s_user_id);
            ViewBag.parent_id = new SelectList(db.sp_okonx, "id", "code", sp_okonx.parent_id);
            return View(sp_okonx);
        }

        // GET: sp_okonx/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_okonx sp_okonx = db.sp_okonx.Find(id);
            if (sp_okonx == null)
            {
                return HttpNotFound();
            }
            return View(sp_okonx);
        }

        // POST: sp_okonx/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            sp_okonx sp_okonx = db.sp_okonx.Find(id);
            db.sp_okonx.Remove(sp_okonx);
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
