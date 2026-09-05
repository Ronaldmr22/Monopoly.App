namespace Monopoly.App
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = Servidor.GetJugador(3).GetInfo();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Jugador.CrearJugador(1, "Leo");
        }
    }
}
