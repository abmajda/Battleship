namespace Battleship
{
    partial class StartScreen
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
            this.MainLogo = new System.Windows.Forms.Label();
            this.ShamelessPlug = new System.Windows.Forms.Label();
            this.AIButton = new System.Windows.Forms.Button();
            this.MPButton = new System.Windows.Forms.Button();
            this.CloseButton = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // MainLogo
            // 
            this.MainLogo.AutoSize = true;
            this.MainLogo.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.MainLogo.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainLogo.Location = new System.Drawing.Point(82, 41);
            this.MainLogo.Name = "MainLogo";
            this.MainLogo.Size = new System.Drawing.Size(167, 37);
            this.MainLogo.TabIndex = 0;
            this.MainLogo.Text = "Battleship";
            // 
            // ShamelessPlug
            // 
            this.ShamelessPlug.AutoSize = true;
            this.ShamelessPlug.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.ShamelessPlug.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShamelessPlug.Location = new System.Drawing.Point(66, 97);
            this.ShamelessPlug.Name = "ShamelessPlug";
            this.ShamelessPlug.Size = new System.Drawing.Size(198, 20);
            this.ShamelessPlug.TabIndex = 1;
            this.ShamelessPlug.Text = "A Project by Andrew Majda";
            // 
            // AIButton
            // 
            this.AIButton.BackColor = System.Drawing.SystemColors.Control;
            this.AIButton.Location = new System.Drawing.Point(12, 176);
            this.AIButton.Name = "AIButton";
            this.AIButton.Size = new System.Drawing.Size(100, 23);
            this.AIButton.TabIndex = 2;
            this.AIButton.Text = "Play against AI";
            this.AIButton.UseVisualStyleBackColor = false;
            this.AIButton.Click += new System.EventHandler(this.AIButton_Click);
            // 
            // MPButton
            // 
            this.MPButton.Location = new System.Drawing.Point(118, 176);
            this.MPButton.Name = "MPButton";
            this.MPButton.Size = new System.Drawing.Size(100, 23);
            this.MPButton.TabIndex = 3;
            this.MPButton.Text = "Play Multiplayer";
            this.MPButton.UseVisualStyleBackColor = true;
            this.MPButton.Click += new System.EventHandler(this.MPButton_Click);
            // 
            // CloseButton
            // 
            this.CloseButton.Location = new System.Drawing.Point(222, 176);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(100, 23);
            this.CloseButton.TabIndex = 4;
            this.CloseButton.Text = "Exit";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(310, 158);
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // StartScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(334, 211);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.MPButton);
            this.Controls.Add(this.AIButton);
            this.Controls.Add(this.ShamelessPlug);
            this.Controls.Add(this.MainLogo);
            this.Controls.Add(this.pictureBox1);
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.MaximizeBox = false;
            this.Name = "StartScreen";
            this.Text = "Battleship";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label MainLogo;
        private System.Windows.Forms.Label ShamelessPlug;
        private System.Windows.Forms.Button AIButton;
        private System.Windows.Forms.Button MPButton;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

