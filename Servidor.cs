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
            for (int i=0; i != 4; i++){
                if (ListaJugadores[i] == null){
                    ListaJugadores[i] = Jugador;
                    break;
                }
            }
        }

        public static Jugador GetJugador(int Indice)
        {
            return ListaJugadores[Indice];
        }
    }
}
