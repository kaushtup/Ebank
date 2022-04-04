using MigrationHelper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DbHelper
{
    public partial class DataHelper
    {
        public async Task<List<Currency>> GetCurrency()
        {
            var list = new List<Currency>();
            (await new Repository<Currency>(contxt).FindAllAsync()).ToList().ForEach(x => list.Add(x));
            return list;
        }
    }
}
