using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KasabaKIAT.Models;
using PagedList.Mvc;
using PagedList;

namespace KasabaKIAT.Controllers
{
    public class useractionsController : Controller
    {
        private KasabaKIATEntities db = new KasabaKIATEntities();

        // GET: useractions
        public ActionResult Index(int? page, string actionNamez = null, string userName = null)
        {
            //var useractions = db.useractions.Include(u => u.users);
            //return View(await useractions.ToListAsync());

            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(db.useractions.
                Include(u => u.users).
                Where(z=>z.action == ((actionNamez == null || actionNamez =="") ? z.action : actionNamez) 
                && z.users.user_name == ((userName == null || userName == "") ? z.users.user_name : userName))
                .OrderByDescending(z => z.id).Take(100).ToList().ToPagedList(pageNumber, pageSize));
        }

        // GET: useractions/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            useractions useractions = await db.useractions.FindAsync(id);
            if (useractions == null)
            {
                return HttpNotFound();
            }
            return View(useractions);
        }

        // GET: useractions/Create
        public ActionResult Create()
        {
            ViewBag.user_id = new SelectList(db.users, "id", "email");
            return View();
        }

        // POST: useractions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,version,accesstime,action,classname,object_id,user_id,object_name,NATIVE_ID")] useractions useractions)
        {
            if (ModelState.IsValid)
            {
                db.useractions.Add(useractions);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.user_id = new SelectList(db.users, "id", "email", useractions.user_id);
            return View(useractions);
        }

        // GET: useractions/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            useractions useractions = await db.useractions.FindAsync(id);
            if (useractions == null)
            {
                return HttpNotFound();
            }
            ViewBag.user_id = new SelectList(db.users, "id", "email", useractions.user_id);
            return View(useractions);
        }

        // POST: useractions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,version,accesstime,action,classname,object_id,user_id,object_name,NATIVE_ID")] useractions useractions)
        {
            if (ModelState.IsValid)
            {
                db.Entry(useractions).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.user_id = new SelectList(db.users, "id", "email", useractions.user_id);
            return View(useractions);
        }

        // GET: useractions/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            useractions useractions = await db.useractions.FindAsync(id);
            if (useractions == null)
            {
                return HttpNotFound();
            }
            return View(useractions);
        }

        // POST: useractions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            useractions useractions = await db.useractions.FindAsync(id);
            db.useractions.Remove(useractions);
            await db.SaveChangesAsync();
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
