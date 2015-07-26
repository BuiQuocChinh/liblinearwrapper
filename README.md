# Liblinearwrapper
A compact C# wrapper for liblinear, see: https://github.com/cjlin1/liblinear for more detail.

This project provides a mininum wrapper for liblinear version 2.01.

The wrapper classes are located in the LibLinearSVM folder.

For usage, see the Program.cs file in the TestLibSVM folder.

Note: I added a new method namely 'destroy_model' to free memory for model. This method is similar to the 'free_and_destroy_model' of the native method but its parameter is a pointer to the model (model * ptr_model) instead of double pointer (model** model_prt_ptr).
