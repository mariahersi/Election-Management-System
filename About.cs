using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JUSA_Election_MS
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void BTN_BACk_Click(object sender, EventArgs e)
        {
            this.Hide(); // Hide the About Form
            Admin_Dashboard dashboard = new Admin_Dashboard(); // Create an instance of Dashboard
            dashboard.Show(); // Show Dashboard
        }
    }
}
