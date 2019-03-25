using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Extensions
{
    public static class CollectionExtensions
    {
        public static bool hasData<T>(this IList<T> list, T item )
        {
            if(list != null)
            {
                if(list.Count > 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
