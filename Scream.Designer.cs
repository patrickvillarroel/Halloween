namespace HalloweenTown
{
    partial class Scream
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
            this.ScreamerBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.ScreamerBox)).BeginInit();
            this.SuspendLayout();
            // 
            // ScreamerBox
            // 
            this.ScreamerBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ScreamerBox.Image = global::HalloweenTown.Properties.Resources.Scream;
            this.ScreamerBox.Location = new System.Drawing.Point(0, 0);
            this.ScreamerBox.Name = "ScreamerBox";
            this.ScreamerBox.Size = new System.Drawing.Size(1283, 682);
            this.ScreamerBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ScreamerBox.TabIndex = 0;
            this.ScreamerBox.TabStop = false;
            // 
            // Scream
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1283, 682);
            this.Controls.Add(this.ScreamerBox);
            this.Name = "Scream";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Scream";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.ScreamerBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox ScreamerBox;
    }
}