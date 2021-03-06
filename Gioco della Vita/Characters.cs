﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Gioco_della_Vita
{
    class CCharacter
    {
        public Thread ThisThread;
        public enum TypeAnimals { Nothing, Fox, Rabbit, Carrot };
        private int mPointLife;
        private TypeAnimals mType = TypeAnimals.Nothing, mTypeSearch = TypeAnimals.Nothing, mTypeAvoid = TypeAnimals.Nothing;
        private int mR, mC;
        private int[] mOldPos = new int[2];
        protected CCamp Table;

        #region property
        public int PointLife
        {
            get
            {
                return mPointLife;
            }
            set
            {
                mPointLife = value;
            }
        }

        public TypeAnimals Type
        {
            get
            {
                return mType;
            }
            set
            {
                mType = value;
            }
        }

        public TypeAnimals TypeSearch
        {
            get
            {
                return mTypeSearch;
            }
            set
            {
                if (value != Type)
                    mTypeSearch = value;
            }
        }

        public TypeAnimals TypeAvoid
        {
            get
            {
                return mTypeAvoid;
            }
            set
            {
                if (value != TypeSearch)
                    mTypeAvoid = value;
            }
        }

        public int R
        {
            get
            {
                return mR;
            }
            set
            {
                mR = value;
            }
        }

        public int C
        {
            get
            {
                return mC;
            }
            set
            {
                mC = value;
            }
        }

        public int[] OldPos
        {
            get
            {
                return mOldPos;
            }
            set
            {
                mOldPos = value;
            }
        }

        #endregion

        //events
        public event EventHandler Died;
        public event EventHandler Eaten;
        public event EventHandler Shift;

        /*public delegate void DiedEventHandler(EventArgs e);
        public delegate void EatenEventHandler(EventArgs e);
        public delegate void ShiftEventHandler(EventArgs e);*/

        protected virtual void OnDied(EventArgs e)
        {
            EventHandler handler = Died;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnEaten(EventArgs e)
        {
            EventHandler handler = Eaten;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnShift(CCampEventArgs e)
        {
            EventHandler handler = Shift;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        //builders
        public CCharacter()
        {
            PointLife = 0;
            Type = 0;
            TypeSearch = TypeAnimals.Nothing;
            TypeAvoid = TypeAnimals.Nothing;
            ThisThread = new Thread(new ThreadStart(Thread_Start));
        }

        protected CCharacter(CCamp Table)
        {
            PointLife = 0;
            Type = 0;
            TypeSearch = TypeAnimals.Nothing;
            TypeAvoid = TypeAnimals.Nothing;
            ThisThread = new Thread(new ThreadStart(Thread_Start));
            this.Table = Table;
        }

        protected CCharacter(int R, int C, CCamp Table)
        {
            this.R = R;
            this.C = C;
            this.OldPos[0] = R;
            this.OldPos[1] = C;
            ThisThread = new Thread(new ThreadStart(Thread_Start));
            this.Table = Table;
        }

        #region methods
        /// <summary>
        /// Start Thread of this element
        /// </summary>
        private void Thread_Start()
        {
            bool oneAlive = true;
            Random a = new Random(DateTime.Now.Millisecond);
            OnShift(new CCampEventArgs(Table));
            while (oneAlive)
            {
                System.Threading.Thread.Sleep(a.Next(300, 2001));
                if (this.Alive()) //if is still alive
                {
                    this.Move();
                    System.Threading.Thread.Sleep(75);
                }

                //verify if one is alive
                oneAlive = false;
                if (this.Alive())
                    oneAlive = true;
            }
            ThisThread.Abort();
        }

        /// <summary>
        /// Find next position
        /// </summary>
        /// <param name="PosRi">Initial Row</param>
        /// <param name="PosCi">Initial Column</param>
        /// <param name="Table">Table where game is played</param>
        /// <returns>An int[] of 2 elements</returns>
        public virtual void Move()
        {
            //no shift
            int[] NextMove = { R, C };

            //not death character
            if (Alive())
            {
                //save old position
                OldPos[0] = R;
                OldPos[1] = C;

                //find new position
                lock (Table)
                {
                    NextMove = FindPosition();
                    R = NextMove[0];
                    C = NextMove[1];

                    OnShift(new CCampEventArgs(Table));
                }
            }
            else
                Death();
        }

        /// <summary>
        /// What to do if still alive
        /// </summary>
        /// <param name="PosRi">Initial Row</param>
        /// <param name="PosCi">Initial Column</param>
        /// <param name="Table">Table where game is played</param>
        /// <returns>An int[] of 2 elements</returns>
        public virtual int[] FindPosition()
        {
            //no shift
            int[] NextMove = { R, C };

            //find next position
            NextMove = Possibility();

            //shift without eat
            if (Table.BlankSpace(NextMove[0], NextMove[1]) && Alive())
                PointLife--;
            else
            {
                //eat
                bool stop = false;
                int i = 0;
                for (; i < Table.Elements.GetLength(0) && !stop; i++)
                    if (Table.Elements[i].R == NextMove[0] && Table.Elements[i].C == NextMove[1])
                    {
                        this.PointLife += Table.Elements[i].PointLife;
                        stop = true;
                    }
                if (PointLife > 100)
                    PointLife = 100;
                i--;
                OnEaten(new EventArgs());

                Table.Elements[i].Death();
                Table.Elements[i].OnDied(new EventArgs());
            }

            return NextMove;
        }

        /// <summary>
        /// Found possible next position
        /// </summary>
        /// <param name="R">Initial Row</param>
        /// <param name="C">Initial Column</param>
        /// <param name="TypeSearch">What to eat</param>
        /// <param name="Table">Table where game is played</param>
        /// <returns>An int[] of 2 elements</returns>
        protected virtual int[] Possibility()
        {
            int[] NextPos = new int[2]; //posizione nella quale andare
            int[,] NearType = new int[Table.Side * Table.Side, 2]; //all near type
            double[] Distance = new double[3]; //distance, r, c with NearType

            #region find element
            //initializing NearType
            for (int a = 0; a < NearType.GetLength(0); a++)
                NearType[a, 1] = -1;

            //position on NearType
            var posNear = 0;

            //look for them
            for (int i = 0; i < Table.Elements.GetLength(0); i++)
                if (Table.Elements[i].Type == TypeSearch)
                {
                    NearType[posNear, 0] = Table.Elements[i].R;
                    NearType[posNear, 1] = Table.Elements[i].C;
                    posNear++;
                }

            //calculate distance
            bool continua = true;
            double temp = 0;
            Distance[0] = Table.Side * Table.Side + 1;
            Distance[1] = Distance[2] = -1;
            for (posNear = 0; posNear < NearType.GetLength(0) && continua; posNear++)
            {
                if (NearType[posNear, 1] != -1)
                {
                    //distance
                    temp = Math.Sqrt(Math.Pow(Convert.ToDouble(NearType[posNear, 0] - R), 2) + Math.Pow(Convert.ToDouble(NearType[posNear, 1] - C), 2));

                    if (temp < Distance[0])
                    {
                        Distance[0] = temp;
                        Distance[1] = Convert.ToDouble(NearType[posNear, 0]);
                        Distance[2] = Convert.ToDouble(NearType[posNear, 1]);
                    }
                }
                else
                    continua = false;
            }
            #endregion

            //no solution
            if (Distance[1] == -1)
            {
                #region no solution
                if (TypeAvoid != TypeAnimals.Nothing)
                    NextPos = Avoid();
                if (NextPos[0] == -1 || TypeAvoid == TypeAnimals.Nothing)
                {
                    if (R != 0 && C != 0 && Table.BlankSpace(R - 1, C - 1))
                    {
                        NextPos[0] = R - 1;
                        NextPos[1] = C - 1;
                    }
                    else
                        if (R != 0 && Table.BlankSpace(R - 1, C))
                        {
                            NextPos[0] = R - 1;
                            NextPos[1] = C;
                        }
                        else
                            if (R != 0 && C != Table.Side - 1 && Table.BlankSpace(R - 1, C + 1))
                            {
                                NextPos[0] = R - 1;
                                NextPos[1] = C + 1;
                            }
                            else
                                if (C != Table.Side - 1 && Table.BlankSpace(R, C + 1))
                                {
                                    NextPos[0] = R;
                                    NextPos[1] = C + 1;
                                }
                                else
                                    if (R != Table.Side - 1 && C != Table.Side - 1 && Table.BlankSpace(R + 1, C + 1))
                                    {
                                        NextPos[0] = R + 1;
                                        NextPos[1] = C + 1;
                                    }
                                    else
                                        if (R != Table.Side - 1 && Table.BlankSpace(R + 1, C))
                                        {
                                            NextPos[0] = R + 1;
                                            NextPos[1] = C;
                                        }
                                        else
                                            if (R != Table.Side - 1 && C != 0 && Table.BlankSpace(R + 1, C - 1))
                                            {
                                                NextPos[0] = R + 1;
                                                NextPos[1] = C - 1;
                                            }
                                            else
                                                if (C != 0 && Table.BlankSpace(R, C - 1))
                                                {
                                                    NextPos[0] = R;
                                                    NextPos[1] = C - 1;
                                                }
                }
                #endregion
            }
            else //possible solution
            {
                #region possible solution
                bool end = false;

                //distance
                double[] chance = new double[8];
                chance[0] = Math.Sqrt(Math.Pow(Convert.ToDouble(Distance[1] - R), 2) + Math.Pow(Convert.ToDouble(Distance[2] - (C + 1)), 2));
                chance[1] = Math.Sqrt(Math.Pow(Convert.ToDouble(Distance[1] - R), 2) + Math.Pow(Convert.ToDouble(Distance[2] - (C - 1)), 2));
                chance[2] = Math.Sqrt(Math.Pow(Convert.ToDouble(Distance[1] - (R + 1)), 2) + Math.Pow(Convert.ToDouble(Distance[2] - (C + 1)), 2));
                chance[3] = Math.Sqrt(Math.Pow(Convert.ToDouble(Distance[1] - (R + 1)), 2) + Math.Pow(Convert.ToDouble(Distance[2] - (C - 1)), 2));
                chance[4] = Math.Sqrt(Math.Pow(Convert.ToDouble(Distance[1] - (R + 1)), 2) + Math.Pow(Convert.ToDouble(Distance[2] - C), 2));
                chance[5] = Math.Sqrt(Math.Pow(Convert.ToDouble(Distance[1] - (R - 1)), 2) + Math.Pow(Convert.ToDouble(Distance[2] - (C + 1)), 2));
                chance[6] = Math.Sqrt(Math.Pow(Convert.ToDouble(Distance[1] - (R - 1)), 2) + Math.Pow(Convert.ToDouble(Distance[2] - (C - 1)), 2));
                chance[7] = Math.Sqrt(Math.Pow(Convert.ToDouble(Distance[1] - (R - 1)), 2) + Math.Pow(Convert.ToDouble(Distance[2] - C), 2));

                while (!end)
                {
                    int l = 0;
                    for (int k = 1; k < 8; k++)
                    {
                        if (chance[k] < chance[l])
                            l = k;
                    }

                    //setting up
                    if (chance[l] == Table.Side * Table.Side + 1)
                        end = true;
                    else
                    {
                        switch (l)
                        {
                            case 0:
                                NextPos[0] = R;
                                NextPos[1] = C + 1;
                                break;
                            case 1:
                                NextPos[0] = R;
                                NextPos[1] = C - 1;
                                break;
                            case 2:
                                NextPos[0] = R + 1;
                                NextPos[1] = C + 1;
                                break;
                            case 3:
                                NextPos[0] = R + 1;
                                NextPos[1] = C - 1;
                                break;
                            case 4:
                                NextPos[0] = R + 1;
                                NextPos[1] = C;
                                break;
                            case 5:
                                NextPos[0] = R - 1;
                                NextPos[1] = C + 1;
                                break;
                            case 6:
                                NextPos[0] = R - 1;
                                NextPos[1] = C - 1;
                                break;
                            case 7:
                                NextPos[0] = R - 1;
                                NextPos[1] = C;
                                break;
                        }

                        //object on the way
                        bool stop = false;
                        for (int i = 0; i < Table.Elements.GetLength(0) && !stop; i++)
                            if (Table.Elements[i].R == NextPos[0] && Table.Elements[i].C == NextPos[1])
                                if (Table.Elements[i].Type != 0 && Table.Elements[i].Type != TypeSearch)
                                {
                                    chance[l] = Table.Side * Table.Side + 1;
                                    stop = true;
                                }
                        if (stop == false)
                            end = true;
                    }
                }
                #endregion
            }

            return NextPos;
        }

        /// <summary>
        /// Found possible next position avoiding predators
        /// </summary>
        /// <param name="Table">Table where game is played</param>
        /// <returns>An int[] of 2 elements</returns>
        public virtual int[] Avoid()
        {
            int[,] AvoidPosition = new int[8, 2]; //nothing to avoid
            int[] AvoidPos = { -1, -1 };

            #region try all possible position
            if (R != 0 && C != 0)
            {
                AvoidPosition[0, 0] = R - 1;
                AvoidPosition[0, 1] = C - 1;
            }
            if (R != 0)
            {
                AvoidPosition[1, 0] = R - 1;
                AvoidPosition[1, 1] = C;
            }
            if (R != 0 && C != Table.Side - 1)
            {
                AvoidPosition[2, 0] = R - 1;
                AvoidPosition[2, 1] = C + 1;
            }
            if (C != Table.Side - 1)
            {
                AvoidPosition[3, 0] = R;
                AvoidPosition[3, 1] = C + 1;
            }
            if (R != Table.Side - 1 && C != Table.Side - 1)
            {
                AvoidPosition[4, 0] = R + 1;
                AvoidPosition[4, 1] = C + 1;
            }
            if (R != Table.Side - 1)
            {
                AvoidPosition[5, 0] = R + 1;
                AvoidPosition[5, 1] = C;
            }
            if (R != Table.Side - 1 && C != 0)
            {
                AvoidPosition[6, 0] = R + 1;
                AvoidPosition[6, 1] = C - 1;
            }
            if (C != 0)
            {
                AvoidPosition[7, 0] = R;
                AvoidPosition[7, 1] = C - 1;
            }
            #endregion

            //exclude dangerous position
            for (int k = 0; k < 8; k++)
                for (int i = 0; i < Table.Elements.GetLength(0); i++)
                    if (Table.Elements[i].R == AvoidPosition[k, 0] && Table.Elements[i].C == AvoidPosition[k, 1])
                        if (Table.Elements[i].Type == TypeAvoid)
                        {
                            AvoidPosition[k, 0] = -2;
                            AvoidPosition[k, 1] = -2;
                        }

            //find opposite of excluded
            bool work = true;
            int find = -1;
            for (int k = 0; k < 8 && work; k++)
            {
                if (AvoidPosition[k, 0] == -2)
                {
                    find = k;
                    work = false;
                }
            }

            #region change position
            if (find != -1)
            {
                switch (find)
                {
                    case 0:
                        if (R != Table.Side - 1 && C != Table.Side - 1)
                        {
                            AvoidPos[0] = R + 1;
                            AvoidPos[1] = C + 1;
                        }
                        else
                            AvoidPosition[find, 0] = -1;
                        break;
                    case 1:
                        if (R != Table.Side - 1)
                        {
                            AvoidPos[0] = R + 1;
                            AvoidPos[1] = C;
                        }
                        else
                            AvoidPosition[find, 0] = -1;
                        break;
                    case 2:
                        if (R != Table.Side - 1 && C != 0)
                        {
                            AvoidPos[0] = R + 1;
                            AvoidPos[1] = C - 1;
                        }
                        else
                            AvoidPosition[find, 0] = -1;
                        break;
                    case 3:
                        if (C != 0)
                        {
                            AvoidPos[0] = R;
                            AvoidPos[1] = C - 1;
                        }
                        else
                            AvoidPosition[find, 0] = -1;
                        break;
                    case 4:
                        if (R != 0 && C != 0)
                        {
                            AvoidPos[0] = R - 1;
                            AvoidPos[1] = C - 1;
                        }
                        else
                            AvoidPosition[find, 0] = -1;
                        break;
                    case 5:
                        if (R != 0)
                        {
                            AvoidPos[0] = R - 1;
                            AvoidPos[1] = C;
                        }
                        else
                            AvoidPosition[find, 0] = -1;
                        break;
                    case 6:
                        if (R != 0 && C != Table.Side - 1)
                        {
                            AvoidPos[0] = R - 1;
                            AvoidPos[1] = C + 1;
                        }
                        else
                            AvoidPosition[find, 0] = -1;
                        break;
                    case 7:
                        if (C != Table.Side - 1)
                        {
                            AvoidPos[0] = R;
                            AvoidPos[1] = C + 1;
                        }
                        else
                            AvoidPosition[find, 0] = -1;
                        break;
                }
            }
            #endregion

            return AvoidPos;
        }

        /// <summary>
        /// Know if this character is alive
        /// </summary>
        /// <returns>true if alive</returns>
        public bool Alive()
        {
            bool alive = false;

            if (PointLife != 0 && Type != TypeAnimals.Nothing)
                alive = true;
            return alive;
        }

        /// <summary>
        /// Kill this character
        /// </summary>
        public void Death()
        {
            PointLife = 0;
            Type = TypeAnimals.Nothing;
        }
        #endregion

    }

    class CFox : CCharacter
    {
        //builders
        public CFox(int R, int C, CCamp Table)
            : base(R, C, Table)
        {
            PointLife = 100;
            Type = TypeAnimals.Fox;
            TypeSearch = TypeAnimals.Rabbit;
        }

        //methods
    }

    class CRabbit : CCharacter
    {
        //builders
        public CRabbit(int R, int C, CCamp Table)
            : base(R, C, Table)
        {
            PointLife = 100;
            Type = TypeAnimals.Rabbit;
            TypeSearch = TypeAnimals.Carrot;
            TypeAvoid = TypeAnimals.Fox;
        }

        //methods
    }

    class CCarrot : CCharacter
    {
        //builders
        public CCarrot(int R, int C, CCamp Table)
            : base(R, C, Table)
        {
            PointLife = 100;
            Type = TypeAnimals.Carrot;
            TypeAvoid = TypeAnimals.Rabbit;
        }

        //methods
        public override void Move()
        {
            //not death character
            if (PointLife > 0 && Type != 0)
            {
                PointLife--;

                OnShift(new CCampEventArgs(Table));
            }
            else
            {
                Type = 0;
                PointLife = 0;
            }
        }
    }
}
