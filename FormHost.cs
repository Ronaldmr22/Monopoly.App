using System;
using System.Windows.Forms;

namespace Monopoly.App
{
    public partial class FormHost : Form
    {
        private Cliente cliente;
        private string nombreJugador;

        private bool esHost;

        public FormHost(Cliente cliente, string nombreJugador, bool esHost)
        {
            InitializeComponent();

            this.cliente = cliente;
            this.nombreJugador = nombreJugador;
            this.esHost = esHost;

            cliente.JugadorConectado += MostrarJugador;
            MostrarMenu();
            cliente.ConsultarLobby();
        }

        private void MostrarMenu()
        {
            if (esHost)
            {
                lbl_esperahost1.Text = "Eres el host de la partida";
                lbl_esperahost2.Text = "Esperando a que se unan los jugadores";

                btn_comenzarH.Visible = true;
            }
            else
            {
                lbl_esperahost1.Text = "Te has unido a la partida";
                lbl_esperahost2.Text = "Esperando a que el host comience";

                btn_comenzarH.Visible = false;
            }
        }

        private void MostrarJugador(int id, string nombre)
        {
            this.Invoke(new Action(() =>
            {
                if (id == 1)
                {
                    lbl_nombre1H.Text = nombre;
                }
                else if (id == 2)
                {
                    lbl_nombre2H.Text = nombre;
                }
                else if (id == 3)
                {
                    lbl_nombre3H.Text = nombre;
                }
                else if (id == 4)
                {
                    lbl_nombre4H.Text = nombre;
                }
            }));
        }

        private void FormEspera_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void lbl_jugador1_Click(object sender, EventArgs e)
        {

        }
    }
}