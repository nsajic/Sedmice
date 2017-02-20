namespace Sedmice
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.odustani = new System.Windows.Forms.Button();
            this.baciKartu = new System.Windows.Forms.Button();
            this.startGame = new System.Windows.Forms.Button();
            this.kartaTalon = new System.Windows.Forms.Label();
            this.brojKarata2 = new System.Windows.Forms.Label();
            this.brojKarata1 = new System.Windows.Forms.Label();
            this.mojeKarte = new System.Windows.Forms.ComboBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // odustani
            // 
            this.odustani.Enabled = false;
            this.odustani.Location = new System.Drawing.Point(477, 354);
            this.odustani.Name = "odustani";
            this.odustani.Size = new System.Drawing.Size(87, 23);
            this.odustani.TabIndex = 25;
            this.odustani.Text = "Odustani";
            this.odustani.UseVisualStyleBackColor = true;
            this.odustani.Click += new System.EventHandler(this.odustani_Click);
            // 
            // baciKartu
            // 
            this.baciKartu.Location = new System.Drawing.Point(383, 354);
            this.baciKartu.Name = "baciKartu";
            this.baciKartu.Size = new System.Drawing.Size(87, 23);
            this.baciKartu.TabIndex = 24;
            this.baciKartu.Text = "Baci";
            this.baciKartu.UseVisualStyleBackColor = true;
            this.baciKartu.Click += new System.EventHandler(this.baciKartu_Click);
            // 
            // startGame
            // 
            this.startGame.Location = new System.Drawing.Point(383, 480);
            this.startGame.Name = "startGame";
            this.startGame.Size = new System.Drawing.Size(87, 23);
            this.startGame.TabIndex = 23;
            this.startGame.Text = "Start Game";
            this.startGame.UseVisualStyleBackColor = true;
            this.startGame.Click += new System.EventHandler(this.startGame_Click);
            // 
            // kartaTalon
            // 
            this.kartaTalon.AutoSize = true;
            this.kartaTalon.BackColor = System.Drawing.Color.CornflowerBlue;
            this.kartaTalon.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.kartaTalon.ForeColor = System.Drawing.SystemColors.ControlText;
            this.kartaTalon.Location = new System.Drawing.Point(206, 170);
            this.kartaTalon.Name = "kartaTalon";
            this.kartaTalon.Size = new System.Drawing.Size(95, 42);
            this.kartaTalon.TabIndex = 22;
            this.kartaTalon.Text = "       ";
            // 
            // brojKarata2
            // 
            this.brojKarata2.AutoSize = true;
            this.brojKarata2.BackColor = System.Drawing.Color.Orange;
            this.brojKarata2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.brojKarata2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.brojKarata2.Location = new System.Drawing.Point(249, 51);
            this.brojKarata2.Name = "brojKarata2";
            this.brojKarata2.Size = new System.Drawing.Size(30, 31);
            this.brojKarata2.TabIndex = 20;
            this.brojKarata2.Text = "0";
            // 
            // brojKarata1
            // 
            this.brojKarata1.AutoSize = true;
            this.brojKarata1.BackColor = System.Drawing.Color.Lime;
            this.brojKarata1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.brojKarata1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.brojKarata1.Location = new System.Drawing.Point(249, 269);
            this.brojKarata1.Name = "brojKarata1";
            this.brojKarata1.Size = new System.Drawing.Size(30, 31);
            this.brojKarata1.TabIndex = 19;
            this.brojKarata1.Text = "0";
            // 
            // mojeKarte
            // 
            this.mojeKarte.FormattingEnabled = true;
            this.mojeKarte.Location = new System.Drawing.Point(213, 356);
            this.mojeKarte.Name = "mojeKarte";
            this.mojeKarte.Size = new System.Drawing.Size(140, 21);
            this.mojeKarte.TabIndex = 18;
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(417, 138);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 26;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(382, 170);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(10, 13);
            this.label2.TabIndex = 27;
            this.label2.Text = " ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(474, 170);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(10, 13);
            this.label3.TabIndex = 28;
            this.label3.Text = " ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(382, 199);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 13);
            this.label4.TabIndex = 29;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(474, 199);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 13);
            this.label5.TabIndex = 30;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(596, 505);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.odustani);
            this.Controls.Add(this.baciKartu);
            this.Controls.Add(this.startGame);
            this.Controls.Add(this.kartaTalon);
            this.Controls.Add(this.brojKarata2);
            this.Controls.Add(this.brojKarata1);
            this.Controls.Add(this.mojeKarte);
            this.Name = "Form1";
            this.Text = "Sedmice";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button odustani;
        private System.Windows.Forms.Button baciKartu;
        private System.Windows.Forms.Button startGame;
        public System.Windows.Forms.Label kartaTalon;
        public System.Windows.Forms.Label brojKarata2;
        public System.Windows.Forms.Label brojKarata1;
        public System.Windows.Forms.ComboBox mojeKarte;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}

