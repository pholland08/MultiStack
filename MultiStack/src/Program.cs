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


            #region MainProcess

            int num_arrays = 3; // Number of sub_arrays
            int L0 = 4; // Lower bound
            int M = 20; // Upper bound
            //InputObj input_data = helpers.get_input("C:\\Users\\philliph\\workspace\\C#\\MultiStack\\MultiStack\\input\\BC_Input.txt");
            InputObj input_data = Helpers.get_input("C:\\Users\\philliph\\workspace\\C#\\MultiStack\\MultiStack\\input\\BC_Input_Reversed.txt");
            MyStack<InputObj> input_stack = new MyStack<InputObj>(input_data.target);
            input_data = input_data.next;

            MultiStack<string> stacks = new MultiStack<string>(num_arrays, (M - L0));
            
            while (input_data != null)
            {
                input_stack.push(input_data);
                input_data = input_data.next;
            }

            for (int i = 0; i < input_stack.Length; i++)
            {
                InputObj input_retrieved = input_stack.pop();
                switch (input_retrieved.action)
                {
                    case 'I':
                        try
                        {
                            stacks.push(input_retrieved.target, input_retrieved.value);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Exception caught: " + ex.Message);
                        }
                        break;
                    case 'D':
                        string popped = stacks.pop(input_retrieved.target);
                        if (!(popped == default(string)))
                        {
                            Console.WriteLine("Deleted " + popped + " from " + input_retrieved.target);
                        }
                        break;
                    default:
                        throw new InvalidDataException("Only (I)nsert and (D)elete are allowed.");
                }
            }
            #endregion MainProcess

        }
    }
}
