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

    public partial class Admin_Dashboard : Form
    {
        public Admin_Dashboard()
        {
            InitializeComponent();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {

        }
        private void BTN_Candidates_Click(object sender, EventArgs e)

        {
            this.Hide();
            Manage_Candidates manage_Candidates = new Manage_Candidates();
            manage_Candidates.Show();

        }

        private void BTN_Voters_Click(object sender, EventArgs e)
        {
            this.Hide();
            Manage_Voters Manage_Voters = new Manage_Voters();
            Manage_Voters.Show();
        }

        private void BTN_Voting_Click(object sender, EventArgs e)
        {
            this.Hide();
            Manage_Voting Manage_Voting = new Manage_Voting();
            Manage_Voting.Show();
        }

        private void BTN_About_Click(object sender, EventArgs e)
        {
            this.Hide();
            About about = new About();
            about.Show();
        }

        private void BTN_LOG_out_Click(object sender, EventArgs e)
        {
            // Show a confirmation message before logging out
            DialogResult result = MessageBox.Show("Are you sure you want to log out?", "Logout Confirmation",
                                                  MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Hide(); // Hide the Dashboard Form
                LOGIN login = new LOGIN(); // Create an instance of the Login Form
                login.Show(); // Show the Login Form
            }
        }

        private void Admin_Dashboard_Load(object sender, EventArgs e)
        {

        }
    }
}
