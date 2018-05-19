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
    public class sp_departmenttypeController : Controller
    {
        private KasabaKIATEntities db = new KasabaKIATEntities();

        // GET: sp_departmenttype
        public ActionResult Index()
        {
            var sp_departmenttype = db.sp_departmenttype.Include(s => s.sp_departmenttype2).Include(s => s.users);
            return View(sp_departmenttype.ToList());
        }

        // GET: sp_departmenttype/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_departmenttype sp_departmenttype = db.sp_departmenttype.Find(id);
            if (sp_departmenttype == null)
            {
                return HttpNotFound();
            }
            return View(sp_departmenttype);
        }

        // GET: sp_departmenttype/Create
        public ActionResult Create()
        {
            ViewBag.parent_id = new SelectList(db.sp_departmenttype, "id", "keys");
            ViewBag.s_user_id = new SelectList(db.users, "id", "email");
            return View();
        }

        // POST: sp_departmenttype/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,version,keys,code,s_comment,s_create,s_status,name_ru,name_uz,name_uzl,parent_id,s_user_id,NATIVE_ID")] sp_departmenttype sp_departmenttype)
        {
            if (ModelState.IsValid)
            {
                db.sp_departmenttype.Add(sp_departmenttype);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.parent_id = new SelectList(db.sp_departmenttype, "id", "keys", sp_departmenttype.parent_id);
            ViewBag.s_user_id = new SelectList(db.users, "id", "email", sp_departmenttype.s_user_id);
            return View(sp_departmenttype);
        }

        // GET: sp_departmenttype/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_departmenttype sp_departmenttype = db.sp_departmenttype.Find(id);
            if (sp_departmenttype == null)
            {
                return HttpNotFound();
            }
            ViewBag.parent_id = new SelectList(db.sp_departmenttype, "id", "keys", sp_departmenttype.parent_id);
            ViewBag.s_user_id = new SelectList(db.users, "id", "email", sp_departmenttype.s_user_id);
            return View(sp_departmenttype);
        }

        // POST: sp_departmenttype/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,version,keys,code,s_comment,s_create,s_status,name_ru,name_uz,name_uzl,parent_id,s_user_id,NATIVE_ID")] sp_departmenttype sp_departmenttype)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sp_departmenttype).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.parent_id = new SelectList(db.sp_departmenttype, "id", "keys", sp_departmenttype.parent_id);
            ViewBag.s_user_id = new SelectList(db.users, "id", "email", sp_departmenttype.s_user_id);
            return View(sp_departmenttype);
        }

        // GET: sp_departmenttype/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_departmenttype sp_departmenttype = db.sp_departmenttype.Find(id);
            if (sp_departmenttype == null)
            {
                return HttpNotFound();
            }
            return View(sp_departmenttype);
        }

        // POST: sp_departmenttype/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            sp_departmenttype sp_departmenttype = db.sp_departmenttype.Find(id);
            db.sp_departmenttype.Remove(sp_departmenttype);
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
