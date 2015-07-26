using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
namespace LibLinearSVM
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct model
    {
        public parameter param;
        public int nr_class;		/* number of classes */
        public int nr_feature;
        public IntPtr w;// double* w;
        public IntPtr label;// int* label;		/* label of each class */
        public double bias;
    };
}
