using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OneSignalApp.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace OneSignalApp.Controllers
{
    public class AppsController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // DO NOT TOUCH THIS 
        public const string URL_App = "https://onesignal.com/api/v1/apps";                                                       //  public const string URL_App = "https://onesignal.com/api/v1/notifications";
        public const string APP_ID = "c39ce5d1-418f-4dd4-9303-0c1ebbff9cda";
        public const string API_KEY = "YTRjOGNlN2ItMTBjMS00OTdjLWJmNzktY2Y5YWFiZTdiYWE3";
        public const string Auth_KEY = "YjQ4ZGMxNWEtN2Y2My00MTM5LWIzNzEtMzZjZDY1NWYwNTc3";

        static HttpClient clients = new HttpClient();
        // DO NOT TOUCH THIS 

        // GET: Apps
        public ActionResult Index()
        {
            return View();
        }
        public Boolean isAdminUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "Admin")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
       
        [HttpGet]
        public PartialViewResult _LoadViewApp()
        {
            ViewBag.id = User.Identity.GetUserId();

            if (isAdminUser())
            {
                ViewBag.displayMenu = "Admin";
            }

            JArray arrResult = new JArray();
            JObject objResult = new JObject();
            var objapp = new List<App>();

            string str = URL_App;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(str);
            request.ContentType = "application/json; charset=utf-8";
            request.Headers["Authorization"] = "Basic " + Auth_KEY;
            request.PreAuthenticate = true;

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;


            using (Stream responseStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream);


                JArray Jobject = JArray.Parse(reader.ReadToEnd());
                for (int i = 0; i < Jobject.Count; i++)
                {
                    var x = new App();
                    x.id = Jobject[i]["id"].ToString();
                    x.name = Jobject[i]["name"].ToString();

                    objapp.Add(x);
                }

            }
       

            return PartialView("~/Views/Apps/_LoadVieApp.cshtml", objapp);
        }
        [HttpGet]
        public JArray View_App()
        {

            //public const string URL_App = "https://onesignal.com/api/v1/apps";
            JArray arrResult = new JArray();
            JObject objResult = new JObject();
            try
            {
                string str = URL_App;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(str);
                request.ContentType = "application/json; charset=utf-8";
                request.Headers["Authorization"] = "Basic " + Auth_KEY;
                request.PreAuthenticate = true;

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;


                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream);


                    // JObject Jobject = JObject.Parse(reader.ReadToEnd());
                    JArray Jobject = JArray.Parse(reader.ReadToEnd());
                  


                    for (int i = 0; i < Jobject.Count; i++)
                    {

                        JObject obj = new JObject();
                        obj.Add("status", "");
                        obj.Add("id", Jobject[i]["id"]);
                        obj.Add("name", Jobject[i]["name"]);



                        arrResult.Add(obj);


                    }

                }
                return arrResult;
            }
            catch (Exception e)
            {
                JArray arrError = new JArray();

                JObject objError = new JObject();
                objError.Add("status", "error");
                objError.Add("messages", e.ToString());
                arrError.Add(objError);


                return arrError;

            }
        }


        [HttpPut]
        public async Task<bool> UpdateApp(string ID,string name)
        {



            var values = new Dictionary<string, string>{
            { "name", name },  };

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Basic " + Auth_KEY);
            client.DefaultRequestHeaders.Add(HttpRequestHeader.ContentType.ToString(), "application/json");
            var json = JsonConvert.SerializeObject(values, Formatting.Indented);

            var stringContent = new StringContent(json);
            var response = client.PostAsync("https://onesignal.com/api/v1/apps?id="+ID +"&api_key=[YTRjOGNlN2ItMTBjMS00OTdjLWJmNzktY2Y5YWFiZTdiYWE3]", stringContent).Result;

            var responseString = await response.Content.ReadAsStringAsync();

            if (responseString == "OK")
            {
                return true;
            }
            else
            {
                return false;

            }

            return true;

        }
        [HttpPost]
        public async Task<bool> CreateApp(string name)
        {



            var values = new Dictionary<string, string>{
            { "name", name },  };

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Basic " + Auth_KEY);
            client.DefaultRequestHeaders.Add(HttpRequestHeader.ContentType.ToString(), "application/json");
            var json = JsonConvert.SerializeObject(values, Formatting.Indented);

            var stringContent = new StringContent(json);
           var response = client.PostAsync("https://onesignal.com/api/v1/apps?api_key=[YTRjOGNlN2ItMTBjMS00OTdjLWJmNzktY2Y5YWFiZTdiYWE3]", stringContent).Result;

            var responseString = await response.Content.ReadAsStringAsync();

            if (responseString == "OK")
            {
                return true;
            }
            else
            {
                return false;

            }


        }


    }
}