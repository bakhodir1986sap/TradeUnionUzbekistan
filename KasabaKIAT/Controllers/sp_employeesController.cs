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
    public class sp_employeesController : Controller
    {
        private KasabaKIATEntities db = new KasabaKIATEntities();

        // GET: sp_employees
        public ActionResult Index()
        {
            var sp_employees = db.sp_employees.Include(s => s.sp_degree).Include(s => s.sp_nation).Include(s => s.sp_position).Include(s => s.sp_section).Include(s => s.t_departmentbase).Include(s => s.user).Take(50);
            return View(sp_employees.ToList());
        }

        // GET: sp_employees/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_employees sp_employees = db.sp_employees.Find(id);
            if (sp_employees == null)
            {
                return HttpNotFound();
            }
            return View(sp_employees);
        }

        // GET: sp_employees/Create
        public ActionResult Create()
        {
            ViewBag.degree_id = new SelectList(db.sp_degree, "id", "s_comment");
            ViewBag.nation_id = new SelectList(db.sp_nation, "id", "s_comment");
            ViewBag.pos_id = new SelectList(db.sp_position, "id", "s_comment");
            ViewBag.section_id = new SelectList(db.sp_section, "id", "s_comment");
            ViewBag.bkut_id = new SelectList(db.t_departmentbase, "id", "dtype");
            ViewBag.s_user_id = new SelectList(db.users, "id", "email");
            return View();
        }

        // POST: sp_employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,version,address,code,s_comment,s_create,s_status,email,middlename,name,passport,phone,surname,dep_id,s_user_id,pos_id,birthday,bkut_id,degree_id,nation_id,section_id,full_name,NATIVE_ID")] sp_employees sp_employees)
        {
            if (ModelState.IsValid)
            {
                db.sp_employees.Add(sp_employees);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.degree_id = new SelectList(db.sp_degree, "id", "s_comment", sp_employees.degree_id);
            ViewBag.nation_id = new SelectList(db.sp_nation, "id", "s_comment", sp_employees.nation_id);
            ViewBag.pos_id = new SelectList(db.sp_position, "id", "s_comment", sp_employees.pos_id);
            ViewBag.section_id = new SelectList(db.sp_section, "id", "s_comment", sp_employees.section_id);
            ViewBag.bkut_id = new SelectList(db.t_departmentbase, "id", "dtype", sp_employees.bkut_id);
            ViewBag.s_user_id = new SelectList(db.users, "id", "email", sp_employees.s_user_id);
            return View(sp_employees);
        }

        // GET: sp_employees/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_employees sp_employees = db.sp_employees.Find(id);
            if (sp_employees == null)
            {
                return HttpNotFound();
            }
            ViewBag.degree_id = new SelectList(db.sp_degree, "id", "s_comment", sp_employees.degree_id);
            ViewBag.nation_id = new SelectList(db.sp_nation, "id", "s_comment", sp_employees.nation_id);
            ViewBag.pos_id = new SelectList(db.sp_position, "id", "s_comment", sp_employees.pos_id);
            ViewBag.section_id = new SelectList(db.sp_section, "id", "s_comment", sp_employees.section_id);
            ViewBag.bkut_id = new SelectList(db.t_departmentbase, "id", "dtype", sp_employees.bkut_id);
            ViewBag.s_user_id = new SelectList(db.users, "id", "email", sp_employees.s_user_id);
            return View(sp_employees);
        }

        // POST: sp_employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,version,address,code,s_comment,s_create,s_status,email,middlename,name,passport,phone,surname,dep_id,s_user_id,pos_id,birthday,bkut_id,degree_id,nation_id,section_id,full_name,NATIVE_ID")] sp_employees sp_employees)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sp_employees).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.degree_id = new SelectList(db.sp_degree, "id", "s_comment", sp_employees.degree_id);
            ViewBag.nation_id = new SelectList(db.sp_nation, "id", "s_comment", sp_employees.nation_id);
            ViewBag.pos_id = new SelectList(db.sp_position, "id", "s_comment", sp_employees.pos_id);
            ViewBag.section_id = new SelectList(db.sp_section, "id", "s_comment", sp_employees.section_id);
            ViewBag.bkut_id = new SelectList(db.t_departmentbase, "id", "dtype", sp_employees.bkut_id);
            ViewBag.s_user_id = new SelectList(db.users, "id", "email", sp_employees.s_user_id);
            return View(sp_employees);
        }

        // GET: sp_employees/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_employees sp_employees = db.sp_employees.Find(id);
            if (sp_employees == null)
            {
                return HttpNotFound();
            }
            return View(sp_employees);
        }

        // POST: sp_employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            sp_employees sp_employees = db.sp_employees.Find(id);
            db.sp_employees.Remove(sp_employees);
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
