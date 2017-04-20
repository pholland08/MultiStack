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
        private int num_stacks { get; set; }
        private int num_locations { get; set; }
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
        private MultiStack() : base() { }

        public MultiStack(int num_stacks, int num_locations) : base(num_locations + 1)
        {
            Top = num_locations + 1;
            this.num_locations = num_locations;
            this.num_stacks = num_stacks;
            bases = new int[this.num_stacks + 2];
            tops = new int[this.num_stacks + 2];
            oldTops = new int[this.num_stacks + 2];
            NewBases = new int[this.num_stacks + 2];
            growth = new int[this.num_stacks + 2];
            EqualAllocate = num_locations / num_stacks;

            // Initialize base and top locations
            for (int i = 1; i <= this.num_stacks; i++)
            {
                int current = Helpers.MyFloor((i - 1d) / num_stacks * num_locations);
                Console.WriteLine(current);
                bases[i] = current;
                tops[i] = current;
                oldTops[i] = current;
            }
            bases[this.num_stacks + 1] = num_locations;
        }

        // Push and Pop -------------------------------------------------------------------------
        public void push(int target, T obj)
        {
            tops[target] = tops[target] + 1;
            if (tops[target] > bases[target + 1])
            {
                // Handle overflow
                Console.WriteLine("Overflow on stack: " + target + " Attempted: " + obj.ToString() + ". Beginning reallocation..."); // TODO Remove before submission
                // TODO print contents of base[], top[], and oldtop[]
                reallocate(target);
                // TODO print contents of base[] and top[]
                Console.WriteLine("Reallocation complete");
            }
            StackSpace[tops[target]] = obj;
            Console.WriteLine("Pushed " + obj.ToString() + " into " + target);

        }

        public T pop(int target)
        {
            if (tops[target] == bases[target])
            {
                // Handle underflow
                Console.WriteLine("Underflow on stack " + tops[target]);
                return default(T);
            }
            else
            {
                T popped = StackSpace[tops[target]];
                tops[target] = tops[target] - 1;
                return popped;
            }
        }

        // MoveStack and Reallocate -------------------------------------------------------------

        // TODO build MoveStack
        private void movestack()
        {
            int delta = 0;

            // Move stacks down if space exists
            for (int j = 2; j <= num_stacks; j++)
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
            for (int j = num_stacks; j >= 2; j--)
            {
                if (NewBases[j] > bases[j])
                {
                    delta = NewBases[j] - bases[j];
                    for (int L = tops[j]; L >= (bases[j] + 1); L--)
                    {
                        // TODO Fix index out of bounds here
                        StackSpace[(L + delta)] = StackSpace[L];
                    }
                    bases[j] = NewBases[j];
                    tops[j] += delta;
                }
            }
        }

        // TODO build reallocate
        private void reallocate(int target)
        {
            SpaceAvail = bases[(num_stacks + 1)] - bases[0];
            int TotalIncrease = 0;
            int j = num_stacks;

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

            double MinSpace = (num_locations-1) * .05;
            if (SpaceAvail < MinSpace) // SpaceAvail < MinSpace - 1
            {
                // Report completely out of memory => terminate
                Console.WriteLine("All out of space!");
                Environment.Exit(500);
            }

            //this.EqualAllocate = this.SpaceAvail / this.num_stacks;
            EqualAllocate = .15d;
            GrowthAllocate = 1d - EqualAllocate;
            alpha = EqualAllocate * SpaceAvail / num_stacks;
            beta = GrowthAllocate * SpaceAvail / TotalIncrease;
            NewBases[1] = bases[1];
            double sigma = 0;

            for (int i = 2; i <= num_stacks; i++)
            {
                double tau = sigma + alpha + (growth[i - 1] * beta);
                NewBases[i] = (NewBases[i - 1] + (tops[i - 1] - bases[i - 1]) + Helpers.MyFloor(tau) - Helpers.MyFloor(sigma));
                sigma = tau;
            }

            tops[target] -= 1;
            movestack();
            tops[target] += 1;

            // TODO push item that caused overflow

            for (int i = 1; i < num_stacks; i++)
            {
                oldTops[i] = tops[i]; // TODO output oldTops
            }
        }
    }
}
