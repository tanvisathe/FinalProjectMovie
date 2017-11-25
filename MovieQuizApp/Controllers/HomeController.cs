using MovieQuizApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using System.Web.Security;
using System.Data.Sql;
using System.Data.SqlClient; //need for reading SQL database
using System.Text; //need for code reading SQL database


namespace MovieQuizApp.Controllers
{
    public class HomeController : Controller
    {

        private MovieQuizApp_dbEntities db = new MovieQuizApp_dbEntities();//reference to our MovieQuiz Databse
        //Registration user; // global user variable of registration

 

        public ActionResult Index()
        {
            return View();
        } 


        
        public ActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Registration reg)
        {
            if (ModelState.IsValid)
            {
                db.Registrations.Add(reg);
                db.SaveChanges();               
                return RedirectToAction("Login");
            }
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Registration reg)
        {
            if (ModelState.IsValid)
            {
                using (MovieQuizApp_dbEntities db = new MovieQuizApp_dbEntities())
                {
                    var obj = db.Registrations.Where(a => a.Username.Equals(reg.Username) && a.Password.Equals(reg.Password)).FirstOrDefault();
                    ViewBag.IsloggedIn = true;
                    
                    if (obj != null)
                    {
                        Session["UserID"] = obj.UserID.ToString();
                        Session["UserName"] = obj.Username.ToString();
                        ViewBag.UserName = obj.Username.ToString();
                        ViewBag.USERid = obj.UserID.ToString();
                        return RedirectToAction("Welcome", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid Credentials");
                    }
                }
            }
           
            return View(reg);
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            //return View();
            return View();
        }


        public ActionResult Welcome()
        {
            ViewBag.UserName = Session["Username"];
            ViewBag.USERid = Session["UserID"];
            
            return View();
        }

        public ActionResult MovieNotFound()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }


        public ActionResult GetData(Descriptor d)
        {
            Registration registration = new Registration();
           
            string genreID = d.genre;
            string primaryreleasedate = d.primaryReleaseDate;
            string voteaverage = d.VoteAverage;
            string primarylanguage = d.PrimaryLanguage;

            ViewBag.UserName = Session["Username"];
            ViewBag.USERid = Session["UserID"];


           
            HttpWebRequest request = WebRequest.CreateHttp("http://api.themoviedb.org/3/discover/movie?api_key=92d7084568b97fb382838cc03254d49e&language=en-US&with_genres=" + (genreID) + "&" + (primaryreleasedate) + "&" + (voteaverage) + "&" + (primarylanguage) + "&type=Json");
            
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

           
             StreamReader rd = new StreamReader(response.GetResponseStream());

            //Return the data in string format 
             String data = rd.ReadToEnd();

            
            JObject o = JObject.Parse(data);

            
            List<string> titlelist = new List<string>();         
            List<string> languagelist = new List<string>();
            List<string> picturelist = new List<string>();
            List<string> overviewlist = new List<string>();
            List<string> primaryreleasedatelist = new List<string>();
            List<string> voteaveragelist = new List<string>();
           
            for (int i = 0; i < o["results"].Count(); i++)
            {
                string title = o["results"][i]["original_title"].ToString();
                string language = o["results"][i]["original_language"].ToString();
                string picture = o["results"][i]["poster_path"].ToString();
                string overview1 = o["results"][i]["overview"].ToString();
                string primaryreleasedate1 = o["results"][i]["release_date"].ToString();
                string voteaverage1 = o["results"][i]["vote_average"].ToString();

                titlelist.Add(title);
                languagelist.Add(language);
                picturelist.Add("http://image.tmdb.org/t/p/w300" + picture);
                overviewlist.Add(overview1);
                primaryreleasedatelist.Add(primaryreleasedate1);
                voteaveragelist.Add(voteaverage1);

            }

            ViewBag.Titles = titlelist;
            ViewBag.AllLanguages = languagelist;
            ViewBag.AllPictures = picturelist;
            ViewBag.AllOverviews = overviewlist;
            ViewBag.AllPrimaryReleaseDates = primaryreleasedatelist;
            ViewBag.AllVoteAverages = voteaveragelist;

            
            Random r = new Random();
            int rando = r.Next(0, 20);

            try
            {
                for (int i = 0; i < titlelist.Count(); i++)

                {
                    ViewBag.Titles = titlelist.ElementAt(rando);
                    ViewBag.AllLanguages = languagelist.ElementAt(rando);
                    ViewBag.AllPictures = picturelist.ElementAt(rando);
                    ViewBag.AllOverviews = overviewlist.ElementAt(rando);
                    ViewBag.AllPrimaryReleaseDates = primaryreleasedatelist.ElementAt(rando);
                    ViewBag.AllVoteAverages = voteaveragelist.ElementAt(rando);
                }
            }

            catch (Exception)

            {
                return RedirectToAction("MovieNotFound", "Home");
            }

            ////Code to save movie title in database begins..
            ////*******************************************************************
            Movy m = new Movy();
                                
            if (ModelState.IsValid)
            {
                m.UserID = int.Parse(Session["UserID"].ToString());                            
                m.Title = ViewBag.Titles;
                db.Movies.Add(m);
                db.SaveChanges();
                return View();

            }
            //*******************************************************************
            
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}