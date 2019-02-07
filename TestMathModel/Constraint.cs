using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMathModel
{
    internal class Constraint<T> : IConstraint<T> where T : IComparable<T>
    {
        public void SetExpression(Func<bool> expression)
        {
            _expression = expression;
        }

        public bool Value()
        {
            return _expression();
        }

        private Func<bool> _expression;
    }


    internal interface IConstraint<T> where T: IComparable<T>
    {
        void SetExpression(Func<bool> expression);        
        bool Value();
    }
}
