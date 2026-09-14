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

        private void btn_host_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txt_nombre.Text))
            {
                FormHost siguiente = new FormHost();
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
