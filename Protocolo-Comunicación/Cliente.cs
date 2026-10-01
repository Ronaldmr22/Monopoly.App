using System;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly.App
{
    // Clase encargada de comunicarse con el servidor.
    public class Cliente
    {
        // Elementos necesarios para establecer y mantener la comunicación.
        private TcpClient socket;
        private StreamReader lector;
        private StreamWriter escritor;
        private bool conectado;


        // ID asignado por el servidor al jugador.
        public int IdJugador { get; private set; }

        // Eventos utilizados para avisarle a la interfaz sobre cambios en el juego.
        public event Action<int, string> JugadorConectado;
        public event Action<string> ActualizacionJuego;
        public event Action<string> ErrorRecibido;
        public event Action<int, int> PropiedadDisponible;
        public event Action<int, int> PropiedadComprada;
        public event Action PartidaIniciada;
        public event Action<int, int> JugadorMovido;
        public event Action<int, int, int> DadosLanzados;
        public event Action<int> TurnoCambiado;
        public event Action<int, int> DineroActualizado;
        public event Action<int> PropiedadPropia;
        public event Action<int, int, int, int> AlquilerPagado;
        public event Action<int> JugadorEliminado;
        public event Action<int> PartidaTerminada;
        public event Action<int, int, string> CartaEventoRecibida;
        public event Action<int, string>? CasillaEspecialRecibida;
        public event Action<string>? TransaccionesGeneradas;


        // Establece la conexión con el servidor y registra al jugador.
        public async Task ConectarAsync(string ip, int puerto, string nombreJugador)
        {
            socket = new TcpClient();
            await socket.ConnectAsync(ip, puerto);

            // Obtiene el flujo de comunicación y prepara la lectura y escritura.
            NetworkStream stream = socket.GetStream();
            lector = new StreamReader(stream, Encoding.UTF8);
            escritor = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
            conectado = true;

            // Comienza a escuchar mensajes del servidor.
            EscucharServidorAsync();

            // Envía el nombre del jugador para registrarlo.
            EnviarMensaje("CONECTAR " + nombreJugador);
        }


        // Se mantiene escuchando los mensajes que llegan desde el servidor.
        private async Task EscucharServidorAsync()
        {
            string linea;
            while (conectado)
            {
                linea = await lector.ReadLineAsync();

                // Si no se recibe información, termina la escucha.
                if (linea == null)
                {
                    break;
                }

                // Procesa el mensaje recibido.
                ProcesarMensaje(linea);
            }
        }


        // Determina qué tipo de mensaje envió el servidor.
        private void ProcesarMensaje(string linea)
        {
            // Separa el mensaje y obtiene el comando principal.
            string[] partes = linea.Split(' ');
            string comando = partes[0];

            // El servidor confirma la conexión del cliente.
            if (comando == "CONECTAR")
            {
                IdJugador = int.Parse(partes[1]);
                string nombre = partes[2];

                if (ActualizacionJuego != null)
                {
                    ActualizacionJuego.Invoke($"Te conectaste como {nombre}, jugador {IdJugador}");
                }
            }

            // Informa sobre un jugador conectado.
            else if (comando == "JUGADOR")
            {
                int id = int.Parse(partes[1]);
                string nombre = partes[2];

                if (JugadorConectado != null)
                {
                    JugadorConectado.Invoke(id, nombre);
                }
            }

            // Recibe el resultado de los dados.
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

            // Informa que se realizó una compra.
            else if (comando == "COMPRAR")
            {
                if (ActualizacionJuego != null)
                {
                    ActualizacionJuego.Invoke("Compraste la casilla " + partes[2]);
                }
            }

            // Informa que un jugador compró una propiedad.
            else if (comando == "PROPIEDAD")
            {
                if (ActualizacionJuego != null)
                {
                    ActualizacionJuego.Invoke("Jugador " + partes[2] + " compró la casilla " + partes[3]);
                }
            }

            // Informa que no existe suficiente dinero.
            else if (comando == "DINERO")
            {
                if (ErrorRecibido != null)
                {
                    ErrorRecibido.Invoke("No tenés suficiente dinero para esa compra");
                }
            }

            // Recibe información del estado del juego.
            else if (comando == "ESTADO")
            {
                if (ActualizacionJuego != null)
                {
                    ActualizacionJuego.Invoke(linea.Substring(comando.Length + 1));
                }
            }

            // Recibe un error enviado por el servidor.
            else if (comando == "ERROR")
            {
                if (ErrorRecibido != null)
                {
                    ErrorRecibido.Invoke(linea);
                }
            }

            // Informa que la propiedad está disponible para comprar.
            else if (comando == "PROPIEDAD_DISPONIBLE")
            {
                int idCasilla = int.Parse(partes[1]);
                int precio = int.Parse(partes[2]);

                PropiedadDisponible.Invoke(idCasilla, precio);

            }

            // Informa que una propiedad fue comprada.
            else if (comando == "PROPIEDAD_COMPRADA")
            {
                int idJugador = int.Parse(partes[1]);
                int idCasilla = int.Parse(partes[2]);

                PropiedadComprada?.Invoke(idJugador, idCasilla);
            }

            // Recibe información general de un alquiler.
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

            // Informa que se realizó el pago de un alquiler.
            else if (comando == "ALQUILER_PAGADO")
            {
                int idJugador = int.Parse(partes[1]);
                int idDueño = int.Parse(partes[2]);
                int idCasilla = int.Parse(partes[3]);
                int monto = int.Parse(partes[4]);

                AlquilerPagado?.Invoke(idJugador, idDueño, idCasilla, monto);
            }

            // Informa que el jugador cayó en una propiedad que ya posee.
            else if (comando == "PROPIEDAD_PROPIA")
            {
                int idCasilla = int.Parse(partes[1]);

                PropiedadPropia?.Invoke(idCasilla);

                if (ActualizacionJuego != null)
                {
                    ActualizacionJuego.Invoke("La casilla " + idCasilla + " ya es tuya");
                }
            }

            // Informa que la partida comenzó.
            else if (comando == "PARTIDA_INICIADA")
            {
                if (PartidaIniciada != null)
                {
                    PartidaIniciada.Invoke();
                }
            }

            // Informa la nueva posición de un jugador.
            else if (comando == "JUGADOR_MOVIDO")
            {
                int idJugador = int.Parse(partes[1]);
                int posicion = int.Parse(partes[2]);

                JugadorMovido?.Invoke(idJugador, posicion);
            }

            // Informa qué jugador tiene el turno.
            else if (comando == "TURNO")
            {
                int idJugador = int.Parse(partes[1]);

                TurnoCambiado?.Invoke(idJugador);
            }

            // Informa el nuevo saldo de un jugador.
            else if (comando == "DINERO_ACTUALIZADO")
            {
                int idJugador = int.Parse(partes[1]);
                int nuevoSaldo = int.Parse(partes[2]);

                DineroActualizado?.Invoke(idJugador, nuevoSaldo);
            }

            // Informa que un jugador fue eliminado.
            else if (comando == "JUGADOR_ELIMINADO")
            {
                int idJugador = int.Parse(partes[1]);

                JugadorEliminado?.Invoke(idJugador);
            }

            // Informa que terminó la partida y quién ganó.
            else if (comando == "FIN_PARTIDA")
            {
                int idGanador = int.Parse(partes[1]);

                PartidaTerminada?.Invoke(idGanador);
            }

            // Recibe la carta de evento que le salió al jugador.
            else if (comando == "CARTA_EVENTO")
            {
                int idJugador = int.Parse(partes[1]);
                int idCarta = int.Parse(partes[2]);

                // Reconstruye la descripción completa de la carta.
                string descripcion = string.Join(" ", partes[3..]);

                CartaEventoRecibida?.Invoke(idJugador, idCarta, descripcion);
            }

            // Informa que un jugador cayó en una casilla especial.
            else if (comando == "CASILLA_ESPECIAL")
            {
                int idJugador = int.Parse(partes[1]);
                string tipoCasilla = partes[2];

                CasillaEspecialRecibida?.Invoke(idJugador, tipoCasilla);
            }

            // Recibe el nombre del archivo de transacciones generado.
            else if (comando == "TRANSACCIONES_GENERADAS")
            {
                string nombreArchivo = string.Join(" ", partes[1..]);
                TransaccionesGeneradas?.Invoke(nombreArchivo);
            }

            // Si el comando no coincide con ninguno de los anteriores.
            else
            {
                if (ErrorRecibido != null)
                {
                    ErrorRecibido.Invoke("Mensaje no reconocido: " + linea);
                }
            }
        }


        // Solicita al servidor lanzar los dados.
        public void TirarDados()
        {
            EnviarMensaje("TIRAR_DADOS");
        }


        // Solicita al servidor comprar una propiedad.
        public void ComprarPropiedad(int idCasilla)
        {
            EnviarMensaje("COMPRAR_PROPIEDAD " + idCasilla);
        }


        // Informa al servidor que no se desea comprar la propiedad.
        public void NoComprar()
        {
            EnviarMensaje("NO_COMPRAR");
        }


        // Solicita el estado actual del juego.
        public void ConsultarEstado()
        {
            EnviarMensaje("CONSULTAR_ESTADO");
        }


        // Envía un mensaje al servidor.
        private void EnviarMensaje(string mensaje)
        {
            if (escritor != null)
            {
                escritor.WriteLine(mensaje);
            }
        }


        // Desconecta al cliente del servidor.
        public void Desconectar()
        {
            conectado = false;

            if (socket != null)
            {
                socket.Close();
            }
        }


        // Solicita la información de los jugadores del lobby.
        public void ConsultarLobby()
        {
            EnviarMensaje("CONSULTAR_LOBBY");
        }


        // Solicita al servidor iniciar la partida.
        public void IniciarPartida()
        {
            EnviarMensaje("INICIAR_PARTIDA");
        }


        // Solicita generar el historial de transacciones según una opción y filtro.
        public void ConsultarTransacciones(string opcion, string? filtro = null)
        {
            string mensaje = $"CONSULTAR_TRANSACCIONES {opcion}";

            // Si existe un filtro, se agrega al mensaje.
            if (!string.IsNullOrWhiteSpace(filtro))
            {
                mensaje += $" {filtro}";
            }

            EnviarMensaje(mensaje);
        }


    }
}