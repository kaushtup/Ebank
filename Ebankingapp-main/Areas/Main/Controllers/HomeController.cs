using DbHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MigrationHelper.Models;
using MigrationHelper.ViewModels;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EbankingApp.Areas.Main.Controllers
{
    [Area("Main")]
    public class HomeController : Controller
    {
        private readonly IDataHelper _Helper;
        private readonly IHttpContextAccessor _httpContAccessor;
        private IHttpClientFactory httpClientFactory;


        public HomeController(IDataHelper helper, IHttpContextAccessor httpContAccessor, IHttpClientFactory httpClientFactory)
        {
            _Helper = helper;
            _httpContAccessor = httpContAccessor;
            this.httpClientFactory = httpClientFactory;

        }

        public async Task<IActionResult> Index()
        {
            var client = httpClientFactory.CreateClient("API Client");
            var result = await client.GetAsync("http://api.exchangeratesapi.io/v1/latest?access_key=14fff920674f1a2697abf815c52c903a&format=1");
            var content = await result.Content.ReadAsStringAsync();

            var val  = JsonConvert.DeserializeObject<ExchangeRate>(content);

            var data = await _Helper.GetRate();
            
            await _Helper.DeleteRateById(data.FirstOrDefault().ID);
            await _Helper.AddRate
               (
                 val.rates.EUR,
                 val.rates.GBP,
                 val.rates.USD
               );

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = _httpContAccessor.HttpContext.User.FindFirstValue("Id");
            var data = await _Helper.GetUserByIdAsync(Convert.ToInt32(userId));

            var userAccountData = await _Helper.GetUserAccountByIdAsync(data.ID);
            var rateData = await _Helper.GetRate();


            //use api to convert currecny
            double balance = 0;
            foreach (var item in userAccountData)
            {
                if(item.CurrencyId == 1) 
                {
                    balance = balance + item.Balance;
                }
                else if (item.CurrencyId == 2) 
                {
                    balance = balance + item.Balance * rateData.FirstOrDefault().GBP;
                }
                else 
                {
                    balance = balance + item.Balance * 
                        (rateData.FirstOrDefault().GBP/ rateData.FirstOrDefault().USD);
                }
            }

            var model = new ProfileInfo()
            {
                FullName = data.Firstname + data.Lastname,
                AccNum = data.AccNum,
                AccountType = data.AccountType,
                Balance = Convert.ToString(balance)
            };

            return View(model);
        }

        public async Task<IActionResult> LoadFund()
        {
            var currencyData = await   _Helper.GetCurrency();
           ViewData["CurrencyData"] = new SelectList(currencyData, "ID", "name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoadFund(UserAccount model)
        {
            var userId = _httpContAccessor.HttpContext.User.FindFirstValue("Id");
            var data = await _Helper.GetUserByIdAsync(Convert.ToInt32(userId));
            
            //save to user accounts
            await _Helper.LoadFund
                (
                data.ID,
                data.AccNum,
                model.Balance,
                model.CurrencyId,
                DateTime.Now,
                model.Remark
                );

            return RedirectToAction("Index");
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
