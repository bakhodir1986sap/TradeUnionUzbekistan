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

namespace KasabaKIAT.Controllers
{
    public class tbcs_dictionaryController : Controller
    {
        private KasabaKIATEntities db = new KasabaKIATEntities();

        // GET: tbcs_dictionary
        public async Task<ActionResult> Index()
        {
            var tbcs_dictionary = db.tbcs_dictionary.Include(t => t.users).Include(t => t.users1).Where(z=>z.DICTIONARY_TYPE_ID == 16);
            return View(await tbcs_dictionary.ToListAsync());
        }

        // GET: tbcs_dictionary/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbcs_dictionary tbcs_dictionary = await db.tbcs_dictionary.FindAsync(id);
            if (tbcs_dictionary == null)
            {
                return HttpNotFound();
            }
            return View(tbcs_dictionary);
        }

        // GET: tbcs_dictionary/Create
        public ActionResult Create()
        {
            ViewBag.USER_ID = new SelectList(db.users, "id", "email");
            ViewBag.USER_ID = new SelectList(db.users, "id", "email");
            return View();
        }

        // POST: tbcs_dictionary/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "REC_ID,CODE,NAME_RU,NAME_UZ,NAME_LT,DICTIONARY_TYPE_ID,STATUS,USER_ID")] tbcs_dictionary tbcs_dictionary)
        {
            if (ModelState.IsValid)
            {
                db.tbcs_dictionary.Add(tbcs_dictionary);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.USER_ID = new SelectList(db.users, "id", "email", tbcs_dictionary.USER_ID);
            ViewBag.USER_ID = new SelectList(db.users, "id", "email", tbcs_dictionary.USER_ID);
            return View(tbcs_dictionary);
        }

        // GET: tbcs_dictionary/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbcs_dictionary tbcs_dictionary = await db.tbcs_dictionary.FindAsync(id);
            if (tbcs_dictionary == null)
            {
                return HttpNotFound();
            }
            ViewBag.USER_ID = new SelectList(db.users, "id", "email", tbcs_dictionary.USER_ID);
            ViewBag.USER_ID = new SelectList(db.users, "id", "email", tbcs_dictionary.USER_ID);
            return View(tbcs_dictionary);
        }

        // POST: tbcs_dictionary/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "REC_ID,CODE,NAME_RU,NAME_UZ,NAME_LT,DICTIONARY_TYPE_ID,STATUS,USER_ID")] tbcs_dictionary tbcs_dictionary)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbcs_dictionary).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.USER_ID = new SelectList(db.users, "id", "email", tbcs_dictionary.USER_ID);
            ViewBag.USER_ID = new SelectList(db.users, "id", "email", tbcs_dictionary.USER_ID);
            return View(tbcs_dictionary);
        }

        // GET: tbcs_dictionary/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbcs_dictionary tbcs_dictionary = await db.tbcs_dictionary.FindAsync(id);
            if (tbcs_dictionary == null)
            {
                return HttpNotFound();
            }
            return View(tbcs_dictionary);
        }

        // POST: tbcs_dictionary/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            tbcs_dictionary tbcs_dictionary = await db.tbcs_dictionary.FindAsync(id);
            db.tbcs_dictionary.Remove(tbcs_dictionary);
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
