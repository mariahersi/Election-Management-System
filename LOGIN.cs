
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JUSA_Election_MS
{
    public partial class LOGIN : Form
    {
        public LOGIN()
        {
            InitializeComponent();
            TextBox_password.PasswordChar = '*';
        }
        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

        }
        private void BTN_Login_Click(object sender, EventArgs e)

        {
            string username = Textbox_user.Text.Trim();
            string password = TextBox_password.Text.Trim();

            ConnStr Cons = new ConnStr();

            if (String.IsNullOrEmpty(Textbox_user.Text) || String.IsNullOrEmpty(TextBox_password.Text))
            {


                MessageBox.Show("Both Username and Password is required");
                return;
            }
            try
            {
                using (Cons.SConn = new SqlConnection(Cons.ConnString))
                {
                    String query = "Select role from Users where Username=@UserName and UserPassword=@UserPassword";
                    Cons.SCommand = new SqlCommand(query, Cons.SConn);
                    Cons.SCommand.Parameters.AddWithValue("@UserName", username);
                    Cons.SCommand.Parameters.AddWithValue("@UserPassword", password);
                    Cons.SConn.Open();

                    object result = Cons.SCommand.ExecuteScalar();

                    if (result!=null)
                    {
                        string role = result.ToString();
                        MessageBox.Show("Welcome Back");
                        new Admin_Dashboard().Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password");
                    }
                }
            }
            catch (SqlException EX)
            {
                MessageBox.Show("Database error" + EX.Message);
            }
        }

        private void BTN_Sign_Up_Click(object sender, EventArgs e)
        {
            ConnStr Cons = new ConnStr();
            if (String.IsNullOrEmpty(Textbox_user.Text) || String.IsNullOrEmpty(TextBox_password.Text))
            {

                MessageBox.Show("Both Username and Password is required");
                return;
            }
            try
            {
                using (Cons.SConn = new SqlConnection(Cons.ConnString))
                {
                    String query = "INSERT INTO Users (UserName, UserPassword) VALUES (@UserName, @UserPassword)";
                    Cons.SCommand = new SqlCommand(query, Cons.SConn);
                    Cons.SCommand.Parameters.AddWithValue("@UserName", Textbox_user.Text);
                    Cons.SCommand.Parameters.AddWithValue("@UserPassword", TextBox_password.Text);
                    Cons.SConn.Open();
                    Cons.SCommand.ExecuteNonQuery();
                }
                MessageBox.Show("Sign up successful!");
                this.Close();
            }
            catch (SqlException EX)
            {
                MessageBox.Show("Database error" + EX.Message);
            }
        }

        private void CHBox_show_pass_CheckedChanged(object sender, EventArgs e)
        {
            if (CHBox_show_pass.Checked)
            {
                TextBox_password.PasswordChar = '\0'; // Show password
            }
            else
            {
                TextBox_password.PasswordChar = '*'; // Hide password
            }

        }

        private void LOGIN_Load(object sender, EventArgs e)
        {

        }
    }
}

        
    


