using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace drawApp.Filters
{
    public class Piramid : Filter
    {
        public override double[,] Data { get; } = {
            { 1, 2, 3, 2, 1 },
            { 2, 4, 6, 4, 2 },
            { 3, 6, 9, 6, 3 },
            { 2, 4, 6, 4, 2 },
            { 1, 2, 3, 2, 1 },
        };

        public override int Norm { get; } = 1;

        public override int Bias { get; } = 0;
    }
}
