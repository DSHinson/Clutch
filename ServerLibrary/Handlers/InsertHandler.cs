using ServerLibrary.Storage.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Handlers
{
    public class InsertHandler
    {
        private readonly TransactionManager _transactionManager;

        public InsertHandler(TransactionManager transactionManager)
        {
            _transactionManager = transactionManager;
        }
    }
}
