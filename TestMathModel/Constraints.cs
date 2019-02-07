using System;
using System.Collections.Generic;
using System.Linq;

namespace TestMathModel
{
    internal class Constraints : IConstraints
    {
        public void Add(Func<IEnumerable<dynamic>, bool> expression, ISet<dynamic>[] sets)
        {
            ISet<IEnumerable<dynamic>> listOfparam = new HashSet<IEnumerable<dynamic>>() { Enumerable.Empty<dynamic>()};
            for (int i = 0; i < sets.Length; i++)
            {
                foreach (ISet<dynamic> set in sets)
                {
                    listOfparam = CartesianProduct(listOfparam, set);
                }
            }

            _results.Add(_expressions.Count, listOfparam);
            _expressions.Add(expression);
        }

        public IEnumerable<IDictionary<string, bool>> Calculate(params int[] idsExpression)
        {
            var resultIdsExpression = (idsExpression.Length > 0) ? idsExpression : Enumerable.Range(0, _expressions.Count);
            string paramsToString = "";
            bool ans;
            foreach (var id in resultIdsExpression)
            {
                var result = new Dictionary<string, bool>();
                foreach (var param in _results[id])
                {
                    paramsToString = param.Select(i => Convert.ToString(i)).Aggregate((a, b) => a + b);
                    ans = _expressions[id](param);
                    result.Add(paramsToString, ans);
                }
                yield return result;
            }
        }

        private ISet<IEnumerable<dynamic>> CartesianProduct(ISet<IEnumerable<dynamic>> set1, ISet<dynamic> set2)
        {
            HashSet<IEnumerable<dynamic>> result = new HashSet<IEnumerable<dynamic>>();
            foreach (var a in set1)
            {
                foreach (var b in set2)
                {
                    result.Add(a.Concat(new[] { b }));
                }
            }
            return result;
        }

        private List<Func<IEnumerable<dynamic>, bool>> _expressions = new List<Func<IEnumerable<dynamic>, bool>>();
        private Dictionary<int, ISet<IEnumerable<dynamic>>> _results = new Dictionary<int, ISet<IEnumerable<dynamic>>>();
    }

    public interface IConstraints
    {
        void Add(Func<IEnumerable<dynamic>, bool> expression, params ISet<dynamic>[] sets);
        IEnumerable<IDictionary<string, bool>> Calculate(params int[] idsExpression);
    }
}