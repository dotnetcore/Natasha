using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace WinFormsSample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }
        private void button1_Click(object sender,EventArgs e)
        {
            var result = MessageBox.Show($"请确129234主程序？{button4.Text}", "HotExecutor 提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (result.HasFlag(DialogResult.OK))
            {
                Debug.WriteLine(1112);
	        }     
        }
    }
}
