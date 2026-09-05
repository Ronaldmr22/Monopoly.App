using System;
using System.Collections.Generic;
using System.Text;

namespace Monopoly.App
{
    internal class Servidor
    {
        static Jugador[] ListaJugadores = [null, null, null, null];


        public static void AgregarJugador(Jugador Jugador)
        {
            ListaJugadores[0] = Jugador;
        }

        public static Jugador GetJugador(int Indice)
        {
            return ListaJugadores[Indice];
        }
    }
}
