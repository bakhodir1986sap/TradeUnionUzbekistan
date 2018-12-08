using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using KasabaKIAT.Models;
using PagedList.Mvc;
using PagedList;
using System;

namespace KasabaKIAT.Controllers
{
    public class okonx
    {
        public string code { get; set; }

        public string name_ru { get; set; }

        public string name_uz { get; set; }

        public string name_uzl { get; set; }

        public DateTime? create_date { get; set; }
    }

    public class IndexViewModel
    {
        public IEnumerable<sp_okonx> sp_okonxs { get; set; }
        public PageInfo PageInfo { get; set; }
    }

    public class sp_okonxController : Controller
    {
        private KasabaKIATEntities db = new KasabaKIATEntities();


        // GET: sp_okonx
        public ActionResult Index(int? page = 1)
        {
            var list = new List<okonx>();

            foreach (var obj in db.sp_okonx.ToList())
            {
                var model = new okonx();
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

        // GET: sp_okonx/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_okonx sp_okonx = db.sp_okonx.Find(id);
            if (sp_okonx == null)
            {
                return HttpNotFound();
            }
            return View(sp_okonx);
        }

        // GET: sp_okonx/Create
        public ActionResult Create()
        {
            ViewBag.s_user_id = new SelectList(db.users, "id", "email");
            ViewBag.parent_id = new SelectList(db.sp_okonx, "id", "code");
            return View();
        }

        // POST: sp_okonx/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,version,code,s_comment,s_create,s_status,name_ru,name_uz,name_uzl,s_user_id,parent_id,keys,branch_code,NATIVE_ID")] sp_okonx sp_okonx)
        {
            if (ModelState.IsValid)
            {
                db.sp_okonx.Add(sp_okonx);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.s_user_id = new SelectList(db.users, "id", "email", sp_okonx.s_user_id);
            ViewBag.parent_id = new SelectList(db.sp_okonx, "id", "code", sp_okonx.parent_id);
            return View(sp_okonx);
        }

        // GET: sp_okonx/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_okonx sp_okonx = db.sp_okonx.Find(id);
            if (sp_okonx == null)
            {
                return HttpNotFound();
            }
            ViewBag.s_user_id = new SelectList(db.users, "id", "email", sp_okonx.s_user_id);
            ViewBag.parent_id = new SelectList(db.sp_okonx, "id", "code", sp_okonx.parent_id);
            return View(sp_okonx);
        }

        // POST: sp_okonx/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,version,code,s_comment,s_create,s_status,name_ru,name_uz,name_uzl,s_user_id,parent_id,keys,branch_code,NATIVE_ID")] sp_okonx sp_okonx)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sp_okonx).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.s_user_id = new SelectList(db.users, "id", "email", sp_okonx.s_user_id);
            ViewBag.parent_id = new SelectList(db.sp_okonx, "id", "code", sp_okonx.parent_id);
            return View(sp_okonx);
        }

        // GET: sp_okonx/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_okonx sp_okonx = db.sp_okonx.Find(id);
            if (sp_okonx == null)
            {
                return HttpNotFound();
            }
            return View(sp_okonx);
        }

        // POST: sp_okonx/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            sp_okonx sp_okonx = db.sp_okonx.Find(id);
            db.sp_okonx.Remove(sp_okonx);
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
