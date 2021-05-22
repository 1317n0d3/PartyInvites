using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PartyInvites.Models;
using System.Data.SqlClient;

namespace PartyInvites.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ViewResult guestsList()
        {
            List<GuestResponse> list = new List<GuestResponse>();
            string str = System.Configuration.ConfigurationManager.ConnectionStrings["guestsDB"].ConnectionString;
            using (SqlConnection con = new SqlConnection(str))
            {
                con.Open();
                SqlCommand com = new SqlCommand("select * from Invitation", con);
                SqlDataReader r = com.ExecuteReader();
                while(r.Read())
                {
                    GuestResponse gs = new GuestResponse();
                    gs.Name = r["Name"].ToString();
                    gs.Email = r["Email"].ToString();
                    gs.Phone = r["Phone"].ToString();
                    gs.WillAttend = (bool)r["Attempt"];
                    list.Add(gs);
                }
            }
            ViewBag.Item = list;
            return View();
        }

        [HttpGet]
        public ViewResult RsvpForm()
        {
            return View();
        }

        [HttpPost]
        public ViewResult RsvpForm(GuestResponse guest)
        {

            if (ModelState.IsValid)
            {
                string str = System.Configuration.ConfigurationManager.ConnectionStrings["guestsDB"].ConnectionString;
                using (SqlConnection con = new SqlConnection(str))
                {
                    con.Open();
                    SqlCommand com = new SqlCommand("insert into Invitation(Name, Email, Phone, Attempt) values ('" + guest.Name + "', '" + guest.Email + "', '" + guest.Phone + "', '" + guest.WillAttend + "')", con);
                    com.ExecuteNonQuery();
                }
                return View("Thanks", guest);
            }
            else
                return View();
        }
    }
}