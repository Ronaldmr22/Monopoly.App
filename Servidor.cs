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
            string[] partes = linea.Split(' ');
            string comando = partes[0];
 
            switch (comando)
            {
                case "CONECTAR":
                    ConectarCLiente(cliente, partes);
                    break;
 
                case "TIRAR_DADOS":
                    TirarDados(cliente, partes);
                    break;
 
                case "COMPRAR_PROPIEDAD":
                    ComprarPropiedad(cliente, partes);
                    break;
 
                case "NO_COMPRAR":
                    break;
 
                case "TERMINAR_TURNO":
                    TerminarTurno(cliente, partes);
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

        public void ConectarCLiente(ClienteConectado cliente, string[] partes)
        {
            string nombreJugador = partes[1];
            int id = jugadorId++;
            cliente.IdJugador = id;
            clientes.Add(cliente);

            banco.RegistrarJugador(nombreJugador, id);
            EnviarCliente(cliente, $"CONECTAR {id}");
            EnviarTodos($"JUGADOR {id} SE HA UNIDO")

        }

        public void TirarDados()
        {
            
        }

        public void ComprarPropiedad()
        {
            
        }

        public void TerminarTurno()
        {
            
        }

        public void EnviarCliente(ClienteConectado cliente, string mensaje)
        {
            
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
