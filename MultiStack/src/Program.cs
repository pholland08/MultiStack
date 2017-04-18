using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiStack
{
    class Program
    {

        static void Main(string[] args)
        {

            Environment.Exit(500);


            #region MainProcess

            int num_arrays = 4; // Number of sub_arrays
            int L0 = 4; // Lower bound
            int M = 20; // Upper bound
            //InputObj input_data = helpers.get_input("C:\\Users\\philliph\\workspace\\C#\\MultiStack\\MultiStack\\input\\BC_Input.txt");
            InputObj input_data = Helpers.get_input("C:\\Users\\philliph\\workspace\\C#\\MultiStack\\MultiStack\\input\\BC_Input_Reversed.txt");
            MyStack<InputObj> input_stack = new MyStack<InputObj>(input_data.target);
            input_data = input_data.next;

            MultiStack<string> multiple = new MultiStack<string>(num_arrays, M - L0);

            while (input_data != null)
            {
                input_stack.push(input_data);
                input_data = input_data.next;
            }

            for (int i = 0; i < input_stack.Length; i++)
            {
                InputObj popped = input_stack.pop();
                switch (popped.action)
                {
                    case 'I':
                        try
                        {
                            multiple.push(popped.target, popped.value);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Exception caught: " + ex.Message);
                        }
                        break;
                    case 'D':
                        Console.WriteLine("Deleted " + multiple.pop(popped.target) + " from " + popped.target);
                        break;
                    default:
                        throw new InvalidDataException("Only (I)nsert and (D)elete are allowed.");
                }
            }
            #endregion MainProcess

        }
    }
}
