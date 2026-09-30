using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Monopoly.App
{
    public partial class FormCliente : Form
    {
        private Cliente cliente;
        private string nombreJugador;

        public FormCliente(Cliente cliente, string nombreJugador)
        {
            InitializeComponent();

            this.cliente = cliente;
            this.nombreJugador = nombreJugador;
        }
    }
}