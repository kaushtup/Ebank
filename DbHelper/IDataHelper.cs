using MigrationHelper.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DbHelper
{
    public interface IDataHelper: IDisposable
    {
        #region register
        Task<bool> RegisterUser(string username, string firstname, string lastname,
          string pinNumber, long accountNumber, string AccountType);
        #endregion

        #region login
        Task<Register> GetLoginUserAsync(string username, string pinnumber);
        #endregion

        #region currency
        Task<List<Currency>> GetCurrency();
        #endregion
    }
}