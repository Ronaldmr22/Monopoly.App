using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;

//https://youtu.be/GWkdPC74css
//https://youtu.be/IyiDYFfDLyc
//https://youtu.be/TAGoid4u6PY
//https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/sockets/tcp-classes

namespace Monopoly.App
{
    // Servidor principal del juego. Se encarga de recibir clientes,
    // procesar sus acciones y mantener sincronizada la partida.
    public class Servidor
    {
        private TcpListener listener;
        private Tablero_LL tablerito;
        private Banco banco;

        private Dado dado;
        private bool prendido;
        private int jugadorId;
        //Lista para guardar los clientes conectados al servidor
        private List<ClienteConectado> clientes;
        private HistorialTransacciones historialtransacciones;
        private Juego juego;

        // Indica si el jugador actual debe decidir si compra una propiedad.
        private bool esperandoCompra = false;

        // Inicializa el servidor y recibe los objetos necesarios para controlar la partida.
        public Servidor(int puerto, Banco banco, Tablero_LL tablerito, HistorialTransacciones historialTransacciones, Juego juego, string puertoDado)
        {
            listener = new TcpListener(IPAddress.Any, puerto);
            clientes = new List<ClienteConectado>();
            this.banco = banco;
            this.tablerito = tablerito;
            this.historialtransacciones = historialTransacciones;
            this.juego = juego;
            dado = new Dado(puertoDado);
            jugadorId = 1;
        }

        //https://learn.microsoft.com/es-es/dotnet/csharp/asynchronous-programming/

        // Inicia el servidor y acepta clientes mientras se encuentre activo.
        public async Task IniciarConexionAsync()
        {
            listener.Start();
            prendido = true;

            while (prendido)
            {
                TcpClient cliente = await listener.AcceptTcpClientAsync();

                // Cada cliente se escucha de forma independiente.
                _ = EscucharClienteAsync(cliente);
            }
        }

        // Escucha continuamente los mensajes enviados por un cliente.
        public async Task EscucharClienteAsync(TcpClient sCliente)
        {
            var cliente = new ClienteConectado(sCliente);
            string linea;

            while ((linea = await cliente.Lector.ReadLineAsync()) != null)
            {
                RealizarAccion(cliente, linea);
            }
        }

        // Interpreta los comandos enviados por los clientes y ejecuta la acción correspondiente.
        private void RealizarAccion(ClienteConectado cliente, string linea)
        {
            string[] comunicacion = linea.Split(' ');
            string comando = comunicacion[0];

            switch (comando)
            {
                // Registra un nuevo jugador en el servidor.
                case "CONECTAR":
                    ConectarCLiente(cliente, comunicacion);
                    break;

                // Solicita realizar un lanzamiento con los dados físicos.
                case "TIRAR_DADOS":
                    TirarDados(cliente);
                    break;

                // Solicita la compra de una propiedad.
                case "COMPRAR_PROPIEDAD":
                    ComprarPropiedad(cliente, comunicacion);
                    break;

                // El jugador decide no comprar la propiedad.
                case "NO_COMPRAR":
                    NoComprar(cliente);
                    break;

                // Genera diferentes consultas sobre el historial de transacciones.
                case "CONSULTAR_TRANSACCIONES":

                    if (comunicacion[1] == "TODAS")
                    {
                        historialtransacciones.TodasLasTransacciones();
                        EnviarCliente(cliente, "TRANSACCIONES_GENERADAS Todas las transacciones.txt");
                    }
                    else if (comunicacion[1] == "ANTIGUAS")
                    {
                        historialtransacciones.RecorrerDesdeMasAntigua();
                        EnviarCliente(cliente, "TRANSACCIONES_GENERADAS Orden desde el más antiguo.txt");
                    }
                    else if (comunicacion[1] == "RECIENTES")
                    {
                        historialtransacciones.RecorrerDesdeMasReciente();
                        EnviarCliente(cliente, "TRANSACCIONES_GENERADAS Orden desde el más reciente.txt");
                    }
                    else if (comunicacion[1] == "JUGADOR")
                    {
                        string nombreJugador = string.Join(" ", comunicacion[2..]);
                        historialtransacciones.BuscarPorJugador(nombreJugador);
                        EnviarCliente(cliente, "TRANSACCIONES_GENERADAS Búsqueda por jugador.txt");
                    }
                    else if (comunicacion[1] == "TIPO")
                    {
                        string tipo = string.Join(" ", comunicacion[2..]);
                        historialtransacciones.BuscarPorTipo(tipo);
                        EnviarCliente(cliente, "TRANSACCIONES_GENERADAS Búsqueda por tipo.txt");
                    }
                    else
                    {
                        EnviarCliente(cliente, "ERROR CONSULTA_TRANSACCIONES_INVALIDA");
                    }

                    break;

                // Envía al cliente los jugadores que se encuentran en el lobby.
                case "CONSULTAR_LOBBY":
                    EnviarLobby(cliente);
                    break;

                // Inicia la partida e informa cuál jugador comienza.
                case "INICIAR_PARTIDA":
                    EnviarTodos("PARTIDA_INICIADA");
                    EnviarTodos($"TURNO {juego.TurnoActual()}");
                    break;

                // Se ejecuta cuando el servidor recibe un comando que no reconoce.
                default:
                    EnviarCliente(cliente, $"ERROR COMANDO_DESCONOCIDO {comando}");
                    break;
            }
        }

