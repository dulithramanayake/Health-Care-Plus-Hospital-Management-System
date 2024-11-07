using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HealthCare.Plus
{
    public partial class Patients : Form
    {
        public Patients()
        {
            InitializeComponent();

            if (Login.Role == "Receptionist")
            {
                
                label11.Enabled = false;
                label12.Enabled = false;
                label5.Enabled = false;
                label3.Enabled = false;
                label4.Enabled = false;
                label13.Enabled = false;


            }
            else if(Login.Role=="Doctor")
            {
                Home.Enabled = false;
                label3.Enabled=false;
                label4.Enabled = false;
                label11.Enabled = false;
                label12.Enabled = false;
                label5.Enabled = false;
                DelBtn.Enabled = false;
                AddBtn.Enabled = false;
                EditBtn.Enabled = false;
                
            }

            DisplayPatient();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Dulith\Documents\HealthCare.Plus.mdf;Integrated Security=True;Connect Timeout=30");
        private void DisplayPatient()
        {
            Con.Open();
            string Query = "Select * from PatientTb1";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            PatientDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        int key = 0;
        private void Clear()
        {
            PatNameTb.Text = "";
            PatGenCb.SelectedIndex = 0;
            PatHIVCb.SelectedIndex = 0;
            PatAddTb.Text = "";
            PatPhoneTb.Text = "";
            PatAllTb.Text = "";
            
            key = 0;
        }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }


        private void AddBtn_Click(object sender, EventArgs e)
        {
            if (PatNameTb.Text == "" || PatAllTb.Text == "" || PatAddTb.Text == "" || PatPhoneTb.Text == "" || PatGenCb.SelectedIndex == -1 || PatHIVCb.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                // Create a custom dialog box with "Ok" and "Cancel" buttons
                DialogResult result = MessageBox.Show("Do you want to add this patient?", "Confirmation", MessageBoxButtons.OKCancel);

                if (result == DialogResult.OK)
                {
                    try
                    {
                        Con.Open();
                        SqlCommand cmd = new SqlCommand("Insert into PatientTb1(PatName, PatGen, PatDOB, PatAdd, PatPhone, PatHIV, PatAll) values(@PN, @PG, @PD, @PA, @PP, @PH, @PAL)", Con);
                        cmd.Parameters.AddWithValue("@PN", PatNameTb.Text);
                        cmd.Parameters.AddWithValue("@PD", PatDOB.Value.Date);
                        cmd.Parameters.AddWithValue("@PG", PatGenCb.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@PA", PatAddTb.Text);
                        cmd.Parameters.AddWithValue("@PP", PatPhoneTb.Text);
                        cmd.Parameters.AddWithValue("@PH", PatHIVCb.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@PAL", PatAllTb.Text);

                        cmd.ExecuteNonQuery();
                        Con.Close();
                        DisplayPatient();
                        Clear();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                // If "Cancel" is clicked, do nothing (patient won't be added)
            }

        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (PatNameTb.Text == "" || PatAllTb.Text == "" || PatPhoneTb.Text == "" || PatAddTb.Text == "" || PatGenCb.SelectedIndex == -1 || PatHIVCb.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                // Create a custom dialog box with "OK" and "Cancel" buttons
                DialogResult result = MessageBox.Show("Are you sure you want to edit this patient's information?", "Confirmation", MessageBoxButtons.OKCancel);

                if (result == DialogResult.OK)
                {
                    try
                    {
                        Con.Open();
                        SqlCommand cmd = new SqlCommand("Update PatientTb1 SET PatName = @PN, PATDOB = @PD, PATGEN = @PG, PATHIV = @PH, PATALL = @PALL, PATPHONE = @PP, PATADD = @PA WHERE PATID = @PKey", Con);
                        cmd.Parameters.AddWithValue("@PN", PatNameTb.Text);
                        cmd.Parameters.AddWithValue("@PD", PatDOB.Value.Date);
                        cmd.Parameters.AddWithValue("@PG", PatGenCb.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@PH", PatHIVCb.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@PALL", PatAllTb.Text);
                        cmd.Parameters.AddWithValue("@PP", PatPhoneTb.Text);
                        cmd.Parameters.AddWithValue("@PA", PatAddTb.Text);
                        cmd.Parameters.AddWithValue("@PKey", key);

                        cmd.ExecuteNonQuery();
                        
                        Con.Close();
                        DisplayPatient();
                        Clear();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                // If "Cancel" is clicked or the dialog is closed, do nothing (patient won't be edited)
            }
        }


        private void PatientDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Populate the textboxes and ComboBox with data from the selected DataGridView row
            PatNameTb.Text = PatientDGV.SelectedRows[0].Cells[1].Value.ToString(); 
            PatGenCb.Text = PatientDGV.SelectedRows[0].Cells[2].Value.ToString();
            PatDOB.Text = PatientDGV.SelectedRows[0].Cells[3].Value.ToString(); 
            PatAddTb.Text = PatientDGV.SelectedRows[0].Cells[4].Value.ToString(); 
            PatPhoneTb.Text = PatientDGV.SelectedRows[0].Cells[5].Value.ToString(); 
            PatHIVCb.Text = PatientDGV.SelectedRows[0].Cells[6].Value.ToString();
            PatAllTb.Text = PatientDGV.SelectedRows[0].Cells[7].Value.ToString();

           
            if (PatNameTb.Text == "")
            {
                key = 0;
            }
            else
            { // Convert the key (usually an ID) from the DataGridView row to an integer
                key = Convert.ToInt32(PatientDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void DelBtn_Click(object sender, EventArgs e)
        {
            if (key == 0)
            {
                MessageBox.Show("Select the Patient");
            }
            else
            {
                // Create a custom dialog box with "OK" and "Cancel" buttons
                DialogResult result = MessageBox.Show("Are you sure you want to delete this patient?", "Confirmation", MessageBoxButtons.OKCancel);

                if (result == DialogResult.OK)
                {
                    try
                    {
                        Con.Open();
                        SqlCommand cmd = new SqlCommand("Delete from PatientTb1 where Patid=@PKey", Con);

                        cmd.Parameters.AddWithValue("@PKey", key);
                        cmd.ExecuteNonQuery();
                        
                        Con.Close();
                        DisplayPatient();
                        Clear();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                // If "Cancel" is clicked or the dialog is closed, do nothing (patient won't be deleted)
            }


        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Patients_Load(object sender, EventArgs e)
        {

        }

        private void Home_Click(object sender, EventArgs e)
        {
            Home obj = new Home();
            obj.Show();
            this.Hide();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Login obj = new Login();
            obj.Show();
            this.Hide();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            LabTests obj = new LabTests();
            obj.Show();
            this.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Doctors obj = new Doctors();
            obj.Show();
            this.Hide();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Receptionists obj = new Receptionists();
            obj.Show();
            this.Hide();
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {

        }

        private void PatientDGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            
        }

        private void PatNameTb_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Set the ComboBox to the first item (assuming it's a gender and HIV selection)
            PatGenCb.SelectedIndex = 0;
            PatHIVCb.SelectedIndex = 0;

            // Clear all other fields
            PatNameTb.Text = "";         
            PatAllTb.Text = "";
            PatAddTb.Text = "";
            PatDOB.Value = DateTime.Now; // Set the DateOfBirth picker to the current date
            PatPhoneTb.Text = "";

        }

        private void PatGenCb_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Disable the key press event to stop input in the middle of the ComboBox
            e.Handled = true;
        }

        private void PatDOB_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }

        private void PatHIVCb_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void PatHIVCb_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Disable the key press event to stop input in the middle of the ComboBox
            e.Handled = true;
        }

        private void PatGenCb_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {
            Nurse obj = new Nurse();
            obj.Show();
            this.Hide();
        }

        private void label12_Click(object sender, EventArgs e)
        {
            Rooms obj = new Rooms();
            obj.Show();
            this.Hide();
        }

        private void label13_Click(object sender, EventArgs e)
        {
            Prescription obj = new Prescription();
            obj.Show();
            this.Hide();
        }
    }
}
