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
    public class sp_opfController : Controller
    {
        private KasabaKIATEntities db = new KasabaKIATEntities();

        // GET: sp_opf
        public ActionResult Index(int? page)
        {
            //var sp_opf = db.sp_opf.Include(s => s.sp_opf2).Include(s => s.users).Take(50);
            //return View(sp_opf.ToList());

            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(db.sp_opf.ToList().ToPagedList(pageNumber, pageSize));
        }

        // GET: sp_opf/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_opf sp_opf = db.sp_opf.Find(id);
            if (sp_opf == null)
            {
                return HttpNotFound();
            }
            return View(sp_opf);
        }

        // GET: sp_opf/Create
        public ActionResult Create()
        {
            ViewBag.parent_id = new SelectList(db.sp_opf, "id", "code");
            ViewBag.s_user_id = new SelectList(db.users, "id", "email");
            return View();
        }

        // POST: sp_opf/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,version,code,s_comment,s_create,s_status,name_ru,name_uz,name_uzl,s_user_id,parent_id,keys,NATIVE_ID")] sp_opf sp_opf)
        {
            if (ModelState.IsValid)
            {
                db.sp_opf.Add(sp_opf);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.parent_id = new SelectList(db.sp_opf, "id", "code", sp_opf.parent_id);
            ViewBag.s_user_id = new SelectList(db.users, "id", "email", sp_opf.s_user_id);
            return View(sp_opf);
        }

        // GET: sp_opf/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_opf sp_opf = db.sp_opf.Find(id);
            if (sp_opf == null)
            {
                return HttpNotFound();
            }
            ViewBag.parent_id = new SelectList(db.sp_opf, "id", "code", sp_opf.parent_id);
            ViewBag.s_user_id = new SelectList(db.users, "id", "email", sp_opf.s_user_id);
            return View(sp_opf);
        }

        // POST: sp_opf/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,version,code,s_comment,s_create,s_status,name_ru,name_uz,name_uzl,s_user_id,parent_id,keys,NATIVE_ID")] sp_opf sp_opf)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sp_opf).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.parent_id = new SelectList(db.sp_opf, "id", "code", sp_opf.parent_id);
            ViewBag.s_user_id = new SelectList(db.users, "id", "email", sp_opf.s_user_id);
            return View(sp_opf);
        }

        // GET: sp_opf/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_opf sp_opf = db.sp_opf.Find(id);
            if (sp_opf == null)
            {
                return HttpNotFound();
            }
            return View(sp_opf);
        }

        // POST: sp_opf/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            sp_opf sp_opf = db.sp_opf.Find(id);
            db.sp_opf.Remove(sp_opf);
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
