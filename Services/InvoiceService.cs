using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlatsAPI.Services
{
    public interface IInvoiceService
    {
        byte[] GetInvoiceForSpecifiedAccount(int accountId);
    }
    public class InvoiceService : IInvoiceService
    {
        public byte[] GetInvoiceForSpecifiedAccount(int accountId)
        {
            throw new NotImplementedException();
        }
    }
}
