using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAS.Common
{
    public interface IRandom
    {
        int Next();
        uint NextUInt();
        double NextDouble();
        float NextFloat();
    }
}
