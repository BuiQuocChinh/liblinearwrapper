using System;
namespace TestLibSVM
{
    using LibLinearSVM;

    class Program
    {
        static void Main(string[] args)
        {
            // This is to test memory leak, please see 'Usage' method for actual use of the wrapper
            Console.WriteLine("Checking memory usage with a loop to allocate/release memory ....");
            TestMemory();
            
        }

        /// <summary>
        /// This method is used to check memory leak when calling 'train' function from native liblinear
        /// These is a small memory leak, it may happend inside the train function or the function that releases memory from model
        /// </summary>
        public static void TestMemory()
        {
            IntPtr ptr_prob = IntPtr.Zero; // problem
            IntPtr ptr_para = IntPtr.Zero; // parameter
            IntPtr ptr_model = IntPtr.Zero; // model
            try
            {
                DataInstances prob = SvmHelper.ReadProblem(@"news20.scale");
                DataInstances test = SvmHelper.ReadProblem(@"news20.t.scale");
                ptr_para = SvmHelper.CreateParameter((int)LibLinearSVM.SolverType.L2R_L2LOSS_SVC_DUAL, 1.0, 0.01);
                ptr_prob = SvmHelper.CreateProblem(prob);
                Console.WriteLine("Before loop, please capture  ....");
                Console.ReadKey();
                int idx = 0;
                for (; idx < 100; idx++) // this loop is used to check memory leak when call 'train' method from native liblinear
                {

                    // train model
                    ptr_model = LinearSvm.train(ptr_prob, ptr_para);
                    
                    // Release memory of the model
                    
                    if (ptr_model != IntPtr.Zero) SvmHelper.FreeModel(ptr_model);
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
                    //if (ptr_model != IntPtr.Zero) SvmHelper.FreeModel(ptr_model);
                    if (ptr_para != IntPtr.Zero) SvmHelper.FreeParameter(ptr_para);
                    if (ptr_prob != IntPtr.Zero) SvmHelper.FreeProblem(ptr_prob);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error:\t" + e.Message);
                    Console.ReadKey();
                }

            }
        }


        /// <summary>
        /// This method shows how to use the wrapper to train and test a given datasets
        /// </summary>
        public static void Usage()
        {
            IntPtr ptr_prob = IntPtr.Zero; // problem
            IntPtr ptr_para = IntPtr.Zero; // parameter
            IntPtr ptr_model = IntPtr.Zero; // model
            try
            {
                // Load train and test data
                DataInstances prob = SvmHelper.ReadProblem(@"news20.scale");
                DataInstances test = SvmHelper.ReadProblem(@"news20.t.scale");

                // Create paramter and train problem

                ptr_para = SvmHelper.CreateParameter((int)LibLinearSVM.SolverType.L2R_L2LOSS_SVC_DUAL, 1.0, 0.01);
                ptr_prob = SvmHelper.CreateProblem(prob);
                // Build model
                ptr_model = LinearSvm.train(ptr_prob, ptr_para);
                
                // Use model to predict 
                if (ptr_model != IntPtr.Zero)
                {
                    // Your logic business is here
                    int nr_class = LinearSvm.get_nr_class(ptr_model);
                    var estimates = new double[nr_class];
                    int i = 0;
                    foreach (var item in test.Instances)
                    {
                        IntPtr inst = SvmHelper.CreateFeatureNode(item);
                        var predicted = LinearSvm.predict_values(ptr_model, inst, estimates);

                        // Insert your code here to further process the predicted value!

                        i++;
                        SvmHelper.FreeNode(inst);
                    }
                }

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
                    if (ptr_model != IntPtr.Zero) SvmHelper.FreeModel(ptr_model);
                    if (ptr_para != IntPtr.Zero) SvmHelper.FreeParameter(ptr_para);
                    if (ptr_prob != IntPtr.Zero) SvmHelper.FreeProblem(ptr_prob);
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
