using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CandleShop.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public int count = 0;
        private Models.ShopDBModel db = new Models.ShopDBModel();
        public ActionResult UsersStorage()
        {
            int User_id = 0;
            if (Session["idUser"] == null)
            {
                Session["idUser"] = User_id;
            }

            if (User_id != (int)Session["idUser"])
            {
                User_id = (int)Session["idUser"];

            }
            var Items = db.Users;

            return View(Items);
        }
        public ActionResult AddProd()
        {
            return View();
        }
        public ActionResult Products()
        {
            //int User_id = 0;
            //if (Session["idUser"] == null)
            //{
            //    Session["idUser"] = User_id;
            //}

            //if (User_id != (int)Session["idUser"])
            //{
            //    User_id = (int)Session["idUser"];

            //}
            var Items = db.Candles;
            
                return View(Items);
        }
        [HttpGet]
        public ActionResult ChangeProducts()
        {
            //int User_id = 0;
            //if (Session["idUser"] == null)
            //{
            //    Session["idUser"] = User_id;
            //}

            //if (User_id != (int)Session["idUser"])
            //{
            //    User_id = (int)Session["idUser"];

            //}
            //var Items = db.Candles;

            return View();
        } 
        [HttpPost]
        public void ChangeProducts(int id_candle, string name, string material, string plase_of_origin, int quantity,
            string time, float new_price, float old_price)
        {
            var item = db.Candles.First(x => x.Id_Candle == id_candle);
            item.Id_Candle = id_candle;
            item.Product_name = name;
            item.Material = material;
            item.Plase_of_origin = plase_of_origin;
            item.Quantity = quantity;
            item.Burning_time = time;
            item.New_Price = new_price;
            item.Old_Price = old_price;

            db.SaveChanges();

            string url = Request.ServerVariables["HTTP_REFERER"];
            Response.Redirect(url);
            //return "Book was changed";
        }
        [HttpPost]
        public void AddProd(string image, string name, string material, string plase_of_origin, int quantity,
            string time, float new_price, float old_price)
        {

            Models.Candles NewCand = new Models.Candles
            {
                Image = image,
                Product_name = name,
                Material = material,
                Plase_of_origin = plase_of_origin,
                Quantity = quantity,
                Burning_time = time,
                Old_Price = old_price,
                New_Price = new_price,
            };
            db.Candles.Add(NewCand);

            db.SaveChanges();

            Response.Redirect("~/Admin/Products");

        }
        public void DellCandle(int id_candle)
        {
            var item = db.Candles.First(x => x.Id_Candle == id_candle);
            db.Candles.Remove(item);
            db.SaveChanges();
            string url = Request.ServerVariables["HTTP_REFERER"];
            Response.Redirect(url);
        }
        public void AddCandle()
        {

        }//сделать!!!
        public void DellUser(int id_user)
        {
            var item = db.Users.First(x => x.Id_Users == id_user);
            db.Users.Remove(item);
            db.SaveChanges();

            string url = Request.ServerVariables["HTTP_REFERER"];
            Response.Redirect(url);
        }
        public ActionResult Orders(int User_Id = 0)
        {
            int i = 0;
            var Need_Candle = db.Candles;

            var order = db.Cost.Where(x => /*x.Id_Users == User_Id && */x.Status_Cost == "Ожидание");//производим выборку всех добавленных заказов в корзину

            int column = order.Count();//подсчитываем кол-во выбранного товара
            Models.Cost[] array = new Models.Cost[column];//выделяем память под массив моделей свечей

            foreach (var a in order)//формируем массив 
            {

                        array[i] = new Models.Cost
                        {
                            Id_Cost = a.Id_Cost,
                            Id_Candle = a.Id_Candle,
                            Name = a.Name,
                            Id_Users = a.Id_Users,
                            Email = a.Email

                        };
                i++;
            }
            count = i;
            return View(array);//передаем массив в качестве параметра м-да View
        }
        public void Take_Order()//заказ обработан
        {
            var order = db.Cost.Where(x => /*x.Id_Users == User_Id && */x.Status_Cost == "Ожидание");//производим выборку всех добавленных заказов в корзину

            foreach (var a in order)
            {
                a.Status_Cost = "Принят";
            };
            db.SaveChanges();

            string url = Request.ServerVariables["HTTP_REFERER"];
            Response.Redirect(url);
        }
    }

}