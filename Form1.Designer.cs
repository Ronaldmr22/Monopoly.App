namespace Monopoly.App
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            lbl_nombre = new Label();
            txt_nombre = new TextBox();
            btn_host = new Button();
            btn_unirse = new Button();
            SuspendLayout();
            // 
            // lbl_nombre
            // 
            lbl_nombre.AutoSize = true;
            lbl_nombre.Font = new Font("Franklin Gothic Medium", 22F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl_nombre.Location = new Point(12, 31);
            lbl_nombre.Name = "lbl_nombre";
            lbl_nombre.Size = new Size(392, 51);
            lbl_nombre.TabIndex = 0;
            lbl_nombre.Text = "Ingrese su nombre";
            lbl_nombre.Click += label1_Click;
            // 
            // txt_nombre
            // 
            txt_nombre.Font = new Font("Franklin Gothic Medium", 20F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txt_nombre.Location = new Point(410, 31);
            txt_nombre.Name = "txt_nombre";
            txt_nombre.Size = new Size(459, 53);
            txt_nombre.TabIndex = 1;
            // 
            // btn_host
            // 
            btn_host.BackColor = Color.White;
            btn_host.Font = new Font("Franklin Gothic Medium", 26F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btn_host.ForeColor = Color.Black;
            btn_host.Location = new Point(105, 197);
            btn_host.Name = "btn_host";
            btn_host.Size = new Size(207, 83);
            btn_host.TabIndex = 2;
            btn_host.Text = "Host";
            btn_host.UseVisualStyleBackColor = false;
            // 
            // btn_unirse
            // 
            btn_unirse.BackColor = Color.White;
            btn_unirse.Font = new Font("Franklin Gothic Medium", 26F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btn_unirse.ForeColor = SystemColors.ActiveCaptionText;
            btn_unirse.Location = new Point(516, 197);
            btn_unirse.Name = "btn_unirse";
            btn_unirse.Size = new Size(219, 83);
            btn_unirse.TabIndex = 3;
            btn_unirse.Text = "Unirse";
            btn_unirse.UseVisualStyleBackColor = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new Size(893, 857);
            Controls.Add(btn_unirse);
            Controls.Add(btn_host);
            Controls.Add(txt_nombre);
            Controls.Add(lbl_nombre);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lbl_nombre;
        private TextBox txt_nombre;
        private Button btn_host;
        private Button btn_unirse;
    }
}
