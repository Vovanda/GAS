using System;
using System.Collections.Generic;
using System.Linq;

namespace TestMathModel
{
    internal class Constraints : IConstraints
    {
        public void Add(Func<IEnumerable<double>, bool> expression, ISet<double>[] sets)
        {
            ISet<IEnumerable<double>> listOfparam = new HashSet<IEnumerable<double>>() { Enumerable.Empty<double>()};
            for (int i = 0; i < sets.Length; i++)
            {
                foreach (ISet<double> set in sets)
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

        private ISet<IEnumerable<double>> CartesianProduct(ISet<IEnumerable<double>> set1, ISet<double> set2)
        {
            HashSet<IEnumerable<double>> result = new HashSet<IEnumerable<double>>();
            foreach (var a in set1)
            {
                foreach (var b in set2)
                {
                    result.Add(a.Concat(new[] { b }));
                }
            }
            return result;
        }

        private List<Func<IEnumerable<double>, bool>> _expressions = new List<Func<IEnumerable<double>, bool>>();
        private Dictionary<int, ISet<IEnumerable<double>>> _results = new Dictionary<int, ISet<IEnumerable<double>>>();
    }

    public interface IConstraints
    {
        void Add(Func<IEnumerable<double>, bool> expression, params ISet<double>[] sets);
        IEnumerable<IDictionary<string, bool>> Calculate(params int[] idsExpression);
    }
}