using System;
using System.IO;

namespace Monopoly.App
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("El programa inició");

            Transaccion transaccion1 = new Transaccion(01,DateTime.Now,5,"compra de propiedad","Banco","Jugador 1",350,"Compra de (nombre de la propiedad)");
            Console.WriteLine("Transacción procesada correctamente");
            transaccion1.GuardarEnTxt();
            Console.WriteLine("Transacción guardada correctamente.");
            Console.WriteLine(Path.GetFullPath("transacciones.txt"));
            Console.ReadKey();
        }
    }
}