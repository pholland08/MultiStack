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

        public MyObj() { }

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
        private int[] tops;


        // Constructors -------------------------------------------------------------------------
        // TODO Enums for subscripts
        private MultiStack() : base() { }

        public MultiStack(int num_stacks, int num_locations) : base(num_locations + 1)
        {
            this.Top = num_locations + 1;
            this.num_stacks = num_stacks;
            this.bases = new int[this.num_stacks + 2];
            this.tops = new int[this.num_stacks + 2];

            // Initialize base and top locations
            for (int i = 1; i < this.bases.Length; i++)
            {
                this.bases[i] = this.tops[i] = Convert.ToInt32(Math.Floor(((i - 1d) / num_stacks) * num_locations));
            }
        }

        // Push and Pop -------------------------------------------------------------------------
        public void push(int index, T obj)
        {
            this.tops[index] += 1;
            if (this.tops[index] > this.bases[index + 1])
            {
                // Handle overflow
                Console.WriteLine("Overflow on stack: " + index + " Attempted: " + obj.ToString()); // TODO Remove before submission
            }
            else
            {
                this.stack_arr[this.tops[index]] = obj;
                Console.WriteLine("Pushed " + obj.ToString() + " into " + index);
            }
        }

        public T pop(int index)
        {
            if (this.tops[index] == this.bases[index])
            {
                // Handle underflow
                Console.WriteLine("Underflow on stack " + index);
                return default(T);
            }
            else
            {
                this.tops[index] -= 1;
                return stack_arr[this.tops[index] + 1];
            }
        }

        // MoveStack and Reallocate -------------------------------------------------------------
        private void movestack()
        {
            // TODO build MoveStack
        }
        private void reallocate()
        {
            // TODO build reallocate
        }
    }
}