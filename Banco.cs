namespace Monopoly.App
{
    public class Banco
    {
        public List<Jugador> ListaJugadores = [];
        public int IdTransaccion = 1;

        private Tablero_LL tablero;
        private HistorialTransacciones historial;
        private Juego juego;
        private ColaCartas cartas;

        public Banco(Tablero_LL tablero, HistorialTransacciones historial, Juego juego, ColaCartas cartas)
        {
            this.tablero = tablero;
            this.historial = historial;
            this.juego = juego;
            this.cartas = cartas;
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
                        IdTransaccion++;
                        jugadorD.RecibirDinero(Monto);
                        return true;
                    }
                    else
                    {
                        Transaccion transaccion1 = new Transaccion(IdTransaccion, DateTime.Now, juego.TurnoActual(), tipo, jugadorO.GetNombre(), jugadorD.GetNombre(), jugadorO.GetSaldo(), Razon);
                        historial.InsertarTransaccion(transaccion1);
                        IdTransaccion++;
                        jugadorD.RecibirDinero(jugadorO.GetSaldo());
                        DestruirJugador(jugadorO);
                        return false;
                    }
                }
                Transaccion transaccion = new Transaccion(IdTransaccion, DateTime.Now, juego.TurnoActual(), tipo, "Banco", jugadorD.GetNombre(), Monto, Razon);
                historial.InsertarTransaccion(transaccion);
                IdTransaccion++;
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
                        IdTransaccion++;
                        return true;
                    }
                    else
                    {
                        Transaccion transaccion = new Transaccion(IdTransaccion, DateTime.Now, juego.TurnoActual(), tipo, jugadorO.GetNombre(), "Banco", jugadorO.GetSaldo(), Razon);
                        historial.InsertarTransaccion(transaccion);
                        IdTransaccion++;
                        DestruirJugador(jugadorO);
                        return false;
                    }
                }
            }
            return false;
        }

        public void DestruirJugador(Jugador jugador)
        {
            juego.MuereJugador(jugador.GetId());
            ListaJugadores.Remove(jugador);
        }

        public bool ComprarPropiedad(int idJugador, int idCasilla)
        {
            Jugador? jugador = null;
            Propiedad? propiedad = null;

            for (int i = 0; i < ListaJugadores.Count; i++)
            {
                if (ListaJugadores[i].GetId() == idJugador)
                {
                    jugador = ListaJugadores[i];
                    break;
                }
            }

            Casilla casilla = tablero.BuscarCasilla(idCasilla);

            if (casilla is Propiedad propiedadObjetivo)
            {
                propiedad = propiedadObjetivo;
            }
            if (jugador.GetSaldo() < propiedad.Precio)
            {
                return false;
            }
            else
            {
                bool compraExitosa = Transferir(jugador, this, propiedad.Precio, "Compra de propiedad", $"{jugador.GetNombre()} ha comprado la propiedad {propiedad.Nombre} por {propiedad.Precio}");
                if (compraExitosa)
                {
                    jugador.AgregarPropiedad(propiedad);
                    return true;
                }

                return false;
            }
        }

        public bool CobrarAlquiler(int idJugador, int idPropiedad, int idDueño)
        {
            Jugador? jugador = null;
            Propiedad? propiedad = null;
            Jugador? dueño = null;

            for (int i = 0; i < ListaJugadores.Count; i++)
            {
                if (ListaJugadores[i].GetId() == idJugador)
                {
                    jugador = ListaJugadores[i];
                    break;
                }
            }
            for (int i = 0; i < ListaJugadores.Count; i++)
            {
                if (ListaJugadores[i].GetId() == idDueño)
                {
                    dueño = ListaJugadores[i];
                    break;
                }
            }
            Casilla casilla = tablero.BuscarCasilla(idPropiedad);

            if (casilla is Propiedad propiedadObjetivo)
            {
                propiedad = propiedadObjetivo;
            }
            return Transferir(jugador, dueño, propiedad.Alquiler, "Cobro de alquiler", $"{jugador.GetNombre()} le ha pagado renta a {dueño.GetNombre()} por una cantidad de {propiedad.Alquiler}");
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
                    return $"DISPONIBLE {propiedad.NumeroCasilla} {propiedad.Precio}";
                }
                else if (dueño.GetId() == idJugador)
                {
                    return $"PROPIA {propiedad.NumeroCasilla}";
                }
                else
                {
                    bool pagoExitoso = CobrarAlquiler(idJugador,propiedad.NumeroCasilla,dueño.GetId());
                    if (!pagoExitoso)
                    {
                        return $"ELIMINADO {idJugador} {dueño.GetId()}";
                    }
                    return $"OCUPADA {propiedad.NumeroCasilla} {dueño.GetId()}";
                }
            }
            else if(casilla is CasillaEvento casillaEvento)
            {
                CartaEvento carta = cartas.SacarCarta();

                carta.EjecutarEvento(jugador, this);

                return $"EVENTO {carta.IdCarta} {carta.Descripcion}";
            }
            else if(casilla is CasillaEspecial casillaEspecial)
            {
                if (casillaEspecial.Nombre == "Salida")
                {
                    Transferir(this, jugador, 200, "Premio de salida", $"{jugador.GetNombre()} recibió $200 por llegar a Salida");
                    return $"SALIDA {casillaEspecial.NumeroCasilla}";
                }
                else if (casillaEspecial.Nombre == "Carcel")
                {
                    jugador.SetTurnoPerdido(true);

                    return $"CARCEL {casillaEspecial.NumeroCasilla}";
                }
                else if (casillaEspecial.Nombre == "Casilla Libre")
                {
                    return $"LIBRE {casillaEspecial.NumeroCasilla}";
                }
            }
            return "SIN_ACCION";
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

        public Jugador ObtenerGanadorPorPatrimonio()
        {
            Jugador ganador = ListaJugadores[0];

            foreach (Jugador jugador in ListaJugadores)
            {
                if (jugador.GetPatrimonio() > ganador.GetPatrimonio())
                {
                    ganador = jugador;
                }
            }

            return ganador;
        }

        public string Getinfo()
        {
            return "1";
        }

    }
}