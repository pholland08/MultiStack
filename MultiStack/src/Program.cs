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

            int num_arrays = 4; // Number of sub_arrays
            int L0 = 4; // Usable lower bound
            int LM = 20; // Usable upper bound
            int LBound = -11; // Absolute lower bound
            int UBound = 51; // Absolute upper bound
            //InputObj input_data = helpers.get_input("C:\\Users\\philliph\\workspace\\C#\\MultiStack\\MultiStack\\input\\BC_Input.txt");
            InputObj InputData = Helpers.get_input("C:\\Users\\philliph\\workspace\\C#\\MultiStack\\MultiStack\\input\\BC_Input_Reversed.txt");
            MyStack<InputObj> InputStack = new MyStack<InputObj>(InputData.target);
            InputData = InputData.next;

            MultiStack<string> stacks = new MultiStack<string>(num_arrays, L0, LM, LBound, UBound);
            
            while (InputData != null)
            {
                InputStack.push(InputData);
                InputData = InputData.next;
            }

            for (int i = 0; i < InputStack.Length; i++)
            {
                InputObj input_retrieved = InputStack.pop();
                switch (input_retrieved.action)
                {
                    case 'I':
                        try
                        {
                            stacks.push(input_retrieved.target, input_retrieved.value);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Exception caught: " + ex.Message); // So many indexes out of bounds
                            Console.WriteLine(ex.StackTrace);
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
