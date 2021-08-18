using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FSWDFinalProject.Data.EF;
using Microsoft.AspNet.Identity;

namespace FSWDFinalProject.UI.MVC.Controllers
{
    public class ReservationsController : Controller
    {
        private FPResSysEntities db = new FPResSysEntities();

        #region Index
        // GET: Reservations Index Page 
        [Authorize]
        public ActionResult Index()
        {
            //this is where we filtered the information being brought into the reservation Index page.  
            var currentUser = User.Identity.GetUserId();
            if (User.IsInRole("Admin"))// If the user is an Admin they can see all records
            {
                var reservations = db.Reservations.Include(r => r.Location).Include(r => r.UserAsset);
                return View(reservations.ToList());
            }
            else if (User.IsInRole("Franchise")) //ELSE IF the user is a Franchise
            {
                //Franchise employees can see all records for a single location 
                var cLocation = (from l in db.UserDetails
                                 where l.LocationId == l.LocationId && l.UserId == currentUser
                                 select l).FirstOrDefault();
                var userReservation = from r in db.Reservations
                                      where r.LocationId == cLocation.LocationId
                                      select r;
                return View(userReservation.ToList());
            }
            else //ANYTHING ELSE if the user is anthing but an Admin or a Franchise
            {
                //Users can see only their own reservation records. 
                var userReserve = from r in db.Reservations
                                  where r.UserAsset.UserId == currentUser
                                  select r;
                return View(userReserve.ToList());
            }
        }
        #endregion

        #region Details
        // GET: Reservations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }
        #endregion

        #region Create
        // GET: Reservations/Create
        public ActionResult Create()
        {
            var currentUser = User.Identity.GetUserId();
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "LocationName");
            if (User.IsInRole("Admin") || User.IsInRole("Franchise"))
            {
                ViewBag.UserAssetId = new SelectList(db.UserAssets, "UserAssetId", "AssetName");
            }
            else
            {
                var resAsset = from u in db.UserAssets
                                where u.UserId == currentUser
                                select u;

                ViewBag.UserAssetId = new SelectList(resAsset, "UserAssetId", "AssetName");
            }

            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserAssetId,LocationId,ReservationDate")] Reservation reservation)
        {
            var currentUser = User.Identity.GetUserId();
            //if the form information, program and database is running correctly with no issues. 
            if (ModelState.IsValid)
            {
                if (User.IsInRole("Admin")) //If the user has the role of admin
                {
                    db.Reservations.Add(reservation); //It pulls whatever information is entered by the user on the reservations page and posts it to the reservation database.
                    db.SaveChanges(); //Saves everything
                    ViewBag.SuccessMessage = "Reservation Successfully Booked";
                    return RedirectToAction("Index"); //Sends them back to a clean Reservation Index page with a success message. 
                }
                else //Anyone Else there is some logic involved to decide if they are allowed to make a reservation
                {

                    //Linq statement to decide the location count for how many reservations are made on a specific date by checking all records in the location DB against the location ID requested by the user.
                    var resRecords = from r in db.Reservations
                                     where reservation.ReservationDate == r.ReservationDate && r.LocationId == reservation.LocationId//this line checks reservation lovactions in DB the location ID
                                     select r;

                    //Linq statement to obtain the Location record for the location requested by the user.  We need this to get the reservation limit for that location. 
                    var locationCnt = (from l in db.Locations  //pulling from location db
                                       where l.LocationId == reservation.LocationId //matching location db info against the user requested location
                                       select l).FirstOrDefault(); //because this query could return a list of locations, we need to specify that we only want ONE location record. The first or default location record. 

                    //Because the reservation limit was entered in the DB as a "char(15)", the controller reads it as a string, rather than a number. This line of code converts the string into a usable integer for the reservation limit of the user requested location.
                    var resLimit = Convert.ToInt32(locationCnt.ReservationLimit);

                    //This line subtracts the number of existing records from the reservation limit of the requested location and stores the anser in the variable resCheck       
                    var resCheck = resLimit - resRecords.Count();

                    //If the the resCheck calculation above gives us any number greater than 0, a reservation can be succesfully created and they are redirected back to the Reservation Index page with a success message
                    if (resCheck > 0)
                    {
                        db.Reservations.Add(reservation);
                        db.SaveChanges();
                        ViewBag.SuccessMessage = "Reservation Successfully Booked";
                        return RedirectToAction("Index");
                    }
                    else //If the resCheck returns an answer of 0 or less, the following message is sent and their information is reentered on the reservation form. 
                    {
                        ViewBag.ResMessage = "We're sorry but this location has already reached the maximum nomber of reservations acilable for this date.  Please try a different date or location.";
                        ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "LcationName", reservation.LocationId);
                        ViewBag.UserAssetId = new SelectList(db.UserAssets, "UserAssetId", "AssetName", reservation.UserAssetId);
                        return View(reservation);
                    }
                }

            }
            //If the there is an error in the system somewhere, the following message is sent and their information is reentered on the reservation form.

            ViewBag.ResMessage = "We're sorry but an error has occured. Please try again or alert your network administator.";
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "LocationName", reservation.LocationId);
            ViewBag.UserAssetId = new SelectList(db.UserAssets, "UserAssetId", "AssetName", reservation.UserAssetId);
            return View(reservation);
        }
        #endregion

        #region Edit
        // GET: Reservations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "LocationName", reservation.LocationId);
            ViewBag.UserAssetId = new SelectList(db.UserAssets, "UserAssetId", "AssetName", reservation.UserAssetId);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserAssetId,LocationId,ReservationDate")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reservation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "LocationName", reservation.LocationId);
            ViewBag.UserAssetId = new SelectList(db.UserAssets, "UserAssetId", "AssetName", reservation.UserAssetId);
            return View(reservation);
        }
        #endregion

        #region Delete
        // GET: Reservations/Delete/5
        [Authorize(Roles = "Admin, Franchise")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reservation reservation = db.Reservations.Find(id);
            db.Reservations.Remove(reservation);
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
        #endregion
    }
}
