using System.Diagnostics.Tracing;

namespace Monopoly.App
{
    public class Banco
    {
        public LL_Jugadores ListaJugadores = new LL_Jugadores();
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
            ListaJugadores.BorrarJugador(jugador);
        }

        public bool ComprarPropiedad(int idJugador, int idCasilla)
        {
            Jugador? jugador = null;
            Propiedad? propiedad = null;

            for (NodoJugador nodo = ListaJugadores.GetHead(); nodo != null; nodo = nodo.GetNext())
            {
                if (nodo.GetData().GetId() == idJugador)
                {
                    jugador = nodo.GetData();
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

            for (NodoJugador nodo = ListaJugadores.GetHead(); nodo != null; nodo = nodo.GetNext())
            {
                if (nodo.GetData().GetId() == idJugador)
                {
                    jugador = nodo.GetData();
                    break;
                }
            }
            for (NodoJugador nodo1 = ListaJugadores.GetHead(); nodo1 != null; nodo1 = nodo1.GetNext())
            {
                if (nodo1.GetData().GetId() == idDueño)
                {
                    dueño = nodo1.GetData();
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
            if (ListaJugadores.GetCantidad() < 4)
            {
                Jugador jugador = new Jugador(id, nombreJugador);
                ListaJugadores.AgregarJugador(jugador);
            }
            else
            {
                ///Se ha alcanzado el máximo de jugadores, no se puede agregar más
            }
        }

        public Jugador BuscarJugador(int idJugador)
        {
            for (NodoJugador nodo = ListaJugadores.GetHead(); nodo != null; nodo = nodo.GetNext())
            {
                if (nodo.GetData().GetId() == idJugador)
                {
                    return nodo.GetData();
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
                jugador.RecibirDinero(200);
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
            for (NodoJugador nodo = ListaJugadores.GetHead(); nodo != null; nodo = nodo.GetNext())
            {
                if (nodo.GetData().GetPropiedades().TienePropiedad(idPropiedad))
                {
                    return nodo.GetData();
                }
            }

            return null;
        }

        public Jugador ObtenerGanadorPorPatrimonio()
        {
            Jugador? ganador = ListaJugadores.GetHead().GetData();

            for (NodoJugador nodo = ListaJugadores.GetHead(); nodo != null; nodo = nodo.GetNext())
            {
                if (nodo.GetData().GetPatrimonio() > ganador.GetPatrimonio())
                {
                    ganador = nodo.GetData();
                }
            }

            return ganador;
        }

        public string Getinfo()
        {
            return "1";
        }

    }
    

    public class NodoJugador
    {
        private Jugador data;
        private NodoJugador? next = null;
        public NodoJugador(Jugador jugador)
        {
            this.data = jugador;
        }
        
        public Jugador GetData()
        {
            return data;
        }

        public void SetData(Jugador jugador)
        {
            data = jugador;
        }

        public NodoJugador GetNext()
        {
            return next;
        }

        public void SetNext(NodoJugador siguiente)
        {
            next = siguiente;
        }
    }

    public class LL_Jugadores
    {
        private NodoJugador? head = null;
        private NodoJugador? tail = null;
        private int cantidad = 0;

        public NodoJugador GetHead()
        {
            return head;
        }

        public NodoJugador GetTail()
        {
            return tail;
        }


        public void SetHead(NodoJugador jugador)
        {
            head = jugador;
        }

        public void SetTail(NodoJugador jugador)
        {
            tail = jugador;
        }


        public int GetCantidad()
        {
            return cantidad;
        }

        public void SetCantidad(int CantidadNueva)
        {
            cantidad = CantidadNueva;
        }
        public void AgregarJugador(Jugador jugador)
        {
            if (cantidad == 4)
            {
                return;
            }

            NodoJugador nuevo = new NodoJugador(jugador);

            if (head == null)
            {
                SetHead(nuevo);
                SetTail(nuevo);
            }
            else
            {
                tail.SetNext(nuevo);
                SetTail(nuevo);
            }

            cantidad++;
        }

        public void BorrarJugador(Jugador jugador)
        {
            if (head.GetData() == jugador)
            {
                cantidad--;
                head = head.GetNext();
                return;
            }
            for (NodoJugador nodo = head; nodo != null; nodo = nodo.GetNext())
            {
                if (nodo.GetNext().GetData() == jugador)
                {
                    cantidad--;
                    if (nodo.GetNext().GetNext() == null)
                    {
                        nodo.SetNext(null);
                        tail = nodo;
                        return;
                    }
                    else
                    {
                        nodo.SetNext(nodo.GetNext().GetNext());
                        return;
                    }
                }
            }
        }
    }
}