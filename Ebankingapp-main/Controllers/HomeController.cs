 using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;
using DbHelper;
using MigrationHelper.Models;

namespace EbankingApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDataHelper _Helper;

        public HomeController(IDataHelper helper)
        {
            _Helper = helper;
        }


        public ActionResult Login()
        {
            var model = new Login();
            return View(model);
        }


        [HttpPost]
        public async Task<ActionResult> Login(Login model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Result = "Enter all the required fields in current format.";
                return View(model);
            }

            var user = await _Helper.GetLoginUserAsync(model.Username, model.PinNumber);

            if (user == null)
            {
                ViewBag.Result = "Invalid username or pin number";
                return View(model);
            }

            if (ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home", new {Area ="Main" });


            }
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task <IActionResult> Register(Register model)
        {
            await _Helper.RegisterUser(
                model.Username,
                model.Firstname,
                model.Lastname,
                model.PinNumber,
                model.AccNum,
                model.AccountType
                );

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
