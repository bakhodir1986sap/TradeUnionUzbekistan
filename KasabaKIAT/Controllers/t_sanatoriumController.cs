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
    public class sanatorium
    {
        public string code { get; set; }

        public string name_ru { get; set; }

        public string name_uz { get; set; }

        public string name_uzl { get; set; }

        public DateTime? create_date { get; set; }
    }

    public class t_sanatoriumController : Controller
    {
        private KasabaKIATEntities db = new KasabaKIATEntities();

        // GET: t_sanatorium
        public ActionResult Index()
        {
            //var t_sanatorium = db.t_sanatorium.Include(t => t.sp_employees).Include(t => t.sp_sanatorium_profil).Include(t => t.sp_sanatorium_type).Include(t => t.sp_soato).Include(t => t.users).Take(50);
            //return View(t_sanatorium.ToList());

            var list = new List<sanatorium>();

            foreach (var obj in db.t_sanatorium)
            {
                var model = new sanatorium();
                model.code = obj.code;
                model.create_date = obj.s_create;
                model.name_ru = obj.sanatorium_name;
                model.name_uz = obj.sanatorium_name;
                model.name_uzl = obj.sanatorium_name;

                list.Add(model);
            }

            ViewBag.datasource = list;

            return View();
        }

        // GET: t_sanatorium/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_sanatorium t_sanatorium = db.t_sanatorium.Find(id);
            if (t_sanatorium == null)
            {
                return HttpNotFound();
            }
            return View(t_sanatorium);
        }

        // GET: t_sanatorium/Create
        public ActionResult Create()
        {
            ViewBag.employee_id = new SelectList(db.sp_employees, "id", "address");
            ViewBag.sanatorium_profil_id = new SelectList(db.sp_sanatorium_profil, "id", "s_comment");
            ViewBag.sanatorium_type_id = new SelectList(db.sp_sanatorium_type, "id", "s_comment");
            ViewBag.soato_id = new SelectList(db.sp_soato, "id", "code");
            ViewBag.s_user_id = new SelectList(db.users, "id", "email");
            return View();
        }

        // POST: t_sanatorium/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,version,address,code,s_comment,s_create,s_status,email,sanatorium_name,site,telephone,s_user_id,employee_id,sanatorium_profil_id,sanatorium_type_id,soato_id")] t_sanatorium t_sanatorium)
        {
            if (ModelState.IsValid)
            {
                db.t_sanatorium.Add(t_sanatorium);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.employee_id = new SelectList(db.sp_employees, "id", "address", t_sanatorium.employee_id);
            ViewBag.sanatorium_profil_id = new SelectList(db.sp_sanatorium_profil, "id", "s_comment", t_sanatorium.sanatorium_profil_id);
            ViewBag.sanatorium_type_id = new SelectList(db.sp_sanatorium_type, "id", "s_comment", t_sanatorium.sanatorium_type_id);
            ViewBag.soato_id = new SelectList(db.sp_soato, "id", "code", t_sanatorium.soato_id);
            ViewBag.s_user_id = new SelectList(db.users, "id", "email", t_sanatorium.s_user_id);
            return View(t_sanatorium);
        }

        // GET: t_sanatorium/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_sanatorium t_sanatorium = db.t_sanatorium.Find(id);
            if (t_sanatorium == null)
            {
                return HttpNotFound();
            }
            ViewBag.employee_id = new SelectList(db.sp_employees, "id", "address", t_sanatorium.employee_id);
            ViewBag.sanatorium_profil_id = new SelectList(db.sp_sanatorium_profil, "id", "s_comment", t_sanatorium.sanatorium_profil_id);
            ViewBag.sanatorium_type_id = new SelectList(db.sp_sanatorium_type, "id", "s_comment", t_sanatorium.sanatorium_type_id);
            ViewBag.soato_id = new SelectList(db.sp_soato, "id", "code", t_sanatorium.soato_id);
            ViewBag.s_user_id = new SelectList(db.users, "id", "email", t_sanatorium.s_user_id);
            return View(t_sanatorium);
        }

        // POST: t_sanatorium/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,version,address,code,s_comment,s_create,s_status,email,sanatorium_name,site,telephone,s_user_id,employee_id,sanatorium_profil_id,sanatorium_type_id,soato_id")] t_sanatorium t_sanatorium)
        {
            if (ModelState.IsValid)
            {
                db.Entry(t_sanatorium).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.employee_id = new SelectList(db.sp_employees, "id", "address", t_sanatorium.employee_id);
            ViewBag.sanatorium_profil_id = new SelectList(db.sp_sanatorium_profil, "id", "s_comment", t_sanatorium.sanatorium_profil_id);
            ViewBag.sanatorium_type_id = new SelectList(db.sp_sanatorium_type, "id", "s_comment", t_sanatorium.sanatorium_type_id);
            ViewBag.soato_id = new SelectList(db.sp_soato, "id", "code", t_sanatorium.soato_id);
            ViewBag.s_user_id = new SelectList(db.users, "id", "email", t_sanatorium.s_user_id);
            return View(t_sanatorium);
        }

        // GET: t_sanatorium/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_sanatorium t_sanatorium = db.t_sanatorium.Find(id);
            if (t_sanatorium == null)
            {
                return HttpNotFound();
            }
            return View(t_sanatorium);
        }

        // POST: t_sanatorium/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            t_sanatorium t_sanatorium = db.t_sanatorium.Find(id);
            db.t_sanatorium.Remove(t_sanatorium);
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