        // Registra un cliente como jugador y le asigna un ID.
        // También le envía información sobre los jugadores ya conectados.
        public void ConectarCLiente(ClienteConectado cliente, string[] comunicaion)
        {
            string nombreJugador = comunicaion[1];

            // Cada nuevo jugador recibe un ID consecutivo.
            int id = jugadorId++;
            cliente.IdJugador = id;

            clientes.Add(cliente);

            // Recorre los jugadores existentes para enviarlos al nuevo cliente.
            NodoJugador nodo = banco.ListaJugadores.GetHead();

            while (nodo != null)
            {
                Jugador jugador = nodo.GetData();

                EnviarCliente(cliente, $"JUGADOR {jugador.GetId()} {jugador.GetNombre()}");

                nodo = nodo.GetNext();
            }

            // Agrega al jugador tanto al banco como al sistema de turnos.
            banco.AgregarJugador(nombreJugador, id);
            juego.AgregarJugador(id);

            // Confirma la conexión y avisa a los demás clientes.
            EnviarCliente(cliente, $"CONECTAR {id} {nombreJugador}");
            EnviarTodos($"JUGADOR {id} {nombreJugador}");
        }

        // Procesa un lanzamiento de dados y posteriormente
        // resuelve la casilla en la que cayó el jugador.
        public void TirarDados(ClienteConectado cliente)
        {
            int idJugador = cliente.IdJugador;

            // Solamente el jugador que tiene el turno puede lanzar.
            if (!juego.EsElTurnoDe(idJugador))
            {
                EnviarCliente(cliente, "ERROR NO_ES_TU_TURNO");
                return;
            }

            // No permite otro lanzamiento mientras se espera
            // una decisión de compra.
            if (esperandoCompra)
            {
                EnviarCliente(cliente, "ERROR ESPERANDO_DECISION_COMPRA");
                return;
            }

            // Obtiene el resultado desde los dados físicos.
            dado.LeerLanzamiento();

            // Verifica que la tarjeta utilizada corresponda al jugador actual.
            if (dado.IdJugador != idJugador)
            {
                EnviarCliente(cliente, "ERROR TARJETA_INCORRECTA");
                return;
            }

            // Calcula cuánto debe avanzar el jugador.
            int movimiento = dado.ObtenerTotal();

            int nuevaPosicion = banco.MoverJugador(idJugador, movimiento);

            Jugador jugadorActualizado = banco.BuscarJugador(idJugador);

            // Actualiza la información en todas las interfaces.
            EnviarTodos($"DINERO_ACTUALIZADO {idJugador} {jugadorActualizado.GetSaldo()}");
            EnviarTodos($"DADOS {idJugador} {dado.Dado1} {dado.Dado2}");
            EnviarTodos($"JUGADOR_MOVIDO {idJugador} {nuevaPosicion}");

            // Determina qué debe ocurrir según la casilla alcanzada.
            string resultadoCasilla = banco.ResolverCasilla(idJugador);
            string[] resultado = resultadoCasilla.Split(' ');

            // El jugador cayó en una propiedad sin dueño.
            if (resultado[0] == "DISPONIBLE")
            {
                esperandoCompra = true;
                EnviarCliente(cliente, $"PROPIEDAD_DISPONIBLE {resultado[1]} {resultado[2]}");
            }

            // El jugador cayó en una propiedad de otro jugador.
            else if (resultado[0] == "OCUPADA")
            {
                int idDueño = int.Parse(resultado[2]);

                Jugador jugador = banco.BuscarJugador(idJugador);
                Jugador dueño = banco.BuscarJugador(idDueño);

                // Actualiza el dinero después del pago del alquiler.
                EnviarTodos($"DINERO_ACTUALIZADO {idJugador} {jugador.GetSaldo()}");
                EnviarTodos($"DINERO_ACTUALIZADO {idDueño} {dueño.GetSaldo()}");
                EnviarTodos($"ALQUILER_PAGADO {idJugador} {idDueño} {resultado[1]}");

                TerminarTurno();
            }

            // El jugador cayó en una propiedad que ya le pertenece.
            else if (resultado[0] == "PROPIA")
            {
                EnviarCliente(cliente, $"PROPIEDAD_PROPIA {resultado[1]}");
                TerminarTurno();
            }

            // El jugador fue eliminado como consecuencia de la casilla.
            else if (resultado[0] == "ELIMINADO")
            {
                int idEliminado = int.Parse(resultado[1]);
                int idDueño = int.Parse(resultado[2]);

                Jugador dueño = banco.BuscarJugador(idDueño);

                EnviarTodos($"JUGADOR_ELIMINADO {idEliminado}");
                EnviarTodos($"DINERO_ACTUALIZADO {idDueño} {dueño.GetSaldo()}");

                // Si solamente queda un jugador, termina la partida.
                if (juego.CantidadJugadores() == 1)
                {
                    int idGanador = juego.TurnoActual();

                    EnviarTodos($"FIN_PARTIDA {idGanador}");
                }
                else
                {
                    // Si todavía quedan jugadores, continúa la partida.
                    int siguienteJugador = juego.TurnoActual();

                    EnviarTodos($"TURNO {siguienteJugador}");
                }
            }

            // Procesa el resultado de una carta de evento.
            else if (resultado[0] == "EVENTO")
            {
                int idCarta = int.Parse(resultado[1]);

                // Reconstruye la descripción completa de la carta.
                string descripcion = string.Join(" ", resultado[2..]);

                jugadorActualizado = banco.BuscarJugador(idJugador);

                // Si el jugador ya no existe después del evnto significa que fue eliminado.
                if (jugadorActualizado == null)
                {
                    EnviarTodos($"CARTA_EVENTO {idJugador} {idCarta} {descripcion}");
                    EnviarTodos($"JUGADOR_ELIMINADO {idJugador}");

                    if (juego.CantidadJugadores() == 1)
                    {
                        int idGanador = juego.TurnoActual();
                        EnviarTodos($"FIN_PARTIDA {idGanador}");
                    }
                    else
                    {
                        int siguienteJugador = juego.TurnoActual();
                        EnviarTodos($"TURNO {siguienteJugador}");
                    }

                    return;
                }

                // Actualiza el estado del jugador después del evento.
                EnviarTodos($"DINERO_ACTUALIZADO {idJugador} {jugadorActualizado.GetSaldo()}");
                EnviarTodos($"JUGADOR_MOVIDO {idJugador} {jugadorActualizado.GetPosicion()}");
                EnviarTodos($"CARTA_EVENTO {idJugador} {idCarta} {descripcion}");

                TerminarTurno();
            }

            // Casilla especial de salida.
            else if (resultado[0] == "SALIDA")
            {
                jugadorActualizado = banco.BuscarJugador(idJugador);

                EnviarTodos($"CASILLA_ESPECIAL {idJugador} SALIDA");
                EnviarTodos($"DINERO_ACTUALIZADO {idJugador} {jugadorActualizado.GetSaldo()}");

                TerminarTurno();
            }

            // Casilla especial de cárcel.
            else if (resultado[0] == "CARCEL")
            {
                EnviarTodos($"CASILLA_ESPECIAL {idJugador} CARCEL");
                TerminarTurno();
            }

            // Casilla que no produce ninguna acción especial.
            else if (resultado[0] == "LIBRE")
            {
                EnviarTodos($"CASILLA_ESPECIAL {idJugador} LIBRE");
                TerminarTurno();
            }
        }

