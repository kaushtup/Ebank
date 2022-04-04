using DbHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MigrationHelper.Models;
using System.Threading.Tasks;

namespace EbankingApp.Areas.Main.Controllers
{
    [Area("Main")]
    public class HomeController : Controller
    {
        private readonly IDataHelper _Helper;

        public HomeController(IDataHelper helper)
        {
            _Helper = helper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Profile()
        {
            return View();
        }

        public async Task<IActionResult> LoadFund()
        {
            var currencyData = await   _Helper.GetCurrency();
           ViewData["CurrencyData"] = new SelectList(currencyData, "ID", "name");

            return View();
        }

        [HttpPost]
        public IActionResult LoadFund(UserAccount model)
        {
            return View();
        }

        [HttpGet]
        public IActionResult Transaction()
        {
            return View();
        }

        [HttpGet]
        public IActionResult TransactionSum()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            return View();
        }
    }
}
