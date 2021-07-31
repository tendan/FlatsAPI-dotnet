using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlatsAPI.Settings.Permissions
{
    public class InvoicePermissions : IPermissions
    {
        public const string Read = "Invoice.Read";
        public const string ReadOthers = "Invoice.Read.Others";
    }
}
