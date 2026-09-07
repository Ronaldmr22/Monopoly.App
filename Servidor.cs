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
        private Banco banco;
        private bool prendido;
        private int jugadorId;
        private List<ClienteConectado> clientes;


        public Servidor(int puerto, Banco banco)
        {
            listener = new TcpListener(IPAddress.Any, puerto);
            clientes = new List<ClienteConectado>();
            this.banco = banco;
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
                    TirarDados(cliente, comunicacion);
                    break;
 
                case "COMPRAR_PROPIEDAD":
                    ComprarPropiedad(cliente, comunicacion);
                    break;
 
                case "NO_COMPRAR":
                    break;
 
                case "TERMINAR_TURNO":
                    TerminarTurno(cliente, comunicacion);
                    break;
 
                case "CONSULTAR_Dinero":
                    EnviarCliente(cliente, "ESTADO " + banco.GetDinero());
                    break;
 
                case "CONSULTAR_TRANSACCIONES":
                    
                    break;
 
                default:
                    EnviarCliente(cliente, $"ERROR COMANDO_DESCONOCIDO {comando}");
                    break;
            }
        }

        public void ConectarCLiente(ClienteConectado cliente, string[] comunicaion)
        {
            string nombreJugador = partes[1];
            int id = jugadorId++;
            cliente.IdJugador = id;
            clientes.Add(cliente);

            banco.RegistrarJugador(nombreJugador, id);
            EnviarCliente(cliente, $"CONECTAR {id}");
            EnviarTodos($"JUGADOR {id} SE HA UNIDO");

        }

        public void TirarDados(ClienteConectado cliente, string[] comunicacion)
        {
            int idJugador = int.Parse(comunicacion[1]);
        }

        public void ComprarPropiedad(ClienteConectado cliente, string[] comunicacion)
        {
            int idJugador = int.Parse(comunicacion[1]);
            int idCasilla = int.Parse(comunicacion[2]);
            bool exito = banco.ComprarPropiedad(idJugador, idCasilla);

            if (!exito)
            {
                EnviarCliente(cliente, $"DINERO INSUFICIENTE");
                return;
            }

            EnviarCliente(cliente, $"COMPRAR PROPIEDAD {idCasilla}");
            EnviarTodos($"PROPIEDAD COMPRADA {idJugador} {idCasilla}");
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
