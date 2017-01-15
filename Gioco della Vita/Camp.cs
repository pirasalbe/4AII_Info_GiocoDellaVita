using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gioco_della_Vita
{
    class CCamp
    {
        public bool ThreadOn = false;
        private int mSide;
        private CCharacter[] mElements;

        //property
        public int Side
        {
            get
            {
                return mSide;
            }
            set
            {
                mSide = value;
            }
        }

        public CCharacter[] Elements
        {
            get
            {
                return mElements;
            }
            set
            {
                mElements = value;
            }
        }

        //builders
        public CCamp()
        {
            //initializing
            Elements = new CCharacter[3];
            Side = 10;

            //creating character
            Generate(1, 1, 1);
        }

        public CCamp(int Number)
        {
            //initializing
            Elements = new CCharacter[3];
            Side = Number;

            //creating character
            Generate(1, 1, 1);
        }

        public CCamp(int Number, int Fox, int Rabbit, int Carrot)
        {
            //initializing
            int FRC = Fox + Rabbit + Carrot;
            Elements = new CCharacter[FRC];
            Side = Number;

            //creating character
            Generate(Fox, Rabbit, Carrot);
        }

        //methods
        /// <summary>
        /// Find if a space is free
        /// </summary>
        /// <param name="R">Row</param>
        /// <param name="C">Column</param>
        /// <returns>True if free</returns>
        public bool BlankSpace(int R, int C)
        {
            //find if there is an object
            bool space = true;

            for (int i = 0; i < Elements.GetLength(0) && space; i++)
                if ((Elements[i].R == R) && (Elements[i].C == C) && Elements[i].Type != 0)
                    space = false;

            return space;
        }

        /// <summary>
        /// Define elements position
        /// </summary>
        /// <param name="Fox">Number of fox</param>
        /// <param name="Rabbit">Number of rabbit</param>
        /// <param name="Carrot">Number of carrot</param>
        private void Generate(int Fox, int Rabbit, int Carrot)
        {
            //must generate random number position for every object
            int i = 0;
            Random posR, posC;
            posR = new Random(DateTime.Now.Millisecond);
            System.Threading.Thread.Sleep(5);
            posC = new Random(DateTime.Now.Millisecond);

            //position
            int r = 0, c = 0;

            //creating foxes
            for (int f = 0; f < Fox; f++)
            {
                r = posR.Next(Side);
                c = posC.Next(Side);

                Elements[i] = new CFox(r, c, this);
                i++;
            }

            //creating rabbits
            for (int R = 0; R < Rabbit; R++)
            {
                r = posR.Next(Side);
                c = posC.Next(Side);

                Elements[i] = new CRabbit(r, c, this);
                i++;
            }

            //creating carrots
            for (int C = 0; C < Carrot; C++)
            {
                r = posR.Next(Side);
                c = posC.Next(Side);

                Elements[i] = new CCarrot(r, c, this);
                i++;
            }

        }

        /// <summary>
        /// Show a text table
        /// </summary>
        /// <returns>table</returns>
        public override string ToString()
        {
            string table = "";

            //creating a table 
            int[,] tab = new int[Side, Side];
            for (int i = 0; i < Side; i++)
                for (int j = 0; j < Side; j++)
                    tab[i, j] = 0;

            //adding elements
            for (int i = 0; i < Elements.GetLength(0); i++)
                if (Elements[i].Type != 0)
                    tab[Elements[i].R, Elements[i].C] = 1000 * (int)Elements[i].Type + Elements[i].PointLife;

            //creating string
            for (int r = 0; r < Side; r++)
            {
                for (int c = 0; c < Side; c++)
                {
                    table += tab[r, c].ToString() + "\t";
                }
                table += "\n\n";
            }

            return table;
        }

        /// <summary>
        /// Start all elements' thread
        /// </summary>
        public void StartThread()
        {
            for (int i = 0; i < Elements.GetLength(0); i++)
            {
                System.Threading.Thread.Sleep(75);
                Elements[i].ThisThread.Start();
            }
            ThreadOn = true;
        }

        /// <summary>
        /// Stop all elements' thread
        /// </summary>
        public void AbortThread()
        {
            for (int i = 0; i < Elements.GetLength(0); i++)
                try
                {
                    Elements[i].ThisThread.Abort();
                }
                catch (Exception exc)
                {
                    Console.WriteLine("Abort failed {0}", i);
                }
            ThreadOn = false;
        }
    }
}
