using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
using System.Net;
using System.Net.Mail;


namespace CandleShop.Controllers
{
    public class HomeController : Controller
    {
        //GET: Home
        private Models.ShopDBModel db = new Models.ShopDBModel();
        public ActionResult Main()
        {
            //Session["Status_autrz"] = null; 
            //Session["Status_Cost"] = 0;

            int User_id = 0;
            if (Session["idUser"] == null)
            {
                Session["idUser"] = User_id;
            }

            if (User_id != (int)Session["idUser"])
            {
                User_id = (int)Session["idUser"];

            }

            return View();
        }
        public ActionResult NewUser()
        {
            int User_id = 0;
            if (Session["NewUser"] == null)
            {
                Session["NewUser"] = User_id;
            }
            else Session["NewUser"] = 1;

            if (Session["idUser"] == null)
            {
                Session["idUser"] = User_id;
            }
            if (User_id != (int)Session["idUser"])
            {
                User_id = (int)Session["idUser"];

            }

            //int User_id = 0;
            if (Session["idUser"] == null)
            {
                Session["idUser"] = User_id;
            }

            if (User_id != (int)Session["idUser"])
            {
                User_id = (int)Session["idUser"];

            }

            return View();

        }
        public ActionResult Login()
        {
           //Session["Status_autrz"] = null; 
            //Session["Status_Cost"] = 0;

            int User_id = 0;
            if (Session["idUser"] == null)
            {
                Session["idUser"] = User_id;
            }

            if (User_id != (int)Session["idUser"])
            {
                User_id = (int)Session["idUser"];

            }

            return View();
        }
        public ActionResult Index()
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
            var Items = db.Candles;
            return View(Items);
            
        }
        public ActionResult FormForCost()
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
            
