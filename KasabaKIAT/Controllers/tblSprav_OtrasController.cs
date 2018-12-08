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
    public class otras
    {
        public string code { get; set; }

        public string name_ru { get; set; }

        public string name_uz { get; set; }

        public string name_uzl { get; set; }

        public DateTime? create_date { get; set; }
    }

    public class tblSprav_OtrasController : Controller
    {
        private KasabaKIATEntities db = new KasabaKIATEntities();

        // GET: tblSprav_Otras
        public ActionResult Index(int? page)
        {
            //var tblSprav_Otras = db.tblSprav_Otras.Include(t => t.users).Take(50);
            //return View(tblSprav_Otras.ToList());

            //int pageSize = 15;
            //int pageNumber = (page ?? 1);
            //return View(db.tblSprav_Otras.ToList().ToPagedList(pageNumber, pageSize));

            var list = new List<otras>();

            foreach (var obj in db.tblSprav_Otras)
            {
                var model = new otras();
                model.code = obj.ID_Ot.Value.ToString();
                model.create_date = DateTime.Now;
                model.name_ru = obj.Name_Ot;
                model.name_uz = obj.Name_Ot;
                model.name_uzl = obj.Name_Ot;

                list.Add(model);
            }

            ViewBag.datasource = list;

            return View();
        }

        // GET: tblSprav_Otras/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSprav_Otras tblSprav_Otras = db.tblSprav_Otras.Find(id);
            if (tblSprav_Otras == null)
            {
                return HttpNotFound();
            }
            return View(tblSprav_Otras);
        }

        // GET: tblSprav_Otras/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.users, "id", "email");
            return View();
        }

        // POST: tblSprav_Otras/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_Ot,Name_Ot,Priz,ID,UserId,DepartmentBase,BRANCH_CODE")] tblSprav_Otras tblSprav_Otras)
        {
            if (ModelState.IsValid)
            {
                db.tblSprav_Otras.Add(tblSprav_Otras);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.users, "id", "email", tblSprav_Otras.UserId);
            return View(tblSprav_Otras);
        }

        // GET: tblSprav_Otras/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSprav_Otras tblSprav_Otras = db.tblSprav_Otras.Find(id);
            if (tblSprav_Otras == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.users, "id", "email", tblSprav_Otras.UserId);
            return View(tblSprav_Otras);
        }

        // POST: tblSprav_Otras/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Ot,Name_Ot,Priz,ID,UserId,DepartmentBase,BRANCH_CODE")] tblSprav_Otras tblSprav_Otras)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblSprav_Otras).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.users, "id", "email", tblSprav_Otras.UserId);
            return View(tblSprav_Otras);
        }

        // GET: tblSprav_Otras/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSprav_Otras tblSprav_Otras = db.tblSprav_Otras.Find(id);
            if (tblSprav_Otras == null)
            {
                return HttpNotFound();
            }
            return View(tblSprav_Otras);
        }

        // POST: tblSprav_Otras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            tblSprav_Otras tblSprav_Otras = db.tblSprav_Otras.Find(id);
            db.tblSprav_Otras.Remove(tblSprav_Otras);
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
