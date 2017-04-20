using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        //--- Methods
        //---------------------------------------------------------------------

        // Push ---------------------------------------------------------------
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

        // Pop ---------------------------------------------------------------
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
        private int[] NewBases;
        private int[] tops;
        private int[] oldTops;
        private int[] growth;
        double EqualAllocate;
        double GrowthAllocate;
        double alpha;
        double beta;
        int SpaceAvail;



        // TODO Enums for subscripts

        // Constructors -------------------------------------------------------------------------
        // No argument Constructor 
        private MultiStack() : base() { }

        // Primary constructor
        public MultiStack(int NumStacks, int L0, int LM, int LBound, int UBound) : base(UBound - LBound)
        {
            this.LBound = LBound;
            this.UBound = UBound;
            this.Shift = L0 - LBound;
            this.L0 = L0;
            //Top = LM + 1;
            this.LM = LM - L0;
            this.NumStacks = NumStacks;
            bases = new int[this.NumStacks + 2];
            tops = new int[this.NumStacks + 2];
            oldTops = new int[this.NumStacks + 2];
            NewBases = new int[this.NumStacks + 2];
            growth = new int[this.NumStacks + 2];
            EqualAllocate = this.LM / NumStacks;

            // Initialize base and top locations
            for (int i = 1; i <= this.NumStacks; i++)
            {
                int current = Helpers.MyFloor((i - 1d) / NumStacks * this.LM) + this.L0 + Shift;
                Console.WriteLine(current);
                bases[i] = tops[i] = oldTops[i] = current;
            }
            bases[this.NumStacks + 1] = tops[this.NumStacks + 1] = oldTops[this.NumStacks + 1] = this.LM + this.L0 + Shift;
        }

        // Push and Pop -------------------------------------------------------------------------
        public void push(int target, T obj)
        {
            tops[target] += 1;
            if (tops[target] > bases[target + 1])
            {
                // Handle overflow
                Console.WriteLine("\nOverflow on stack " + target + ". Attempted: " + obj.ToString() + ".\nBeginning reallocation..."); // TODO Remove before submission
                // TODO print contents of base[], top[], and oldtop[]
                Console.WriteLine("\tStack:" + Helpers.IndexesToString(1, NumStacks+1));
                Console.WriteLine("\tBase:" + Helpers.ArrayToString(bases, Shift));
                Console.WriteLine("\tTop:" + Helpers.ArrayToString(tops, Shift));
                Console.WriteLine("\tOldTop:" + Helpers.ArrayToString(oldTops, Shift));
                reallocate(target);
                // TODO print contents of base[] and top[]
                Console.WriteLine("Reallocation complete");
                Console.WriteLine("\tStack:" + Helpers.IndexesToString(1, NumStacks+1));
                Console.WriteLine("\tBase:" + Helpers.ArrayToString(bases, Shift));
                Console.WriteLine("\tTop:" + Helpers.ArrayToString(tops, Shift) + "\n");
            }
            StackSpace[tops[target]] = obj;
            Console.WriteLine("Pushed " + obj.ToString() + " into " + target);

        }

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

        // MoveStack and Reallocate -------------------------------------------------------------

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

        private void reallocate(int target)
        {
            SpaceAvail = bases[(NumStacks + 1)] - bases[1];
            int TotalIncrease = 0;
            int j = NumStacks;

            while (j > 0)
            {
                SpaceAvail = SpaceAvail - (tops[j] - bases[j]);
                if (tops[j] > oldTops[j])
                {
                    growth[j] = tops[j] - oldTops[j];
                    TotalIncrease += growth[j];
                }
                else
                {
                    growth[j] = 0;
                }
                j--;
            }

            double MinSpace = (LM - 1) * .05;
            if (SpaceAvail < MinSpace) // SpaceAvail < MinSpace - 1
            {
                // Report completely out of memory => terminate
                Console.WriteLine("All out of space!\n");
                DateTime wait = System.DateTime.Now.AddSeconds(.5);
                while (System.DateTime.Now < wait)
                {
                    int pause = 1 + 1;
                    pause++;
                }
                Environment.Exit(500);
            }

            //this.EqualAllocate = this.SpaceAvail / this.num_stacks;
            EqualAllocate = .15d;
            GrowthAllocate = 1d - EqualAllocate;
            alpha = EqualAllocate * SpaceAvail / NumStacks;
            beta = GrowthAllocate * SpaceAvail / TotalIncrease;
            NewBases[1] = bases[1];
            double sigma = 0;

            for (int i = 2; i <= NumStacks; i++)
            {
                double tau = sigma + alpha + (growth[i - 1] * beta);
                NewBases[i] = (NewBases[i - 1] + (tops[i - 1] - bases[i - 1]) + Helpers.MyFloor(tau) - Helpers.MyFloor(sigma));
                sigma = tau;
            }

            tops[target] -= 1;
            movestack();
            tops[target] += 1;

            // TODO push item that caused overflow

            for (int i = 1; i < NumStacks; i++)
            {
                oldTops[i] = tops[i];
            }
        }
    }
}
