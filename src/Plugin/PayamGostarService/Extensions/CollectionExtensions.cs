using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Septa.PgNopIntegration.Plugin.PayamGostarService.Extensions
{
    public static partial class CollectionExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            if (collection == null || !collection.Any())
                return true;
            else
                return false;
        }
    }
}
