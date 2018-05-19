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
    public class t_campController : Controller
    {
        private KasabaKIATEntities db = new KasabaKIATEntities();

        // GET: t_camp
        public ActionResult Index()
        {
            var t_camp = db.t_camp.Include(t => t.sp_soato).Include(t => t.sp_soato1).Include(t => t.users);
            return View(t_camp.ToList());
        }

        // GET: t_camp/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_camp t_camp = db.t_camp.Find(id);
            if (t_camp == null)
            {
                return HttpNotFound();
            }
            return View(t_camp);
        }

        // GET: t_camp/Create
        public ActionResult Create()
        {
            ViewBag.soato_organization_id = new SelectList(db.sp_soato, "id", "code");
            ViewBag.soato_id = new SelectList(db.sp_soato, "id", "code");
            ViewBag.s_user_id = new SelectList(db.users, "id", "email");
            return View();
        }

        // POST: t_camp/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,version,camps_address,camps_type1,camps_type2,camps_organization_address,camps_organization_cheef,camps_organization_cheef_tel,camps_organization_name,camps_union,camps_union_tel,camps_cheef,camps_cheef_tel,code,s_comment,s_create,s_status,camps_email,camps_name,camps_site,camps_tel,soato_organization_id,s_user_id,soato_id,NATIVE_ID")] t_camp t_camp)
        {
            if (ModelState.IsValid)
            {
                db.t_camp.Add(t_camp);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.soato_organization_id = new SelectList(db.sp_soato, "id", "code", t_camp.soato_organization_id);
            ViewBag.soato_id = new SelectList(db.sp_soato, "id", "code", t_camp.soato_id);
            ViewBag.s_user_id = new SelectList(db.users, "id", "email", t_camp.s_user_id);
            return View(t_camp);
        }

        // GET: t_camp/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_camp t_camp = db.t_camp.Find(id);
            if (t_camp == null)
            {
                return HttpNotFound();
            }
            ViewBag.soato_organization_id = new SelectList(db.sp_soato, "id", "code", t_camp.soato_organization_id);
            ViewBag.soato_id = new SelectList(db.sp_soato, "id", "code", t_camp.soato_id);
            ViewBag.s_user_id = new SelectList(db.users, "id", "email", t_camp.s_user_id);
            return View(t_camp);
        }

        // POST: t_camp/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,version,camps_address,camps_type1,camps_type2,camps_organization_address,camps_organization_cheef,camps_organization_cheef_tel,camps_organization_name,camps_union,camps_union_tel,camps_cheef,camps_cheef_tel,code,s_comment,s_create,s_status,camps_email,camps_name,camps_site,camps_tel,soato_organization_id,s_user_id,soato_id,NATIVE_ID")] t_camp t_camp)
        {
            if (ModelState.IsValid)
            {
                db.Entry(t_camp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.soato_organization_id = new SelectList(db.sp_soato, "id", "code", t_camp.soato_organization_id);
            ViewBag.soato_id = new SelectList(db.sp_soato, "id", "code", t_camp.soato_id);
            ViewBag.s_user_id = new SelectList(db.users, "id", "email", t_camp.s_user_id);
            return View(t_camp);
        }

        // GET: t_camp/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_camp t_camp = db.t_camp.Find(id);
            if (t_camp == null)
            {
                return HttpNotFound();
            }
            return View(t_camp);
        }

        // POST: t_camp/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            t_camp t_camp = db.t_camp.Find(id);
            db.t_camp.Remove(t_camp);
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
