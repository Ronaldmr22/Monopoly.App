using System;
using System.IO;

namespace Monopoly.App
{
    class Program
    {
        static void Main(string[] args)
        {
            Jugador jugador = new Jugador(01, "Leo");
            Juego.servidor.GetBanco().ListaJugadores.Add(jugador);
            Console.WriteLine(Juego.servidor.GetBanco().ListaJugadores[0]);
            Console.WriteLine(Juego.servidor.GetBanco().ComprarPropiedad(01,02));
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}