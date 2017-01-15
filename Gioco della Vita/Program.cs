using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gioco_della_Vita
{
    class Program
    {
        static void Main(string[] args)
        {
            CCamp prova1 = new CCamp();

            Console.WriteLine(prova1.ToString());

            for (int i = 0; i < 120; i++)
            {
                prova1.Elements[1].Move(prova1);
                prova1.Elements[0].Move(prova1);
                prova1.Elements[2].Move(prova1);
                Console.WriteLine(prova1.ToString());
                Console.ReadKey();
            }

            Console.ReadKey();
        }
    }
}
