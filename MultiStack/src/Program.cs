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
            // C&B Start file, A Start File
            string MyPath = "C:\\Users\\philliph\\workspace\\C#\\MultiStack\\MultiStack\\input\\C_Start.txt";

            StreamReader file = new StreamReader(MyPath);

            int N = Int32.Parse(file.ReadLine());
            int L0 = Int32.Parse(file.ReadLine());
            int MaxLocation = Int32.Parse(file.ReadLine());
            int LowerBound = Int32.Parse(file.ReadLine());
            int UpperBound = Int32.Parse(file.ReadLine());
            String C_InputFile = file.ReadLine();

            Helpers.RunMultiStack(N, L0, MaxLocation, LowerBound, UpperBound, C_InputFile);

        }
    }
}
