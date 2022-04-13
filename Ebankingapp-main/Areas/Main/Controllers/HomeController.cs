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

            //need to change balance after transaction

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
        public async Task<IActionResult> TransactionSum()
        {
            var currencyData = await _Helper.GetCurrency();
            ViewData["CurrencyData"] = new SelectList(currencyData, "ID", "name");

            var userId = _httpContAccessor.HttpContext.User.FindFirstValue("Id");
            var data = await _Helper.GetUserByIdAsync(Convert.ToInt32(userId));

            var userAccountData = await _Helper.GetUserAccountByIdAsync(data.ID);
            var rateData = await _Helper.GetRate();

            double balance = 0;
            foreach (var item in userAccountData)
            {
                if (item.CurrencyId == 1)
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
                        (rateData.FirstOrDefault().GBP / rateData.FirstOrDefault().USD);
                }
            }

            var transData = new TransactionViewModel();

            transData.FromAccNum = data.AccNum;
            transData.FromUserId = data.ID;
            transData.FromName = data.Firstname + data.Lastname;
            transData.Balance = balance;

            return View(transData);
        }

        [HttpPost]
        public async Task<IActionResult> TransactionSum(TransactionViewModel model)
        {
            var rateData = await _Helper.GetRate();
            bool check = false;
            string message = "";
            if (model.CurrencyId == 1) 
            {
                if (model.Amount > model.Balance)
                {
                    message = "Not Enough Balance";
                    check = true;
                }
            }
            else if(model.CurrencyId == 2) 
            {
                if(model.Amount > model.Balance * rateData.FirstOrDefault().GBP)
                {
                    message = "Not Enough Balance";
                    check = true;
                }
            }
            else 
            {
                if (model.Amount > model.Balance * 
                        (rateData.FirstOrDefault().GBP / rateData.FirstOrDefault().USD))
                {
                    message = "Not Enough Balance";
                    check = true;
                }
            }

            var chk = await _Helper.GetUserByAccountNumberAsync(model.ToAccNum);
            var userId = _httpContAccessor.HttpContext.User.FindFirstValue("Id");
            var data = await _Helper.GetUserByIdAsync(Convert.ToInt32(userId));
            if (chk != null)
            {

                await _Helper.AddTransaction(data.ID, chk.ID,
                                     model.FromAccNum, model.ToAccNum,
                                     model.Amount, model.Remarks,
                                     model.CurrencyId
                                );
            }
            else 
            {
                check = true;
                message = "Given Account is not available";
            }

            var currencyData = await _Helper.GetCurrency();
            ViewData["CurrencyData"] = new SelectList(currencyData, "ID", "name");

            var transData = new TransactionViewModel();

            transData.FromAccNum = model.FromAccNum;
            transData.FromUserId = model.FromUserId;
            transData.FromName = model.FromName;
            transData.Balance = model.Balance;

            if (check)
            {
                ViewBag.Result = message;
                return View(transData);
            }


            return View(transData);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            return View();
        }
    }
}
