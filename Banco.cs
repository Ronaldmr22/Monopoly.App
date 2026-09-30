using System.Diagnostics.Tracing;

namespace Monopoly.App
{
    // Clase encargada de administrar el dinero, jugadores, propiedade y las acciones que ocurren al caer en una casilla.
    public class Banco
    {
        // Lista enlazada que almacena los jugadores de la partida.
        public LL_Jugadores ListaJugadores = new LL_Jugadores();

        // Identificador utilizado para registrar cada transacción.
        public int IdTransaccion = 1;

        // Componentes necesarios para manejar la lógica del juego.
        private Tablero_LL tablero;

        //Referencia al Historial de Transacciones.
        private HistorialTransacciones historial;

        //Referencia al Juego.
        private Juego juego;

        //Referencia a la cola de cartas.
        private ColaCartas cartas;

        // Inicializa el Banco con los componentes principales del juego.
        public Banco(Tablero_LL tablero, HistorialTransacciones historial, Juego juego, ColaCartas cartas)
        {
            this.tablero = tablero;
            this.historial = historial;
            this.juego = juego;
            this.cartas = cartas;
        }

        // Realiza transferencias de dinero entre jugadores o entre un jugador y el Banco.
        public bool Transferir(object Origen, object Destino, int Monto, string tipo, string Razon)
        {
            // Comprueba si el destino del dinero es un jugador.
            if (Destino is Jugador jugadorD)
            {
                // Si el origen también es un jugador, la transferencia es entre jugadores.
                if (Origen is Jugador jugadorO)
                {
                    // Intenta descontar el monto al jugador que debe pagar.
                    if (jugadorO.PagarDinero(Monto))
                    {
                        // Registra la transacción realizada entre los jugadores.
                        Transaccion transaccion1 = new Transaccion(IdTransaccion, DateTime.Now, juego.TurnoActual(), tipo, jugadorO.GetNombre(), jugadorD.GetNombre(), Monto, Razon);

                        historial.InsertarTransaccion(transaccion1);

                        IdTransaccion++;

                        // Entrega el dinero al jugador destino.
                        jugadorD.RecibirDinero(Monto);

                        return true;
                    }
                    else
                    {
                        // Si no puede pagar todo el monto, registra únicamente el saldo disponible.
                        Transaccion transaccion1 = new Transaccion(IdTransaccion, DateTime.Now, juego.TurnoActual(), tipo, jugadorO.GetNombre(), jugadorD.GetNombre(), jugadorO.GetSaldo(), Razon);

                        historial.InsertarTransaccion(transaccion1);

                        IdTransaccion++;

                        // Entrega al jugador destino el dinero restante.
                        jugadorD.RecibirDinero(jugadorO.GetSaldo());

                        // Elimina al jugador que no pudo realizar el pago.
                        DestruirJugador(jugadorO);

                        return false;
                    }
                }

                // Si el origen no es un jugador, el dinero proviene del Banco.
                Transaccion transaccion = new Transaccion(IdTransaccion, DateTime.Now, juego.TurnoActual(), tipo, "Banco", jugadorD.GetNombre(), Monto, Razon);

                historial.InsertarTransaccion(transaccion);

                IdTransaccion++;

                // El jugador recibe el dinero entregado por el Banco.
                jugadorD.RecibirDinero(Monto);

                return true;
            }
            else
            {
                // Si el origen es un jugador y el destino no,
                // se considera un pago realizado al Banco.
                if (Origen is Jugador jugadorO){ 
                    if (jugadorO.PagarDinero(Monto))
                    {
                        // Registra el pago realizado por el jugador al Banco.
                        Transaccion transaccion = new Transaccion(IdTransaccion, DateTime.Now, juego.TurnoActual(), tipo, jugadorO.GetNombre(), "Banco", Monto, Razon);
                        historial.InsertarTransaccion(transaccion);
                        IdTransaccion++;
                        return true;
                    }
                    else
                    {
                        // Registra el saldo restante del jugador antes de eliminarlo.
                        Transaccion transaccion = new Transaccion(IdTransaccion, DateTime.Now, juego.TurnoActual(), tipo, jugadorO.GetNombre(), "Banco", jugadorO.GetSaldo(), Razon);
                        historial.InsertarTransaccion(transaccion);
                        IdTransaccion++;
                        // El jugador es eliminado al no poder realizar el pago.
                        DestruirJugador(jugadorO);
                        return false;
                    }
                }
            }
            // La transferencia no pudo realizarse.
            return false;
        }

