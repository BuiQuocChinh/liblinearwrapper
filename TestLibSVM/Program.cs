using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibLinearSVM;
using Microsoft.Win32.SafeHandles;
namespace TestLibSVM
{
    using LibLinearSVM;

    class Program
    {
        static void Main(string[] args)
        {
            Test();
            Console.WriteLine("After releasing memory....");
            Console.ReadKey();
        }

        public static void Test()
        {
            IntPtr ptr_prob = IntPtr.Zero; // problem
            IntPtr ptr_para = IntPtr.Zero; // parameter
            IntPtr ptr_model = IntPtr.Zero; // model
            try
            {
                DataInstances prob = SvmHelper.ReadProblem(@"news20.scale");
                DataInstances test = SvmHelper.ReadProblem(@"news20.t.scale");
                Console.WriteLine("Before loop ....");
                Console.ReadKey();
                int idx = 0;
                for (; idx < 100; idx++)
                {
                    ptr_para = SvmHelper.CreateParameter((int)LibLinearSVM.SolverType.L2R_L2LOSS_SVC_DUAL, 1.0, 0.01);
                    ptr_prob = SvmHelper.CreateProblem(prob);
                    ptr_model = LinearSvm.train(ptr_prob, ptr_para);
                    if (ptr_model != IntPtr.Zero)
                    {
                    //    Console.WriteLine("Build model: OK");
                    //    int nr_class = LinearSvm.get_nr_class(ptr_model);
                    //    var estimates = new double[nr_class];
                    //    int i = 0;
                    //    foreach (var item in test.Instances)
                    //    {
                    //        IntPtr inst = SvmHelper.CreateFeatureNode(item);
                    //        var predicted = LinearSvm.predict_values(ptr_model, inst, estimates);
                    //        i++;
                    //        SvmHelper.FreeNode(inst);
                    //    }
                    }
                    if (ptr_model != IntPtr.Zero) SvmHelper.FreeModel(ptr_model);
                    if (ptr_para != IntPtr.Zero) SvmHelper.FreeParameter(ptr_para);
                    if (ptr_prob != IntPtr.Zero) SvmHelper.FreeProblem(ptr_prob);
                }
                Console.WriteLine("After loop ....");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:\t" + e.Message);
            }
            finally
            {
                // release memory: Important!!!!
                try
                {
                    
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error:\t" + e.Message);
                    Console.ReadKey();
                }
                
            }
        }
    }
}
