using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RoleProject.Models;

namespace RoleProject.Controllers
{
    public class CarsController:Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Cars
        public ActionResult List_Of_All()
        {
            var cars = db.Cars.ToList();
            return View(cars);
        }

        // GET: Cars/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.FirstOrDefault(Car_ => Car_.Car_Id == id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }



        // GET: Cars/Create
        //[Authorize(Roles = "Clinet")]
        public ActionResult Create()
        {

            return View();
        }

        // POST: Cars/Create

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create( Car car)
        {
            if (ModelState.IsValid)
            {
                if (car.photo_path != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(car.photo_path.FileName);
                    string Extintion = Path.GetExtension(car.photo_path.FileName);
                    filename = filename + DateTime.Now.ToString("yymmssfff") + Extintion;
                    car.photo_Car = filename;
                    filename = Path.Combine(Server.MapPath("~/images/"), filename);
                    car.photo_path.SaveAs(filename);
                }
                db.Cars.Add(car);
                db.SaveChanges();
                return RedirectToAction("List_Of_All" , "Car_properties" ,car);
            }

            return View(car);
        }
        public ActionResult Add_properity(int? id)
        {
            Car car;
           
                car = TempData["car_called"] as Car;
                car.Additional_properties = new System.Collections.ObjectModel.Collection<Car_properties>();
                var properity = db.Car_properties.FirstOrDefault(prop => prop.id == id);
                car.Additional_properties.Add(properity);
                db.SaveChanges();
            
            return View("List_Of_All", "Car_properties", car);
        }
        // GET: Cars/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }

            return View(car);
        }
        // POST: Cars/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id,Car car)
        {

            try
            {
                //return car to edit
                var Car_Edititing = db.Cars.FirstOrDefault(car_ => car_.Car_Id == id);
                // update data in car
                Car_Edititing.Car_Brand = car.Car_Brand;
                Car_Edititing.Car_Model = car.Car_Model;
                Car_Edititing.Chassis_No = car.Chassis_No;
                Car_Edititing.Start_Book_Date= car.Start_Book_Date;
                Car_Edititing.End_Book_Date= car.End_Book_Date;
                Car_Edititing.price_in_day= car.price_in_day;
                Car_Edititing.Is_reseved= car.Is_reseved;


                if (car.photo_path != null)
                {
                    //to update photo-------------------------------------------------
                    string filename = Path.GetFileNameWithoutExtension(car.photo_path.FileName);
                    string Extintion = Path.GetExtension(car.photo_path.FileName);
                    filename = filename + DateTime.Now.ToString("yymmssfff") + Extintion;
                    Car_Edititing.photo_Car = filename;
                    filename = Path.Combine(Server.MapPath("~/images/"), filename);
                    car.photo_path.SaveAs(filename);
                    //-----------------------------------------------------------------
                }
                
                // save update
                db.SaveChanges();
                return RedirectToAction("List_Of_All");
            }
            catch
            {

                return View(car);
            }
        }

        // GET: Cars/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }
        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Car car = db.Cars.Find(id);
            db.Cars.Remove(car);
            db.SaveChanges();
            return RedirectToAction("List_Of_All");
        }
        public ActionResult Recive(int? id)
        {
            var Cars_Recived = db.Cars.FirstOrDefault(car => car.Car_Id == id);
            if (Cars_Recived != null)
            {
                
                
                    return View(Cars_Recived);// display View of recive
                
                
                
                    //return View("Recive_Alredy_Done", Cars_Recived);//display Cars is Elredy recirved
                
            }
            else
            {
                return View("Car_Not_Found"); // dispaly Cars is Not Fount ;

            }
        }
        // POST: Cars/Edit/5

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public object Recive(int? Car_Id, DateTime Start_Book_Date, DateTime End_Book_Date)
        {
            /*check by Cars number*/
            var Cars_Recived = db.Cars.FirstOrDefault(car => car.Car_Id == Car_Id);
            if (Cars_Recived.Is_reseved == false)
            {
                if ((Cars_Recived.Start_Book_Date <= Start_Book_Date && Cars_Recived.End_Book_Date >= End_Book_Date))
                {
                    return View("Recive_Alredy_Done", Cars_Recived);//display Cars is Elredy recirved}
                }
                Cars_Recived.Is_reseved = true;
                Cars_Recived.Start_Book_Date = Start_Book_Date;
                Cars_Recived.End_Book_Date = End_Book_Date;
                Calc_price(Cars_Recived);
                db.SaveChanges();
                return View("Displa_Reciving", Cars_Recived);// display confirm for client recived done
            }
            else
            {
                return View("Recive_Alredy_Done", Cars_Recived);//display Cars is Elredy recirved
            }


        }
        public ActionResult CancelRecive(int? id)
        {
            var Cars_Recived = db.Cars.FirstOrDefault(car => car.Car_Id == id);
            if (Cars_Recived != null)
            {
                if (Cars_Recived.Is_reseved == true)
                {
                    return View(Cars_Recived);// display View of recive
                }
                else
                {
                    return View("Recive_Not_Done_Yet", Cars_Recived);//display Cars is not recirved yet
                }
            }
            else
            {
                return View("Car_Not_Found"); // dispaly Cars is Not Fount ;

            }
        }

        // POST: Cars/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CancelRecive(int Id, Car car_)
        {
            var Cars_Recived = db.Cars.FirstOrDefault(car => car.Car_Id == Id);
            if (Cars_Recived.Is_reseved == true)
            {
                Cars_Recived.Is_reseved = false;
                if (Cars_Recived.Start_Book_Date <= DateTime.Now)
                {
                    Cars_Recived.End_Book_Date = DateTime.Now;
                    Calc_price(Cars_Recived);
                    db.SaveChanges();
                    return View("bill", Cars_Recived);
                }
                else
                {
                    Cars_Recived.End_Book_Date = Cars_Recived.Start_Book_Date;
                    Calc_price(Cars_Recived);
                    db.SaveChanges();
                    return View("bill", Cars_Recived);//display Cancel is Done with no cost 
                }
            }
            else
            {
                return View();//display Cars is not recirved yet
            }

        }
        public void Calc_price(Car Cars_reciveing)
        {
            int days_is_recived = Cars_reciveing.End_Book_Date.Subtract(Cars_reciveing.Start_Book_Date).Days;
            Cars_reciveing.price_Total = days_is_recived * Cars_reciveing.price_in_day;
        }
    }
}