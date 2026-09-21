using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Monopoly.App
{
    public class Host
    {
        public Servidor Servidor { get; private set; }
        public Cliente Cliente { get; private set; }
        public Banco Banco { get; private set; }
        public Tablero_LL Tablero { get; private set; }
        public Juego Juego { get; private set; }
        public HistorialTransacciones Historial { get; private set; }

        public Host()
        {
            Tablero = new Tablero_LL();
            Historial = new HistorialTransacciones();
            Juego = new Juego();

            Banco = new Banco(Tablero, Historial, Juego);

            Cliente = new Cliente();
        }

        public async Task IniciarAsync(int puerto, string nombreJugador)
        {
            Servidor = new Servidor(puerto,Banco,Tablero,Historial,"COM5");

            _ = Servidor.IniciarConexionAsync();

            Cliente = new Cliente();

            await Cliente.ConectarAsync("127.0.0.1",puerto,nombreJugador);
        }

        public string ObtenerIpLocal()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            return host.AddressList.First(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToString();
        }
    }
}