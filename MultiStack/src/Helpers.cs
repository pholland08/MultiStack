using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiStack
{
    public static class Helpers
    {
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
                InputObj new_object = new InputObj(line.Split('\t'));
                current_obj.next = new_object;
                current_obj = current_obj.next;
                list_head.target++;
            }
            file.Close();
            return list_head;
        }

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

        public static string ArrayToString(int[] input, int Shift)
        {
            string output = "";
            for (int i=1; i< input.Length; i++)
            {
                output += ("\t" + (input[i]-Shift));
            }
            return output;
        }

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
