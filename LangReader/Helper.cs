using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LangReader
{
    public static class Helper
    {
        public static IEnumerable<T> CombineArray<T>(params IEnumerable<T>[] cas)
        {
            return cas.SelectMany(enumerable => enumerable);
        }
        public static IEnumerable<T> CombineArray<T>(T t, params IEnumerable<T>[] cas)
        {
            var j = new List<T>(cas.SelectMany(enumerable => enumerable));
            j.Insert(0, t);
            return j;
        }
    }
}
