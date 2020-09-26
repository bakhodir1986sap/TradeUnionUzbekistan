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
    public class kvartal
    {
        public string code { get; set; }

        public string name_ru { get; set; }

        public string name_uz { get; set; }

        public string name_uzl { get; set; }

        public DateTime? create_date { get; set; }
    }

    public class tblSprav_KvController : Controller
    {
        private KasabaKIATEntities db = new KasabaKIATEntities();

        // GET: tblSprav_Kv
        public ActionResult Index()
        {
            var list = new List<kvartal>();

            foreach (var obj in db.tblSprav_Kv)
            {
                var model = new kvartal();
                model.code = obj.ID.ToString();
                model.create_date = DateTime.Now;
                model.name_ru = obj.Name_Kv;
                model.name_uz = obj.Name_Kv;
                model.name_uzl = obj.Name_Kv;

                list.Add(model);
            }

            ViewBag.datasource = list;

            return View();
        }

        // GET: tblSprav_Kv/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSprav_Kv tblSprav_Kv = db.tblSprav_Kv.Find(id);
            if (tblSprav_Kv == null)
            {
                return HttpNotFound();
            }
            return View(tblSprav_Kv);
        }

        // GET: tblSprav_Kv/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.users, "id", "email");
            return View();
        }

        // POST: tblSprav_Kv/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name_Kv,UserId")] tblSprav_Kv tblSprav_Kv)
        {
            if (ModelState.IsValid)
            {
                db.tblSprav_Kv.Add(tblSprav_Kv);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.users, "id", "email", tblSprav_Kv.UserId);
            return View(tblSprav_Kv);
        }

        // GET: tblSprav_Kv/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSprav_Kv tblSprav_Kv = db.tblSprav_Kv.Find(id);
            if (tblSprav_Kv == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.users, "id", "email", tblSprav_Kv.UserId);
            return View(tblSprav_Kv);
        }

        // POST: tblSprav_Kv/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name_Kv,UserId")] tblSprav_Kv tblSprav_Kv)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblSprav_Kv).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.users, "id", "email", tblSprav_Kv.UserId);
            return View(tblSprav_Kv);
        }

        // GET: tblSprav_Kv/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSprav_Kv tblSprav_Kv = db.tblSprav_Kv.Find(id);
            if (tblSprav_Kv == null)
            {
                return HttpNotFound();
            }
            return View(tblSprav_Kv);
        }

        // POST: tblSprav_Kv/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblSprav_Kv tblSprav_Kv = db.tblSprav_Kv.Find(id);
            db.tblSprav_Kv.Remove(tblSprav_Kv);
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
