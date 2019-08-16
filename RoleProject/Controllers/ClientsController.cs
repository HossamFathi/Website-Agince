using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RoleProject.Models;

namespace RoleProject.Controllers
{
    public class ClientsController:Controller
    {


            private ApplicationDbContext db = new ApplicationDbContext();

            // GET: Clinets
            public ActionResult List_Of_All()
            {
                return View(db.Client.ToList());
            }

        //Search

        [AllowAnonymous]

        public ActionResult Search(string searchItem)
        {

            return PartialView("_Search_Client_Partial");
        }
        [AllowAnonymous]

        public ActionResult Go_Search(string searchItem)
        {

                    var c = db.Client.FirstOrDefault( v=>v.Client_ID== searchItem);

            if (c == null)
            {
                return View("SearchError");// go to error page

            }
            else
            return View("Details", c);
        }



        public ActionResult sorting()
        {
            return PartialView("_Sorting_clinet_Partial");
        }

        public ActionResult Go_sorting(int? num)
        {


            if (num == null)
            {

                return View("List_Of_All", db.Client.ToList());


            }
            else

            {

                var clinet = new List<Client>();
                switch (num)
                {

                    case 1:
                        clinet = db.Client.OrderBy(e => e.age).ToList();
                        break;
                    case 2:
                        clinet = db.Client.OrderBy(e => e.city).ToList();
                        break;
                    case 3:
                        clinet = db.Client.OrderBy(e => e.street).ToList();
                        break;
                    case 4:
                        clinet = db.Client.OrderBy(e => e.Name).ToList();
                        break;
                    case 5:
                        clinet = db.Client.OrderBy(e => e.date_of_licience_expiry).ToList();
                        break;

                }



                return View("List_Of_All", clinet);
            }
        }

        // GET: Clinets/Details/5
        public ActionResult Details(String id)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            Client clinet = db.Client.FirstOrDefault(client_ => client_.Client_ID == id) ;
                if (clinet == null)
                {
                    return HttpNotFound();
                }
                return View(clinet);
            }

            // GET: Clinets/Create
            public ActionResult Create()
            {
                return View();
            }

            // POST: Clinets/Create

            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Create(Client clinet)
            {
                if (ModelState.IsValid)
                {

                    db.Client.Add(clinet);
                    db.SaveChanges();
                    return RedirectToAction("List_Of_All");
                }

                return View(clinet);
            }

            // GET: Clinets/Edit/5
            public ActionResult Edit(string id)
            {
                return View(db.Client.Find(id));
            }

            // POST: Clinets/Edit/5

            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Edit(Client clinet)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(clinet).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("List_Of_All");
                }
                return View(clinet);
            }

            // GET: Clinets/Delete/5
            public ActionResult Delete(String id)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            Client clinet = db.Client.Find(id);
                if (clinet == null)
                {
                    return HttpNotFound();
                }
                return View(clinet);
            }

            // POST: Clinets/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public ActionResult DeleteConfirmed(String id)
            {
            Client clinet = db.Client.Find(id);
                db.Client.Remove(clinet);
                db.SaveChanges();
                return RedirectToAction("List_Of_All");
            }





        }
    }


