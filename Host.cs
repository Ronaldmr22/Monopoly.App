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
            CrearTablero();
        }

        public async Task IniciarAsync(int puerto, string nombreJugador)
        {
            Servidor = new Servidor(puerto,Banco,Tablero,Historial,Juego,"COM9");

            _ = Servidor.IniciarConexionAsync();

            await Cliente.ConectarAsync("127.0.0.1",puerto,nombreJugador);
        }

        public string ObtenerIpLocal()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            return host.AddressList.First(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToString();
        }

        private void CrearTablero()
        {
            CasillaEspecial salida= new CasillaEspecial(1,"Salida");
            Propiedad Casa1 = new Propiedad(2, 1, "Avenida Mediterráneo", 50, 25);
            Propiedad Casa2 = new Propiedad(3, 2, "Avenida Báltica", 50, 25);
            Propiedad Casa3 = new Propiedad(4, 3, "Avenida Oriental", 100, 50);
            Propiedad Casa4 = new Propiedad(5, 4, "Avenida Vermont", 100, 50);
            Propiedad Casa5 = new Propiedad(6, 5, "Avenida Connecticut", 120, 60);
            CasillaEspecial carcel = new CasillaEspecial(7, "Carcel");
            Propiedad Casa6 = new Propiedad(8, 6, "Plaza San Carlos", 140, 70);
            CasillaEvento primerevento = new CasillaEvento(9,1);
            Propiedad Casa7 = new Propiedad(10, 7, "Avenida Estados", 140, 70);
            Propiedad Casa8 = new Propiedad(11, 8, "Avenida Virginia", 160, 80);
            Propiedad Casa9 = new Propiedad(12, 9, "Plaza St. James", 180, 90);
            Propiedad Casa10= new Propiedad(13, 10, "Avenida Tenesse", 180, 90);
            CasillaEvento segundoevento = new CasillaEvento(14, 2);
            Propiedad Casa11 = new Propiedad(15, 11, "Avenida Nueva York", 200, 100);
            Propiedad Casa12 = new Propiedad(16, 12, "Avenida Kentucky", 220, 110);
            Propiedad Casa13 = new Propiedad(17, 13, "Avenida Pennsylvania", 220, 110);
            CasillaEvento tercerevento = new CasillaEvento(18, 2);
            Propiedad Casa14 = new Propiedad(19, 14, "Avenida Indiana", 240, 120);
            Propiedad Casa15 = new Propiedad(20, 15, "Avenida Illinois", 240, 120);
            Propiedad Casa16 = new Propiedad(21, 16, "Avenida Atlántico", 240, 120);
            CasillaEspecial libre = new CasillaEspecial(22, "Casilla Libre");
            Propiedad Casa17 = new Propiedad(23, 17, "Jardines Marvin", 260, 130);
            Propiedad Casa18 = new Propiedad(24, 18, "Plaza Park", 280, 140);


            Tablero.AgregarCasilla(salida);
            Tablero.AgregarCasilla(Casa1);
            Tablero.AgregarCasilla(Casa2);
            Tablero.AgregarCasilla(Casa3);
            Tablero.AgregarCasilla(Casa4);
            Tablero.AgregarCasilla(Casa5);
            Tablero.AgregarCasilla(carcel);
            Tablero.AgregarCasilla(Casa6);
            Tablero.AgregarCasilla(primerevento);
            Tablero.AgregarCasilla(Casa7);
            Tablero.AgregarCasilla(Casa8);
            Tablero.AgregarCasilla(Casa9);
            Tablero.AgregarCasilla(Casa10);
            Tablero.AgregarCasilla(segundoevento);
            Tablero.AgregarCasilla(Casa11);
            Tablero.AgregarCasilla(Casa12);
            Tablero.AgregarCasilla(Casa13);
            Tablero.AgregarCasilla(tercerevento);
            Tablero.AgregarCasilla(Casa14);
            Tablero.AgregarCasilla(Casa15);
            Tablero.AgregarCasilla(Casa16);
            Tablero.AgregarCasilla(libre);
            Tablero.AgregarCasilla(Casa17);
            Tablero.AgregarCasilla(Casa18);
        }
    }
}