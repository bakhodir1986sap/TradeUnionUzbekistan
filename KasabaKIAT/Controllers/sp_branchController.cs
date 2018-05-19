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
    public class sp_branchController : Controller
    {
        private KasabaKIATEntities db = new KasabaKIATEntities();

        // GET: sp_branch
        public ActionResult Index()
        {
            var sp_branch = db.sp_branch.Include(s => s.users);
            return View(sp_branch.ToList());
        }

        // GET: sp_branch/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_branch sp_branch = db.sp_branch.Find(id);
            if (sp_branch == null)
            {
                return HttpNotFound();
            }
            return View(sp_branch);
        }

        // GET: sp_branch/Create
        public ActionResult Create()
        {
            ViewBag.s_user_id = new SelectList(db.users, "id", "email");
            return View();
        }

        // POST: sp_branch/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,version,code,s_comment,s_create,s_status,name_ru,name_uz,name_uzl,s_user_id,NATIVE_ID")] sp_branch sp_branch)
        {
            if (ModelState.IsValid)
            {
                db.sp_branch.Add(sp_branch);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.s_user_id = new SelectList(db.users, "id", "email", sp_branch.s_user_id);
            return View(sp_branch);
        }

        // GET: sp_branch/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_branch sp_branch = db.sp_branch.Find(id);
            if (sp_branch == null)
            {
                return HttpNotFound();
            }
            ViewBag.s_user_id = new SelectList(db.users, "id", "email", sp_branch.s_user_id);
            return View(sp_branch);
        }

        // POST: sp_branch/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,version,code,s_comment,s_create,s_status,name_ru,name_uz,name_uzl,s_user_id,NATIVE_ID")] sp_branch sp_branch)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sp_branch).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.s_user_id = new SelectList(db.users, "id", "email", sp_branch.s_user_id);
            return View(sp_branch);
        }

        // GET: sp_branch/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_branch sp_branch = db.sp_branch.Find(id);
            if (sp_branch == null)
            {
                return HttpNotFound();
            }
            return View(sp_branch);
        }

        // POST: sp_branch/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            sp_branch sp_branch = db.sp_branch.Find(id);
            db.sp_branch.Remove(sp_branch);
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
