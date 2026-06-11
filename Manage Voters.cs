using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JUSA_Election_MS
{
    public partial class Manage_Voters : Form
    {
        public Manage_Voters()
        {
            InitializeComponent();

            ComboBox_Gender.Items.Add("Female");
            ComboBox_Gender.Items.Add("Male");

            Combox_department.Items.Add("Computer And IT");
            Combox_department.Items.Add("Medicine");
            Combox_department.Items.Add("Engineer");
            Combox_department.Items.Add("Economic");
            Combox_department.Items.Add("Education");


        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Manage_Voters_Load(object sender, EventArgs e)
        {

        }

        //validation voters fuction
        private bool Validationvoter()
        {

            //voterName valide

            if (String.IsNullOrWhiteSpace(Text_Voter_Name.Text))
            {
                MessageBox.Show("Please Enter Voter Name");
                return false;
            }
            // Check if the voter name contains only letters
            if (!Regex.IsMatch(Text_Voter_Name.Text, @"^[a-zA-Z\s]+$"))
            {
                MessageBox.Show("Voter Name must contain only letters.");
                return false;
            }
            /// Email Validate

            //  Only letters before '@', and a valid format
            if (!Regex.IsMatch(Text_Email.Text, @"^[a-zA-Z]+@[a-zA-Z]+\.[a-zA-Z]+$"))
            {
                MessageBox.Show("Invalid Email! Email should contain only letters @ and . ).");
                return false;
            }
            ///Phonenumber validatin
            if (Text_Phone_Number.Text.Length < 7 || Text_Phone_Number.Text.Length > 10 || !int.TryParse(Text_Phone_Number.Text, out int phone_number))
            {

                MessageBox.Show("Please Phone Number must be Contain 7 and 10 Numbers");
                return false;
            }

            // Validate Department
            if (Combox_department.SelectedItem == null || Combox_department.SelectedItem.ToString() != "Computer And IT" && Combox_department.SelectedItem.ToString() != "Medicine"
                 && Combox_department.SelectedItem.ToString() != "Engineer" && Combox_department.SelectedItem.ToString() != "Economic" && Combox_department.SelectedItem.ToString() != "Education")
            {
                MessageBox.Show("Please Selecet Department");
                return false;
            }

            //Validate Departments

            if (ComboBox_Gender.SelectedItem == null || ComboBox_Gender.SelectedItem.ToString() != "Female" && ComboBox_Gender.SelectedItem.ToString() != "Male")
            {

                MessageBox.Show("please Select Female Or Male");
                return false;
            }

            return true;
        }


        // search voter information by id or name into datagridview
        public void LoadVoters(string searchValue)
        {
            try
            {
                ConnStr Cons = new ConnStr();

                using (Cons.SConn = new SqlConnection(Cons.ConnString))
                {
                    Cons.SConn.Open();

                    string query;
                    Cons.SCommand = new SqlCommand();

                    // Check if input is a number (assume it's an ID), otherwise treat as name
                    if (int.TryParse(searchValue, out int VoterID))
                    {
                        query = "SELECT * FROM Voters WHERE VoterID = @VoterID";
                        Cons.SCommand = new SqlCommand(query, Cons.SConn);
                        Cons.SCommand.Parameters.AddWithValue("@VoterID", VoterID);
                    }
                    else
                    {
                        query = "SELECT * FROM Voters WHERE VoterName LIKE @VoterName";
                        Cons.SCommand = new SqlCommand(query, Cons.SConn);
                        Cons.SCommand.Parameters.AddWithValue("@VoterName", "%" + searchValue + "%");
                    }

                    Cons.DataAdap = new SqlDataAdapter(Cons.SCommand);
                    Cons.Dtable = new DataTable();
                    Cons.DataAdap.Fill(Cons.Dtable);

                    dgridViewVoters.DataSource = Cons.Dtable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading Voters: " + ex.Message);
            }
        }


        //Add voters add voter

        private void BTN_Voter_Add_Click(object sender, EventArgs e)
        {
            ConnStr ConnS = new ConnStr();

            if (!Validationvoter())
            {
                return;// stop excute if valid
            }

            try
            {
                using (ConnS.SConn = new SqlConnection(ConnS.ConnString))
                {
                    String query = "insert into Voters Values (@VoterName,@VoterEmail,@VoterPhoneNumber,@VoterGender,@VoterDepartment)";
                    ConnS.SCommand = new SqlCommand(query, ConnS.SConn);
                    ConnS.SCommand.Parameters.AddWithValue("@VoterName", Text_Voter_Name.Text);
                    ConnS.SCommand.Parameters.AddWithValue("@VoterEmail", Text_Email.Text);
                    ConnS.SCommand.Parameters.AddWithValue("@VoterPhoneNumber", Text_Phone_Number.Text);
                    ConnS.SCommand.Parameters.AddWithValue("@VoterGender", ComboBox_Gender.Text);
                    ConnS.SCommand.Parameters.AddWithValue("@VoterDepartment", Combox_department.Text);

                    ConnS.SConn.Open();
                    ConnS.SCommand.ExecuteNonQuery();

                    MessageBox.Show("Seccessfull Addition");

                    //clear automatally
                    Text_Voter_ID.Text = string.Empty;
                    Text_Voter_Name.Text = string.Empty;
                    Text_Email.Text = string.Empty;
                    Text_Phone_Number.Text = string.Empty;
                    ComboBox_Gender.SelectedIndex = -1;
                    Combox_department.SelectedIndex = -1;

                }

            }
            catch (SqlException ex)
            {
                MessageBox.Show(" database Error", ex.Message);

            }
            catch (Exception ex)
            {

                MessageBox.Show("Error", ex.Message);
            }

        }

        private void BTN_BackDash_Click(object sender, EventArgs e)
        {
            this.Hide(); // Hide the About Form
            Admin_Dashboard dashboard = new Admin_Dashboard(); // Create an instance of Dashboard
            dashboard.Show(); // Show Dashboard
        }

        //method display all voter information into datagrid view
        public void loaddatagridV()
        {
            try
            {
                ConnStr Cons = new ConnStr();

                using (Cons.SConn = new SqlConnection(Cons.ConnString))
                {
                    Cons.SConn.Open();
                    string query = "SELECT* from Voters ";

                    Cons.DataAdap = new SqlDataAdapter(query, Cons.SConn);

                    Cons.Dtable = new DataTable();
                    Cons.DataAdap.Fill(Cons.Dtable);
                    dgridViewVoters.DataSource = Cons.Dtable;

                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error loading candidates: ", ex.Message);
            }


            catch (Exception ex)
            {
                MessageBox.Show("Error", ex.Message);

            }
        

            
        }

        private void BTN_all_Voter_Informarion_Click(object sender, EventArgs e)
        {
            loaddatagridV();

        }

        private void Btn_search_Click(object sender, EventArgs e)
        {
            string searchValue = text_search.Text.Trim();

            if (!string.IsNullOrEmpty(searchValue))
            {
                LoadVoters(searchValue);
                text_search.Text =string.Empty;
            }
            else
            {
                MessageBox.Show("Please enter a Voter ID or Name ");
            }
        }

        private void dgridViewVoters_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //fill data from selected row
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgridViewVoters.Rows[e.RowIndex];
                Text_Voter_ID.Text = row.Cells["VoterID"].Value.ToString();
                Text_Voter_Name.Text = row.Cells["VoterName"].Value.ToString();
                Text_Email.Text = row.Cells["VoterEmail"].Value.ToString();
                Text_Phone_Number.Text = row.Cells["VoterPhoneNumber"].Value.ToString();
                Combox_department.Text = row.Cells["VoterDepartment"].Value.ToString();
                ComboBox_Gender.Text = row.Cells["VoterGender"].Value.ToString();


            }
        }

        private void BTN_Voter_Update_Click(object sender, EventArgs e)
        {
            if (!Validationvoter())
            {
                return;// stop excute if valid
            }
            try
            {
                ConnStr ConnS = new ConnStr();
                using (ConnS.SConn = new SqlConnection(ConnS.ConnString))
                {
                    String query = "update Voters set VoterName=@VoterName,VoterEmail=@VoterEmail,VoterPhoneNumber=@VoterPhoneNumber,VoterGender=@VoterGender,VoterDepartment=@VoterDepartment where VoterID=@VoterID";
                    ConnS.SCommand = new SqlCommand(query, ConnS.SConn);
                    ConnS.SCommand.Parameters.AddWithValue("VoterName", Text_Voter_Name.Text);
                    ConnS.SCommand.Parameters.AddWithValue("VoterEmail", Text_Email.Text);
                    ConnS.SCommand.Parameters.AddWithValue("VoterPhoneNumber", Text_Phone_Number.Text);
                    ConnS.SCommand.Parameters.AddWithValue("VoterGender", ComboBox_Gender.Text);
                    ConnS.SCommand.Parameters.AddWithValue("VoterDepartment", Combox_department.Text);
                    ConnS.SCommand.Parameters.AddWithValue("VoterID ", Text_Voter_ID.Text);

                    ConnS.SConn.Open();
                    ConnS.SCommand.ExecuteNonQuery();

                    MessageBox.Show("Seccessfull Update");
                    loaddatagridV();


                    //clear automatally
                    Text_Voter_ID.Text = string.Empty;
                    Text_Voter_Name.Text = string.Empty;
                    Text_Email.Text = string.Empty;
                    Text_Phone_Number.Text = string.Empty;
                    ComboBox_Gender.SelectedIndex = -1;
                    Combox_department.SelectedIndex = -1;

                }
            }

            catch (SqlException ex)
            {
                MessageBox.Show(" Database Error", ex.Message);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error", ex.Message);
            }
        }
    }
}
