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
using System.Windows.Forms.Design;

namespace JUSA_Election_MS
{
    public partial class Manage_Candidates : Form
    {

        public Manage_Candidates()
        {
            InitializeComponent();



            //combboxgender itema
            ComBox_gender.Items.Add("Female");
            ComBox_gender.Items.Add("Male");

            //combox deperment item
            Combox_department.Items.Add("Computer And IT");
            Combox_department.Items.Add("Medicine");
            Combox_department.Items.Add("Engineer");
            Combox_department.Items.Add("Economic");
            Combox_department.Items.Add("Education");

            //comboxposition items
            Combox_position.Items.Add("President");
      


        }

        //fuction dispaly all candidates into datagridview

        public void loaddatagridV()
        {
            try
            {
                ConnStr Cons = new ConnStr();

                using (Cons.SConn = new SqlConnection(Cons.ConnString))
                {
                    Cons.SConn.Open();
                    string query = "SELECT* from Candidates ";

                    Cons.DataAdap = new SqlDataAdapter(query, Cons.SConn);

                    Cons.Dtable = new DataTable();
                    Cons.DataAdap.Fill(Cons.Dtable);
                    dgvCanditate.DataSource = Cons.Dtable;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading candidates: " + ex.Message);
                {

                }

            }
        }

        private bool ValidationCandidate()
        {

            //candidateName valide

            if (String.IsNullOrWhiteSpace(TxtCandi_name.Text))
            {
                MessageBox.Show("Please Enter Candidate Name");
                return false;
            }

            // Check if the candidates name contains only letters
            if (!Regex.IsMatch(TxtCandi_name.Text, @"^[a-zA-Z\s]+$"))
            {
                MessageBox.Show("candidate Name must contain only letters.");
                return false;
            }


            //  Only letters before '@', and a valid format
            if (!Regex.IsMatch(Txt_Email.Text, @"^[a-zA-Z]+@[a-zA-Z]+\.[a-zA-Z]+$"))
            {
                MessageBox.Show("Invalid Email! Email should contain only letters @ and . ).");
                return false;
            }
            ///Phonenumber validatin
            if (Txt_phone_number.Text.Length < 7 || Txt_phone_number.Text.Length > 10 || !int.TryParse(Txt_phone_number.Text, out int phone_number))
            {

                MessageBox.Show("Please Phone Number must be Contain  7 and 10 Numbers");
                return false;


            }

            // Validate Department
            if (Combox_department.SelectedItem == null || Combox_department.SelectedItem.ToString() != "Computer And IT" && Combox_department.SelectedItem.ToString() != "Medicine"
                 && Combox_department.SelectedItem.ToString() != "Engineer" && Combox_department.SelectedItem.ToString() != "Economic" && Combox_department.SelectedItem.ToString() != "Education")
            {
                MessageBox.Show("Please Selecet Department");
                return false;
            }

            //Validate gender

            if (ComBox_gender.SelectedItem == null || ComBox_gender.SelectedItem.ToString() != "Female" && ComBox_gender.SelectedItem.ToString() != "Male")
            {

                MessageBox.Show("please Select Female Or Male");
                return false;
            }

            //validate position 
            if (Combox_position.SelectedItem == null || Combox_position.SelectedItem.ToString() != "President") 
            { 
                MessageBox.Show("Please Select the position");
                return false;
            }

            //valide candidate vision

            if (string.IsNullOrWhiteSpace(RTxt_Candi_Vision.Text))
            {
                MessageBox.Show("Please Enter Candidate Vision");
                return false;
            }


            // Check if the candidates vision contains only letters
            if (!Regex.IsMatch(RTxt_Candi_Vision.Text, @"^[a-zA-Z\s]+$"))
            {
                MessageBox.Show("Candidates Vision must be only letters.");
                return false;
            }

            return true;
        }

        private void BTN_candidate_Add_Click(object sender, EventArgs e)
        {
            ConnStr ConnS = new ConnStr();

            if (!ValidationCandidate())
            {
                return;// stop excute if valid
            }

            try
            {
                using (ConnS.SConn = new SqlConnection(ConnS.ConnString))
                {
                    String query = "insert into Candidates Values (@CandidateName,@Email,@PhoneNumber,@Gender,@Department,@PositionRunningFor,@CandidatesVision, @VoteCount)";
                    ConnS.SCommand = new SqlCommand(query, ConnS.SConn);
                    ConnS.SCommand.Parameters.AddWithValue("@CandidateName", TxtCandi_name.Text);
                    ConnS.SCommand.Parameters.AddWithValue("@Email", Txt_Email.Text);
                    ConnS.SCommand.Parameters.AddWithValue("@PhoneNumber", Txt_phone_number.Text);
                    ConnS.SCommand.Parameters.AddWithValue("@Gender", ComBox_gender.Text);
                    ConnS.SCommand.Parameters.AddWithValue("@Department", Combox_department.Text);
                    ConnS.SCommand.Parameters.AddWithValue("@PositionRunningFor", Combox_position.Text);
                    ConnS.SCommand.Parameters.AddWithValue("@CandidatesVision", RTxt_Candi_Vision.Text);
                    // Add parameter for the VoteCount column (default to 0 or any initial value)
                    ConnS.SCommand.Parameters.AddWithValue("@VoteCount", 0); // Or another initial value



                    ConnS.SConn.Open();
                    ConnS.SCommand.ExecuteNonQuery();

                    MessageBox.Show("Seccessfull Addition");

                    //clear automatally
                    TxtCandi_name.Text = string.Empty;
                    Txt_Email.Text = string.Empty;
                    Txt_phone_number.Text = string.Empty;
                    RTxt_Candi_Vision.Text = string.Empty;
                    Text_Candi_ID.Text = string.Empty;
                    Combox_department.SelectedIndex = -1;
                    ComBox_gender.SelectedIndex = -1;
                    Combox_position.SelectedIndex = -1;




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



        private void BTN_all_Can_Informarion_Click(object sender, EventArgs e)
        {
            loaddatagridV();
        }

        private void Manage_Candidates_Load(object sender, EventArgs e)
        {

        }

        //fuction search candidates information by ID or NAME
        public void LoadCandidate(string searchValue)
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
                    if (int.TryParse(searchValue, out int candidateID))
                    {
                        query = "SELECT * FROM Candidates WHERE CandidateID = @CandidateID";
                        Cons.SCommand = new SqlCommand(query, Cons.SConn);
                        Cons.SCommand.Parameters.AddWithValue("@CandidateID", candidateID);
                    }
                    else
                    {
                        query = "SELECT * FROM Candidates WHERE CandidateName LIKE @CandidateName";
                        Cons.SCommand = new SqlCommand(query, Cons.SConn);
                        Cons.SCommand.Parameters.AddWithValue("@CandidateName", "%" + searchValue + "%");
                    }

                    Cons.DataAdap = new SqlDataAdapter(Cons.SCommand);
                    Cons.Dtable = new DataTable();
                    Cons.DataAdap.Fill(Cons.Dtable);

                    dgvCanditate.DataSource = Cons.Dtable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading candidate: " + ex.Message);
            }
        }

        //btn search 
        private void Btn_search_Click(object sender, EventArgs e)
        {

            string searchValue = text_search.Text.Trim();

            if (!string.IsNullOrEmpty(searchValue))
            {
                LoadCandidate(searchValue);

                //clear automatally
                text_search.Text = string.Empty;
            }
            else
            {
                MessageBox.Show("Please enter a Candidate ID or Name ");
            }

        }
        private void BTN_Candidate_Update_Click(object sender, EventArgs e)
        {
            if (!ValidationCandidate())
            {
                return;// stop excute if valid
            }
            try
            {
                ConnStr ConnS = new ConnStr();
                using (ConnS.SConn = new SqlConnection(ConnS.ConnString))
                {
                    String query = "update Candidates set CandidateName=@CandidateName,Email=@Email,PhoneNumber=@PhoneNumber,Gender=@Gender,Department=@Department,PositionRunningFor=@PositionRunningFor,CandidatesVision=@CandidatesVision where  CandidateID=@CandidateID";
                    ConnS.SCommand = new SqlCommand(query, ConnS.SConn);
                    ConnS.SCommand.Parameters.AddWithValue("CandidateName", TxtCandi_name.Text);
                    ConnS.SCommand.Parameters.AddWithValue("Email", Txt_Email.Text);
                    ConnS.SCommand.Parameters.AddWithValue("PhoneNumber", Txt_phone_number.Text);
                    ConnS.SCommand.Parameters.AddWithValue("Gender", ComBox_gender.Text);
                    ConnS.SCommand.Parameters.AddWithValue("Department", Combox_department.Text);
                    ConnS.SCommand.Parameters.AddWithValue("PositionRunningFor", Combox_position.Text);
                    ConnS.SCommand.Parameters.AddWithValue("CandidatesVision ", RTxt_Candi_Vision.Text);
                    ConnS.SCommand.Parameters.AddWithValue("CandidateID", Text_Candi_ID.Text);

                    ConnS.SConn.Open();
                    ConnS.SCommand.ExecuteNonQuery();

                    MessageBox.Show("Seccessfull Update");
                    loaddatagridV();

                    //clear automatally
                    TxtCandi_name.Text = string.Empty;
                    Txt_Email.Text = string.Empty;
                    Txt_phone_number.Text = string.Empty;
                    RTxt_Candi_Vision.Text = string.Empty;
                    Text_Candi_ID.Text = string.Empty;
                    Combox_department.SelectedIndex = -1;
                    ComBox_gender.SelectedIndex = -1;
                    Combox_position.SelectedIndex = -1;

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
            {

            }
        }
        //method select data to make update
        private void dgvCanditate_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //fill data from selected row
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCanditate.Rows[e.RowIndex];
                Text_Candi_ID.Text = row.Cells["CandidateID"].Value.ToString();
                TxtCandi_name.Text = row.Cells["CandidateName"].Value.ToString();
                Txt_Email.Text = row.Cells["Email"].Value.ToString();
                Txt_phone_number.Text = row.Cells["PhoneNumber"].Value.ToString();
                Combox_department.Text = row.Cells["Department"].Value.ToString();
                ComBox_gender.Text = row.Cells["Gender"].Value.ToString();
                Combox_position.Text = row.Cells["PositionRunningFor"].Value.ToString();
                RTxt_Candi_Vision.Text = row.Cells["CandidatesVision"].Value.ToString();

            }
        }

        private void BTN_Candidate__Delete_Click(object sender, EventArgs e)
        {
            ConnStr cons = new ConnStr();
            using (cons.SConn = new SqlConnection(cons.ConnString))
            {
                try
                {
                    cons.SConn.Open();
                    //retrieve data from select candidate id

                    int CandidateId = Convert.ToInt32(dgvCanditate.SelectedRows[0].Cells["CandidateID"].Value);
                    // confirm delete

                    DialogResult delete = MessageBox.Show("Are you sure to delete this candidate information", "Confrim delete", MessageBoxButtons.YesNo);
                    if (delete == DialogResult.Yes)
                    {
                        string query = "delete from Candidates where CandidateID=@CandidateID";
                        cons.SCommand = new SqlCommand(query, cons.SConn);
                        cons.SCommand.Parameters.AddWithValue("@CandidateID", CandidateId);
                        cons.SCommand.ExecuteNonQuery();
                        MessageBox.Show("Succesfull delete");

                        loaddatagridV();
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Error database", ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error", ex.Message);

                }

            }
        }

        private void BTN_BackToDashboard_Click(object sender, EventArgs e)
        {
            this.Hide(); // Hide the About Form
            Admin_Dashboard dashboard = new Admin_Dashboard(); // Create an instance of Dashboard
            dashboard.Show(); // Show Dashboard
        }

        private void guna2HtmlLabel2_Click(object sender, EventArgs e)
        {

        }

        private void Combox_position_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
