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
    public class sp_soatoController : Controller
    {
        private KasabaKIATEntities db = new KasabaKIATEntities();

        // GET: sp_soato
        public ActionResult Index(int? page)
        {
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(db.sp_soato.ToList().ToPagedList(pageNumber, pageSize));
        }

        // GET: sp_soato/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_soato sp_soato = db.sp_soato.Find(id);
            if (sp_soato == null)
            {
                return HttpNotFound();
            }
            return View(sp_soato);
        }

        // GET: sp_soato/Create
        public ActionResult Create()
        {
            ViewBag.s_user_id = new SelectList(db.users, "id", "email");
            return View();
        }

        // POST: sp_soato/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,version,code,s_comment,s_create,s_status,name_ru,name_uz,name_uzl,name_center_ru,name_center_uz,name_center_uzl,s_user_id,parent_code,keys,NATIVE_ID")] sp_soato sp_soato)
        {
            if (ModelState.IsValid)
            {
                db.sp_soato.Add(sp_soato);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.s_user_id = new SelectList(db.users, "id", "email", sp_soato.s_user_id);
            return View(sp_soato);
        }

        // GET: sp_soato/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_soato sp_soato = db.sp_soato.Find(id);
            if (sp_soato == null)
            {
                return HttpNotFound();
            }
            ViewBag.s_user_id = new SelectList(db.users, "id", "email", sp_soato.s_user_id);
            return View(sp_soato);
        }

        // POST: sp_soato/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,version,code,s_comment,s_create,s_status,name_ru,name_uz,name_uzl,name_center_ru,name_center_uz,name_center_uzl,s_user_id,parent_code,keys,NATIVE_ID")] sp_soato sp_soato)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sp_soato).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.s_user_id = new SelectList(db.users, "id", "email", sp_soato.s_user_id);
            return View(sp_soato);
        }

        // GET: sp_soato/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_soato sp_soato = db.sp_soato.Find(id);
            if (sp_soato == null)
            {
                return HttpNotFound();
            }
            return View(sp_soato);
        }

        // POST: sp_soato/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            sp_soato sp_soato = db.sp_soato.Find(id);
            db.sp_soato.Remove(sp_soato);
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
