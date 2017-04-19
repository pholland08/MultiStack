﻿using System;
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
        internal T[] stack_arr;

        int top;
        public int Top { get; set; }

        public int Length
        {
            get { return this.stack_arr.Length; }
        }

        //---------------------------------------------------------------------
        //--- Constructors
        //---------------------------------------------------------------------
        public MyStack()
        {
            this.top = 0;
        }
        public MyStack(int size) : this()
        {
            this.stack_arr = new T[size];
        }


        //---------------------------------------------------------------------
        //--- Methods
        //---------------------------------------------------------------------

        // Push ---------------------------------------------------------------
        public void push(T obj)
        {
            if (this.top == this.Length)
            {
                throw new IndexOutOfRangeException();
            }
            else if (this.top < this.Length)
            {
                this.stack_arr[top] = obj;
                this.top++;
            }
            else
            {
                throw new Exception("How did you get here??????");
            }
        }

        // Pop ---------------------------------------------------------------
        public T pop()
        {
            if (this.top == 0)
            {
                throw new IndexOutOfRangeException();
            }
            else
            {
                this.top--;
                return this.stack_arr[this.top];
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
            this.action = '\0';
            this.target = -1;
            this.value = null;
            this.next = null;

        }
        public InputObj(string[] line) : this()
        {
            this.action = Char.Parse(line[0]);
            this.target = Int32.Parse(line[1]);
            this.value = line[2];
        }

        // Methods ----------------------------------------------------------
        public override string ToString()
        {
            return (this.action + "  " + (char)this.target + "  " + this.value);
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
            this.Top = num_locations + 1;
            this.num_locations = num_locations;
            this.num_stacks = num_stacks;
            this.bases = new int[this.num_stacks + 2];
            this.tops = new int[this.num_stacks + 2];
            this.oldTops = new int[this.num_stacks + 2];
            this.NewBases = new int[this.num_stacks + 2];
            this.growth = new int[this.num_stacks + 2];
            this.EqualAllocate = num_locations / num_stacks;

            // Initialize base and top locations
            for (int i = 1; i <= this.num_stacks; i++)
            {
                int current = Helpers.MyFloor((i - 1d) / num_stacks * num_locations);
                Console.WriteLine(current);
                this.bases[i] = current;
                this.tops[i] = current;
                this.oldTops[i] = current;
            }
            this.bases[this.num_stacks + 1] = num_locations;
        }

        // Push and Pop -------------------------------------------------------------------------
        public void push(int target, T obj)
        {
            this.tops[target] = this.tops[target] + 1;
            if (this.tops[target] > this.bases[target + 1])
            {
                // Handle overflow
                Console.WriteLine("Overflow on stack: " + target + " Attempted: " + obj.ToString() + ". Beginning reallocation..."); // TODO Remove before submission
                this.reallocate(target);
                Console.WriteLine("Reallocation complete");
            }
            this.stack_arr[this.tops[target]] = obj;
            Console.WriteLine("Pushed " + obj.ToString() + " into " + target);

        }

        public T pop(int target)
        {
            if (this.tops[target] == this.bases[target])
            {
                // Handle underflow
                Console.WriteLine("Underflow on stack " + this.tops[target]);
                return default(T);
            }
            else
            {
                T popped = stack_arr[this.tops[target]];
                this.tops[target] = this.tops[target] - 1;
                return popped;
            }
        }

        // MoveStack and Reallocate -------------------------------------------------------------

        // TODO build MoveStack
        private void movestack()
        {
            // Move stacks down if space exists
            for (int i = 2; i <= this.num_stacks; i++)
            {
                if (this.NewBases[i] < this.bases[i])
                {
                    int delta = this.bases[i] - this.NewBases[i];
                    for (int L = (this.bases[i] + 1); L <= (this.tops[i]); L++)
                    {
                        this.stack_arr[L - delta] = this.stack_arr[L];
                    }
                    this.bases[i] = this.NewBases[i];
                    this.tops[i] = this.tops[i - delta];
                }
            }

            // Move stacks up if space exists
            for (int i = this.num_stacks; i >= 2; i--)
            {
                if (this.NewBases[i] > this.bases[i])
                {
                    int delta = this.NewBases[i] - this.bases[i];
                    for (int L = this.tops[i]; L <= this.bases[i]; L--)
                    {
                        this.stack_arr[L + delta] = this.stack_arr[L];
                    }
                    this.bases[i] = this.NewBases[i];
                    this.tops[i] = this.tops[i] + delta;
                }
            }
        }

        // TODO build reallocate
        private void reallocate(int target)
        {
            SpaceAvail = this.bases[(this.num_stacks + 1)] - this.bases[0];
            int TotalIncrease = 0;
            int j = this.num_stacks;

            while (j > 0)
            {
                this.SpaceAvail = this.SpaceAvail - (this.tops[j] - this.bases[j]);
                if (this.tops[j] > this.oldTops[j])
                {
                    growth[j] = this.tops[j] - this.oldTops[j];
                    TotalIncrease += growth[j];
                }
                else
                {
                    this.growth[j] = 0;
                }
                j--;
            }

            if (this.SpaceAvail < 0) // SpaceAvail < MinSpace - 1
            {
                // Report completely out of memory => terminate
                Console.WriteLine("All out of space!");
                Environment.Exit(500);
            }

            this.EqualAllocate = this.SpaceAvail / this.num_stacks;
            this.GrowthAllocate = 1d - (this.EqualAllocate);
            this.alpha = this.EqualAllocate * this.SpaceAvail / this.num_stacks;
            this.beta = this.GrowthAllocate * this.SpaceAvail / TotalIncrease;
            this.NewBases[1] = this.bases[1];
            double sigma = 0;

            for (int i = 2; i <= this.num_stacks; i++)
            {
                double tau = sigma + alpha + (growth[i - 1] * beta);
                NewBases[i] = (Int32)(this.NewBases[i - 1] + (this.tops[i - 1] - this.bases[i - 1]) + Helpers.MyFloor(tau) - Helpers.MyFloor(sigma));
                sigma = tau;
            }

            this.tops[target]--;
            this.movestack();
            this.tops[target]++;

            // TODO push item that caused overflow

            for (int i = 1; i < num_stacks; i++)
            {
                this.oldTops[i] = this.tops[i]; // TODO output oldTops
            }
        }
    }
}
