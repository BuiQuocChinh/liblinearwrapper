using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
namespace LibLinearSVM
{
    
    
    public enum SolverType : int { L2R_LR = 0, L2R_L2LOSS_SVC_DUAL = 1, L2R_L2LOSS_SVC = 2, L2R_L1LOSS_SVC_DUAL = 3, MCSVM_CS = 4, L1R_L2LOSS_SVC = 5, 
        L1R_LR, L2R_LR_DUAL = 6, L2R_L2LOSS_SVR = 11, L2R_L2LOSS_SVR_DUAL, L2R_L1LOSS_SVR_DUAL }; /* solver_type */

    public static class LinearSvm
    {
        // struct model* train(const struct problem *prob, const struct parameter *param);
        [DllImport("liblinear.dll", CallingConvention = CallingConvention.Cdecl)]       
        public static extern IntPtr train(IntPtr prob, IntPtr param);

        //double predict_values(const struct model *model_, const struct feature_node *x, double* dec_values);
        [DllImport("liblinear.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double predict_values(IntPtr model_, IntPtr x, double[] dec_values);

        // int save_model(const char *model_file_name, const struct model *model_);
        [DllImport("liblinear.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int save_model(string model_file_name, IntPtr model_);

        // struct model *load_model(const char *model_file_name);
        [DllImport("liblinear.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr load_model(string model_file_name);

        [DllImport("liblinear.dll", CharSet = CharSet.Ansi,CallingConvention = CallingConvention.Cdecl)]
        // int get_nr_feature(const struct model *model_);
        public static extern int get_nr_feature(IntPtr model_);

        [DllImport("liblinear.dll", CallingConvention = CallingConvention.Cdecl)]
        // int get_nr_class(const struct model *model_);
        public static extern int get_nr_class(IntPtr model_);

        [DllImport("liblinear.dll", CallingConvention = CallingConvention.Cdecl)]
        // void get_labels(const struct model *model_, int* label);
        public static extern void get_labels(IntPtr model_, out IntPtr label);
        
        [DllImport("liblinear.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void destroy_model(IntPtr model_ptr);

    }
}
