using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeAll.model
{
    class LimitsCpu
    {
        public int PositionWrite { get; set; }
        public int PositionRead { get; set; }
        public int PositionMin { get; set; }
        public int PositionMax { get; set; }
    }
}
