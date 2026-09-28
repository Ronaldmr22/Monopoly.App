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
    public class Servidor
    {
        private TcpListener listener;
        private Tablero_LL tablerito;
        private Banco banco;

        private Dado dado;
        private bool prendido;
        private int jugadorId;
        private List<ClienteConectado> clientes;
        private HistorialTransacciones historialtransacciones;
        private Juego juego;

        private bool esperandoCompra = false;

        public Servidor(int puerto, Banco banco, Tablero_LL tablerito, HistorialTransacciones historialTransacciones,Juego juego, string puertoDado)
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
        public async Task IniciarConexionAsync()
        {
            listener.Start();
            prendido = true;
            while (prendido)
            {
                TcpClient cliente = await listener.AcceptTcpClientAsync();
                _ = EscucharClienteAsync(cliente);
            }
        }

        public async Task EscucharClienteAsync(TcpClient sCliente)
        {
            var cliente = new ClienteConectado(sCliente);
            string linea;
                while ((linea = await cliente.Lector.ReadLineAsync()) != null)
                {
                    RealizarAccion(cliente, linea);
                }
        }

        private void RealizarAccion(ClienteConectado cliente, string linea)
        {
            string[] comunicacion = linea.Split(' ');
            string comando = comunicacion[0];
 
            switch (comando)
            {
                case "CONECTAR":
                    ConectarCLiente(cliente, comunicacion);
                    break;
 
                case "TIRAR_DADOS":
                    TirarDados(cliente);
                    break;
 
                case "COMPRAR_PROPIEDAD":
                    ComprarPropiedad(cliente, comunicacion);
                    break;
 
                case "NO_COMPRAR":
                    NoComprar(cliente);
                    break;
 
                case "CONSULTAR_ESTADO":
                    EnviarCliente(cliente, "ESTADO " + banco.Getinfo());
                    break;
 
                case "CONSULTAR_TRANSACCIONES":
                    
                    break;

                case "CONSULTAR_LOBBY":
                    EnviarLobby(cliente);
                    break;

                case "INICIAR_PARTIDA":
                    EnviarTodos("PARTIDA_INICIADA");
                    EnviarTodos($"TURNO {juego.TurnoActual()}");
                    break;
 
                default:
                    EnviarCliente(cliente, $"ERROR COMANDO_DESCONOCIDO {comando}");
                    break;
            }
        }

        public void ConectarCLiente(ClienteConectado cliente, string[] comunicaion)
        {
            string nombreJugador = comunicaion[1];
            int id = jugadorId++;
            cliente.IdJugador = id;
            clientes.Add(cliente);

            NodoJugador nodo = banco.ListaJugadores.GetHead();

            while (nodo != null)
            {
                Jugador jugador = nodo.GetData();

                EnviarCliente(cliente,$"JUGADOR {jugador.GetId()} {jugador.GetNombre()}");

                nodo = nodo.GetNext();
            }

            banco.AgregarJugador(nombreJugador, id);
            juego.AgregarJugador(id);

            EnviarCliente(cliente,$"CONECTAR {id} {nombreJugador}");
            EnviarTodos($"JUGADOR {id} {nombreJugador}");
        }

        public void TirarDados(ClienteConectado cliente)
        {
            int idJugador = cliente.IdJugador;

            if (!juego.EsElTurnoDe(idJugador))
            {
                EnviarCliente(cliente, "ERROR NO_ES_TU_TURNO");
                return;
            }
            if (esperandoCompra)
            {
                EnviarCliente(cliente, "ERROR ESPERANDO_DECISION_COMPRA");
                return;
            }

            dado.LeerLanzamiento();

            if (dado.IdJugador != idJugador)
            {
                EnviarCliente(cliente, "ERROR TARJETA_INCORRECTA");
                return;
            }

            int movimiento = dado.Dado1 + dado.Dado2;

            int nuevaPosicion = banco.MoverJugador(idJugador, movimiento);

            Jugador jugadorActualizado = banco.BuscarJugador(idJugador);

            EnviarTodos($"DINERO_ACTUALIZADO {idJugador} {jugadorActualizado.GetSaldo()}");

            EnviarTodos($"DADOS {idJugador} {dado.Dado1} {dado.Dado2}");
            EnviarTodos($"JUGADOR_MOVIDO {idJugador} {nuevaPosicion}");
        
            string resultadoCasilla = banco.ResolverCasilla(idJugador);
            string[] resultado = resultadoCasilla.Split(' ');
            if (resultado[0] == "DISPONIBLE")
            {
                esperandoCompra = true;

                Console.WriteLine(
                    $"ESPERANDO COMPRA - Jugador {idJugador} - Casilla {resultado[1]} - Precio {resultado[2]}"
                );
                EnviarCliente(cliente,$"PROPIEDAD_DISPONIBLE {resultado[1]} {resultado[2]}");
            }
            else if (resultado[0] == "OCUPADA")
            {
                int idDueño = int.Parse(resultado[2]);

                Jugador jugador = banco.BuscarJugador(idJugador);
                Jugador dueño = banco.BuscarJugador(idDueño);

                EnviarTodos($"DINERO_ACTUALIZADO {idJugador} {jugador.GetSaldo()}");
                EnviarTodos($"DINERO_ACTUALIZADO {idDueño} {dueño.GetSaldo()}");
                EnviarTodos($"ALQUILER_PAGADO {idJugador} {idDueño} {resultado[1]}");
                TerminarTurno();
            }
            else if (resultado[0] == "PROPIA")
            {
                EnviarCliente(cliente, $"PROPIEDAD_PROPIA {resultado[1]}");
                TerminarTurno();
            }
            else if (resultado[0] == "ELIMINADO")
            {
                int idEliminado = int.Parse(resultado[1]);
                int idDueño = int.Parse(resultado[2]);

                Jugador dueño = banco.BuscarJugador(idDueño);

                EnviarTodos($"JUGADOR_ELIMINADO {idEliminado}");
                EnviarTodos($"DINERO_ACTUALIZADO {idDueño} {dueño.GetSaldo()}");
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
            }
            else if (resultado[0] == "EVENTO")
            {
                int idCarta = int.Parse(resultado[1]);

                string descripcion = string.Join(" ", resultado[2..]);

                jugadorActualizado = banco.BuscarJugador(idJugador);
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

                EnviarTodos($"DINERO_ACTUALIZADO {idJugador} {jugadorActualizado.GetSaldo()}");
                EnviarTodos($"JUGADOR_MOVIDO {idJugador} {jugadorActualizado.GetPosicion()}");
                EnviarTodos($"CARTA_EVENTO {idJugador} {idCarta} {descripcion}");

                TerminarTurno();
            }
            else if (resultado[0] == "SALIDA")
            {
                jugadorActualizado = banco.BuscarJugador(idJugador);

                EnviarTodos($"CASILLA_ESPECIAL {idJugador} SALIDA");
                EnviarTodos($"DINERO_ACTUALIZADO {idJugador} {jugadorActualizado.GetSaldo()}");

                TerminarTurno();
            }
            else if (resultado[0] == "CARCEL")
            {
                EnviarTodos($"CASILLA_ESPECIAL {idJugador} CARCEL");
                TerminarTurno();
            }
            else if (resultado[0] == "LIBRE")
            {
                EnviarTodos($"CASILLA_ESPECIAL {idJugador} LIBRE");
                TerminarTurno();
            }
        }

        public void ComprarPropiedad(ClienteConectado cliente, string[] comunicacion)
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

            esperandoCompra = false;

            int idCasilla = int.Parse(comunicacion[1]);
            bool exito = banco.ComprarPropiedad(idJugador, idCasilla);

            if (!exito)
            {
                EnviarCliente(cliente, $"ERROR DINERO INSUFICIENTE");
                TerminarTurno();
                return;
            }

            Jugador jugadorActualizado = banco.BuscarJugador(idJugador);

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

            EnviarTodos($"DINERO_ACTUALIZADO {idJugador} {jugadorActualizado.GetSaldo()}");
            EnviarTodos($"JUGADOR_MOVIDO {idJugador} {jugadorActualizado.GetPosicion()}");
            EnviarTodos($"PROPIEDAD_COMPRADA {idJugador} {idCasilla}");


            TerminarTurno();
        }


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

        public void TerminarTurno()
        {
            juego.PasarTurno();

            if (juego.TerminoPorRondas())
            {
                Jugador ganador = banco.ObtenerGanadorPorPatrimonio();

                EnviarTodos($"FIN_PARTIDA {ganador.GetId()}");
                return;
            }

            int siguienteJugador = juego.TurnoActual();
            
            Jugador jugadorSiguiente = banco.BuscarJugador(siguienteJugador);

            if (jugadorSiguiente.GetTurnoPerdido())
            {
                jugadorSiguiente.SetTurnoPerdido(false);
                juego.PasarTurno();
                siguienteJugador = juego.TurnoActual();
            }

            EnviarTodos($"TURNO {siguienteJugador}");
        }

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

            esperandoCompra = false;

            TerminarTurno();
        }

        public void EnviarCliente(ClienteConectado cliente, string mensaje)
        {
            cliente.Escritor.WriteLine(mensaje);
        }

        public void EnviarTodos(string mensaje)
        {
            foreach(ClienteConectado cliente in clientes)
            {
                cliente.Escritor.WriteLine(mensaje);
            }
        }




        public Tablero_LL GetTablero()
        {
            return tablerito;
        }

        public Banco GetBanco()
        {
            return banco;
        }

        public HistorialTransacciones GetHistorialTransacciones()
        {
            return historialtransacciones;
        }
    }

    public class ClienteConectado
    {
        public int IdJugador { get; set; }
        public TcpClient Socket { get; }
        public StreamReader Lector { get; }
        public StreamWriter Escritor { get; }
        public ClienteConectado(TcpClient socket)
        {
            Socket = socket;
            var stream = socket.GetStream();
            Lector = new StreamReader(stream, Encoding.UTF8);
            Escritor = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
        }
    }
}
