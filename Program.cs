using System;
using System.IO;

namespace Monopoly.App
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("El programa inició");

            Transaccion transaccion1 = new Transaccion(
                1,
                DateTime.Now,
                1,
                "Compra",
                "Banco",
                "Jugador 1",
                200,
                "Compra de propiedad"
            );

            Console.WriteLine("Transacción creada");

            transaccion1.GuardarEnTxt();

            Console.WriteLine("Transacción guardada correctamente.");
            Console.WriteLine(Path.GetFullPath("transacciones.txt"));

            Console.ReadKey();
        }
    }
}