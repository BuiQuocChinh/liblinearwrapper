using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
namespace LibLinearSVM
{
    [StructLayout(LayoutKind.Sequential)]
    public struct parameter
    {
        public int solver_type;
        /* these are for training only */
        public double eps;	        /* stopping criteria */
        public double C;
        public int nr_weight ;
        public IntPtr weight_label ;//int* weight_label;
        public IntPtr weight ;//double* weight;
        public double p;
        public IntPtr init_sol; //double* init_sol;
    };
}
