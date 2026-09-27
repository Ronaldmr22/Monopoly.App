namespace Monopoly.App
{
    partial class FormTransacciones
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTransacciones));
            btnBuscarMasReciente = new Button();
            label1 = new Label();
            btnBuscarMasAntigua = new Button();
            btnBuscarPorJugador = new Button();
            btnBuscarPorTipo = new Button();
            btnMostrarTodas = new Button();
            SuspendLayout();
            // 
            // btnBuscarMasReciente
            // 
            btnBuscarMasReciente.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnBuscarMasReciente.Location = new Point(30, 198);
            btnBuscarMasReciente.Name = "btnBuscarMasReciente";
            btnBuscarMasReciente.Size = new Size(401, 65);
            btnBuscarMasReciente.TabIndex = 0;
            btnBuscarMasReciente.Text = "Desde la más reciente";
            btnBuscarMasReciente.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(93, 76);
            label1.Name = "label1";
            label1.Size = new Size(706, 68);
            label1.TabIndex = 1;
            label1.Text = "Seleccione la categoría por la que desea generar \r\nel historial de transacciones";
            label1.TextAlign = ContentAlignment.TopCenter;
            label1.Click += label1_Click;
            // 
            // btnBuscarMasAntigua
            // 
            btnBuscarMasAntigua.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnBuscarMasAntigua.Location = new Point(466, 198);
            btnBuscarMasAntigua.Name = "btnBuscarMasAntigua";
            btnBuscarMasAntigua.Size = new Size(401, 65);
            btnBuscarMasAntigua.TabIndex = 2;
            btnBuscarMasAntigua.Text = "Desde la más antigua";
            btnBuscarMasAntigua.UseVisualStyleBackColor = true;
            // 
            // btnBuscarPorJugador
            // 
            btnBuscarPorJugador.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnBuscarPorJugador.Location = new Point(21, 325);
            btnBuscarPorJugador.Name = "btnBuscarPorJugador";
            btnBuscarPorJugador.Size = new Size(401, 70);
            btnBuscarPorJugador.TabIndex = 3;
            btnBuscarPorJugador.Text = "Buscar por juagdor";
            btnBuscarPorJugador.UseVisualStyleBackColor = true;
            // 
            // btnBuscarPorTipo
            // 
            btnBuscarPorTipo.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnBuscarPorTipo.Location = new Point(466, 327);
            btnBuscarPorTipo.Name = "btnBuscarPorTipo";
            btnBuscarPorTipo.Size = new Size(401, 68);
            btnBuscarPorTipo.TabIndex = 4;
            btnBuscarPorTipo.Text = "Buscar por tipo";
            btnBuscarPorTipo.UseVisualStyleBackColor = true;
            // 
            // btnMostrarTodas
            // 
            btnMostrarTodas.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnMostrarTodas.Location = new Point(248, 431);
            btnMostrarTodas.Name = "btnMostrarTodas";
            btnMostrarTodas.Size = new Size(401, 76);
            btnMostrarTodas.TabIndex = 5;
            btnMostrarTodas.Text = "Mostrar todas";
            btnMostrarTodas.UseVisualStyleBackColor = true;
            btnMostrarTodas.Click += btnMostrarTodas_Click;
            // 
            // FormTransacciones
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(891, 857);
            Controls.Add(btnMostrarTodas);
            Controls.Add(btnBuscarPorTipo);
            Controls.Add(btnBuscarPorJugador);
            Controls.Add(btnBuscarMasAntigua);
            Controls.Add(label1);
            Controls.Add(btnBuscarMasReciente);
            Name = "FormTransacciones";
            Text = "Form2";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnBuscarMasReciente;
        private Label label1;
        private Button btnBuscarMasAntigua;
        private Button btnBuscarPorJugador;
        private Button btnBuscarPorTipo;
        private Button btnMostrarTodas;
    }
}