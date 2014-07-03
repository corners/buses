using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ReadingBusesCore.Entities;
using ReadingBusesCore.Persistence;
using ReadingBusesCore.Routes;

namespace readingBuses.Controllers
{
    public class RoutesController : Controller
    {
        private Context db = new Context();

        // GET: Routes
        public async Task<ActionResult> Index()
        {
            return View(await db.Routes.ToListAsync());
        }

        // GET: Routes/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Route route = await db.Routes.FindAsync(id);
            if (route == null)
            {
                return HttpNotFound();
            }
            return View(route);
        }

        // GET: Routes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Routes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Name,TargetStopsJson")] Route route)
        {
            if (ModelState.IsValid)
            {
                db.Routes.Add(route);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(route);
        }

        // GET: Routes/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //// todo move else where
            //try
            //{
            //    var locations = await RouteApi.GetLocations();
            //    var services = RouteApi.Services(locations);
            //    var forService = RouteApi.LocationsForService("3", locations);
            //    //var patterns = await RouteApi.GetServicePatterns();
            //}
            //catch (Exception ex)
            //{
            //    var s = ex.Message;
            //}

            Route route = await db.Routes.FindAsync(id);
            if (route == null)
            {
                return HttpNotFound();
            }
            return View(route);
        }

        // POST: Routes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Name,TargetStopsJson")] Route route)
        {
            if (ModelState.IsValid)
            {
                db.Entry(route).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(route);
        }

        // GET: Routes/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Route route = await db.Routes.FindAsync(id);
            if (route == null)
            {
                return HttpNotFound();
            }
            return View(route);
        }

        // POST: Routes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Route route = await db.Routes.FindAsync(id);
            db.Routes.Remove(route);
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
