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
    public class section
    {
        public string code { get; set; }

        public string name_ru { get; set; }

        public string name_uz { get; set; }

        public string name_uzl { get; set; }

        public DateTime? create_date { get; set; }
    }

    public class sp_sectionController : Controller
    {
        private KasabaKIATEntities db = new KasabaKIATEntities();

        // GET: sp_section
        public ActionResult Index()
        {
            var list = new List<section>();

            foreach (var obj in db.sp_section)
            {
                var model = new section();
                model.code = obj.id.ToString();
                model.create_date = obj.s_create;
                model.name_ru = obj.name_ru;
                model.name_uz = obj.name_uz;
                model.name_uzl = obj.name_uzl;

                list.Add(model);
            }

            ViewBag.datasource = list;

            return View();
        }

        // GET: sp_section/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_section sp_section = db.sp_section.Find(id);
            if (sp_section == null)
            {
                return HttpNotFound();
            }
            return View(sp_section);
        }

        // GET: sp_section/Create
        public ActionResult Create()
        {
            ViewBag.s_user_id = new SelectList(db.users, "id", "email");
            return View();
        }

        // POST: sp_section/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,version,s_comment,s_create,s_status,name_ru,name_uz,name_uzl,s_user_id,NATIVE_ID")] sp_section sp_section)
        {
            if (ModelState.IsValid)
            {
                db.sp_section.Add(sp_section);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.s_user_id = new SelectList(db.users, "id", "email", sp_section.s_user_id);
            return View(sp_section);
        }

        // GET: sp_section/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_section sp_section = db.sp_section.Find(id);
            if (sp_section == null)
            {
                return HttpNotFound();
            }
            ViewBag.s_user_id = new SelectList(db.users, "id", "email", sp_section.s_user_id);
            return View(sp_section);
        }

        // POST: sp_section/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,version,s_comment,s_create,s_status,name_ru,name_uz,name_uzl,s_user_id,NATIVE_ID")] sp_section sp_section)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sp_section).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.s_user_id = new SelectList(db.users, "id", "email", sp_section.s_user_id);
            return View(sp_section);
        }

        // GET: sp_section/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_section sp_section = db.sp_section.Find(id);
            if (sp_section == null)
            {
                return HttpNotFound();
            }
            return View(sp_section);
        }

        // POST: sp_section/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            sp_section sp_section = db.sp_section.Find(id);
            db.sp_section.Remove(sp_section);
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
