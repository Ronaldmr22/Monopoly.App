namespace Monopoly.App
{
    partial class FormHost
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormHost));
            lbl_esperahost1 = new Label();
            lbl_esperahost2 = new Label();
            lbl_jugador2 = new Label();
            lbl_nombre2 = new Label();
            lbl_jugador3 = new Label();
            lbl_nombre3 = new Label();
            lbl_jugador4 = new Label();
            lbl_nombre4 = new Label();
            btn_comenzar = new Button();
            lbl_jugador1 = new Label();
            lbl_nombre1 = new Label();
            SuspendLayout();
            // 
            // lbl_esperahost1
            // 
            lbl_esperahost1.AutoSize = true;
            lbl_esperahost1.Font = new Font("Franklin Gothic Medium", 28F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl_esperahost1.Location = new Point(141, 38);
            lbl_esperahost1.Name = "lbl_esperahost1";
            lbl_esperahost1.Size = new Size(623, 70);
            lbl_esperahost1.TabIndex = 0;
            lbl_esperahost1.Text = "Eres el host de la partida";
            lbl_esperahost1.TextAlign = ContentAlignment.TopRight;
            lbl_esperahost1.Click += label1_Click;
            // 
            // lbl_esperahost2
            // 
            lbl_esperahost2.AutoSize = true;
            lbl_esperahost2.Font = new Font("Franklin Gothic Medium", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl_esperahost2.Location = new Point(141, 120);
            lbl_esperahost2.Name = "lbl_esperahost2";
            lbl_esperahost2.Size = new Size(619, 42);
            lbl_esperahost2.TabIndex = 1;
            lbl_esperahost2.Text = "Esperando a que se unan los jugadores";
            lbl_esperahost2.TextAlign = ContentAlignment.TopRight;
            // 
            // lbl_jugador2
            // 
            lbl_jugador2.AutoSize = true;
            lbl_jugador2.Font = new Font("Franklin Gothic Medium", 22F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl_jugador2.Location = new Point(208, 267);
            lbl_jugador2.Name = "lbl_jugador2";
            lbl_jugador2.Size = new Size(208, 51);
            lbl_jugador2.TabIndex = 2;
            lbl_jugador2.Text = "Jugador 2";
            lbl_jugador2.TextAlign = ContentAlignment.TopRight;
            lbl_jugador2.Click += lbl_jugador1_Click;
            // 
            // lbl_nombre2
            // 
            lbl_nombre2.AutoSize = true;
            lbl_nombre2.Font = new Font("Franklin Gothic Medium", 22F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl_nombre2.Location = new Point(452, 267);
            lbl_nombre2.Name = "lbl_nombre2";
            lbl_nombre2.Size = new Size(195, 51);
            lbl_nombre2.TabIndex = 3;
            lbl_nombre2.Text = "[nombre]";
            lbl_nombre2.TextAlign = ContentAlignment.TopRight;
            // 
            // lbl_jugador3
            // 
            lbl_jugador3.AutoSize = true;
            lbl_jugador3.Font = new Font("Franklin Gothic Medium", 22F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl_jugador3.Location = new Point(208, 349);
            lbl_jugador3.Name = "lbl_jugador3";
            lbl_jugador3.Size = new Size(208, 51);
            lbl_jugador3.TabIndex = 4;
            lbl_jugador3.Text = "Jugador 3";
            lbl_jugador3.TextAlign = ContentAlignment.TopRight;
            // 
            // lbl_nombre3
            // 
            lbl_nombre3.AutoSize = true;
            lbl_nombre3.Font = new Font("Franklin Gothic Medium", 22F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl_nombre3.Location = new Point(452, 349);
            lbl_nombre3.Name = "lbl_nombre3";
            lbl_nombre3.Size = new Size(195, 51);
            lbl_nombre3.TabIndex = 5;
            lbl_nombre3.Text = "[nombre]";
            lbl_nombre3.TextAlign = ContentAlignment.TopRight;
            // 
            // lbl_jugador4
            // 
            lbl_jugador4.AutoSize = true;
            lbl_jugador4.Font = new Font("Franklin Gothic Medium", 22F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl_jugador4.Location = new Point(208, 426);
            lbl_jugador4.Name = "lbl_jugador4";
            lbl_jugador4.Size = new Size(208, 51);
            lbl_jugador4.TabIndex = 6;
            lbl_jugador4.Text = "Jugador 4";
            lbl_jugador4.TextAlign = ContentAlignment.TopRight;
            // 
            // lbl_nombre4
            // 
            lbl_nombre4.AutoSize = true;
            lbl_nombre4.Font = new Font("Franklin Gothic Medium", 22F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl_nombre4.Location = new Point(452, 426);
            lbl_nombre4.Name = "lbl_nombre4";
            lbl_nombre4.Size = new Size(195, 51);
            lbl_nombre4.TabIndex = 7;
            lbl_nombre4.Text = "[nombre]";
            lbl_nombre4.TextAlign = ContentAlignment.TopRight;
            // 
            // btn_comenzar
            // 
            btn_comenzar.BackColor = Color.Brown;
            btn_comenzar.Font = new Font("Franklin Gothic Medium", 28F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btn_comenzar.ForeColor = SystemColors.ButtonHighlight;
            btn_comenzar.Location = new Point(233, 500);
            btn_comenzar.Name = "btn_comenzar";
            btn_comenzar.Size = new Size(388, 74);
            btn_comenzar.TabIndex = 8;
            btn_comenzar.Text = "¡Comenzar!";
            btn_comenzar.UseVisualStyleBackColor = false;
            // 
            // lbl_jugador1
            // 
            lbl_jugador1.AutoSize = true;
            lbl_jugador1.Font = new Font("Franklin Gothic Medium", 22F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl_jugador1.Location = new Point(141, 191);
            lbl_jugador1.Name = "lbl_jugador1";
            lbl_jugador1.Size = new Size(326, 51);
            lbl_jugador1.TabIndex = 9;
            lbl_jugador1.Text = "Host - Jugador 1";
            lbl_jugador1.TextAlign = ContentAlignment.TopRight;
            // 
            // lbl_nombre1
            // 
            lbl_nombre1.AutoSize = true;
            lbl_nombre1.Font = new Font("Franklin Gothic Medium", 22F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl_nombre1.Location = new Point(495, 191);
            lbl_nombre1.Name = "lbl_nombre1";
            lbl_nombre1.Size = new Size(195, 51);
            lbl_nombre1.TabIndex = 10;
            lbl_nombre1.Text = "[nombre]";
            lbl_nombre1.TextAlign = ContentAlignment.TopRight;
            // 
            // FormHost
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new Size(891, 858);
            Controls.Add(lbl_nombre1);
            Controls.Add(lbl_jugador1);
            Controls.Add(btn_comenzar);
            Controls.Add(lbl_nombre4);
            Controls.Add(lbl_jugador4);
            Controls.Add(lbl_nombre3);
            Controls.Add(lbl_jugador3);
            Controls.Add(lbl_nombre2);
            Controls.Add(lbl_jugador2);
            Controls.Add(lbl_esperahost2);
            Controls.Add(lbl_esperahost1);
            Name = "FormHost";
            Text = "FormHost";
            Load += FormEspera_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lbl_esperahost1;
        private Label lbl_esperahost2;
        private Label lbl_jugador2;
        private Label lbl_nombre2;
        private Label lbl_jugador3;
        private Label lbl_nombre3;
        private Label lbl_jugador4;
        private Label lbl_nombre4;
        private Button btn_comenzar;
        private Label lbl_jugador1;
        private Label lbl_nombre1;
    }
}