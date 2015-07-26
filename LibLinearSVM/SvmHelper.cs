using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
namespace LibLinearSVM
{
    public class SvmHelper
    {
        public static IntPtr CreateParameter(int solverType, double c, double eps)
        {
            parameter para = new parameter();
            para.solver_type = solverType;
            para.eps = eps;
            para.C = c;                       
            para.nr_weight = 0;
            para.weight_label = IntPtr.Zero;
            para.weight = IntPtr.Zero;
            para.p = 0.1d;
            para.init_sol = IntPtr.Zero;

            int size = Marshal.SizeOf(para);
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(para,ptr,true);
            return ptr;
            
        }

        public static IntPtr CreateFeatureNode(List<KeyValuePair<int, double>> instance)
        {
            int size = Marshal.SizeOf(typeof(feature_node));
            IntPtr ptr = Marshal.AllocHGlobal( size * instance.Count);
            IntPtr i_ptr_nodes = ptr;
            for (int i = 0; i < instance.Count; i++)
            {
                feature_node item = new feature_node();
                item.index = instance[i].Key;
                item.value = instance[i].Value;
                Marshal.StructureToPtr(item, i_ptr_nodes, true);
                i_ptr_nodes = IntPtr.Add(i_ptr_nodes, size);
            }
            return ptr;
        }

        public static IntPtr CreateProblem(DataInstances data)
        {
            
            problem prob = new problem();
            prob.l = data.Instances.Count;
            prob.n = data.FeatureCount;
            prob.y = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(double)) * data.Instances.Count);
            Marshal.Copy(data.Labels.ToArray(),0,prob.y,data.Labels.Count);
            prob.x = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)) * data.Instances.Count);
            IntPtr i_ptr_x = prob.x;
            for (int i = 0; i < data.Instances.Count; i++)
            {
                IntPtr inst = CreateFeatureNode(data.Instances[i]);
                Marshal.StructureToPtr(inst, i_ptr_x, true);
                i_ptr_x = IntPtr.Add(i_ptr_x, Marshal.SizeOf(typeof(IntPtr)));
            }
            
            prob.bias = -1.0d;

            int size = Marshal.SizeOf(prob);
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(prob, ptr, true);
            return ptr;
        }

        public static void FreeNode(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
                return;
            Marshal.DestroyStructure(ptr, typeof(IntPtr));
            Marshal.FreeHGlobal(ptr);
            ptr = IntPtr.Zero;
        }

        public static void FreeProblem(problem x)
        {
            Marshal.FreeHGlobal(x.y);
            x.y = IntPtr.Zero;
            IntPtr i_ptr_x = x.x;
            for (int i = 0; i < x.l; i++)
            {
                IntPtr ptr_nodes = (IntPtr)Marshal.PtrToStructure(i_ptr_x, typeof(IntPtr));
                FreeNode(ptr_nodes);
                i_ptr_x = IntPtr.Add(i_ptr_x, Marshal.SizeOf(typeof(IntPtr)));
            }
            Marshal.FreeHGlobal(x.x);
            x.x = IntPtr.Zero;
        }
        public static void FreeProblem(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
                return;
            problem x = (problem)Marshal.PtrToStructure(ptr, typeof(problem));
            FreeProblem(x);
            Marshal.DestroyStructure(ptr, typeof(problem));
            Marshal.FreeHGlobal(ptr);
            ptr = IntPtr.Zero;
        }

        public static void FreeModel(IntPtr ptr)
        {
            if (ptr != IntPtr.Zero)
            {
                LinearSvm.destroy_model(ptr);
            }
            ptr = IntPtr.Zero;
        }

        public static void FreeParameter(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
                return;
            parameter x = (parameter)Marshal.PtrToStructure(ptr, typeof(parameter));
            
            Marshal.FreeHGlobal(x.weight);
            x.weight = IntPtr.Zero;
            
            Marshal.FreeHGlobal(x.weight_label);
            x.weight_label = IntPtr.Zero;

            Marshal.FreeHGlobal(x.init_sol);
            x.init_sol = IntPtr.Zero;

            Marshal.DestroyStructure(ptr, typeof(parameter));
            Marshal.FreeHGlobal(ptr);
            ptr = IntPtr.Zero;
        }


        public static LibLinearSVM.DataInstances ReadProblem(string path)
        {
            var dataset = new LibLinearSVM.DataInstances();
            var max_index = 0;
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            using (StreamReader sr = new StreamReader(path))
            {
                while (true)
                {
                    string line = sr.ReadLine();
                    if (line == null)
                        break;

                    string[] list = line.Trim().Split(' ');
                    double y = Convert.ToDouble(list[0].Trim(), provider);
                    var nodes = new List<KeyValuePair<int, double>>();
                    for (int i = 1; i < list.Length; i++)
                    {
                        string[] temp = list[i].Split(':');
                        var Key = Convert.ToInt32(temp[0].Trim());
                        max_index = Math.Max(max_index, Key);
                        var Value = Convert.ToDouble(temp[1].Trim(), provider);
                        var node = new KeyValuePair<int, double>(Key,Value);
                        nodes.Add(node);
                    }
                    dataset.Add(nodes, y);
                }
            }
            dataset.FeatureCount = max_index;
            return dataset;
        }
    }
}
