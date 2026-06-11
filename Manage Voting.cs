using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace JUSA_Election_MS
{
    public partial class Manage_Voting : Form
    {
        public Manage_Voting()
        {
            InitializeComponent();

            LoadCandidates();
            LoadVotersID();

        }
        private void Manage_Voting_Load(object sender, EventArgs e)
        {

        }
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
                        query = "SELECT CandidateName,Department, CandidatesVision FROM Candidates WHERE CandidateID = @CandidateID";
                        Cons.SCommand = new SqlCommand(query, Cons.SConn);
                        Cons.SCommand.Parameters.AddWithValue("@CandidateID", candidateID);
                    }
                    else
                    {
                        query = "SELECT CandidateName,Department,CandidatesVision,FROM Candidates WHERE CandidateName LIKE @CandidateName";
                        Cons.SCommand = new SqlCommand(query, Cons.SConn);
                        Cons.SCommand.Parameters.AddWithValue("@CandidateName", "%" + searchValue + "%");
                    }

                    Cons.DataAdap = new SqlDataAdapter(Cons.SCommand);
                    Cons.Dtable = new DataTable();
                    Cons.DataAdap.Fill(Cons.Dtable);

                    dgvCandidate.DataSource = Cons.Dtable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading candidate: " + ex.Message);
            }
        }

        private void BTN_all_Can_Informarion_Click(object sender, EventArgs e)
        {
            string searchValue = Text_Search.Text.Trim();

            if (!string.IsNullOrEmpty(searchValue))
            {
                LoadCandidate(searchValue);
            }
            else
            {
                MessageBox.Show("Please enter a Candidate ID or Name.");
            }

        }

        private void LoadCandidates()
        {
            try

            {
                ConnStr Cons = new ConnStr();

                // Create a connection to the database
                using (Cons.SConn = new SqlConnection(Cons.ConnString))
                {
                    // Open the connection
                    Cons.SConn.Open();

                    string query = "SELECT CandidateID FROM Candidates ";
                    Cons.DataAdap = new SqlDataAdapter(query, Cons.SConn);
                    Cons.Dtable = new DataTable();
                    Cons.DataAdap.Fill(Cons.Dtable);

                    Combox_Candidates.DataSource = Cons.Dtable;
                    Combox_Candidates.DisplayMember = "CandidateID";
                    Combox_Candidates.ValueMember = "CandidateID";
                }

            }
            catch (SqlException ex)
            {
                // Handle any errors
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private void Combox_Candidates_SelectedIndexChanged(object sender, EventArgs e)
        {
            Combox_Candidates.SelectedItem.ToString();


        }

        private void LoadVotersID()
        {
            try
            {
                ConnStr Cons = new ConnStr();

                // Create a connection to the database
                using (Cons.SConn = new SqlConnection(Cons.ConnString))
                {
                    // Open the connection
                    Cons.SConn.Open();

                    string query = "SELECT VoterID FROM Voters";
                    Cons.DataAdap = new SqlDataAdapter(query, Cons.SConn);
                    Cons.Dtable = new DataTable();
                    Cons.DataAdap.Fill(Cons.Dtable);

                    Combo_voter_id.DataSource = Cons.Dtable;
                    Combo_voter_id.DisplayMember = "VoterID";
                    Combo_voter_id.ValueMember = "VoterID";


                }
            }
            catch (SqlException ex)
            {
                // Handle any errors
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void Combo_voter_id_SelectedIndexChanged(object sender, EventArgs e)
        {
            Combo_voter_id.SelectedItem.ToString();
        }





        private void Combox_Search_SelectedIndexChanged(object sender, EventArgs e)
        {


        }
        //dislay candidates informatii=on into datagridview
        public void loaddatagridV()
        {
            try
            {
                ConnStr Cons = new ConnStr();

                using (Cons.SConn = new SqlConnection(Cons.ConnString))
                {
                    Cons.SConn.Open();
                    string query = "SELECT CandidateName,Department,PositionRunningFor, CandidatesVision from Candidates  ";


                    Cons.DataAdap = new SqlDataAdapter(query, Cons.SConn);

                    Cons.Dtable = new DataTable();
                    Cons.DataAdap.Fill(Cons.Dtable);
                    dgvCandidate.DataSource = Cons.Dtable;

                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Database Error loading candidates ", ex.Message);
            }


            catch (Exception ex)
            {
                MessageBox.Show("Error", ex.Message);

            }

        }
        private void BTN_View_All_Click(object sender, EventArgs e)
        {
            loaddatagridV();
        }

        private void BTN_Sumit_Vote_Click(object sender, EventArgs e)
        {
            int voterId = Convert.ToInt32(Combo_voter_id.SelectedValue);
            int candidateId = Convert.ToInt32(Combox_Candidates.SelectedValue);


            ConnStr ConnS = new ConnStr();
            try
            {
                using (ConnS.SConn = new SqlConnection(ConnS.ConnString))
                {
                    //check if voter Voted
                    String CheckVoteQuery = "Select count(*) from Votes WHERE VoterID = @VoterID";
                    ConnS.SCommand = new SqlCommand(CheckVoteQuery, ConnS.SConn);
                    ConnS.SCommand.Parameters.AddWithValue("@VoterID", voterId);
                    ConnS.SConn.Open();
                    int voteCount = (int)ConnS.SCommand.ExecuteScalar();

                    if (voteCount > 0)
                    {
                        MessageBox.Show("You have already voted!");
                        return;
                    }
                    //insert voter voting
                    string voteQuery = "INSERT INTO Votes (VoterID, CandidateID) VALUES (@VoterID, @CandidateID)";
                    ConnS.SCommand = new SqlCommand(voteQuery, ConnS.SConn);
                    ConnS.SCommand.Parameters.AddWithValue("@VoterID", voterId);
                    ConnS.SCommand.Parameters.AddWithValue("@CandidateID", candidateId);
                    ConnS.SCommand.ExecuteNonQuery();

                    //update votes

                    // Update VoteCount in Candidates table
                    string updateQuery = "UPDATE Candidates SET VoteCount = VoteCount + 1 WHERE CandidateID = @CandidateID";
                    SqlCommand updateCmd = new SqlCommand(updateQuery, ConnS.SConn);
                    updateCmd.Parameters.AddWithValue("@CandidateID", candidateId);
                    updateCmd.ExecuteNonQuery();

                    MessageBox.Show("Vote submitted successfully!", "Success");

                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error", ex.Message);

            }
        }

        private void BTN_BackToDashboard_Click(object sender, EventArgs e)
        {
            this.Hide(); // Hide the About Form
            Admin_Dashboard dashboard = new Admin_Dashboard(); // Create an instance of Dashboard
            dashboard.Show(); // Show Dashboard
        }

        private void Text_Search_TextChanged(object sender, EventArgs e)
        {

        }

        public void ViewResults()
        {
            try
            {
                ConnStr Cons = new ConnStr();

                using (Cons.SConn = new SqlConnection(Cons.ConnString))
                {
                    Cons.SConn.Open();
                    string query = "SELECT  CandidateID,CandidateName,VoteCount from Candidates ";


                    Cons.DataAdap = new SqlDataAdapter(query, Cons.SConn);

                    Cons.Dtable = new DataTable();
                    Cons.DataAdap.Fill(Cons.Dtable);
                    DgridView_Results.DataSource = Cons.Dtable;

                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Database Error loading candidates ", ex.Message);
            }
        }

        private void BTN_View_Results_Click(object sender, EventArgs e)
        {
            ViewResults();
        }

        private void DgridView_Results_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void dgvCandidate_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
} 

