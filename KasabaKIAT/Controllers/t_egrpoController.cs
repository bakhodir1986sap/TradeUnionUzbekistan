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
    public class t_egrpoController : Controller
    {
        private KasabaKIATEntities db = new KasabaKIATEntities();

        // GET: t_egrpo
        public ActionResult Index()
        {
            var t_egrpo = db.t_egrpo.Include(t => t.users);
            return View(t_egrpo.ToList());
        }

        // GET: t_egrpo/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_egrpo t_egrpo = db.t_egrpo.Find(id);
            if (t_egrpo == null)
            {
                return HttpNotFound();
            }
            return View(t_egrpo);
        }

        // GET: t_egrpo/Create
        public ActionResult Create()
        {
            ViewBag.s_user_id = new SelectList(db.users, "id", "email");
            return View();
        }

        // POST: t_egrpo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,version,address,close_date,close_number,count_workers,s_comment,s_create,s_status,email,fin_xoz_work,name_full,inn,liquidation,osn_worktype,phones,recreate,reg_date,reg_number,reg_org,reorganization,restored,name_short,street,update_doc,update_email,mail_index,s_user_id,okonx_id,opf_id,fs_id,soato_id,soogu_id,okpo,NATIVE_ID")] t_egrpo t_egrpo)
        {
            if (ModelState.IsValid)
            {
                db.t_egrpo.Add(t_egrpo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.s_user_id = new SelectList(db.users, "id", "email", t_egrpo.s_user_id);
            return View(t_egrpo);
        }

        // GET: t_egrpo/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_egrpo t_egrpo = db.t_egrpo.Find(id);
            if (t_egrpo == null)
            {
                return HttpNotFound();
            }
            ViewBag.s_user_id = new SelectList(db.users, "id", "email", t_egrpo.s_user_id);
            return View(t_egrpo);
        }

        // POST: t_egrpo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,version,address,close_date,close_number,count_workers,s_comment,s_create,s_status,email,fin_xoz_work,name_full,inn,liquidation,osn_worktype,phones,recreate,reg_date,reg_number,reg_org,reorganization,restored,name_short,street,update_doc,update_email,mail_index,s_user_id,okonx_id,opf_id,fs_id,soato_id,soogu_id,okpo,NATIVE_ID")] t_egrpo t_egrpo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(t_egrpo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.s_user_id = new SelectList(db.users, "id", "email", t_egrpo.s_user_id);
            return View(t_egrpo);
        }

        // GET: t_egrpo/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            t_egrpo t_egrpo = db.t_egrpo.Find(id);
            if (t_egrpo == null)
            {
                return HttpNotFound();
            }
            return View(t_egrpo);
        }

        // POST: t_egrpo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            t_egrpo t_egrpo = db.t_egrpo.Find(id);
            db.t_egrpo.Remove(t_egrpo);
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
