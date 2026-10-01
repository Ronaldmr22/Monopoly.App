using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Monopoly.App
{
    public partial class FormsTablero : Form
    {
        // Conexión del primer jugador local - recibe también las notificaciones generales de la partida.
        private Cliente cliente;
        // Conexión opcional de un segundo jugador 
        private Cliente? cliente2;
        // Referencia al cliente local autorizado por el turno mostrado
        private Cliente? clienteEnTurno;
        // Relaciona cada número de casilla con su panel
        private Dictionary<int, Panel> casillas;

        // Relaciona el ID de cada jugador con el Label utilizado como ficha.
        private Dictionary<int, Label> fichas = new Dictionary<int, Label>();
        // Permite obtener los nombrs a partir de los IDs recibidos.
        private Dictionary<int, string> nombresJugadores = new Dictionary<int, string>();
        // Guarda casilla con dueño para retirar las marcas visuales cuando se elimina un jugador.
        private Dictionary<int, int> dueñosPropiedades = new Dictionary<int, int>();
        // Número de la propiedad pendiente de decisión, utilizado por el botón Comprar.
        private int casillaDisponible;

        // Construye la ventana, crea el mapa de casillas y conecta los eventos del cliente con los métodos que actualizan la interfaz. 
        public FormsTablero(Cliente cliente, Cliente? cliente2)
        {
            // Crea y configura los controles definidos en el archivo Designer
            InitializeComponent();
            btnDados.Click += btnDados_Click;
            btnComprar.Click += btnComprar_Click;
            btnNoComprar.Click += btnNoComprar_Click;
            // Mapa entre las 24 posiciones lógicas del tablero y los paneles creados en el diseñador.
            casillas = new Dictionary<int, Panel>
            {
                { 1, salida },
                { 2, Casa1 },
                { 3, Casa2 },
                { 4, Casa3 },
                { 5, Casa4 },
                { 6, Casa5 },
                { 7, carsel },
                { 8, Casa6 },
                { 9, primerevento },
                { 10, Casa7 },
                { 11, Casa8 },
                { 12, Casa9 },
                { 13, Casa10 },
                { 14, segundoevento },
                { 15, Casa11 },
                { 16, Casa12 },
                { 17, Casa13 },
                { 18, tercerevento },
                { 19, Casa14 },
                { 20, Casa15 },
                { 21, Casa16 },
                { 22, libre },
                { 23, Casa17 },
                { 24, Casa18 }
            };
            this.cliente = cliente;
            this.cliente2 = cliente2;

            btnDados.Enabled = false;
            // Cliente interpreta mensajes de red y publica eventos
            cliente.TurnoCambiado += MostrarTurno;
            cliente.JugadorConectado += CrearFicha;
            // Al mostrarse la ventana, solicita los jugadores para crear nombres y fichas. La lambda ignora los argumentos del evento.
            Shown += (_, _) => cliente.ConsultarLobby();
            cliente.JugadorMovido += MostrarMovimiento;
            cliente.DadosLanzados += MostrarDados;
            cliente.DineroActualizado += MostrarDinero;
            btnComprar.Enabled = false;
            btnNoComprar.Enabled = false;
            btnConsultar.Click += btnConsultar_Click;
            cliente.PropiedadDisponible += MostrarPropiedadDisponible;
            if (cliente2 != null)
            {
                cliente2.PropiedadDisponible += MostrarPropiedadDisponible;
            }
            cliente.PropiedadComprada += MostrarPropiedadComprada;
            cliente.AlquilerPagado += MostrarAlquilerPagado;
            cliente.CartaEventoRecibida += MostrarCartaEvento;
            cliente.JugadorEliminado += MostrarJugadorEliminado;
            cliente.PartidaTerminada += MostrarFinPartida;
            cliente.CasillaEspecialRecibida += MostrarCasillaEspecial;
            cliente.ErrorRecibido += MostrarError;
            if (cliente2 != null)
            {
                cliente2.ErrorRecibido += MostrarError;
            }
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tablaTablero_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint_2(object sender, PaintEventArgs e)
        {

        }

        private void lblCasa9_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        // Recibe del servidor la posición final del jugador y traslada su ficha al panel correspondiente. 
        private void MostrarMovimiento(int idJugador, int posicion)
        {
            // Si la llamada llega desde otro hilo, la actualización debe ejecutarse en el hilo que creó la ventana.
            if (InvokeRequired)
            {
                // Invoke ejecuta esta acción en el hilo de la interfaz; la lambda conserva los argumentos recibidos.
                Invoke(new Action(() => MostrarMovimiento(idJugador, posicion)));
                return;
            }
            // Ignora posiciones o jugadores que todavía no tienen una representación visual válida.
            if (!fichas.ContainsKey(idJugador) || !casillas.ContainsKey(posicion))
                return;

            Label ficha = fichas[idJugador];
            Panel casillaDestino = casillas[posicion];

            int fichasEnDestino = 0;

            // Cuenta las otras fichas en destino para aplicar un desplazamiento horizontal.
            foreach (Label otraFicha in fichas.Values)
            {
                if (otraFicha != ficha && otraFicha.Parent == casillaDestino)
                    fichasEnDestino++;
            }

            // Agregar la ficha al nuevo panel cambia su Parent y la retira del contenedor anterior.
            casillaDestino.Controls.Add(ficha);
            // Calcula la ubicación dentro del panel. El desplazamiento reduce la superposición en destinos compartidos.
            ficha.Location = new Point(10 + fichasEnDestino * 35, 10);
            // Coloca la ficha delante de los demás controles de su contenedor.
            ficha.BringToFront();
            // Limpia el mensaje anterior al recibir un movimiento.
            lblMensaje.Text = string.Empty;

            // TryGetValue intenta recuperar el nombre pero si falta, se utiliza el texto alternativo con el ID.
            string nombre = nombresJugadores.TryGetValue(idJugador, out string? encontrado)
                ? encontrado
                : $"Jugador {idJugador}";
        }

        // Guarda el nombre, actualiza la lista visible y crea una ficha por ID. Si la ficha ya existe, evita duplicarla al recibir nuevamente el lobby.
        private void CrearFicha(int idJugador, string nombre)
        {
            // Si la llamada llega desde otro hilo, la actualización debe ejecutarse en el hilo que creó la ventana.
            if (InvokeRequired)
            {
                Invoke(new Action(() => CrearFicha(idJugador, nombre)));
                return;
            }
            nombresJugadores[idJugador] = nombre;
            // Arreglo que permite seleccionar la etiqueta mediante idJugador - 1.
            Label[] etiquetasNombres = { Jugador1, Jugador2, Jugador3, Jugador4 };

            if (idJugador >= 1 && idJugador <= etiquetasNombres.Length)
            {
                etiquetasNombres[idJugador - 1].Text = $"{idJugador}. {nombre}";
            }
            if (fichas.ContainsKey(idJugador))
                return;

            // La ficha se crea dinámicamente: no es necesario dibujar una ficha fija por jugador en el diseñador.
            Label ficha = new Label();
            ficha.Text = idJugador.ToString();
            ficha.Size = new Size(30, 30);
            ficha.Location = new Point(10 + (idJugador - 1) * 35, 10);
            ficha.BackColor = Color.SkyBlue;
            ficha.ForeColor = Color.White;
            ficha.TextAlign = ContentAlignment.MiddleCenter;

            fichas.Add(idJugador, ficha);
            // Las fichas nuevas se colocan inicialmente en Salida, que corresponde a la posición 1.
            casillas[1].Controls.Add(ficha);
            // Coloca la ficha delante de los demás controles de su contenedor.
            ficha.BringToFront();
        }

        // Muestra quién tiene el turno y selecciona la conexión local que debe enviar las acciones. Si el turno es de un jugador remoto, deshabilita los dados.
        private void MostrarTurno(int idJugador)
        {
            // Si la llamada llega desde otro hilo, la actualización debe ejecutarse en el hilo que creó la ventana.
            if (InvokeRequired)
            {
                Invoke(new Action(() => MostrarTurno(idJugador)));
                return;
            }

            label2.Text = nombresJugadores.TryGetValue(idJugador, out string? nombre)
                ? $"Turno de {nombre}"
                : $"Turno del jugador {idJugador}";
            // El operador condicional elige el primer cliente, el segundo o null según el ID en turno.
            clienteEnTurno = idJugador == cliente.IdJugador
                ? cliente
                : cliente2 != null && idJugador == cliente2.IdJugador
                    ? cliente2
                    : null;

            // Enabled controla si el usuario puede presionar el botón; la validación definitiva corresponde al servidor.
            btnDados.Enabled = clienteEnTurno != null;
        }

        // Deshabilita el botón para evitar clics repetidos y solicita el lanzamiento al servidor mediante el cliente local en turno.
        private void btnDados_Click(object? sender, EventArgs e)
        {
            btnDados.Enabled = false;
            // ?. llama al método solamente cuando clienteEnTurno no es null. Cliente envía TIRAR_DADOS.
            clienteEnTurno?.TirarDados();
        }
        // Muestra los dos valores recibidos y su suma. Los resultados provienen del servidor y su integración con el hardware.
        private void MostrarDados(int idJugador, int dado1, int dado2)
        {
            // Si la llamada llega desde otro hilo, la actualización debe ejecutarse en el hilo que creó la ventana.
            if (InvokeRequired)
            {
                Invoke(new Action(() => MostrarDados(idJugador, dado1, dado2)));
                return;
            }

            lblResultadoDado1.Text = dado1.ToString();
            lblResultadoDado2.Text = dado2.ToString();
            lblNumTotal.Text = (dado1 + dado2).ToString();
        }

        // Actualiza la etiqueta del saldo indicado por el servidor. Los IDs empiezan en 1, mientras que los índices del arreglo empiezan en 0.
        private void MostrarDinero(int idJugador, int nuevoSaldo)
        {
            // Si la llamada llega desde otro hilo, la actualización debe ejecutarse en el hilo que creó la ventana.
            if (InvokeRequired)
            {
                Invoke(new Action(() => MostrarDinero(idJugador, nuevoSaldo)));
                return;
            }

            Label[] etiquetasDinero =
            {
                dineroJugador1,
                dineroJugador2,
                dineroJugador3,
                dineroJugador4
            };

            if (idJugador < 1 || idJugador > etiquetasDinero.Length)
                return;

            etiquetasDinero[idJugador - 1].Text = $"${nuevoSaldo}";
        }

        // Guarda la casilla ofrecida, muestra su precio y habilita las dos decisiones de compra.
        private void MostrarPropiedadDisponible(int idCasilla, int precio)
        {
            // Si la llamada llega desde otro hilo, la actualización debe ejecutarse en el hilo que creó la ventana.
            if (InvokeRequired)
            {
                Invoke(new Action(() => MostrarPropiedadDisponible(idCasilla, precio)));
                return;
            }

            casillaDisponible = idCasilla;
            lblMensaje.Text = $"La casilla {idCasilla} cuesta ${precio}. ¿Deseas comprarla?";
            btnComprar.Enabled = true;
            btnNoComprar.Enabled = true;
        }

        // Deshabilita las decisiones y solicita comprar la casilla guardada. El servidor valida la operación antes de confirmar la compra.
        private void btnComprar_Click(object? sender, EventArgs e)
        {
            btnComprar.Enabled = false;
            btnNoComprar.Enabled = false;
            // Envía COMPRAR_PROPIEDAD con el número de casilla utilizando la conexión del jugador en turno.
            clienteEnTurno?.ComprarPropiedad(casillaDisponible);
        }

        // Deshabilita las decisiones, muestra el rechazo y envía NO_COMPRAR al servidor.
        private void btnNoComprar_Click(object? sender, EventArgs e)
        {

            btnComprar.Enabled = false;
            btnNoComprar.Enabled = false;
            lblMensaje.Text = "Decidiste no comprar la propiedad.";
            // Envía NO_COMPRAR utilizando la conexión del jugador en turno.
            clienteEnTurno?.NoComprar();

        }
        // Al recibir la confirmación, registra el dueño para la representación visual y resalta la casilla comprada.
        private void MostrarPropiedadComprada(int idJugador, int idCasilla)
        {
            // Si la llamada llega desde otro hilo, la actualización debe ejecutarse en el hilo que creó la ventana.
            if (InvokeRequired)
            {
                Invoke(() => MostrarPropiedadComprada(idJugador, idCasilla));
                return;
            }
            // TryGetValue intenta recuperar el nombre; si falta, se utiliza el texto alternativo con el ID.
            string nombre = nombresJugadores.TryGetValue(idJugador, out string? encontrado)
                ? encontrado
                : $"Jugador {idJugador}";

            // Conserva la asociación visual para poder restaurar la casilla ante una eliminación.
            dueñosPropiedades[idCasilla] = idJugador;

            lblMensaje.Text = $"{nombre} compró la casilla {idCasilla}.";
            // Comprueba que exista el panel antes de cambiar su apariencia.
            if (casillas.TryGetValue(idCasilla, out Panel? casilla))
            {
                casilla.BackColor = Color.LightGoldenrodYellow;
                casilla.BorderStyle = BorderStyle.Fixed3D;
            }
        }

        // Convierte los IDs en nombres para mostrar quién pagó alquiler, quién lo recibió y en qué casilla ocurrió.
        private void MostrarAlquilerPagado(int idJugador, int idDueño, int idCasilla)
        {
            // Si la llamada llega desde otro hilo, la actualización debe ejecutarse en el hilo que creó la ventana.
            if (InvokeRequired)
            {
                Invoke(new Action(() => MostrarAlquilerPagado(idJugador, idDueño, idCasilla)));
                return;
            }

            string quienPaga = nombresJugadores.TryGetValue(idJugador, out string? nombreJugador)
                ? nombreJugador
                : $"Jugador {idJugador}";

            string quienCobra = nombresJugadores.TryGetValue(idDueño, out string? nombreDueño)
                ? nombreDueño
                : $"Jugador {idDueño}";

            lblMensaje.Text = $"{quienPaga} pagó alquiler a {quienCobra} por la casilla {idCasilla}.";
        }

        // Presenta el jugador, el número de carta y su descripción. Los efectos de la carta se resuelven en la lógica del juego.
        private void MostrarCartaEvento(int idJugador, int idCarta, string descripcion)
        {
            // Si la llamada llega desde otro hilo, la actualización debe ejecutarse en el hilo que creó la ventana.
            if (InvokeRequired)
            {
                Invoke(new Action(() => MostrarCartaEvento(idJugador, idCarta, descripcion)));
                return;
            }

            // TryGetValue intenta recuperar el nombre; si falta, se utiliza el texto alternativo con el ID.
            string nombre = nombresJugadores.TryGetValue(idJugador, out string? encontrado)
                ? encontrado
                : $"Jugador {idJugador}";

            lblMensaje.Text = $"{nombre} sacó la carta {idCarta}: {descripcion}.";
        }

        // Retira y libera la ficha, restaura el aspecto de sus propiedades y muestra la eliminación informada por el servidor.
        private void MostrarJugadorEliminado(int idJugador)
        {
            // Si la llamada llega desde otro hilo, la actualización debe ejecutarse en el hilo que creó la ventana.
            if (InvokeRequired)
            {
                Invoke(new Action(() => MostrarJugadorEliminado(idJugador)));
                return;
            }

            if (fichas.TryGetValue(idJugador, out Label? ficha))
            {
                // Retira la ficha de su contenedor si todavía tiene uno.
                ficha.Parent?.Controls.Remove(ficha);
                // Libera los recursos del control que ya no se mostrará.
                ficha.Dispose();
                fichas.Remove(idJugador);
            }
            // Primero reúne las claves; después modifica el diccionario para no alterarlo durante su recorrido.
            List<int> propiedadesLiberadas = new List<int>();

            foreach (var propiedad in dueñosPropiedades)
            {
                if (propiedad.Value == idJugador)
                {
                    propiedadesLiberadas.Add(propiedad.Key);
                }
            }

            // Restaura el aspecto de cada propiedad y elimina la asociación con el jugador eliminado.
            foreach (int idCasilla in propiedadesLiberadas)
            {
                // Comprueba que exista el panel antes de cambiar su apariencia.
                if (casillas.TryGetValue(idCasilla, out Panel? casilla))
                {
                    casilla.BackColor = SystemColors.Control;
                    casilla.BorderStyle = BorderStyle.FixedSingle;
                }

                dueñosPropiedades.Remove(idCasilla);
            }
            // TryGetValue intenta recuperar el nombre; si falta, se utiliza el texto alternativo con el ID.
            string nombre = nombresJugadores.TryGetValue(idJugador, out string? encontrado)
                ? encontrado
                : $"Jugador {idJugador}";
            if (lblMensaje.Text.StartsWith($"{nombre} sacó la carta "))
            {
                lblMensaje.Text += " Quedó eliminado.";
            }
            else
            {
                lblMensaje.Text = $"{nombre} quedó eliminado.";
            }
        }

        // Bloquea las acciones de juego y agrega el nombre del ganador recibido. La interfaz no determina al ganador.
        private void MostrarFinPartida(int idGanador)
        {
            // Si la llamada llega desde otro hilo, la actualización debe ejecutarse en el hilo que creó la ventana.
            if (InvokeRequired)
            {
                Invoke(new Action(() => MostrarFinPartida(idGanador)));
                return;
            }

            btnDados.Enabled = false;
            btnComprar.Enabled = false;
            btnNoComprar.Enabled = false;
            string nombreGanador = nombresJugadores.TryGetValue(idGanador, out string? encontrado) ? encontrado : $"Jugador {idGanador}";
            // Agrega el resultado en una nueva línea conservando el mensaje anterior.
            lblMensaje.Text += Environment.NewLine + $"Terminó la partida. Ganó {nombreGanador}.";
        }

        // Traduce el tipo de casilla especial a un mensaje comprensible mediante una expresión switch.
        private void MostrarCasillaEspecial(int idJugador, string tipoCasilla)
        {
            // Si la llamada llega desde otro hilo, la actualización debe ejecutarse en el hilo que creó la ventana.
            if (InvokeRequired)
            {
                Invoke(new Action(() => MostrarCasillaEspecial(idJugador, tipoCasilla)));
                return;
            }
            string nombre = nombresJugadores.TryGetValue(idJugador, out string? encontrado) ? encontrado : $"Jugador {idJugador}";
            // Selecciona el mensaje correspondiente a Salida, Cárcel, Libre u otro tipo recibido.
            lblMensaje.Text = tipoCasilla switch
            {
                "SALIDA" => $"Jugador {nombre} cayó en Salida.",
                "CARCEL" => $"Jugador {nombre} cayó en Cárcel y perderá un turno.",
                "LIBRE" => $"Jugador {nombre} cayó en Casilla Libre.",
                _ => $"Jugador {nombre} cayó en {tipoCasilla}."
            };
        }

        // Muestra el error recibido. Ante TARJETA_INCORRECTA permite reintentar los dados si el turno pertenece a un cliente local.
        private void MostrarError(string mensaje)
        {
            // Si la llamada llega desde otro hilo, la actualización debe ejecutarse en el hilo que creó la ventana.
            if (InvokeRequired)
            {
                Invoke(new Action(() => MostrarError(mensaje)));
                return;
            }

            lblMensaje.Text = mensaje;

            if (mensaje.Contains("TARJETA_INCORRECTA"))
            {
                // Enabled controla si el usuario puede presionar el botón; la validación definitiva corresponde al servidor.
                btnDados.Enabled = clienteEnTurno != null;
            }
        }
        // Abre una ventana de consultas con la conexión existente. Show permite mantener el tablero accesible mientras la ventana está abierta.
        private void btnConsultar_Click(object? sender, EventArgs e)
        {
            // Reutiliza el cliente existente para solicitar reportes al mismo servidor.
            FormTransacciones ventana = new FormTransacciones(cliente);
            ventana.Show();
        }

          private void lblCasa14_Click(object sender, EventArgs e)
        {

        }
        private void panel2_Paint_1(object sender, PaintEventArgs e)
        {

        }
    }
}