        // Procesa la decisión de comprar una propiedad.
        public void ComprarPropiedad(ClienteConectado cliente, string[] comunicacion)
        {
            int idJugador = cliente.IdJugador;

            // Comprueba que sea el turno del jugador.
            if (!juego.EsElTurnoDe(idJugador))
            {
                EnviarCliente(cliente, "ERROR NO_ES_TU_TURNO");
                return;
            }

            // Comprueba que realmente exista una compra pendiente.
            if (!esperandoCompra)
            {
                EnviarCliente(cliente, "ERROR NO_HAY_COMPRA_PENDIENTE");
                return;
            }

            esperandoCompra = false;

            int idCasilla = int.Parse(comunicacion[1]);

            // El banco intenta realizar la compra.
            bool exito = banco.ComprarPropiedad(idJugador, idCasilla);

            // Si no tiene suficiente dinero, la compra no se realiza.
            if (!exito)
            {
                EnviarCliente(cliente, $"ERROR DINERO INSUFICIENTE");
                TerminarTurno();
                return;
            }

            Jugador jugadorActualizado = banco.BuscarJugador(idJugador);

            // Comprueba si el jugador fue eliminado.
            if (jugadorActualizado == null)
            {
                EnviarTodos($"JUGADOR_ELIMINADO {idJugador}");

                if (juego.CantidadJugadores() == 1)
                {
                    int idGanador = juego.TurnoActual();
                    EnviarTodos($"FIN_PARTIDA {idGanador}");
                }
                else
                {
                    int siguienteJugador = juego.TurnoActual();
                    EnviarTodos($"TURNO {siguienteJugador}");
                }

                return;
            }

            // Actualiza la información después de una compra exitosa.
            EnviarTodos($"DINERO_ACTUALIZADO {idJugador} {jugadorActualizado.GetSaldo()}");
            EnviarTodos($"JUGADOR_MOVIDO {idJugador} {jugadorActualizado.GetPosicion()}");
            EnviarTodos($"PROPIEDAD_COMPRADA {idJugador} {idCasilla}");

            TerminarTurno();
        }