        // Elimina al jugador del sistema de turnos y de la lista de jugadores.
        public void DestruirJugador(Jugador jugador)
        {
            juego.MuereJugador(jugador.GetId());
            ListaJugadores.BorrarJugador(jugador);
        }


        // Intenta realizar la compra de una propiedad.
        public bool ComprarPropiedad(int idJugador, int idCasilla)
        {
            Jugador? jugador = BuscarJugador(idJugador);
            Propiedad? propiedad = null;

            // Busca la casilla que el jugador desea comprar.
            Casilla casilla = tablero.BuscarCasilla(idCasilla);

            // Comprueba que la casilla encontrada sea una propiedad.
            if (casilla is Propiedad propiedadObjetivo)
            {
                propiedad = propiedadObjetivo;
            }

            // Comprueba si el jugador tiene suficiente dinero.
            if (jugador.GetSaldo() < propiedad.Precio)
            {
                return false;
            }
            else
            {
                // Realiza el pago del jugador al Banco por la propiedad.
                bool compraExitosa = Transferir(jugador, this, propiedad.Precio, "Compra de propiedad", $"{jugador.GetNombre()} ha comprado la propiedad {propiedad.Nombre} por {propiedad.Precio}");

                // Si el pago fue exitoso, agrega la propiedad al jugador.
                if (compraExitosa)
                {
                    jugador.AgregarPropiedad(propiedad);
                    return true;
                }
                return false;
            }


        }

        // Cobra el alquiler cuando un jugador cae en una propiedad de otro jugador.
        public bool CobrarAlquiler(int idJugador, int idPropiedad, int idDueño)
        {
            Jugador? jugador = BuscarJugador(idJugador);
            Propiedad? propiedad = null;
            Jugador? dueño = BuscarJugador(idDueño);

            // Busca la propiedad correspondiente dentro del tablero.
            Casilla casilla = tablero.BuscarCasilla(idPropiedad);

            if (casilla is Propiedad propiedadObjetivo)
            {
                propiedad = propiedadObjetivo;
            }

            // Transfiere el alquiler del jugador al dueño de la propiedad.
            return Transferir(jugador, dueño, propiedad.Alquiler, "Cobro de alquiler", $"{jugador.GetNombre()} le ha pagado renta a {dueño.GetNombre()} por una cantidad de {propiedad.Alquiler}");
        }

