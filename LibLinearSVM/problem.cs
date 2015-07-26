using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
namespace LibLinearSVM
{
    [StructLayout(LayoutKind.Sequential)]
    public struct problem
    {
        public int l, n;
        public IntPtr y; // double * y
        public IntPtr x; //feature_node** x;
        public double bias;            /* < 0 if no bias term */
    };
}