        // Envía al cliente la información de todos los jugadores
        // que se encuentran actualmente en el lobby.
        public void EnviarLobby(ClienteConectado cliente)
        {
            NodoJugador nodo = banco.ListaJugadores.GetHead();

            while (nodo != null)
            {
                Jugador jugador = nodo.GetData();

                EnviarCliente(cliente, $"JUGADOR {jugador.GetId()} {jugador.GetNombre()}");
                EnviarCliente(cliente, $"DINERO_ACTUALIZADO {jugador.GetId()} {jugador.GetSaldo()}");

                nodo = nodo.GetNext();
            }
        }

        // Finaliza el turno actual y determina quién debe jugar después.
        public void TerminarTurno()
        {
            juego.PasarTurno();

            // Comprueba si se alcanzó el límite de rondas.
            if (juego.TerminoPorRondas())
            {
                Jugador ganador = banco.ObtenerGanadorPorPatrimonio();

                EnviarTodos($"FIN_PARTIDA {ganador.GetId()}");
                return;
            }

            int siguienteJugador = juego.TurnoActual();

            Jugador jugadorSiguiente = banco.BuscarJugador(siguienteJugador);

            // Si el siguiente jugador tiene una penalización de turno,
            // se elimina la penalización y se pasa al siguiente jugador.
            if (jugadorSiguiente.GetTurnoPerdido())
            {
                jugadorSiguiente.SetTurnoPerdido(false);
                juego.PasarTurno();
                siguienteJugador = juego.TurnoActual();
            }

            // Informa a todos quién tiene el siguiente turno.
            EnviarTodos($"TURNO {siguienteJugador}");
        }

        // Procesa la decisión del jugador de no comprar una propiedad.
        public void NoComprar(ClienteConectado cliente)
        {
            int idJugador = cliente.IdJugador;

            if (!juego.EsElTurnoDe(idJugador))
            {
                EnviarCliente(cliente, "ERROR NO_ES_TU_TURNO");
                return;
            }

            if (!esperandoCompra)
            {
                EnviarCliente(cliente, "ERROR NO_HAY_COMPRA_PENDIENTE");
                return;
            }

            // Ya se recibió la decisión del jugador.
            esperandoCompra = false;

            TerminarTurno();
        }

        // Envía un mensaje únicamente a un cliente.
        public void EnviarCliente(ClienteConectado cliente, string mensaje)
        {
            cliente.Escritor.WriteLine(mensaje);
        }

        // Envía el mismo mensaje a todos los clientes conectados.
        public void EnviarTodos(string mensaje)
        {
            foreach (ClienteConectado cliente in clientes)
            {
                cliente.Escritor.WriteLine(mensaje);
            }
        }

    }

    // Representa individualmente a cada cliente conectado al servidor.
    // Guarda su ID, conexión TCP y los flujos para enviar y recibir mensajes.
    public class ClienteConectado
    {
        public int IdJugador { get; set; }

        // Conexión TCP del cliente.
        public TcpClient Socket { get; }

        // Se utiliza para recibir mensajes del cliente.
        public StreamReader Lector { get; }

        // Se utiliza para enviar mensajes al cliente.
        public StreamWriter Escritor { get; }

        public ClienteConectado(TcpClient socket)
        {
            Socket = socket;

            // Obtiene el flujo de comunicación del socket.
            var stream = socket.GetStream();

            // Crea los objetos para recibir y enviar información en UTF-8.
            Lector = new StreamReader(stream, Encoding.UTF8);

            // AutoFlush permite que cada mensaje se envíe inmediatamente.
            Escritor = new StreamWriter(stream, Encoding.UTF8)
            {
                AutoFlush = true
            };
        }
    }
}