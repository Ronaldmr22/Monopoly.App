namespace Monopoly.App
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void btn_host_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txt_nombre.Text))
            {
                string nombreJugador = txt_nombre.Text;
                int puerto = 5000;

                Host host = new Host();

                // Suscribo ANTES de conectar, así no me pierdo el mensaje
                host.Cliente.ActualizacionJuego += texto =>
                {
                    this.Invoke(new Action(() => MessageBox.Show(texto)));
                };

                await host.IniciarAsync(puerto, nombreJugador);

                FormHost siguiente = new FormHost(host, nombreJugador);
                siguiente.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Debe ingresar un nombre.");
            }
        }

        private void btn_unirse_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txt_nombre.Text))
            {
                FormCliente siguiente = new FormCliente();
                siguiente.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Debe ingresar un nombre.");
            }
        }

    }
}
