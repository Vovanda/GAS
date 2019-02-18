using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAS.Common
{
    public interface IUniform
    {
        int Next();
        uint NextUInt();
        double NextDouble();
        float NextFloat();
    }
}
