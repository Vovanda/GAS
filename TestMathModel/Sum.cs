using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMathModel
{
    internal class Sum
    {
        public Sum SetAdditionFunc<T>(Func<T, T, T> plus)
        {
            _plusFunctions.Add(typeof(T), plus);
            return this;
        }

        public T _<T>(Func<int, T> expression, int count) => _(expression, Enumerable.Range(0, count).ToHashSet());

        public T _<T>(Func<int, T> expression, int start, int end) => _(expression, Enumerable.Range(start, end - start).ToHashSet());
        public T _<U, T>(Func<U, T> expression, ISet<U> set)
        {
            if (set.Any())
            {
                var plus = _plusFunctions[typeof(T)] as Func<T, T, T>;
                return set.Select(i => expression(i)).Aggregate((a, b) => plus(a, b));
            }
            else
            {
                return default(T);
            }
        }

        private readonly Dictionary<Type, object> _plusFunctions = new Dictionary<Type, object>();
    }
}