            return View();

        }
        public ActionResult UserCost(int User_Id = 0)//корзина
        {      

            if (Session["idUser"] == null)//проверка на существование значения сессии
            {
                Session["idUser"] = User_Id;
            }
            if (User_Id != (int)Session["idUser"])//если юзер авторизован то он перенапрявляется на другую стр
            {
                User_Id = (int)Session["idUser"];
                Response.Redirect("/Home/UserCost?User_Id="+ User_Id);
            }

            int i = 0;
            var Need_Candle = db.Candles;

            var Cost_This_User = db.Cost.Where(x => x.Id_Users == User_Id && x.Status_Cost == "Корзина");//производим выборку всех добавленных заказов в корзину

            int column = Cost_This_User.Count();//подсчитываем кол-во выбранного товара
            Models.Candles[] array = new Models.Candles[column];//выделяем память под массив моделей свечей
            
            foreach (var a in Cost_This_User)//формируем массив с нужными книгами
            {
                foreach(var b in Need_Candle)

                {
                    if (a.Id_Candle == b.Id_Candle)
                    {
                        array[i] = new Models.Candles
                        {
                            Id_Candle = b.Id_Candle,
                            Plase_of_origin = b.Plase_of_origin,
                            Product_name = b.Product_name,
                            Material = b.Material,
                            Color = b.Color,
                            Height = b.Height,
                            Weight = b.Weight,
                            Diameter = b.Diameter,
                            Old_Price = b.Old_Price,
                            Fragrance = b.Fragrance,
                            Image = b.Image,
                            Burning_time = b.Burning_time,
                            New_Price = b.New_Price

                        };
                    }
                }
                i++;
            }
            return View(array);//передаем массив в качестве параметра м-да View
        }

        [ChildActionOnly]//сво-во для предотвращения входа на эту страницу. При попытке отобразить тек страницу будет выдана ошибка о том что ее нет
        public ActionResult Nav_Material()
        {
            //вытащили все и провели сортировку по жанрам

            var Item = db.Candles.OrderBy(x => x.Material);
            int i = 0;
            Session["Column"] = Item.Count();

            string[] spisok = new string[Item.Count()];//выделяем память кпод массив материалов

            foreach(var t in Item)
            {
                if(i == 0)//поскольку свечи были зранее отсортированф по материалам. можем воспользоваться элементарным отбором значений без повторов
                {
                    spisok[i] = t.Material;

                    i++;
                }
                else
                if( spisok[i-1] != t.Material )
                {
                    spisok[i] = t.Material;
                    i++;
                }
            }
            string result = "";
            for (int p=0; p<i; p++)//формируем рез строку, содержащую список для навигации
            {
                result += "<li><a href = '/Home/This_material?material= " + spisok[p] + "align = center>" + spisok[p] + "</a><li><br>";
            }
            return Content(result);//возвращаем результат
        }

        public void Add_Cost(int User_Id, int Candle_Id)//добавить в корзину
        {
            //Session["Status_Cost"] = 0;
            //Session["NewUser"] = 0;
            Models.Cost NewCost = new Models.Cost
            {
                Id_Candle = Candle_Id,
                Id_Users = User_Id,
                Status_Cost = "Корзина"
            };
            db.Cost.Add(NewCost);
            db.SaveChanges();

            string url = Request.ServerVariables["HTTP_REFERER"];
            Response.Redirect(url);
        }

        public void Delete_Cost(int id_user, int id_candle)//удалить из корзины
        {
            var item = db.Cost.First(x => x.Id_Users == id_user && x.Id_Candle == id_candle && x.Status_Cost == "Корзина");
            db.Cost.Remove(item);
            db.SaveChanges();
            string url = Request.ServerVariables["HTTP_REFERER"];
            Response.Redirect(url);
        }
        public void Buy(string name, string family, string first_name, string telephon, string email)
        {
            int id_user = (int)Session["idUser"];
            var Items = db.Cost.Where(x => x.Id_Users == id_user && x.Status_Cost == "Корзина");
            var Cand = db.Candles;
            foreach(var p in Items)
            {
                //p.Email = email;
                //p.Name = name;
                //p.Second_name = family;
                //p.Telephon = telephon;
                p.Status_Cost = "Ожидание";
                foreach(var c in Cand)
                {
                    if(p.Id_Candle == c.Id_Candle)
                    c.Quantity = c.Quantity - 1;
                }
            }
            db.SaveChanges();
            Session["Status_Cost"] = 1;
            
            string url = Request.ServerVariables["HTTP_REFERER"];
            Response.Redirect("~/Home/FormForCost");
            //Response.Redirect(url);
        }

        [HttpPost]
        public void ToMeassage(string user_name, string email, string telephon_number, string user_message)
        {
            Models.Message NewMessage = new Models.Message
            {
                Name = user_name,
                Email = email,
                Telephon = telephon_number,
                Message1 = user_message
            };
            db.Message.Add(NewMessage);
            db.SaveChanges();
            Session["StatusMessage"] = 1;
            Response.Redirect("/Home/Contacts");
        }

        [HttpPost]    
        public void NewUser(string full_name, string first_name, string username, string password, string email, string telephon)
        {
            //Session["idUser"] = 0;
            var User = db.Users.Where(x => x.Username == username); //поиск существующих юзеров
            int q = User.Count();                                   //подсчет

            if(q == 0)
            {
                Models.Users NewUser = new Models.Users             //если таких нет, вносим в БД
                {
                    Name = full_name,
                    Second_name = first_name,
                    Email = email,
                    Username = username,
                    Password = password,
                    Telephon = telephon,
                    Avatar = "noavatar.jpg",
                    Status = 0
                };
                db.Users.Add(NewUser);

                db.SaveChanges();                                   //авторизация

                var ThisUser = db.Users.First(p => p.Username == username);

                Session["idUser"] = ThisUser.Id_Users;
                Session["Status_user"] = ThisUser.Status;
                Session["Name_user"] = ThisUser.Username;
                Session["Full_name"] = ThisUser.Name;
                Session["First_name"] = ThisUser.Second_name;
                Session["Email"] = ThisUser.Email;

                Response.Redirect("~/Home/Index");//Response.Redirect("~/Home/Intropage");
            }
            else
            {
                Session["NewUser"] = 1;                             //Обновляем стр и выводим сообщение
                Response.Redirect("~/Home/Register");
            }
        }
        [HttpPost]
        public void Authorization(string username, string password)
        {
            //Session["idUser"] = 0;
            var User = db.Users.Where(x => x.Username == username && x.Password == password);
            if (User.Any() == true)//Any, который возвращает значение Истина, если переменная хранит значение, и Ложь в противном случае. 
            {
                foreach (var t in User)
                {
                    Session["idUser"] = t.Id_Users;
                    Session["Status_user"] = t.Status;
                    Session["Name_user"] = t.Username;
                    Session["Full_name"] = t.Name;
                    Session["First_name"] = t.Second_name;
                    Session["Email"] = t.Email;
                    break;
                }
                Response.Redirect("~/Home/Index"); //Response.Redirect("~/Home/Intropage");
            }
            else
            {
                Session["Status_autrz"] = 1;
                Response.Redirect("~/Home/Login");
            }
        }
        [HttpPost]
        public void FormForCost(string username, string email)
        {
            //Session["idUser"] = 0;
            var User = db.Cost.Where(x => x.Status_Cost == "Ожидание" /*&& x.Id_Users == (int)Session["idUser"]*/);
            
                foreach (var t in User)
                {
                    t.Name = username;
                    t.Email = email;
                }
                db.SaveChanges();
            PushEmail();
            Response.Redirect("~/Home/Index"); //Response.Redirect("~/Home/Intropage");
           

        }

        public void PushEmail(int id_mess=1)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("photo.storage.066@gmail.com", "photo666"),
                EnableSsl = true
            };
            client.Send("photo.storage.066@gmail.com", "natasha.bidnyk@gmail.com",
                "Менеджер магазина CandleShop", "Спасибо за заказ!\n Ожидайте отправки товара.");
            Session["MessCompl"] = 1;

            //string url = Request.ServerVariables["HTTP_REFERER"];
            //Response.Redirect(url);

        }
    }

}