
using MigrationHelper.Models;
using System;
using System.Threading.Tasks;

namespace DbHelper
{
    public partial class DataHelper
    {
        public async Task<bool> RegisterUser(string username, string firstname, string lastname,
            string pinNumber, long accountNumber, string accountType)
        {
            var objRegister = new Register
            {
                Username = username,
                Firstname = firstname,
                Lastname = lastname,
                PinNumber = pinNumber,
                AccNum = accountNumber,
                AccountType = accountType
            };

            try
            {
                await new Repository<Register>(contxt).AddAsync(objRegister);
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
    }
}
