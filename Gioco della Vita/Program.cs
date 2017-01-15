using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gioco_della_Vita
{
    static class Program
    {
        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            CCamp prova1 = new CCamp();

            Console.WriteLine(prova1.ToString());

            for (int i = 0; i < prova1.Elements.GetLength(0); i++)
            {
                prova1.Elements[i].Shift += ShiftEventHandler;
            }

            for (int i = 0; i < 120; i++)
            {
                prova1.Elements[0].Move(prova1);
                prova1.Elements[1].Move(prova1);
                prova1.Elements[2].Move(prova1);
            }

            Console.ReadKey();
        }

        static void ShiftEventHandler(object sender, EventArgs e)
        {
            CCampEventArgs a = e as CCampEventArgs;
            if (a != null)
                Console.WriteLine(a.Table.ToString());
            Console.ReadKey();
        }
    }
}
