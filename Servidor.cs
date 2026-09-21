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
                    break;
 
                //case "TERMINAR_TURNO":
                    //TerminarTurno(cliente, comunicacion);
                    //break;
 
                case "CONSULTAR_ESTADO":
                    EnviarCliente(cliente, "ESTADO " + banco.Getinfo());
                    break;
 
                case "CONSULTAR_TRANSACCIONES":
                    
                    break;

                case "CONSULTAR_LOBBY":
                    EnviarLobby(cliente);
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

            foreach (Jugador jugador in banco.ListaJugadores)
            {
                EnviarCliente(cliente,$"JUGADOR {jugador.GetId()} {jugador.GetNombre()}");
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

            dado.LeerLanzamiento();

            if (dado.IdJugador != idJugador)
            {
                EnviarCliente(cliente, "ERROR TARJETA_INCORRECTA");
                return;
            }

            int movimiento = dado.Dado1 + dado.Dado2;

            int nuevaPosicion = banco.MoverJugador(idJugador, movimiento);

            EnviarTodos($"DADOS {idJugador} {dado.Dado1} {dado.Dado2}");

            Console.WriteLine($"Jugador {idJugador} se movio a la casilla {nuevaPosicion}");
        }

        public void ComprarPropiedad(ClienteConectado cliente, string[] comunicacion)
        {
            int idJugador = cliente.IdJugador;
            int idCasilla = int.Parse(comunicacion[1]);
            bool exito = banco.ComprarPropiedad(idJugador, idCasilla);

            if (!exito)
            {
                EnviarCliente(cliente, $"DINERO INSUFICIENTE");
                return;
            }

            EnviarCliente(cliente, $"COMPRAR PROPIEDAD {idCasilla}");
            EnviarTodos($"PROPIEDAD COMPRADA {idJugador} {idCasilla}");
        }

        public void CobrarAlquiler(ClienteConectado cliente, int idPropiedad, int idDueño)
        {
            int idJugador = cliente.IdJugador;
            int alquiler;

            alquiler = banco.CobrarAlquiler(idJugador, idPropiedad, idDueño);

            if (alquiler > 0)
            {
                EnviarCliente(
                    cliente,
                    $"El alquiler ha sido pagado"
                    ///$"PAGAR_ALQUILER {idPropiedad} {alquiler} {idDueño}"
                );
                
            }
            else
            {
                EnviarCliente(
                    cliente,
                    $"El alquiler ha sido pagado, has quedado en bancarrota"
                    ///$"PAGAR_ALQUILER {idPropiedad} {alquiler} {idDueño}"
                );
            }
            ClienteConectado dueño = clientes.Find(c => c.IdJugador == idDueño);
            if (dueño != null)
                {
                    EnviarCliente(dueño,$"Ha recibido el alquiler de la propiedad");
                }
        }

        public void EnviarLobby(ClienteConectado cliente)
        {
            foreach (Jugador jugador in banco.ListaJugadores)
            {
                EnviarCliente(cliente,$"JUGADOR {jugador.GetId()} {jugador.GetNombre()}");
            }
        }

        public void TerminarTurno()
        {
            
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
