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
        private Cliente cliente; 
        private Dictionary<int, Panel> casillas;
        private Dictionary<int, Label> fichas = new Dictionary<int, Label>();
        private int casillaDisponible;

        public FormsTablero(Cliente cliente)
        {
            InitializeComponent();
            btnDados.Click += btnDados_Click;
            btnComprar.Click += btnComprar_Click;
            btnNoComprar.Click += btnNoComprar_Click;
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
            btnDados.Enabled = false;
            cliente.TurnoCambiado += MostrarTurno;
            cliente.JugadorConectado += CrearFicha;
            Shown += (_, _) => cliente.ConsultarLobby();
            cliente.JugadorMovido += MostrarMovimiento;
            cliente.DadosLanzados += MostrarDados;
            cliente.DineroActualizado += MostrarDinero;
            btnComprar.Enabled = false;
            btnNoComprar.Enabled = false;
            cliente.PropiedadDisponible += MostrarPropiedadDisponible;
            cliente.PropiedadComprada += MostrarPropiedadComprada;
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

        private void MostrarMovimiento(int idJugador, int posicion)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => MostrarMovimiento(idJugador, posicion)));
                return;
            }
            if (!fichas.ContainsKey(idJugador) || !casillas.ContainsKey(posicion))
                return;

            Label ficha = fichas[idJugador];
            Panel casillaDestino = casillas[posicion];

            int fichasEnDestino = 0;

            foreach (Label otraFicha in fichas.Values)
            {
                if (otraFicha != ficha && otraFicha.Parent == casillaDestino)
                    fichasEnDestino++;
            }

            casillaDestino.Controls.Add(ficha);
            ficha.Location = new Point(10 + fichasEnDestino * 35, 10);
            ficha.BringToFront();

            lblMensaje.Text = $"Jugador {idJugador} llegó a la casilla {posicion}";
        }

        private void CrearFicha(int idJugador, string nombre)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => CrearFicha(idJugador, nombre)));
                return;
            }

            if (fichas.ContainsKey(idJugador))
                return;

            Label ficha = new Label();
            ficha.Text = idJugador.ToString();
            ficha.Size = new Size(30, 30);
            ficha.Location = new Point(10 + (idJugador - 1) * 35, 10);
            ficha.BackColor = Color.DarkRed;
            ficha.ForeColor = Color.White;
            ficha.TextAlign = ContentAlignment.MiddleCenter;

            fichas.Add(idJugador, ficha);
            casillas[1].Controls.Add(ficha);
            ficha.BringToFront();
        }

        private void MostrarTurno(int idJugador)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => MostrarTurno(idJugador)));
                return;
            }

            label2.Text = $"Jugador {idJugador}";
            btnDados.Enabled = idJugador == cliente.IdJugador;
        }

        private void btnDados_Click(object? sender, EventArgs e)
        {
            btnDados.Enabled = false;
            cliente.TirarDados();
        }
        private void MostrarDados(int idJugador, int dado1, int dado2)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => MostrarDados(idJugador, dado1, dado2)));
                return;
            }

            lblResultadoDado1.Text = dado1.ToString();
            lblResultadoDado2.Text = dado2.ToString();
            lblNumTotal.Text = (dado1 + dado2).ToString();
        }

        private void MostrarDinero(int idJugador, int nuevoSaldo)
        {
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

        private void MostrarPropiedadDisponible(int idCasilla, int precio)
        {
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

        private void btnComprar_Click(object? sender, EventArgs e)
        {
            btnComprar.Enabled = false;
            btnNoComprar.Enabled = false;
            cliente.ComprarPropiedad(casillaDisponible);
        }

        private void btnNoComprar_Click(object? sender, EventArgs e)
        {
            
            btnComprar.Enabled = false;
            btnNoComprar.Enabled = false;
            lblMensaje.Text = "Decidiste no comprar la propiedad.";
            cliente.NoComprar();

        }
         private void MostrarPropiedadComprada(int idJugador, int idCasilla)
        {
            if (InvokeRequired)
            {
                Invoke(() => MostrarPropiedadComprada(idJugador, idCasilla));
                return;
            }

            lblMensaje.Text = $"Jugador {idJugador} compró la casilla {idCasilla}.";
        }

    }
}
