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
    public class procent
    {
        public string code { get; set; }

        public string name_ru { get; set; }

        public string name_uz { get; set; }

        public string name_uzl { get; set; }

        public DateTime? create_date { get; set; }
    }

    public class tblSprav_PrController : Controller
    {
        private KasabaKIATEntities db = new KasabaKIATEntities();

        // GET: tblSprav_Pr
        public ActionResult Index()
        {
            var list = new List<procent>();

            foreach (var obj in db.tblSprav_Pr)
            {
                var model = new procent();
                model.code = obj.ID.ToString();
                model.create_date = DateTime.Now;
                model.name_ru = obj.Name_Proc;
                model.name_uz = obj.Name_Proc;
                model.name_uzl = obj.Name_Proc;

                list.Add(model);
            }

            ViewBag.datasource = list;

            return View();
        }

        // GET: tblSprav_Pr/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSprav_Pr tblSprav_Pr = db.tblSprav_Pr.Find(id);
            if (tblSprav_Pr == null)
            {
                return HttpNotFound();
            }
            return View(tblSprav_Pr);
        }

        // GET: tblSprav_Pr/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.users, "id", "email");
            return View();
        }

        // POST: tblSprav_Pr/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name_Proc,Num,UserId")] tblSprav_Pr tblSprav_Pr)
        {
            if (ModelState.IsValid)
            {
                db.tblSprav_Pr.Add(tblSprav_Pr);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.users, "id", "email", tblSprav_Pr.UserId);
            return View(tblSprav_Pr);
        }

        // GET: tblSprav_Pr/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSprav_Pr tblSprav_Pr = db.tblSprav_Pr.Find(id);
            if (tblSprav_Pr == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.users, "id", "email", tblSprav_Pr.UserId);
            return View(tblSprav_Pr);
        }

        // POST: tblSprav_Pr/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name_Proc,Num,UserId")] tblSprav_Pr tblSprav_Pr)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblSprav_Pr).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.users, "id", "email", tblSprav_Pr.UserId);
            return View(tblSprav_Pr);
        }

        // GET: tblSprav_Pr/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSprav_Pr tblSprav_Pr = db.tblSprav_Pr.Find(id);
            if (tblSprav_Pr == null)
            {
                return HttpNotFound();
            }
            return View(tblSprav_Pr);
        }

        // POST: tblSprav_Pr/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            tblSprav_Pr tblSprav_Pr = db.tblSprav_Pr.Find(id);
            db.tblSprav_Pr.Remove(tblSprav_Pr);
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
