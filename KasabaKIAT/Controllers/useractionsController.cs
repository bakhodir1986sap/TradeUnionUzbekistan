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
using System.Globalization;
using System.Data.Entity.Core.Objects;

namespace KasabaKIAT.Controllers
{
    public class userActionOut
    {
        public long id            {get; set;}
        public DateTime? accesstime    {get; set;}
        public string action        {get; set;}
        public string classname     {get; set;}
        public string object_name   {get; set;}
        public string email         {get;set;}
    }

    public class useractionsController : Controller
    {
        private KasabaKIATEntities db = new KasabaKIATEntities();
        static string actionNameL;
        static string userNameL;
        // GET: useractions
        public ActionResult Index(int? page, string actionNamez = null, string userName = null)
        {
            //var useractions = db.useractions.Include(u => u.users);
            //return View(await useractions.ToListAsync());

            List<userActionOut> listUserAction = new List<userActionOut>(); 

            if (!string.IsNullOrEmpty(actionNameL))
            {
                if (!string.IsNullOrEmpty(actionNamez))
                {
                    if (!actionNamez.Equals(actionNameL))
                    {
                        actionNameL = actionNamez;
                    }
                }
            }
            else
            {
                actionNameL = actionNamez;
            }

            if (!string.IsNullOrEmpty(userNameL))
            {
                if (!string.IsNullOrEmpty(userName))
                {
                    if (!userName.Equals(userNameL))
                    {
                        userNameL = userName;
                    }
                }
            }
            else
            {
                userNameL = userName;
            }

            DateTime dateparam;

            if (DateTime.TryParseExact(actionNameL , "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None , out dateparam))
            {

            }
            else
            {
                dateparam = DateTime.MinValue;
            }

            int pageSize = 50;
            int pageNumber = (page ?? 1);

            //var result = db.useractions.
            //    Include(u => u.users).
            //    Where(z => z.action.Contains(actionNameL) ||
            //    DbFunctions.TruncateTime(z.accesstime.Value) ==
            //    (dateparam == DateTime.MinValue && (actionNameL == null || actionNameL == "") ?
            //    DbFunctions.TruncateTime(z.accesstime.Value) :
            //    DbFunctions.TruncateTime(dateparam)) ||
            //    z.classname.Contains(actionNameL) ||
            //    z.users.user_name.Contains(actionNameL) ||
            //    z.object_name.Contains(actionNameL) ||
            //    z.action == ((actionNameL == null || actionNameL == "") ? z.action : actionNameL)
            //    )
            //    .OrderByDescending(z => z.id).Take(100000).ToList();

            var result = db.useractions.
                Include(u => u.user).OrderByDescending(z => z.id).Take(1000);

            foreach (var obj in result)
            {
                var inObj = new userActionOut();
                inObj.id = obj.id;
                inObj.accesstime = obj.accesstime;
                inObj.action = obj.action;
                inObj.classname = obj.classname;
                inObj.object_name = obj.object_name;
                inObj.email = obj.user.email;

                listUserAction.Add(inObj);
            }

            ViewBag.datasource = listUserAction;
            
            return View();
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
