using Natasha;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NET472Form
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            AssemblyDomain.Init();
        }
    }
}
