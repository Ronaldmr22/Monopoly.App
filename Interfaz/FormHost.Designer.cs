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
            lbl_jugador2H = new Label();
            lbl_nombre2H = new Label();
            lbl_jugador3H = new Label();
            lbl_nombre3H = new Label();
            lbl_jugador4H = new Label();
            lbl_nombre4H = new Label();
            btn_comenzarH = new Button();
            lbl_jugador1H = new Label();
            lbl_nombre1H = new Label();
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
            // lbl_jugador2H
            // 
            lbl_jugador2H.AutoSize = true;
            lbl_jugador2H.Font = new Font("Franklin Gothic Medium", 22F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl_jugador2H.Location = new Point(208, 267);
            lbl_jugador2H.Name = "lbl_jugador2H";
            lbl_jugador2H.Size = new Size(208, 51);
            lbl_jugador2H.TabIndex = 2;
            lbl_jugador2H.Text = "Jugador 2";
            lbl_jugador2H.TextAlign = ContentAlignment.TopRight;
            lbl_jugador2H.Click += lbl_jugador1_Click;
            // 
            // lbl_nombre2H
            // 
            lbl_nombre2H.AutoSize = true;
            lbl_nombre2H.Font = new Font("Franklin Gothic Medium", 22F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl_nombre2H.Location = new Point(452, 267);
            lbl_nombre2H.Name = "lbl_nombre2H";
            lbl_nombre2H.Size = new Size(195, 51);
            lbl_nombre2H.TabIndex = 3;
            lbl_nombre2H.Text = "[nombre]";
            lbl_nombre2H.TextAlign = ContentAlignment.TopRight;
            // 
            // lbl_jugador3H
            // 
            lbl_jugador3H.AutoSize = true;
            lbl_jugador3H.Font = new Font("Franklin Gothic Medium", 22F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl_jugador3H.Location = new Point(208, 349);
            lbl_jugador3H.Name = "lbl_jugador3H";
            lbl_jugador3H.Size = new Size(208, 51);
            lbl_jugador3H.TabIndex = 4;
            lbl_jugador3H.Text = "Jugador 3";
            lbl_jugador3H.TextAlign = ContentAlignment.TopRight;
            // 
            // lbl_nombre3H
            // 
            lbl_nombre3H.AutoSize = true;
            lbl_nombre3H.Font = new Font("Franklin Gothic Medium", 22F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl_nombre3H.Location = new Point(452, 349);
            lbl_nombre3H.Name = "lbl_nombre3H";
            lbl_nombre3H.Size = new Size(195, 51);
            lbl_nombre3H.TabIndex = 5;
            lbl_nombre3H.Text = "[nombre]";
            lbl_nombre3H.TextAlign = ContentAlignment.TopRight;
            // 
            // lbl_jugador4H
            // 
            lbl_jugador4H.AutoSize = true;
            lbl_jugador4H.Font = new Font("Franklin Gothic Medium", 22F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl_jugador4H.Location = new Point(208, 426);
            lbl_jugador4H.Name = "lbl_jugador4H";
            lbl_jugador4H.Size = new Size(208, 51);
            lbl_jugador4H.TabIndex = 6;
            lbl_jugador4H.Text = "Jugador 4";
            lbl_jugador4H.TextAlign = ContentAlignment.TopRight;
            // 
            // lbl_nombre4H
            // 
            lbl_nombre4H.AutoSize = true;
            lbl_nombre4H.Font = new Font("Franklin Gothic Medium", 22F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl_nombre4H.Location = new Point(452, 426);
            lbl_nombre4H.Name = "lbl_nombre4H";
            lbl_nombre4H.Size = new Size(195, 51);
            lbl_nombre4H.TabIndex = 7;
            lbl_nombre4H.Text = "[nombre]";
            lbl_nombre4H.TextAlign = ContentAlignment.TopRight;
            // 
            // btn_comenzarH
            // 
            btn_comenzarH.BackColor = Color.Brown;
            btn_comenzarH.Font = new Font("Franklin Gothic Medium", 28F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btn_comenzarH.ForeColor = SystemColors.ButtonHighlight;
            btn_comenzarH.Location = new Point(233, 500);
            btn_comenzarH.Name = "btn_comenzarH";
            btn_comenzarH.Size = new Size(388, 74);
            btn_comenzarH.TabIndex = 8;
            btn_comenzarH.Text = "¡Comenzar!";
            btn_comenzarH.UseVisualStyleBackColor = false;
            btn_comenzarH.Click += btn_comenzarH_Click;
            // 
            // lbl_jugador1H
            // 
            lbl_jugador1H.AutoSize = true;
            lbl_jugador1H.Font = new Font("Franklin Gothic Medium", 22F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl_jugador1H.Location = new Point(141, 191);
            lbl_jugador1H.Name = "lbl_jugador1H";
            lbl_jugador1H.Size = new Size(326, 51);
            lbl_jugador1H.TabIndex = 9;
            lbl_jugador1H.Text = "Host - Jugador 1";
            lbl_jugador1H.TextAlign = ContentAlignment.TopRight;
            // 
            // lbl_nombre1H
            // 
            lbl_nombre1H.AutoSize = true;
            lbl_nombre1H.Font = new Font("Franklin Gothic Medium", 22F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl_nombre1H.Location = new Point(495, 191);
            lbl_nombre1H.Name = "lbl_nombre1H";
            lbl_nombre1H.Size = new Size(195, 51);
            lbl_nombre1H.TabIndex = 10;
            lbl_nombre1H.Text = "[nombre]";
            lbl_nombre1H.TextAlign = ContentAlignment.TopRight;
            // 
            // FormHost
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new Size(891, 858);
            Controls.Add(lbl_nombre1H);
            Controls.Add(lbl_jugador1H);
            Controls.Add(btn_comenzarH);
            Controls.Add(lbl_nombre4H);
            Controls.Add(lbl_jugador4H);
            Controls.Add(lbl_nombre3H);
            Controls.Add(lbl_jugador3H);
            Controls.Add(lbl_nombre2H);
            Controls.Add(lbl_jugador2H);
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
        private Label lbl_jugador2H;
        private Label lbl_nombre2H;
        private Label lbl_jugador3H;
        private Label lbl_nombre3H;
        private Label lbl_jugador4H;
        private Label lbl_nombre4H;
        private Button btn_comenzarH;
        private Label lbl_jugador1H;
        private Label lbl_nombre1H;
    }
}