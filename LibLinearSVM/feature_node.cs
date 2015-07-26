using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace LibLinearSVM
{
    [StructLayout(LayoutKind.Sequential)]
    public struct feature_node
    {
        public int index;
        public double value;
    };
}
