using System;
using System.IO;

namespace MultiStack
{
    public class MyStack<T>
    {

        //---------------------------------------------------------------------
        //--- Fields
        //---------------------------------------------------------------------
        internal T[] StackSpace;

        int top;
        public int Top { get; set; }

        public int Length
        {
            get { return StackSpace.Length; }
        }

        //---------------------------------------------------------------------
        //--- Constructors
        //---------------------------------------------------------------------
        public MyStack()
        {
            top = 0;
        }
        public MyStack(int size) : this()
        {
            StackSpace = new T[size];
        }


        //---------------------------------------------------------------------
        // Push ---------------------------------------------------------------
        //---------------------------------------------------------------------
        public void push(T obj)
        {
            if (top == Length)
            {
                throw new IndexOutOfRangeException();
            }
            else if (top < Length)
            {
                StackSpace[top] = obj;
                top++;
            }
            else
            {
                throw new Exception("How did you get here??????");
            }
        }






        //---------------------------------------------------------------------
        // Pop ---------------------------------------------------------------
        //---------------------------------------------------------------------
        public T pop()
        {
            if (top == 0)
            {
                throw new IndexOutOfRangeException();
            }
            else
            {
                top--;
                return StackSpace[top];
            }
        }

        /////////////////// End of MyStack ////////////////////////////////
    }


    public class MyObj<T>
    {
        public T info { get; set; }

        private MyObj() { }

        public MyObj(T info) : this()
        {
            this.info = info;
        }

        /////////////////// End of MyObj ////////////////////////////////
    }

    public class InputObj
    {
        public char action { get; set; }
        public int target { get; set; }
        public string value { get; set; }
        public InputObj next { get; set; }

        // Constructors -----------------------------------------------------
        public InputObj()
        {
            action = '\0';
            target = -1;
            value = null;
            next = null;

        }
        public InputObj(string[] line) : this()
        {
            action = Char.Parse(line[0]);
            target = Int32.Parse(line[1]);
            value = line[2];
        }

        // Methods ----------------------------------------------------------
        public override string ToString()
        {
            return (action + "  " + (char)target + "  " + value);
        }

        /////////////////// End of InputObj ////////////////////////////////
    }

    public class MultiStack<T> : MyStack<T>
    {
        private int NumStacks { get; set; }
        private int LBound { get; set; }
        private int UBound { get; set; }
        private int LM { get; set; }
        private int L0 { get; set; }
        private int Shift { get; set; }
        private int[] bases;
        private int[] Optimized;
        private int[] tops;
        double EqualAllocate;
        double GrowthAllocate;
        double alpha;
        double beta;
        int SpaceAvail;
        int[] Growth;
        int[] OldTop;
        int[] NewBases;


        //-----------------------------------------------------------------
        // Constructors ---------------------------------------------------
        //-----------------------------------------------------------------

        // No argument Constructor 
        private MultiStack() : base() { }

        // Primary constructor
        public MultiStack(int NumStacks, int L0, int LM, int LBound, int UBound) : base(UBound - LBound)
        {
            this.LBound = LBound;
            this.UBound = UBound;
            this.Shift = L0 - LBound;
            this.L0 = L0;
            this.LM = LM - L0;
            this.NumStacks = NumStacks;
            bases = new int[this.NumStacks + 2];
            tops = new int[this.NumStacks + 2];
            Optimized = Growth = OldTop = NewBases = new int[this.NumStacks + 2];
            EqualAllocate = .15d;

            // Initialize base and top locations
            for (int i = 1; i <= this.NumStacks; i++)
            {
                int current = Helpers.MyFloor((i - 1d) / NumStacks * this.LM) + this.L0 + Shift;
                Console.WriteLine(current);
                bases[i] = tops[i] = Optimized[i] = current;
            }
            bases[this.NumStacks + 1] = tops[this.NumStacks + 1] = Optimized[this.NumStacks + 1] = this.LM + this.L0 + Shift;
        }

