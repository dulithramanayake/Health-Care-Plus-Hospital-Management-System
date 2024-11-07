using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HealthCare.Plus
{
    public partial class Nurse : Form
    {
        public Nurse()
        {
            InitializeComponent();
            DisplayNurse();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Dulith\Documents\HealthCare.Plus.mdf;Integrated Security=True;Connect Timeout=30");
        private void DisplayNurse()
        {
            Con.Open();
            string Query = "Select * from NurseTb1";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            NurseDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        int key = 0;
        private void Clear()
        {
            NurseNameTb.Text = "";
            NurseGenCb.SelectedIndex = 0;
            NursePhoneTb.Text = "";
            NurseAddTb.Text = "";


            key = 0;
        }

        private void Home_Click(object sender, EventArgs e)
        {
            Home obj = new Home();
            obj.Show();
            this.Hide();
        }

        private void PatientLbl_Click(object sender, EventArgs e)
        {
            Patients obj = new Patients();
            obj.Show();
            this.Hide();
        }

        private void DoctorLbl_Click(object sender, EventArgs e)
        {
            Doctors obj = new Doctors();
            obj.Show();
            this.Hide();
        }

        private void LabLbl_Click(object sender, EventArgs e)
        {
            LabTests obj = new LabTests();
            obj.Show();
            this.Hide();
        }

        private void RecepLbl_Click(object sender, EventArgs e)
        {
            Receptionists obj = new Receptionists();
            obj.Show();
            this.Hide();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            if (NurseNameTb.Text == "" || NursePhoneTb.Text == "" || NurseAddTb.Text == "" || NurseGenCb.SelectedIndex == -1 )
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                // Create a custom dialog box with "Ok" and "Cancel" buttons
                DialogResult result = MessageBox.Show("Do you want to add this Nurse?", "Confirmation", MessageBoxButtons.OKCancel);

                if (result == DialogResult.OK)
                {
                    try
                    {
                        Con.Open();
                        SqlCommand cmd = new SqlCommand("Insert into NurseTb1(NurseName,NurseDOB,NurseGen,NursePhone,NurseAdd)values(@NN,@ND,@NG,@NP,@NA)", Con);
                        cmd.Parameters.AddWithValue("@NN", NurseNameTb.Text);
                        cmd.Parameters.AddWithValue("@ND", NurseDOB.Value.Date);
                        cmd.Parameters.AddWithValue("@NG", NurseGenCb.SelectedItem.ToString());                      
                        cmd.Parameters.AddWithValue("@NP", NursePhoneTb.Text);                       
                        cmd.Parameters.AddWithValue("@NA", NurseAddTb.Text);
                        
                        cmd.ExecuteNonQuery();
                        Con.Close();
                        DisplayNurse();
                        Clear();
                    }
                    catch (Exception EX)
                    {
                        MessageBox.Show(EX.Message);
                    }
                }
                // If "Cancel" is clicked, do nothing (Nurse won't be added)
            }
        }

        private void NurseDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        { // Populate textboxes with data from the selected row
            NurseNameTb.Text = NurseDGV.SelectedRows[0].Cells[1].Value.ToString();
            NurseDOB.Text = NurseDGV.SelectedRows[0].Cells[4].Value.ToString();
            NurseGenCb.Text = NurseDGV.SelectedRows[0].Cells[2].Value.ToString();
            NurseAddTb.Text = NurseDGV.SelectedRows[0].Cells[3].Value.ToString();
            NursePhoneTb.Text = NurseDGV.SelectedRows[0].Cells[5].Value.ToString();

            // Check if a row is selected and set the 'key' value
            if (NurseNameTb.Text == "")
            {
                key = 0; // Reset 'key' if no row is selected
            }
            else
            {
                key = Convert.ToInt32(NurseDGV.SelectedRows[0].Cells[0].Value.ToString());

            }
        }


        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (key == 0)
            {
                MessageBox.Show("Select the Nurse");
            }
            else if (NurseNameTb.Text == "" || NurseAddTb.Text == "" || NursePhoneTb.Text == "" || NurseGenCb.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                // Create a custom confirmation dialog with "OK" and "Cancel" buttons
                DialogResult result = MessageBox.Show("Are you sure you want to edit this Nurse's information?", "Confirmation", MessageBoxButtons.OKCancel);

                if (result == DialogResult.OK)
                {
                    try
                    {
                        Con.Open();
                        SqlCommand cmd = new SqlCommand("UPDATE NurseTb1 SET NurseName = @NN, NurseDOB = @ND, NurseGen = @NG, NursePhone = @NP, NurseAdd = @NA WHERE NurseId = @NKey", Con);
                        cmd.Parameters.AddWithValue("@NN", NurseNameTb.Text);
                        cmd.Parameters.AddWithValue("@ND", NurseDOB.Value.Date);
                        cmd.Parameters.AddWithValue("@NG", NurseGenCb.SelectedItem.ToString());                       
                        cmd.Parameters.AddWithValue("@NP", NurseGenCb.Text);
                        cmd.Parameters.AddWithValue("@NA", NurseAddTb.Text);
                        cmd.Parameters.AddWithValue("@NKey", key);

                        cmd.ExecuteNonQuery();

                        Con.Close();
                        DisplayNurse();
                        Clear();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                // If "Cancel" is clicked or the dialog is closed, do nothing (Nurse won't be edited)
            }
        }

        private void DelBtn_Click(object sender, EventArgs e)
        {
            if (key == 0)
            {
                MessageBox.Show("Select the Nurse");
            }
            else
            {
                // Display a confirmation dialog with "OK" and "Cancel" buttons
                DialogResult result = MessageBox.Show("Are you sure you want to delete this Nurse's information?", "Confirmation", MessageBoxButtons.OKCancel);

                if (result == DialogResult.OK)
                {
                    try
                    {
                        Con.Open();
                        SqlCommand cmd = new SqlCommand("DELETE FROM NurseTb1 WHERE NurseId = @NKey", Con);
                        cmd.Parameters.AddWithValue("@NKey", key);
                        cmd.ExecuteNonQuery();

                        Con.Close();
                        DisplayNurse();
                        Clear();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                // If "Cancel" or close button (cross icon) is clicked, do nothing (Nurse won't be deleted)
            }
        }

        private void ResetBtn_Click(object sender, EventArgs e)
        {
            // Set the ComboBox to the first item (assuming it's a gender and Specialiasion selection)
            NurseGenCb.SelectedIndex = 0;
           


            // Clear all other fields
            NurseNameTb.Text = "";
            NurseAddTb.Text = "";
            NurseDOB.Value = DateTime.Now; // Set the DateOfBirth picker to the current date
            NursePhoneTb.Text = "";
        }

        private void NurseGenCb_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}