        // Crea un jugador y lo agrega a la lista si todavía hay espacio.
        public void AgregarJugador(string nombreJugador, int id)
        {
            // La partida permite un máximo de cuatro jugadores.
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

        // Busca un jugador utilizando su ID.
        public Jugador BuscarJugador(int idJugador)
        {
            // Recorre la lista enlazada hasta encontrar el jugador.
            for (NodoJugador nodo = ListaJugadores.GetHead(); nodo != null; nodo = nodo.GetNext())
            {
                if (nodo.GetData().GetId() == idJugador)
                {
                    return nodo.GetData();
                }
            }
            // Si no existe el jugador, devuelve null.
            return null;
        }


        // Mueve al jugador la cantidad de casillas indicada.
        public int MoverJugador(int idJugador, int movimiento)
        {
            Jugador jugador = BuscarJugador(idJugador);

            // Calcula la nueva posición utilizando la posición actual y el movimiento.
            int nuevaPosicion = jugador.GetPosicion() + movimiento;

            // Si supera la casilla 24 significa que pasó por Salida.
            if (nuevaPosicion > 24)
            {
                // Reinicia la posición tomando en cuenta las 24 casillas.
                nuevaPosicion = nuevaPosicion - 24;

                // Entrega $200 al jugador por pasar por Salida.
                Transferir(this,jugador,200,"Premio de salida","Paso por Salida");
            }

            // Actualiza la posición del jugador.
            jugador.SetPosicion(nuevaPosicion);
            return nuevaPosicion;
        }


        // Determina qué debe ocurrir según la casilla donde cayó el jugador.
        public string ResolverCasilla(int idJugador)
        {
            Jugador jugador = BuscarJugador(idJugador);

            // Busca la casilla correspondiente a la posición actual.
            Casilla casilla = tablero.BuscarCasilla(jugador.GetPosicion());

            // Comprueba si el jugador cayó en una propiedad.
            if (casilla is Propiedad propiedad)
            {
                // Busca si la propiedad ya pertenece a algún jugador.
                Jugador? dueño = BuscarDueñoPropiedad(propiedad.IdPropiedad);

                // Si no tiene dueño, se encuentra disponible para comprar.
                if (dueño == null)
                {
                    return $"DISPONIBLE {propiedad.NumeroCasilla} {propiedad.Precio}";
                }

                // Si el dueño es el mismo jugador, la propiedad ya es suya.
                else if (dueño.GetId() == idJugador)
                {
                    return $"PROPIA {propiedad.NumeroCasilla}";
                }
                else
                {
                    // Si pertenece a otro jugador, intenta cobrar el alquiler.
                    bool pagoExitoso = CobrarAlquiler(idJugador,propiedad.NumeroCasilla,dueño.GetId());

                    // Si no pudo pagar el alquiler, el jugador fue eliminado.
                    if (!pagoExitoso)
                    {
                        return $"ELIMINADO {idJugador} {dueño.GetId()}";
                    }

                    return $"OCUPADA {propiedad.NumeroCasilla} {dueño.GetId()}";
                }
            }

            // Comprueba si cayó en una casilla de evento.
            else if(casilla is CasillaEvento casillaEvento)
            {
                // Obtiene la siguiente carta de la cola.
                CartaEvento carta = cartas.SacarCarta();

                // Ejecuta el efecto de la carta sobre el jugador.
                carta.EjecutarEvento(jugador, this);

                // Devuelve la información del evento al servidor.
                return $"EVENTO {carta.IdCarta} {carta.Descripcion}";
            }

            // Comprueba si cayó en una casilla especial.
            else if(casilla is CasillaEspecial casillaEspecial)
            {
                // Casilla de Salida.
                if (casillaEspecial.Nombre == "Salida")
                {
                    return $"SALIDA {casillaEspecial.NumeroCasilla}";
                }

                // En la cárcel el jugador pierde un turno.
                else if (casillaEspecial.Nombre == "Carcel")
                {
                    jugador.SetTurnoPerdido(true);
                    return $"CARCEL {casillaEspecial.NumeroCasilla}";
                }

                // La casilla libre no realiza ninguna acción adicional.
                else if (casillaEspecial.Nombre == "Casilla Libre")
                {
                    return $"LIBRE {casillaEspecial.NumeroCasilla}";
                }
            }

            // Se devuelve cuando la casilla no requiere ninguna acción.
            return "SIN_ACCION";
        }


        // Busca al jugador que posee una propiedad específica.
        public Jugador? BuscarDueñoPropiedad(int idPropiedad)
        {
            // Recorre todos los jugadores y revisa sus propiedades.
            for (NodoJugador nodo = ListaJugadores.GetHead(); nodo != null; nodo = nodo.GetNext())
            {
                if (nodo.GetData().GetPropiedades().TienePropiedad(idPropiedad))
                {
                    return nodo.GetData();
                }
            }

            // Si nadie posee la propiedad, devuelve null.
            return null;
        }

        // Busca al jugador que tenga el patrimonio más alto.
        public Jugador ObtenerGanadorPorPatrimonio()
        {
            // Inicialmente se considera ganador al primer jugador.
            Jugador? ganador = ListaJugadores.GetHead().GetData();

            // Compara el patrimonio de todos los jugadores.
            for (NodoJugador nodo = ListaJugadores.GetHead(); nodo != null; nodo = nodo.GetNext())
            {
                if (nodo.GetData().GetPatrimonio() > ganador.GetPatrimonio())
                {
                    ganador = nodo.GetData();
                }
            }

            return ganador;
        }


    }