        //-----------------------------------------------------------------
        // Push -----------------------------------------------------------
        //-----------------------------------------------------------------
        public void push(int target, T obj)
        {
            tops[target] += 1;
            if (tops[target] > bases[target + 1])
            {
                // Handle overflow
                Console.WriteLine("\nOverflow on stack " + target + ". Attempted: " + obj.ToString() + ".\nBeginning reallocation...");

                // Print contents of base[], top[], and oldtop[]
                Console.WriteLine("\tStack:" + Helpers.IndexesToString(1, NumStacks + 1));
                Console.WriteLine("\tBase:" + Helpers.ArrayToString(bases, Shift));
                Console.WriteLine("\tTop:" + Helpers.ArrayToString(tops, Shift));
                Console.WriteLine("\tOldTop:" + Helpers.ArrayToString(OldTop, Shift));

                // Perform reallocation algorithm
                reallocate(target, obj);

                // Print contents of base[] and top[]
                Console.WriteLine("Reallocation complete");
                Console.WriteLine("\tStack:" + Helpers.IndexesToString(1, NumStacks + 1));
                Console.WriteLine("\tBase:" + Helpers.ArrayToString(bases, Shift));
                Console.WriteLine("\tTop:" + Helpers.ArrayToString(tops, Shift) + "\n");
            }
            else
            {
                StackSpace[tops[target]] = obj;
                Console.WriteLine("Pushed " + obj.ToString() + " into " + target);
            }

        }


        //-----------------------------------------------------------------
        // Pop ------------------------------------------------------------
        //-----------------------------------------------------------------
        public T pop(int target)
        {
            if (tops[target] == bases[target])
            {
                // Handle underflow
                Console.WriteLine("Underflow on stack {0}.", tops[target]);
                return default(T);
            }
            else
            {
                T popped = StackSpace[tops[target] + Shift];
                tops[target] = tops[target] - 1;
                return popped;
            }
        }

        //-----------------------------------------------------------------
        // MoveStack ------------------------------------------------------
        //-----------------------------------------------------------------

        private void movestack()
        {
            int delta = 0;

            // Move stacks down if space exists
            for (int j = 2; j <= NumStacks; j++)
            {
                if (NewBases[j] < bases[j])
                {
                    delta = bases[j] - NewBases[j];
                    for (int L = (bases[j] + 1); L <= (tops[j]); L++)
                    {
                        StackSpace[L - delta] = StackSpace[L];
                    }
                    bases[j] = NewBases[j];
                    tops[j] -= delta;
                }
            }


            // Move stacks up if space exists
            for (int j = NumStacks; j >= 2; j--)
            {
                if (NewBases[j] > bases[j])
                {
                    delta = NewBases[j] - bases[j];
                    for (int L = tops[j]; L >= (bases[j] + 1); L--)
                    {
                        StackSpace[(L + delta)] = StackSpace[L];
                    }
                    bases[j] = NewBases[j];
                    tops[j] += delta;
                }
            }
        }

        //-----------------------------------------------------------------
        // Reallocate -----------------------------------------------------
        //-----------------------------------------------------------------
        private void reallocate(int target, T obj)
        {
            SpaceAvail = LM - L0;
            int TotalIncrease = 0;
            int j = NumStacks;

            while (j > 0)
            {
                SpaceAvail = SpaceAvail - (tops[j] - bases[j]);
                if (tops[j] > OldTop[j])
                {
                    Growth[j] = tops[j] - OldTop[j];
                    TotalIncrease += Growth[j];
                }
                else
                {
                    Growth[j] = 0;
                }
                j--;
            }

            double MinSpace = (LM - (L0 + 1)) * .05d;
            if (SpaceAvail < MinSpace)
            {
                // Report completely out of memory => terminate
                Console.WriteLine("All out of space!\n");
                int i = 1;
                while (i<300000000)
                {
                    i++;
                }
                Environment.Exit(500);
            }

            GrowthAllocate = 1d - EqualAllocate;
            alpha = EqualAllocate * SpaceAvail / NumStacks;
            beta = GrowthAllocate * SpaceAvail / TotalIncrease;
            NewBases[1] = bases[1];
            double sigma = 0;

            for (int i = 2; i <= NumStacks; i++)
            {
                double tau = sigma + alpha + (Growth[i - 1] * beta);
                NewBases[i] = (NewBases[i - 1] + (tops[i - 1] - bases[i - 1]) + Helpers.MyFloor(tau) - Helpers.MyFloor(sigma));
                sigma = tau;
            }

            // Decrement the top of the item that triggered overflow
            tops[target] -= 1;

            // Perform repacking
            movestack();

            // Re-increment the item that triggered overflow
            tops[target] += 1;

            // Push the item that caused overflow
            StackSpace[tops[target]] = obj;
            Console.WriteLine("Pushed " + obj.ToString() + " into " + target);

            // Prepare OldTop for potential next overflow
            for (int i = 1; i < NumStacks; i++)
            {
                OldTop[i] = tops[i];
            }
        }
    }
}