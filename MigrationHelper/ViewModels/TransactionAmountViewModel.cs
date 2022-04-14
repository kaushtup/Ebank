using MigrationHelper.Models;

namespace MigrationHelper.ViewModels
{
    public class TransactionAmountViewModel : MainModel
    {
        public string Name { get; set; }
        public long AccNum { get; set; }
        public string AccountType { get; set; }
        public double Balance { get; set; }
        public bool SendStatus { get; set; }
        public string Currency { get; set; }
    }
}