    // Nodo utilizado para construir la lista enlazada de jugadores.
    public class NodoJugador
    {
        // Guarda el jugador y la referencia al siguiente nodo.
        private Jugador data;

        private NodoJugador? next = null;

        public NodoJugador(Jugador jugador)
        {
            this.data = jugador;
        }


        // Devuelve el jugador almacenado en el nodo.
        public Jugador GetData()
        {
            return data;
        }


        // Cambia el jugador almacenado en el nodo.
        public void SetData(Jugador jugador)
        {
            data = jugador;
        }


        // Devuelve el siguiente nodo de la lista.
        public NodoJugador GetNext()
        {
            return next;
        }


        // Establece cuál será el siguiente nodo.
        public void SetNext(NodoJugador siguiente)
        {
            next = siguiente;
        }
    }


    // Lista enlazada simple utilizada para almacenar los jugadores.
    public class LL_Jugadores
    {
        // Primer nodo de la lista.
        private NodoJugador? head = null;

        // Último nodo de la lista.
        private NodoJugador? tail = null;

        // Cantidad de jugadores almacenados.
        private int cantidad = 0;


        // Devuelve el primer nodo.
        public NodoJugador GetHead()
        {
            return head;
        }
        // Devuelve el último nodo.
        public NodoJugador GetTail()
        {
            return tail;
        }
        // Cambia el primer nodo de la lista.
        public void SetHead(NodoJugador jugador)
        {
            head = jugador;
        }
        // Cambia el último nodo de la lista.
        public void SetTail(NodoJugador jugador)
        {
            tail = jugador;
        }
        // Devuelve la cantidad actual de jugadores.
        public int GetCantidad()
        {
            return cantidad;
        }
        // Permite modificar la cantidad de jugadores.
        public void SetCantidad(int CantidadNueva)
        {
            cantidad = CantidadNueva;
        }
        // Agrega un nuevo jugador al final de la lista enlazada.
        public void AgregarJugador(Jugador jugador)
        {
            // Evita que existan más de cuatro jugadores.
            if (cantidad == 4)
            {
                return;
            }

            // Crea el nodo que almacenará al nuevo jugador.
            NodoJugador nuevo = new NodoJugador(jugador);

            // Si la lista está vacía, el nuevo nodo será head y tail.
            if (head == null)
            {
                SetHead(nuevo);

                SetTail(nuevo);
            }
            else
            {
                // Conecta el último nodo con el nuevo nodo.
                tail.SetNext(nuevo);

                // El nuevo nodo pasa a ser el último.
                SetTail(nuevo);
            }


            cantidad++;
        }

        // Elimina un jugador de la lista enlazada.
        public void BorrarJugador(Jugador jugador)
        {
            // Caso especial: el jugador se encuentra al inicio de la lista.
            if (head.GetData() == jugador)
            {
                cantidad--;

                head = head.GetNext();

                return;
            }

            // Recorre la lista buscando el nodo anterior al jugador.
            for (NodoJugador nodo = head; nodo != null; nodo = nodo.GetNext())
            {
                if (nodo.GetNext().GetData() == jugador)
                {
                    cantidad--;

                    // Comprueba si el jugador que se elimina es el último.
                    if (nodo.GetNext().GetNext() == null)
                    {
                        nodo.SetNext(null);
                        // El nodo anterior pasa a ser el nuevo tail.
                        tail = nodo;
                        return;
                    }
                    else
                    {
                        // Salta el nodo eliminado y conecta con el siguiente.
                        nodo.SetNext(nodo.GetNext().GetNext());

                        return;
                    }
                }
            }
        }
    }
}