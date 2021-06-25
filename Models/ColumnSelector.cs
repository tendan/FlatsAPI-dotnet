using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FlatsAPI.Models
{
    public class ColumnSelector<T> : Dictionary<string, Expression<Func<T, object>>>
    {
    }
}
