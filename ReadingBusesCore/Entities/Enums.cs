using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingBusesCore
{
    public enum LikelyhoodOfSuccess
    {
        Likely,
        Marginal,
        NotPossible,
    }

    public enum NamedRoute
    {
        Unknown = 0,
        HomeToWork,
        WorkToHome
    }
}
