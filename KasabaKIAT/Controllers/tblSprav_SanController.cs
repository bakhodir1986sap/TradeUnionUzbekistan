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
    public class sanatotry
    {
        public string code { get; set; }

        public string name_ru { get; set; }

        public string name_uz { get; set; }

        public string name_uzl { get; set; }

        public DateTime? create_date { get; set; }
    }

    public class tblSprav_SanController : Controller
    {
        private KasabaKIATEntities db = new KasabaKIATEntities();

        // GET: tblSprav_San
        public ActionResult Index()
        {
            var list = new List<sanatotry>();

            foreach (var obj in db.tblSprav_San)
            {
                var model = new sanatotry();
                model.code = obj.KurortSanID.ToString();
                model.create_date = DateTime.Now;
                model.name_ru = obj.Name_San;
                model.name_uz = obj.Name_San;
                model.name_uzl = obj.Name_San;

                list.Add(model);
            }

            ViewBag.datasource = list;

            return View();
        }

        // GET: tblSprav_San/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSprav_San tblSprav_San = db.tblSprav_San.Find(id);
            if (tblSprav_San == null)
            {
                return HttpNotFound();
            }
            return View(tblSprav_San);
        }

        // GET: tblSprav_San/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.users, "id", "email");
            return View();
        }

        // POST: tblSprav_San/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name_San,Num_Kod,UserId,KurortSanID")] tblSprav_San tblSprav_San)
        {
            if (ModelState.IsValid)
            {
                db.tblSprav_San.Add(tblSprav_San);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.users, "id", "email", tblSprav_San.UserId);
            return View(tblSprav_San);
        }

        // GET: tblSprav_San/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSprav_San tblSprav_San = db.tblSprav_San.Find(id);
            if (tblSprav_San == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.users, "id", "email", tblSprav_San.UserId);
            return View(tblSprav_San);
        }

        // POST: tblSprav_San/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name_San,Num_Kod,UserId,KurortSanID")] tblSprav_San tblSprav_San)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblSprav_San).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.users, "id", "email", tblSprav_San.UserId);
            return View(tblSprav_San);
        }

        // GET: tblSprav_San/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSprav_San tblSprav_San = db.tblSprav_San.Find(id);
            if (tblSprav_San == null)
            {
                return HttpNotFound();
            }
            return View(tblSprav_San);
        }

        // POST: tblSprav_San/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblSprav_San tblSprav_San = db.tblSprav_San.Find(id);
            db.tblSprav_San.Remove(tblSprav_San);
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
