using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace drawApp.Filters
{
    public class Custom : Filter
    {
        public Custom(double [,] data)
        {
            this.Data = data;
        }
        public override double[,] Data { get; }

        public override int Norm { get; } = 1;

        public override int Bias { get; } = 0;
    }
}
