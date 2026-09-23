namespace Monopoly.App
{
    public class Banco
    {
        public List<Jugador> ListaJugadores = [];
        public int IdTransaccion = 1;

        private Tablero_LL tablero;
        private HistorialTransacciones historial;
        private Juego juego;

        public Banco(Tablero_LL tablero, HistorialTransacciones historial, Juego juego)
        {
            this.tablero = tablero;
            this.historial = historial;
            this.juego = juego;
        }


        
        public bool Transferir(object Origen, object Destino, int Monto, string tipo, string Razon)
        {
            if (Destino is Jugador jugadorD)
            {
                if (Origen is Jugador jugadorO)
                {
                    if (jugadorO.PagarDinero(Monto))
                    {
                        Transaccion transaccion1 = new Transaccion(IdTransaccion, DateTime.Now, juego.TurnoActual(), tipo, jugadorO.GetNombre(), jugadorD.GetNombre(), Monto, Razon);
                        historial.InsertarTransaccion(transaccion1);
                        jugadorD.RecibirDinero(Monto);
                        return true;
                    }
                    else
                    {
                        Transaccion transaccion1 = new Transaccion(IdTransaccion, DateTime.Now, juego.TurnoActual(), tipo, jugadorO.GetNombre(), jugadorD.GetNombre(), jugadorO.GetSaldo(), Razon);
                        historial.InsertarTransaccion(transaccion1);
                        jugadorD.RecibirDinero(jugadorO.GetSaldo());
                        DestruirJugador(jugadorO);
                        return false;
                    }
                }
                Transaccion transaccion = new Transaccion(IdTransaccion, DateTime.Now, juego.TurnoActual(), tipo, "Banco", jugadorD.GetNombre(), Monto, Razon);
                historial.InsertarTransaccion(transaccion);
                jugadorD.RecibirDinero(Monto);
                return true;
            }
            else
            {
                if (Origen is Jugador jugadorO){ 
                    if (jugadorO.PagarDinero(Monto))
                    {
                        Transaccion transaccion = new Transaccion(IdTransaccion, DateTime.Now, juego.TurnoActual(), tipo, jugadorO.GetNombre(), "Banco", Monto, Razon);
                        historial.InsertarTransaccion(transaccion);
                        return true;
                    }
                    else
                    {
                        Transaccion transaccion = new Transaccion(IdTransaccion, DateTime.Now, juego.TurnoActual(), tipo, jugadorO.GetNombre(), "Banco", jugadorO.GetSaldo(), Razon);
                        historial.InsertarTransaccion(transaccion);
                        DestruirJugador(jugadorO);
                        return false;
                    }
                }
            }
            return false;
        }

        public void DestruirJugador(Jugador jugador)
        {

            ListaJugadores.Remove(jugador);
        }

        public bool ComprarPropiedad(int idJugador, int idCasilla)
        {
            Jugador? jugador = null;
            Propiedad? propiedad = null;

            for (int i = 0; i < 4; i++)
            {
                if (ListaJugadores[i].GetId() == idJugador)
                {
                    jugador = ListaJugadores[i];
                    break;
                }
            }

            for (Nodo nodo = tablero.GetHead(); nodo.Next != tablero.GetHead(); nodo = nodo.Next)
            {
                if (nodo.Data.NumeroCasilla == idCasilla && nodo.Data is Propiedad propiedadObjetivo)
                {
                    propiedad = propiedadObjetivo;
                }
            }
            if (jugador.GetSaldo() < propiedad.Precio)
            {
                return false;
            }
            else
            {
                Transferir(jugador, this, propiedad.Precio, "Compra de propiedad", $"{jugador.GetNombre()} ha comprado la propiedad {propiedad.Nombre} por {propiedad.Precio}");
                return true;
            }
        }

        public int CobrarAlquiler(int idJugador, int idPropiedad, int idDueño)
        {
            Jugador? jugador = null;
            Propiedad? propiedad = null;
            Jugador? dueño = null;

            for (int i = 0; i < 4; i++)
            {
                if (ListaJugadores[i].GetId() == idJugador)
                {
                    jugador = ListaJugadores[i];
                    break;
                }
            }
            for (int i = 0; i < 4; i++)
            {
                if (ListaJugadores[i].GetId() == idDueño)
                {
                    dueño = ListaJugadores[i];
                    break;
                }
            }
            for (Nodo nodo = tablero.GetHead(); nodo.Next != tablero.GetHead(); nodo = nodo.Next)
            {
                if (nodo.Data.NumeroCasilla == idPropiedad && nodo.Data is Propiedad propiedadObjetivo)
                {
                    propiedad = propiedadObjetivo;
                    break;
                }
            }
            Transferir(jugador, dueño, propiedad.Precio, "Cobro de alquiler", $"{jugador.GetNombre()} le ha pagado renta a {dueño.GetNombre()} por una cantidad de {propiedad.Precio}");
            return jugador.GetSaldo();
        }

        public void AgregarJugador(string nombreJugador, int id)
        {
            if (ListaJugadores.Count() < 4)
            {
                Jugador jugador = new Jugador(id, nombreJugador);
                ListaJugadores.Add(jugador);
            }
            else
            {
                ///Se ha alcanzado el máximo de jugadores, no se puede agregar más
            }
        }

        public Jugador BuscarJugador(int idJugador)
        {
            foreach (Jugador jugador in ListaJugadores)
            {
                if (jugador.GetId() == idJugador)
                {
                    return jugador;
                }
            }

            return null;
        }

        public int MoverJugador(int idJugador, int movimiento)
        {
            Jugador jugador = BuscarJugador(idJugador);

            int nuevaPosicion = jugador.GetPosicion() + movimiento;

            if (nuevaPosicion > 24)
            {
                nuevaPosicion = nuevaPosicion - 24;
            }

            jugador.SetPosicion(nuevaPosicion);

            return nuevaPosicion;
        }
        public string ResolverCasilla(int idJugador)
        {
            Jugador jugador = BuscarJugador(idJugador);

            Casilla casilla = tablero.BuscarCasilla(jugador.GetPosicion());

            if (casilla is Propiedad propiedad)
            {
                Jugador? dueño = BuscarDueñoPropiedad(propiedad.IdPropiedad);
                if (dueño == null)
                {
                    return "DISPONIBLE";
                }
                else if (dueño.GetId() == idJugador)
                {
                    return "PROPIA";
                }
                else
                {
                    return "OCUPADA";
                }
            }
            else if(casilla is CasillaEvento casillaEvento)
            {
                //return casillaEvento;
            }
            else if(casilla is CasillaEspecial casillaEspecial)
            {
                //return casillaEspecial;
            }
            return null;
        }

        public Jugador? BuscarDueñoPropiedad(int idPropiedad)
        {
            foreach (Jugador jugador in ListaJugadores)
            {
                if (jugador.GetPropiedades().TienePropiedad(idPropiedad))
                {
                    return jugador;
                }
            }

            return null;
        }

        public string Getinfo()
        {
            return "1";
        }

    }
}