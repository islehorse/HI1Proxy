namespace Hi1Proxy
{
    partial class MapForm
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
            this.draw_map = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.draw_map)).BeginInit();
            this.SuspendLayout();
            // 
            // draw_map
            // 
            this.draw_map.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.draw_map.Dock = System.Windows.Forms.DockStyle.Fill;
            this.draw_map.Location = new System.Drawing.Point(0, 0);
            this.draw_map.Name = "draw_map";
            this.draw_map.Size = new System.Drawing.Size(784, 461);
            this.draw_map.TabIndex = 0;
            this.draw_map.TabStop = false;
            this.draw_map.Click += new System.EventHandler(this.draw_map_Click);
            // 
            // MapForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 461);
            this.Controls.Add(this.draw_map);
            this.Name = "MapForm";
            this.Text = "MapForm";
            this.Load += new System.EventHandler(this.MapForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.draw_map)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.PictureBox draw_map;
    }
}