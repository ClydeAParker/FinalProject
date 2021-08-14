using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FSWDFinalProject.Data.EF;

namespace FSWDFinalProject.UI.MVC.Controllers
{
    public class UserAssetsController : Controller
    {
        private FPResSysEntities db = new FPResSysEntities();

        // GET: UserAssets
        public ActionResult Index()
        {
            var userAssets = db.UserAssets.Include(u => u.UserDetail);
            return View(userAssets.ToList());
        }

        // GET: UserAssets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserAsset userAsset = db.UserAssets.Find(id);
            if (userAsset == null)
            {
                return HttpNotFound();
            }
            return View(userAsset);
        }

        // GET: UserAssets/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.UserDetails, "UserId", "FirstName");
            return View();
        }

        // POST: UserAssets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserAssetId,AssetName,UserId,AssetPhoto,SpecialNotes,IsActive,DateAdded,AssetYear")] UserAsset userAsset)
        {
            if (ModelState.IsValid)
            {
                db.UserAssets.Add(userAsset);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.UserDetails, "UserId", "FirstName", userAsset.UserId);
            return View(userAsset);
        }

        // GET: UserAssets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserAsset userAsset = db.UserAssets.Find(id);
            if (userAsset == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.UserDetails, "UserId", "FirstName", userAsset.UserId);
            return View(userAsset);
        }

        // POST: UserAssets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserAssetId,AssetName,UserId,AssetPhoto,SpecialNotes,IsActive,DateAdded,AssetYear")] UserAsset userAsset)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userAsset).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.UserDetails, "UserId", "FirstName", userAsset.UserId);
            return View(userAsset);
        }

        // GET: UserAssets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserAsset userAsset = db.UserAssets.Find(id);
            if (userAsset == null)
            {
                return HttpNotFound();
            }
            return View(userAsset);
        }

        // POST: UserAssets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserAsset userAsset = db.UserAssets.Find(id);
            db.UserAssets.Remove(userAsset);
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
