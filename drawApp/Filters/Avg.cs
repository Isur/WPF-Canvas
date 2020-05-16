using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace drawApp.Filters
{
    public class Avg : Filter
    {
        public override double[,] Data { get; } = {
            { 1, 1, 1 },
            { 1, 1, 1 },
            { 1, 1, 1 }
        };

        public override int Norm { get; } = 1;

        public override int Bias { get; } = 0;
    }
}
