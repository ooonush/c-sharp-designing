using System;
using System.Linq;

namespace Incapsulation.EnterpriseTask
{
    public class Enterprise
    {
        public readonly Guid Guid;
        
        public string Name { get; set; }
        public DateTime EstablishDate { get; set; }
        
        string _inn;
        public string Inn
        {
            get => _inn;
            set
            {
                if (value.Length != 10 || !value.All(char.IsDigit))
                    throw new ArgumentException();
                _inn = value;
            }
        }
        
        public Enterprise(Guid guid)
        {
            Guid = guid;
        }
        
        public TimeSpan ActiveTimeSpan => DateTime.Now - EstablishDate;

        public double GetTotalTransactionsAmount()
        {
            DataBase.OpenConnection();
            var amount = 0.0;
            foreach (Transaction t in DataBase.Transactions().Where(z => z.EnterpriseGuid == Guid))
                amount += t.Amount;
            return amount;
        }
    }
}
