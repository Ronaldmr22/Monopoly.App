using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Monopoly.App
{
    public partial class FormTransacciones : Form
    {
        private readonly Cliente cliente;
        private readonly TextBox txtJugador = new TextBox();
        private readonly ComboBox cmbTipo = new ComboBox();

        public FormTransacciones(Cliente cliente)
        {
            InitializeComponent();
            this.cliente = cliente;
            cmbTipo.Location = new Point(466, 285);
            cmbTipo.Size = new Size(401, 30);
            cmbTipo.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTipo.Items.AddRange(new object[]
            {
                "Compra de propiedad",
                "Cobro de alquiler",
                "Premio de salida",
                "Jugador gana dinero",
                "Jugador pierde dinero"
            });
            Controls.Add(cmbTipo);
            txtJugador.PlaceholderText = "Nombre del jugador";
            txtJugador.Location = new Point(21, 285);
            txtJugador.Size = new Size(401, 30);
            Controls.Add(txtJugador);
            cliente.TransaccionesGeneradas += MostrarArchivoGenerado;
            btnBuscarMasAntigua.Click += (_, _)=>cliente.ConsultarTransacciones("ANTIGUAS");
            btnBuscarMasReciente.Click += (_, _) =>cliente.ConsultarTransacciones("RECIENTES");
            btnBuscarPorJugador.Click += btnBuscarPorJugador_Click;
            btnBuscarPorTipo.Click += btnBuscarPorTipo_Click;
            FormClosed += (_, _) => cliente.TransaccionesGeneradas -= MostrarArchivoGenerado;
        }

        private void btnMostrarTodas_Click(object sender, EventArgs e)
        {
            cliente.ConsultarTransacciones("TODAS");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void MostrarArchivoGenerado(string nombreArchivo)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => MostrarArchivoGenerado(nombreArchivo)));
                return;
            }

            MessageBox.Show(
                $"Se generó \"{nombreArchivo}\" en la computadora del host.",
                "Consulta completada"
            );
        }
        private void btnBuscarPorJugador_Click(object? sender, EventArgs e)
        {
            string nombre = txtJugador.Text.Trim();

            if (nombre.Length == 0)
            {
                MessageBox.Show("Escribe el nombre del jugador.");
                return;
            }

            cliente.ConsultarTransacciones("JUGADOR", nombre);
        }
        private void btnBuscarPorTipo_Click(object? sender, EventArgs e)
        {
            if (cmbTipo.SelectedItem is not string tipo)
            {
                MessageBox.Show("Selecciona un tipo de transacción.");
                return;
            }

            cliente.ConsultarTransacciones("TIPO", tipo);
        }
    }
}
