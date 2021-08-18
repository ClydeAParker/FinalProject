using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FSWDFinalProject.Data.EF;
using FSWDFinalProject.UI.MVC.Utilities;
using Microsoft.AspNet.Identity;

namespace FSWDFinalProject.UI.MVC.Controllers
{
    public class UserAssetsController : Controller
    {
        private FPResSysEntities db = new FPResSysEntities();

        // GET: UserAssets
        public ActionResult Index()
        {
            var currentUser = User.Identity.GetUserId();
            if (User.IsInRole("Admin") || User.IsInRole("Franchise"))// If the user is an Admin they can see all records
            {
                var userAssets = db.UserAssets.Include(u => u.UserDetail);
                return View(userAssets.ToList());
            }
            else
            {
                var userAssets = (from o in db.UserAssets
                                  where o.UserId == currentUser
                                  select o).Include(u => u.UserDetail);
                return View(userAssets.ToList());
            }
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
            var currentUser = User.Identity.GetUserId();
            if (User.IsInRole("Admin") || User.IsInRole("Franchise"))
            {
                ViewBag.UserId = new SelectList(db.UserDetails, "UserId", "FullName");
            }
            else
            {
                var thisUser = from u in db.UserDetails
                               where u.UserId == currentUser
                               select u;

                ViewBag.UserId = new SelectList(thisUser, "UserId", "FullName");
            }
            return View();
        }

        // POST: UserAssets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserAssetId,AssetName,UserId,AssetPhoto,SpecialNotes,IsActive,DateAdded,AssetYear")] UserAsset userAsset, HttpPostedFileBase myImage)
        {
            if (ModelState.IsValid)
            {
                string imageName = "NoImage.png";
                if (myImage != null)
                {
                    //Get file extension - alternative to using a Substring() and IndexOf()
                    string imgExt = System.IO.Path.GetExtension(myImage.FileName);

                    //List of allowed extentions
                    string[] allowedExtensions = { ".png", ".gif", ".jpg", ".jpeg" };

                    if (allowedExtensions.Contains(imgExt))
                    {
                        //Use a GUID for saved file name on server
                        imageName = Guid.NewGuid() + imgExt;

                        //Create variables to pass to the ResizeImage method
                        //Signature for ResizingImage(savePath, fileName, image, maxImageSize, maxThumbSize)
                        string savePath = Server.MapPath("~/Content/img/CAI/");

                        Image convertedImage = Image.FromStream(myImage.InputStream);

                        int maxImageSize = 500;
                        int maxThumbSize = 100;

                        //Calling this method will use the variables created above to save the full size image and thumbnail image to your server.
                        UploadUtility.ResizeImage(savePath, imageName, convertedImage, maxImageSize, maxThumbSize);
                    }
                    userAsset.AssetPhoto = imageName;
                }
                string currentUserId = User.Identity.GetUserId();
                userAsset.UserId = currentUserId;
                userAsset.DateAdded = DateTime.Now;
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
