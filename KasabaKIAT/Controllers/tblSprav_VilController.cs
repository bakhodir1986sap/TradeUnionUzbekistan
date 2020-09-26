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
    public class viloyat
    {
        public string code { get; set; }

        public string name_ru { get; set; }

        public string name_uz { get; set; }

        public string name_uzl { get; set; }

        public DateTime? create_date { get; set; }
    }

    public class tblSprav_VilController : Controller
    {
        private KasabaKIATEntities db = new KasabaKIATEntities();

        // GET: tblSprav_Vil
        public ActionResult Index()
        {
            var list = new List<viloyat>();

            foreach (var obj in db.tblSprav_Vil)
            {
                var model = new viloyat();
                model.code = obj.ID_Code.ToString();
                model.create_date = DateTime.Now;
                model.name_ru = obj.FullName;
                model.name_uz = obj.FullName;
                model.name_uzl = obj.FullName;

                list.Add(model);
            }

            ViewBag.datasource = list;

            return View();
        }

        // GET: tblSprav_Vil/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSprav_Vil tblSprav_Vil = db.tblSprav_Vil.Find(id);
            if (tblSprav_Vil == null)
            {
                return HttpNotFound();
            }
            return View(tblSprav_Vil);
        }

        // GET: tblSprav_Vil/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.users, "id", "email");
            return View();
        }

        // POST: tblSprav_Vil/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FullName,ID_Code,UserId,SOATO_CODE")] tblSprav_Vil tblSprav_Vil)
        {
            if (ModelState.IsValid)
            {
                db.tblSprav_Vil.Add(tblSprav_Vil);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.users, "id", "email", tblSprav_Vil.UserId);
            return View(tblSprav_Vil);
        }

        // GET: tblSprav_Vil/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSprav_Vil tblSprav_Vil = db.tblSprav_Vil.Find(id);
            if (tblSprav_Vil == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.users, "id", "email", tblSprav_Vil.UserId);
            return View(tblSprav_Vil);
        }

        // POST: tblSprav_Vil/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FullName,ID_Code,UserId,SOATO_CODE")] tblSprav_Vil tblSprav_Vil)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblSprav_Vil).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.users, "id", "email", tblSprav_Vil.UserId);
            return View(tblSprav_Vil);
        }

        // GET: tblSprav_Vil/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSprav_Vil tblSprav_Vil = db.tblSprav_Vil.Find(id);
            if (tblSprav_Vil == null)
            {
                return HttpNotFound();
            }
            return View(tblSprav_Vil);
        }

        // POST: tblSprav_Vil/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            tblSprav_Vil tblSprav_Vil = db.tblSprav_Vil.Find(id);
            db.tblSprav_Vil.Remove(tblSprav_Vil);
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
