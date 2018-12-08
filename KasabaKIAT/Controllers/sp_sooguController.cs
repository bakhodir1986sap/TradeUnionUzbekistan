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
    public class soogu
    {
        public string code { get; set; }

        public string name_ru { get; set; }

        public string name_uz { get; set; }

        public string name_uzl { get; set; }

        public DateTime? create_date { get; set; }
    }

    public class sp_sooguController : Controller
    {
        private KasabaKIATEntities db = new KasabaKIATEntities();

        // GET: sp_soogu
        public ActionResult Index(int? page)
        {
            //int pageSize = 15;
            //int pageNumber = (page ?? 1);
            //return View(db.sp_soogu.ToList().ToPagedList(pageNumber, pageSize));

            var list = new List<soogu>();

            foreach (var obj in db.sp_soogu)
            {
                var model = new soogu();
                model.code = obj.code;
                model.create_date = obj.s_create;
                model.name_ru = obj.name_ru;
                model.name_uz = obj.name_uz;
                model.name_uzl = obj.name_uzl;

                list.Add(model);
            }

            ViewBag.datasource = list;

            return View();
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
