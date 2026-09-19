using System.Security.Cryptography.X509Certificates;

namespace Monopoly.App
{
    public class Juego
    {
        private ColaTurnos turnos;

        public Juego()
        {
            turnos = new ColaTurnos();
        }

        public void AgregarJugador(int idJugador)
        {
            turnos.Encolar(idJugador);
        }

        public int TurnoActual()
        {
            return turnos.TurnoActual();
        }

        public bool EsElTurnoDe(int idJugador)
        {
            return turnos.EsElTurnoDe(idJugador);
        }

        public void PasarTurno()
        {
            turnos.PasarTurno();
        }

        public void MuereJugador(int idJugador)
        {
            turnos.MuereJugador(idJugador);
        }
    }







    //https://youtu.be/ZsQMMeWXypk

    public class NodoTurno
    {
        public int IdJugador;
        public NodoTurno Siguiente;

        public NodoTurno(int idJugador)
        {
            IdJugador = idJugador;
            Siguiente = null;
        }
    }

    public class ColaTurnos
    {
        private NodoTurno turnoActual;
        private NodoTurno turnoUltimo;
        private int cantidad;

        public ColaTurnos()
        {
            turnoActual = null;
            turnoUltimo = null;
            cantidad = 0;
        }

        public void Encolar(int idJugador)
        {
            NodoTurno nuevo = new NodoTurno(idJugador);

            if (turnoActual == null)
            {
                turnoActual = nuevo;
                turnoUltimo = nuevo;
            }
            else
            {
                turnoUltimo.Siguiente = nuevo;
                turnoUltimo = nuevo;
            }

            cantidad++;
        }
        public int Desencolar()
        {
            if (turnoActual == null)
            {
                return -1;
            }

            int idJugador = turnoActual.IdJugador;
            turnoActual = turnoActual.Siguiente;

            if (turnoActual == null)
            {
                turnoUltimo = null;
            }

            cantidad--;
            return idJugador;
        }

        public int TurnoActual()
        {
            if (turnoActual == null)
            {
                return -1;
            }
            return turnoActual.IdJugador;
        }

        public void PasarTurno()
        {
            int idJugador = Desencolar();
            if (idJugador != -1)
            {
                Encolar(idJugador);
            }
        }

        public bool EsElTurnoDe(int idJugador)
        {
            return TurnoActual() == idJugador;
        }

        public int Cantidad()
        {
            return cantidad;
        }

        public void MuereJugador(int idJugador)
        {
            if (turnoActual == null)
            {
                return;
            }

            if (turnoActual.IdJugador == idJugador)
            {
                Desencolar();
                return;
            }

            NodoTurno anterior = turnoActual;
            NodoTurno nodo = turnoActual.Siguiente;

            while (nodo != null)
            {
                if (nodo.IdJugador == idJugador)
                {
                    anterior.Siguiente = nodo.Siguiente;

                    if (nodo == turnoUltimo)
                    {
                        turnoUltimo = anterior;
                    }

                    cantidad--;
                    return;
                }
                anterior = nodo;
                nodo = nodo.Siguiente;
            }
        }
    }
}