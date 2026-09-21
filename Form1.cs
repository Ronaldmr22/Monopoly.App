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
                await host.IniciarAsync(5000,nombreJugador);
                FormHost siguiente = new FormHost(host.Cliente,nombreJugador, true);
                siguiente.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Debe ingresar un nombre.");
            }
        }

        private async void btn_unirse_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txt_nombre.Text))
            {
                string nombreJugador = txt_nombre.Text;

                Cliente cliente = new Cliente();

                await cliente.ConectarAsync("192.168.0.221",5000,nombreJugador);

                FormHost siguiente = new FormHost(cliente,nombreJugador, false);

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
