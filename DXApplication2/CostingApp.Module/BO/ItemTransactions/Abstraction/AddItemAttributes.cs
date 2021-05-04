using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostingApp.Module.BO.ItemTransactions.Abstraction {       
    [AttributeUsage(AttributeTargets.Class)]
    public class AddItemClassAttribute : Attribute {
        public AddItemClassAttribute(EnumInventoryTransactionType transactionType) {
            TransactionType = transactionType;
        }
        public EnumInventoryTransactionType TransactionType;
    }
}
