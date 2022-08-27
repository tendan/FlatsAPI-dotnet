using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FlatsAPI.Models;

public class ColumnSelector<T> : Dictionary<string, Expression<Func<T, object>>>
{
}
