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
        private bool conectado;

        public int IdJugador { get; private set; }
        public event Action<int, string> JugadorConectado;
        public event Action<string> ActualizacionJuego;
        public event Action<string> ErrorRecibido;
        public event Action<int, int> PropiedadDisponible;
        public event Action PartidaIniciada;
        public event Action<int, int> JugadorMovido;
        public event Action<int, int, int> DadosLanzados;
        public event Action<int> TurnoCambiado;
        public event Action<int, int> DineroActualizado;


        public async Task ConectarAsync(string ip, int puerto, string nombreJugador)
        {
            socket = new TcpClient();
            await socket.ConnectAsync(ip, puerto);

            NetworkStream stream = socket.GetStream();
            lector = new StreamReader(stream, Encoding.UTF8);
            escritor = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
            conectado = true;

            EscucharServidorAsync();

            EnviarMensaje("CONECTAR " + nombreJugador);
        }

        private async Task EscucharServidorAsync()
        {
            string linea;
            while (conectado)
            {
                linea = await lector.ReadLineAsync();
                if (linea == null)
                {
                    break;
                }
                ProcesarMensaje(linea);
            }
        }

        private void ProcesarMensaje(string linea)
        {
            string[] partes = linea.Split(' ');
            string comando = partes[0];

            if (comando == "CONECTAR")
            {
                IdJugador = int.Parse(partes[1]);
                string nombre = partes[2];
                if (ActualizacionJuego != null)
                {
                    ActualizacionJuego.Invoke($"Te conectaste como {nombre}, jugador {IdJugador}");
                }
            }
            else if (comando == "JUGADOR")
            {
                int id = int.Parse(partes[1]);
                string nombre = partes[2];
                if (JugadorConectado != null)
                {
                    JugadorConectado.Invoke(id, nombre);
                }
            }
            else if (comando == "DADOS")
            {
                int idJugador = int.Parse(partes[1]);
                int dado1 = int.Parse(partes[2]);
                int dado2 = int.Parse(partes[3]);

                DadosLanzados?.Invoke(idJugador, dado1, dado2);

                if (ActualizacionJuego != null)
                {
                    ActualizacionJuego.Invoke("Jugador " + idJugador + " tiró " +dado1 + " y " + dado2);
                }
            }
            else if (comando == "COMPRAR")
            {
                if (ActualizacionJuego != null)
                {
                    ActualizacionJuego.Invoke("Compraste la casilla " + partes[2]);
                }
            }
            else if (comando == "PROPIEDAD")
            {
                if (ActualizacionJuego != null)
                {
                    ActualizacionJuego.Invoke("Jugador " + partes[2] + " compró la casilla " + partes[3]);
                }
            }
            else if (comando == "DINERO")
            {
                if (ErrorRecibido != null)
                {
                    ErrorRecibido.Invoke("No tenés suficiente dinero para esa compra");
                }
            }
            else if (comando == "ESTADO")
            {
                if (ActualizacionJuego != null)
                {
                    ActualizacionJuego.Invoke(linea.Substring(comando.Length + 1));
                }
            }
            else if (comando == "ERROR")
            {
                if (ErrorRecibido != null)
                {
                    ErrorRecibido.Invoke(linea);
                }
            }
            else if (comando == "PROPIEDAD_DISPONIBLE")
            {
                int idCasilla = int.Parse(partes[1]);
                int precio = int.Parse(partes[2]);

                PropiedadDisponible.Invoke(idCasilla, precio);

            }
            else if (comando == "ALQUILER")
            {
                int jugadorPaga = int.Parse(partes[1]);
                int propietario = int.Parse(partes[2]);
                int monto = int.Parse(partes[3]);

                if (ActualizacionJuego != null)
                {
                    ActualizacionJuego.Invoke("Jugador " + jugadorPaga +" pagó $" + monto +" al jugador " + propietario);
                }
            }
            else if (comando == "PROPIEDAD_PROPIA")
            {
                int idCasilla = int.Parse(partes[1]);

                if (ActualizacionJuego != null)
                {
                    ActualizacionJuego.Invoke("La casilla " + idCasilla + " ya es tuya");
                }
            }
            else if (comando == "PARTIDA_INICIADA")
            {
                if (PartidaIniciada != null)
                {
                    PartidaIniciada.Invoke();
                }
            }
            else if (comando == "JUGADOR_MOVIDO")
            {
                int idJugador = int.Parse(partes[1]);
                int posicion = int.Parse(partes[2]);

                JugadorMovido?.Invoke(idJugador, posicion);
            }
            else if (comando == "TURNO")
            {
                int idJugador = int.Parse(partes[1]);

                TurnoCambiado?.Invoke(idJugador);
            }
            else if (comando == "DINERO_ACTUALIZADO")
            {
                int idJugador = int.Parse(partes[1]);
                int nuevoSaldo = int.Parse(partes[2]);

                DineroActualizado?.Invoke(idJugador, nuevoSaldo);
            }
            
            else
            {
                if (ErrorRecibido != null)
                {
                    ErrorRecibido.Invoke("Mensaje no reconocido: " + linea);
                }
            }
        }

        public void TirarDados()
        {
            EnviarMensaje("TIRAR_DADOS");
        }

        public void ComprarPropiedad(int idCasilla)
        {
            EnviarMensaje("COMPRAR_PROPIEDAD " + IdJugador + " " + idCasilla);
        }

        public void NoComprar()
        {
            EnviarMensaje("NO_COMPRAR");
        }

        public void ConsultarEstado()
        {
            EnviarMensaje("CONSULTAR_ESTADO");
        }

        private void EnviarMensaje(string mensaje)
        {
            if (escritor != null)
            {
                escritor.WriteLine(mensaje);
            }
        }

        public void Desconectar()
        {
            conectado = false;
            if (socket != null)
            {
                socket.Close();
            }
        }

        public void ConsultarLobby()
        {
            EnviarMensaje("CONSULTAR_LOBBY");
        }

        public void IniciarPartida()
        {
            EnviarMensaje("INICIAR_PARTIDA");
        }

        public void TerminarTurno()
        {
            EnviarMensaje("TERMINAR_TURNO");
        }
    }
}