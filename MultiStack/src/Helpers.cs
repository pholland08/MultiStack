using System;
using System.IO;

namespace MultiStack
{
    public static class Helpers
    {
        // Returns a list of InputObj items built from an input file
        public static InputObj get_input(String input_path)
        {
            InputObj list_head = new InputObj();
            list_head.target = 0;
            InputObj current_obj = list_head;
            string line;

            // Read the file and display it line by line.
            StreamReader file = new StreamReader(input_path);
            while ((line = file.ReadLine()) != null)
            {
                // Build the list
                InputObj new_object = new InputObj(line.Split('\t'));
                current_obj.next = new_object;
                current_obj = current_obj.next;
                list_head.target++;
            }
            file.Close();
            return list_head;
        }

        // Replacement for built in Math.Floor method
        public static int MyFloor(double num)
        {
            int temp = (Int32)num;
            if ((float)temp <= num)
            {
                return temp;
            }
            else
            {
                return temp - 1;
            }

        }




        public static void RunMultiStack(int N, int Location0, int MaxLocation, int LowerBound, int UpperBound, string InputFile)
        {
            int num_arrays = N; // Number of sub_arrays
            int L0 = Location0; // Usable lower bound
            int LM = MaxLocation; // Usable upper bound
            int LBound = LowerBound; // Absolute lower bound
            int UBound = UpperBound; // Absolute upper bound
            InputObj InputData = Helpers.get_input(InputFile);
            MyStack<InputObj> InputStack = new MyStack<InputObj>(InputData.target);
            InputData = InputData.next;

            MultiStack<String> stacks = new MultiStack<String>(num_arrays, L0, LM, LBound, UBound);

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

        }









        // Process int[] and return a string
        // EX. "1  2  3  4"
        public static string ArrayToString(int[] input, int Shift)
        {
            string output = "";
            for (int i=1; i< input.Length; i++)
            {
                output += ("\t" + (input[i]-Shift));
            }
            return output;
        }

        // Process lower and upper bounds and return a string
        // EX. "4  5  ...  19  20"
        public static string IndexesToString(int LBound, int UBound)
        {
            string output = "";
            int current = LBound;
            while (current <= UBound)
            {
                output += ("\t" + current);
                current++;
            }
            return output;
        }
    }
}
