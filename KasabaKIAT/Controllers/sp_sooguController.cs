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
    public class sp_sooguController : Controller
    {
        private KasabaKIATEntities db = new KasabaKIATEntities();

        // GET: sp_soogu
        public ActionResult Index()
        {
            var sp_soogu = db.sp_soogu.Include(s => s.users);
            return View(sp_soogu.ToList());
        }

        // GET: sp_soogu/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_soogu sp_soogu = db.sp_soogu.Find(id);
            if (sp_soogu == null)
            {
                return HttpNotFound();
            }
            return View(sp_soogu);
        }

        // GET: sp_soogu/Create
        public ActionResult Create()
        {
            ViewBag.s_user_id = new SelectList(db.users, "id", "email");
            return View();
        }

        // POST: sp_soogu/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,version,code_okpo,s_comment,s_create,s_status,name_ru,name_uz,name_uzl,s_user_id,code,NATIVE_ID")] sp_soogu sp_soogu)
        {
            if (ModelState.IsValid)
            {
                db.sp_soogu.Add(sp_soogu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.s_user_id = new SelectList(db.users, "id", "email", sp_soogu.s_user_id);
            return View(sp_soogu);
        }

        // GET: sp_soogu/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_soogu sp_soogu = db.sp_soogu.Find(id);
            if (sp_soogu == null)
            {
                return HttpNotFound();
            }
            ViewBag.s_user_id = new SelectList(db.users, "id", "email", sp_soogu.s_user_id);
            return View(sp_soogu);
        }

        // POST: sp_soogu/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,version,code_okpo,s_comment,s_create,s_status,name_ru,name_uz,name_uzl,s_user_id,code,NATIVE_ID")] sp_soogu sp_soogu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sp_soogu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.s_user_id = new SelectList(db.users, "id", "email", sp_soogu.s_user_id);
            return View(sp_soogu);
        }

        // GET: sp_soogu/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_soogu sp_soogu = db.sp_soogu.Find(id);
            if (sp_soogu == null)
            {
                return HttpNotFound();
            }
            return View(sp_soogu);
        }

        // POST: sp_soogu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            sp_soogu sp_soogu = db.sp_soogu.Find(id);
            db.sp_soogu.Remove(sp_soogu);
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
