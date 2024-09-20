using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsSample
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
        private Button button1;
        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "Form1";

            button1 = new Button();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(305, 240);
            button1.Name = "button1";
            button1.Size = new Size(197, 23);
            button1.TabIndex = 0;
            button1.Text = "b115";
            button1.UseVisualStyleBackColor = true;
            button1.Click += Button1_Click;

            Controls.Add(button1);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
        }

        private void Button1_Click(object sender, System.EventArgs e)
        {
            var result = MessageBox.Show("请111222？", "HotExecutor", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (result.HasFlag(DialogResult.OK))
            {
                Debug.WriteLine(this.GetHashCode());
            }
        }

        #endregion
    }
}
