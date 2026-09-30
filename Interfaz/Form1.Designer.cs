namespace Monopoly.App
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            lbl_nombre = new Label();
            txt_nombre = new TextBox();
            btn_host = new Button();
            btn_unirse = new Button();
            label_Jugador1 = new Label();
            label_Jugador2 = new Label();
            lbl_nombre2 = new Label();
            txt_nombre2 = new TextBox();
            SuspendLayout();
            // 
            // lbl_nombre
            // 
            lbl_nombre.AutoSize = true;
            lbl_nombre.Font = new Font("Franklin Gothic Medium", 22F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl_nombre.Location = new Point(16, 118);
            lbl_nombre.Name = "lbl_nombre";
            lbl_nombre.Size = new Size(392, 51);
            lbl_nombre.TabIndex = 0;
            lbl_nombre.Text = "Ingrese su nombre";
            lbl_nombre.Click += label1_Click;
            // 
            // txt_nombre
            // 
            txt_nombre.Font = new Font("Franklin Gothic Medium", 20F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txt_nombre.Location = new Point(416, 118);
            txt_nombre.Name = "txt_nombre";
            txt_nombre.Size = new Size(458, 53);
            txt_nombre.TabIndex = 1;
            // 
            // btn_host
            // 
            btn_host.BackColor = Color.Purple;
            btn_host.Font = new Font("Franklin Gothic Medium", 26F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btn_host.ForeColor = Color.White;
            btn_host.Location = new Point(116, 468);
            btn_host.Name = "btn_host";
            btn_host.Size = new Size(207, 83);
            btn_host.TabIndex = 2;
            btn_host.Text = "Host";
            btn_host.UseVisualStyleBackColor = false;
            btn_host.Click += btn_host_Click;
            // 
            // btn_unirse
            // 
            btn_unirse.BackColor = Color.Purple;
            btn_unirse.Font = new Font("Franklin Gothic Medium", 26F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btn_unirse.ForeColor = SystemColors.ButtonHighlight;
            btn_unirse.Location = new Point(494, 468);
            btn_unirse.Name = "btn_unirse";
            btn_unirse.Size = new Size(219, 83);
            btn_unirse.TabIndex = 3;
            btn_unirse.Text = "Unirse";
            btn_unirse.UseVisualStyleBackColor = false;
            btn_unirse.Click += btn_unirse_Click;
            // 
            // label_Jugador1
            // 
            label_Jugador1.AutoSize = true;
            label_Jugador1.Font = new Font("Franklin Gothic Medium", 22F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label_Jugador1.Location = new Point(16, 30);
            label_Jugador1.Name = "label_Jugador1";
            label_Jugador1.Size = new Size(217, 51);
            label_Jugador1.TabIndex = 4;
            label_Jugador1.Text = "Jugador 1";
            label_Jugador1.Click += label1_Click_1;
            // 
            // label_Jugador2
            // 
            label_Jugador2.AutoSize = true;
            label_Jugador2.Font = new Font("Franklin Gothic Medium", 22F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label_Jugador2.Location = new Point(16, 240);
            label_Jugador2.Name = "label_Jugador2";
            label_Jugador2.Size = new Size(217, 51);
            label_Jugador2.TabIndex = 7;
            label_Jugador2.Text = "Jugador 2";
            // 
            // lbl_nombre2
            // 
            lbl_nombre2.AutoSize = true;
            lbl_nombre2.Font = new Font("Franklin Gothic Medium", 22F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl_nombre2.Location = new Point(16, 328);
            lbl_nombre2.Name = "lbl_nombre2";
            lbl_nombre2.Size = new Size(392, 51);
            lbl_nombre2.TabIndex = 5;
            lbl_nombre2.Text = "Ingrese su nombre";
            // 
            // txt_nombre2
            // 
            txt_nombre2.Font = new Font("Franklin Gothic Medium", 20F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txt_nombre2.Location = new Point(416, 328);
            txt_nombre2.Name = "txt_nombre2";
            txt_nombre2.Size = new Size(458, 53);
            txt_nombre2.TabIndex = 6;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(913, 913);
            Controls.Add(label_Jugador2);
            Controls.Add(lbl_nombre2);
            Controls.Add(txt_nombre2);
            Controls.Add(label_Jugador1);
            Controls.Add(lbl_nombre);
            Controls.Add(txt_nombre);
            Controls.Add(btn_host);
            Controls.Add(btn_unirse);
            Name = "Form1";
            Text = "Lobby";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lbl_nombre;
        private TextBox txt_nombre;
        private Button btn_host;
        private Button btn_unirse;
        private Label label_Jugador1;
        private Label label_Jugador2;
        private Label lbl_nombre2;
        private TextBox txt_nombre2;
    }
}
