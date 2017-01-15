using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Gioco_della_Vita
{
    public partial class Form1 : Form
    {
        private bool fox = false, rabbit = false, carrot = false; //coordinates defined by user (true)
        private Thread Fox, Rabbit, Carrot;
        private int width, height;
        private CCamp main;

        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void cbx1_CheckedChanged(object sender, EventArgs e)
        {
            if (textBox1.Enabled && textBox2.Enabled)
            {
                fox = false;
                textBox1.Enabled = false;
                textBox2.Enabled = false;
            }
            else
            {
                fox = true;
                textBox1.Enabled = true;
                textBox2.Enabled = true;
            }
        }

        private void cbx2_CheckedChanged(object sender, EventArgs e)
        {
            if (textBox3.Enabled && textBox4.Enabled)
            {
                rabbit = false;
                textBox3.Enabled = false;
                textBox4.Enabled = false;
            }
            else
            {
                rabbit = true;
                textBox4.Enabled = true;
                textBox3.Enabled = true;
            }
        }

        private void cbx3_CheckedChanged(object sender, EventArgs e)
        {
            if (textBox5.Enabled && textBox6.Enabled)
            {
                carrot = false;
                textBox5.Enabled = false;
                textBox6.Enabled = false;
            }
            else
            {
                carrot = true;
                textBox5.Enabled = true;
                textBox6.Enabled = true;
            }
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            btn1.Enabled = false;
            main = new CCamp();

            //setting starting position
            try
            {
                if (fox)
                {
                    main.Elements[0].R = Int32.Parse(textBox1.Text);
                    main.Elements[0].C = Int32.Parse(textBox2.Text);
                }
            }
            catch (Exception exc)
            {

            }

            try
            {
                if (rabbit)
                {
                    main.Elements[1].R = Int32.Parse(textBox4.Text);
                    main.Elements[1].C = Int32.Parse(textBox3.Text);
                }
            }
            catch (Exception exc)
            {

            }

            try
            {
                if (carrot)
                {
                    main.Elements[0].R = Int32.Parse(textBox6.Text);
                    main.Elements[0].C = Int32.Parse(textBox5.Text);
                }
            }
            catch (Exception exc)
            {

            }

            //setting properties
            width = btn0x0.Width;
            height = btn0x0.Height;

            //adding graphic information
            for (int i = 0; i < main.Elements.GetLength(0); i++)
                main.Elements[i].Shift += ShiftEventHandler;

            //start game
            Fox = new Thread(new ThreadStart(Fox_Start));
            Rabbit = new Thread(new ThreadStart(Rabbit_Start));
            Carrot = new Thread(new ThreadStart(Carrot_Start));
            Fox.Start();
            Rabbit.Start();
            Carrot.Start();
        }

        private void Fox_Start()
        {
            while (main.Elements[0].Alive())
            {
                System.Threading.Thread.Sleep(250);
                main.Elements[0].Move(main);
            }
            Fox.Abort();
        }

        private void Rabbit_Start()
        {
            while (main.Elements[1].Alive())
            {
                System.Threading.Thread.Sleep(250);
                main.Elements[1].Move(main);
            }
            Rabbit.Abort();
        }

        private void Carrot_Start()
        {
            while (main.Elements[2].Alive())
            {
                System.Threading.Thread.Sleep(250);
                main.Elements[2].Move(main);
            }
            Carrot.Abort();
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            try
            {
                btn1.Enabled = true;
                Fox.Abort();
                Rabbit.Abort();
                Carrot.Abort();
            }
            catch (Exception exc)
            {

            }

            main = null;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            btn2_Click(sender, e);
        }

        //eventhandler
        private delegate void CallShiftEventHandller(object sender, EventArgs e);

        public void ShiftEventHandler(object sender, EventArgs e)
        {
            CCampEventArgs a = e as CCampEventArgs;
            if (a == null)
                return;

            //set default
            btn0x0.Image = null;
            btn0x1.Image = null;
            btn0x2.Image = null;
            btn0x3.Image = null;
            btn0x4.Image = null;
            btn0x5.Image = null;
            btn0x6.Image = null;
            btn0x7.Image = null;
            btn0x8.Image = null;
            btn0x9.Image = null;
            btn1x0.Image = null;
            btn1x1.Image = null;
            btn1x2.Image = null;
            btn1x3.Image = null;
            btn1x4.Image = null;
            btn1x5.Image = null;
            btn1x6.Image = null;
            btn1x7.Image = null;
            btn1x8.Image = null;
            btn1x9.Image = null;
            btn2x0.Image = null;
            btn2x1.Image = null;
            btn2x2.Image = null;
            btn2x3.Image = null;
            btn2x4.Image = null;
            btn2x5.Image = null;
            btn2x6.Image = null;
            btn2x7.Image = null;
            btn2x8.Image = null;
            btn2x9.Image = null;
            btn3x0.Image = null;
            btn3x1.Image = null;
            btn3x2.Image = null;
            btn3x3.Image = null;
            btn3x4.Image = null;
            btn3x5.Image = null;
            btn3x6.Image = null;
            btn3x7.Image = null;
            btn3x8.Image = null;
            btn3x9.Image = null;
            btn4x0.Image = null;
            btn4x1.Image = null;
            btn4x2.Image = null;
            btn4x3.Image = null;
            btn4x4.Image = null;
            btn4x5.Image = null;
            btn4x6.Image = null;
            btn4x7.Image = null;
            btn4x8.Image = null;
            btn4x9.Image = null;
            btn5x0.Image = null;
            btn5x1.Image = null;
            btn5x2.Image = null;
            btn5x3.Image = null;
            btn5x4.Image = null;
            btn5x5.Image = null;
            btn5x6.Image = null;
            btn5x7.Image = null;
            btn5x8.Image = null;
            btn5x9.Image = null;
            btn6x0.Image = null;
            btn6x1.Image = null;
            btn6x2.Image = null;
            btn6x3.Image = null;
            btn6x4.Image = null;
            btn6x5.Image = null;
            btn6x6.Image = null;
            btn6x7.Image = null;
            btn6x8.Image = null;
            btn6x9.Image = null;
            btn7x0.Image = null;
            btn7x1.Image = null;
            btn7x2.Image = null;
            btn7x3.Image = null;
            btn7x4.Image = null;
            btn7x5.Image = null;
            btn7x6.Image = null;
            btn7x7.Image = null;
            btn7x8.Image = null;
            btn7x9.Image = null;
            btn8x0.Image = null;
            btn8x1.Image = null;
            btn8x2.Image = null;
            btn8x3.Image = null;
            btn8x4.Image = null;
            btn8x5.Image = null;
            btn8x6.Image = null;
            btn8x7.Image = null;
            btn8x8.Image = null;
            btn8x9.Image = null;
            btn9x0.Image = null;
            btn9x1.Image = null;
            btn9x2.Image = null;
            btn9x3.Image = null;
            btn9x4.Image = null;
            btn9x5.Image = null;
            btn9x6.Image = null;
            btn9x7.Image = null;
            btn9x8.Image = null;
            btn9x9.Image = null;

            //chose picture for every elements
            var foxPic = new Bitmap(Properties.Resources.Fox_Full, new Size(width, height));
            var rabbitPic = new Bitmap(Properties.Resources.Rabbit_Full, new Size(width, height));
            var carrotPic = new Bitmap(Properties.Resources.Carrot_Full, new Size(width, height));

            for (int i = 0; i < a.Table.Elements.GetLength(0); i++)
            {
                if ((a.Table.Elements[i] as CFox) != null) //fox
                {
                    if (a.Table.Elements[i].PointLife > 20 && a.Table.Elements[i].PointLife <= 50)
                        foxPic = new Bitmap(Properties.Resources.Fox_Half, new Size(width, height));
                    else
                        if (a.Table.Elements[i].PointLife > 0 && a.Table.Elements[i].PointLife <= 20)
                            foxPic = new Bitmap(Properties.Resources.Fox_AlmostDeath, new Size(width, height));
                }
                else
                    if ((a.Table.Elements[i] as CRabbit) != null) //rabbit
                    {
                        if (a.Table.Elements[i].PointLife > 20 && a.Table.Elements[i].PointLife <= 50)
                            rabbitPic = new Bitmap(Properties.Resources.Rabbit_Half, new Size(width, height));
                        else
                            if (a.Table.Elements[i].PointLife > 0 && a.Table.Elements[i].PointLife <= 20)
                                rabbitPic = new Bitmap(Properties.Resources.Rabbit_AlmostDeath, new Size(width, height));
                    }
                    else
                        if ((a.Table.Elements[i] as CCarrot) != null) //carrot
                        {
                            if (a.Table.Elements[i].PointLife > 20 && a.Table.Elements[i].PointLife <= 50)
                                carrotPic = new Bitmap(Properties.Resources.Carrot_Half, new Size(width, height));
                            else
                                if (a.Table.Elements[i].PointLife > 0 && a.Table.Elements[i].PointLife <= 20)
                                    carrotPic = new Bitmap(Properties.Resources.Carrot_AlmostDeath, new Size(width, height));
                        }
            }

            //setting position
            var picTab = foxPic;

            for (int i = 0; i < a.Table.Elements.GetLength(0); i++)
            {
                if ((a.Table.Elements[i] as CRabbit) != null) //rabbit
                    picTab = rabbitPic;
                else
                    if ((a.Table.Elements[i] as CCarrot) != null) //carrot
                        picTab = carrotPic;

                if (this.textBox1.InvokeRequired)
                {
                    CallShiftEventHandller d = new CallShiftEventHandller(ShiftEventHandler);
                    this.Invoke(d, new object[] { sender, a });
                }
                else
                if (a.Table.Elements[i].Alive()) //if it is in the camp
                {
                    switch (a.Table.Elements[i].R)
                    {
                        case 0:
                            switch (a.Table.Elements[i].C)
                            {
                                case 0:
                                    btn0x0.Image = picTab;
                                    break;
                                case 1:
                                    btn0x1.Image = picTab;
                                    break;
                                case 2:
                                    btn0x2.Image = picTab;
                                    break;
                                case 3:
                                    btn0x3.Image = picTab;
                                    break;
                                case 4:
                                    btn0x4.Image = picTab;
                                    break;
                                case 5:
                                    btn0x5.Image = picTab;
                                    break;
                                case 6:
                                    btn0x6.Image = picTab;
                                    break;
                                case 7:
                                    btn0x7.Image = picTab;
                                    break;
                                case 8:
                                    btn0x8.Image = picTab;
                                    break;
                                case 9:
                                    btn0x9.Image = picTab;
                                    break;
                            }
                            break;
                        case 1:
                            switch (a.Table.Elements[i].C)
                            {
                                case 0:
                                    btn1x0.Image = picTab;
                                    break;
                                case 1:
                                    btn1x1.Image = picTab;
                                    break;
                                case 2:
                                    btn1x2.Image = picTab;
                                    break;
                                case 3:
                                    btn1x3.Image = picTab;
                                    break;
                                case 4:
                                    btn1x4.Image = picTab;
                                    break;
                                case 5:
                                    btn1x5.Image = picTab;
                                    break;
                                case 6:
                                    btn1x6.Image = picTab;
                                    break;
                                case 7:
                                    btn1x7.Image = picTab;
                                    break;
                                case 8:
                                    btn1x8.Image = picTab;
                                    break;
                                case 9:
                                    btn1x9.Image = picTab;
                                    break;
                            }
                            break;
                        case 2:
                            switch (a.Table.Elements[i].C)
                            {
                                case 0:
                                    btn2x0.Image = picTab;
                                    break;
                                case 1:
                                    btn2x1.Image = picTab;
                                    break;
                                case 2:
                                    btn2x2.Image = picTab;
                                    break;
                                case 3:
                                    btn2x3.Image = picTab;
                                    break;
                                case 4:
                                    btn2x4.Image = picTab;
                                    break;
                                case 5:
                                    btn2x5.Image = picTab;
                                    break;
                                case 6:
                                    btn2x6.Image = picTab;
                                    break;
                                case 7:
                                    btn2x7.Image = picTab;
                                    break;
                                case 8:
                                    btn2x8.Image = picTab;
                                    break;
                                case 9:
                                    btn2x9.Image = picTab;
                                    break;
                            }
                            break;
                        case 3:
                            switch (a.Table.Elements[i].C)
                            {
                                case 0:
                                    btn3x0.Image = picTab;
                                    break;
                                case 1:
                                    btn3x1.Image = picTab;
                                    break;
                                case 2:
                                    btn3x2.Image = picTab;
                                    break;
                                case 3:
                                    btn3x3.Image = picTab;
                                    break;
                                case 4:
                                    btn3x4.Image = picTab;
                                    break;
                                case 5:
                                    btn3x5.Image = picTab;
                                    break;
                                case 6:
                                    btn3x6.Image = picTab;
                                    break;
                                case 7:
                                    btn3x7.Image = picTab;
                                    break;
                                case 8:
                                    btn3x8.Image = picTab;
                                    break;
                                case 9:
                                    btn3x9.Image = picTab;
                                    break;
                            }
                            break;
                        case 4:
                            switch (a.Table.Elements[i].C)
                            {
                                case 0:
                                    btn4x0.Image = picTab;
                                    break;
                                case 1:
                                    btn4x1.Image = picTab;
                                    break;
                                case 2:
                                    btn4x2.Image = picTab;
                                    break;
                                case 3:
                                    btn4x3.Image = picTab;
                                    break;
                                case 4:
                                    btn4x4.Image = picTab;
                                    break;
                                case 5:
                                    btn4x5.Image = picTab;
                                    break;
                                case 6:
                                    btn4x6.Image = picTab;
                                    break;
                                case 7:
                                    btn4x7.Image = picTab;
                                    break;
                                case 8:
                                    btn4x8.Image = picTab;
                                    break;
                                case 9:
                                    btn4x9.Image = picTab;
                                    break;
                            }
                            break;
                        case 5:
                            switch (a.Table.Elements[i].C)
                            {
                                case 0:
                                    btn5x0.Image = picTab;
                                    break;
                                case 1:
                                    btn5x1.Image = picTab;
                                    break;
                                case 2:
                                    btn5x2.Image = picTab;
                                    break;
                                case 3:
                                    btn5x3.Image = picTab;
                                    break;
                                case 4:
                                    btn5x4.Image = picTab;
                                    break;
                                case 5:
                                    btn5x5.Image = picTab;
                                    break;
                                case 6:
                                    btn5x6.Image = picTab;
                                    break;
                                case 7:
                                    btn5x7.Image = picTab;
                                    break;
                                case 8:
                                    btn5x8.Image = picTab;
                                    break;
                                case 9:
                                    btn5x9.Image = picTab;
                                    break;
                            }
                            break;
                        case 6:
                            switch (a.Table.Elements[i].C)
                            {
                                case 0:
                                    btn6x0.Image = picTab;
                                    break;
                                case 1:
                                    btn6x1.Image = picTab;
                                    break;
                                case 2:
                                    btn6x2.Image = picTab;
                                    break;
                                case 3:
                                    btn6x3.Image = picTab;
                                    break;
                                case 4:
                                    btn6x4.Image = picTab;
                                    break;
                                case 5:
                                    btn6x5.Image = picTab;
                                    break;
                                case 6:
                                    btn6x6.Image = picTab;
                                    break;
                                case 7:
                                    btn6x7.Image = picTab;
                                    break;
                                case 8:
                                    btn6x8.Image = picTab;
                                    break;
                                case 9:
                                    btn6x9.Image = picTab;
                                    break;
                            }
                            break;
                        case 7:
                            switch (a.Table.Elements[i].C)
                            {
                                case 0:
                                    btn7x0.Image = picTab;
                                    break;
                                case 1:
                                    btn7x1.Image = picTab;
                                    break;
                                case 2:
                                    btn7x2.Image = picTab;
                                    break;
                                case 3:
                                    btn7x3.Image = picTab;
                                    break;
                                case 4:
                                    btn7x4.Image = picTab;
                                    break;
                                case 5:
                                    btn7x5.Image = picTab;
                                    break;
                                case 6:
                                    btn7x6.Image = picTab;
                                    break;
                                case 7:
                                    btn7x7.Image = picTab;
                                    break;
                                case 8:
                                    btn7x8.Image = picTab;
                                    break;
                                case 9:
                                    btn7x9.Image = picTab;
                                    break;
                            }
                            break;
                        case 8:
                            switch (a.Table.Elements[i].C)
                            {
                                case 0:
                                    btn8x0.Image = picTab;
                                    break;
                                case 1:
                                    btn8x1.Image = picTab;
                                    break;
                                case 2:
                                    btn8x2.Image = picTab;
                                    break;
                                case 3:
                                    btn8x3.Image = picTab;
                                    break;
                                case 4:
                                    btn8x4.Image = picTab;
                                    break;
                                case 5:
                                    btn8x5.Image = picTab;
                                    break;
                                case 6:
                                    btn8x6.Image = picTab;
                                    break;
                                case 7:
                                    btn8x7.Image = picTab;
                                    break;
                                case 8:
                                    btn8x8.Image = picTab;
                                    break;
                                case 9:
                                    btn8x9.Image = picTab;
                                    break;
                            }
                            break;
                        case 9:
                            switch (a.Table.Elements[i].C)
                            {
                                case 0:
                                    btn9x0.Image = picTab;
                                    break;
                                case 1:
                                    btn9x1.Image = picTab;
                                    break;
                                case 2:
                                    btn9x2.Image = picTab;
                                    break;
                                case 3:
                                    btn9x3.Image = picTab;
                                    break;
                                case 4:
                                    btn9x4.Image = picTab;
                                    break;
                                case 5:
                                    btn9x5.Image = picTab;
                                    break;
                                case 6:
                                    btn9x6.Image = picTab;
                                    break;
                                case 7:
                                    btn9x7.Image = picTab;
                                    break;
                                case 8:
                                    btn9x8.Image = picTab;
                                    break;
                                case 9:
                                    btn9x9.Image = picTab;
                                    break;
                            }
                            break;
                    }
                }
            }
        }
    }
}
