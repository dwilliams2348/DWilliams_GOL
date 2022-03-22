
namespace DWilliams_GOL
{
    partial class ModalDialog
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.yUniverse = new System.Windows.Forms.TextBox();
            this.xUniverse = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.timerInterval = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(12, 415);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(113, 27);
            this.button1.TabIndex = 0;
            this.button1.Text = "Accept";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(181, 415);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(113, 27);
            this.button2.TabIndex = 1;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(1, 1);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(304, 20);
            this.textBox1.TabIndex = 2;
            this.textBox1.Text = "X-Dimension for the Universe:";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(1, 88);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(304, 20);
            this.textBox2.TabIndex = 4;
            this.textBox2.Text = "Y-Dimension for the Universe:";
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // yUniverse
            // 
            this.yUniverse.Location = new System.Drawing.Point(106, 114);
            this.yUniverse.Name = "yUniverse";
            this.yUniverse.Size = new System.Drawing.Size(93, 20);
            this.yUniverse.TabIndex = 5;
            this.yUniverse.Text = "25";
            this.yUniverse.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.yUniverse.TextChanged += new System.EventHandler(this.yUniverse_TextChanged);
            // 
            // xUniverse
            // 
            this.xUniverse.Location = new System.Drawing.Point(106, 27);
            this.xUniverse.Name = "xUniverse";
            this.xUniverse.Size = new System.Drawing.Size(93, 20);
            this.xUniverse.TabIndex = 6;
            this.xUniverse.Text = "25";
            this.xUniverse.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.xUniverse.TextChanged += new System.EventHandler(this.xUniverse_TextChanged);
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(1, 176);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(304, 20);
            this.textBox3.TabIndex = 7;
            this.textBox3.Text = "Time between generations(milliseconds):";
            this.textBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // timerInterval
            // 
            this.timerInterval.Location = new System.Drawing.Point(106, 202);
            this.timerInterval.Name = "timerInterval";
            this.timerInterval.Size = new System.Drawing.Size(93, 20);
            this.timerInterval.TabIndex = 8;
            this.timerInterval.Text = "100";
            this.timerInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.timerInterval.TextChanged += new System.EventHandler(this.timerInterval_TextChanged);
            // 
            // ModalDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(306, 454);
            this.Controls.Add(this.timerInterval);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.xUniverse);
            this.Controls.Add(this.yUniverse);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ModalDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ModalDialog";
            this.Load += new System.EventHandler(this.ModalDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox yUniverse;
        private System.Windows.Forms.TextBox xUniverse;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox timerInterval;
    }
}