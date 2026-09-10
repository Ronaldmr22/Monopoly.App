using System;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly.App
{
    public class Cliente
    {
        private TcpClient socket;
        private StreamReader lector;
        private StreamWriter escritor;
        public async Task ConectarAsync(string ip, int puerto)
        {
            socket = new TcpClient();

            await socket.ConnectAsync(ip, puerto);

            NetworkStream stream = socket.GetStream();

            lector = new StreamReader(stream, Encoding.UTF8);
            escritor = new StreamWriter(stream, Encoding.UTF8) {AutoFlush = true};
        }

        public void EnviarMensaje(string mensaje)
        {
            escritor.WriteLine(mensaje);
        }

        // Recibe un mensaje del servidor
        public async Task<string> RecibirMensajeAsync()
        {
            return await lector.ReadLineAsync();
        }

        // Conecta al jugador al juego
        public async Task<string> ConectarJugadorAsync(string nombre)
        {
            string mensaje = $"CONECTAR {nombre}";
            EnviarMensaje(mensaje);
            return await RecibirMensajeAsync();
        }

        // Solicita tirar los dados
        public async Task<string> TirarDadosAsync()
        {
            string mensaje = "TIRAR_DADOS";
            EnviarMensaje(mensaje);
            return await RecibirMensajeAsync();
        }

        // Solicita comprar una propiedad
        public async Task<string> ComprarPropiedadAsync(int idCasilla)
        {
            string mensaje = $"COMPRAR_PROPIEDAD {idCasilla}";

            EnviarMensaje(mensaje);

            return await RecibirMensajeAsync();
        }

        // Indica que no quiere comprar
        public async Task<string> NoComprarAsync()
        {
            string mensaje = "NO_COMPRAR";

            EnviarMensaje(mensaje);

            return await RecibirMensajeAsync();
        }

        // Consulta el estado del juego
        public async Task<string> ConsultarEstadoAsync()
        {
            string mensaje = "CONSULTAR_ESTADO";

            EnviarMensaje(mensaje);

            return await RecibirMensajeAsync();
        }

        // Consulta las transacciones
        public async Task<string> ConsultarTransaccionesAsync()
        {
            string mensaje = "CONSULTAR_TRANSACCIONES";

            EnviarMensaje(mensaje);

            return await RecibirMensajeAsync();
        }

        // Desconecta el cliente
        public void Desconectar()
        {
            if (socket != null)
            {
                socket.Close();
            }
        }
    }
}
