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
    public partial class Doctors : Form
    {
        public Doctors()
        {
            InitializeComponent();

           

            DisplayDoc();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Dulith\Documents\HealthCare.Plus.mdf;Integrated Security=True;Connect Timeout=30");
        private void DisplayDoc()
        {
            Con.Open();
            string Query = "Select * from DoctorTb1";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            DoctorDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void Clear()
        {
            DNameTb.Text = "";
            DocPhoneTb.Text = "";
            DocAddTb.Text = "";
            DocExpTb.Text = "";
            DocPassTb.Text = "";
            DocGenCb.SelectedIndex = 0;
            DocSpecCb.SelectedIndex = 0;
            key = 0;
        }
        private void AddBtn_Click(object sender, EventArgs e)
        {
            if (DNameTb.Text == "" || DocPassTb.Text == "" || DocPhoneTb.Text == "" || DocAddTb.Text == "" || DocGenCb.SelectedIndex == -1 || DocSpecCb.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                // Create a custom dialog box with "Ok" and "Cancel" buttons
                DialogResult result = MessageBox.Show("Do you want to add this doctor?", "Confirmation", MessageBoxButtons.OKCancel);

                if (result == DialogResult.OK)
                {
                    try
                    {
                        Con.Open();
                        SqlCommand cmd = new SqlCommand("Insert into DoctorTb1(DocName,DOCDOB,DOCGEN,DOCSPEC,DOCEXP,DOCPHONE,DOCADD,DOCPASS)values(@DN,@DD,@DG,@DS,@DE,@DP,@DA,@DPA)", Con);
                        cmd.Parameters.AddWithValue("@DN", DNameTb.Text);
                        cmd.Parameters.AddWithValue("@DD", DocDOB.Value.Date);
                        cmd.Parameters.AddWithValue("@DG", DocGenCb.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@DS", DocSpecCb.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@DE", DocExpTb.Text);
                        cmd.Parameters.AddWithValue("@DP", DocPhoneTb.Text);
                        cmd.Parameters.AddWithValue("@DA", DocAddTb.Text);
                        cmd.Parameters.AddWithValue("@DPA", DocPassTb.Text);
                        cmd.ExecuteNonQuery();
                        Con.Close();
                        DisplayDoc();
                        Clear();
                    }
                    catch (Exception EX)
                    {
                        MessageBox.Show(EX.Message);
                    }
                }
                // If "Cancel" is clicked, do nothing (doctor won't be added)
            }
        }



        int key = 0;
        private void DoctorDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if a row is selected
            if (DoctorDGV.SelectedRows.Count > 0)
            {
                // Populate textboxes with data from the selected row
                DNameTb.Text = DoctorDGV.SelectedRows[0].Cells[1].Value.ToString(); // Doctor's Name
                DocDOB.Text = DoctorDGV.SelectedRows[0].Cells[2].Value.ToString(); // Doctor's Date of Birth
                DocGenCb.Text = DoctorDGV.SelectedRows[0].Cells[3].Value.ToString(); // Doctor's Gender
                DocSpecCb.Text = DoctorDGV.SelectedRows[0].Cells[4].Value.ToString(); // Doctor's Specialization
                DocExpTb.Text = DoctorDGV.SelectedRows[0].Cells[5].Value.ToString(); // Doctor's Experience
                DocPhoneTb.Text = DoctorDGV.SelectedRows[0].Cells[6].Value.ToString(); // Doctor's Phone Number
                DocAddTb.Text = DoctorDGV.SelectedRows[0].Cells[7].Value.ToString(); // Doctor's Address
                DocPassTb.Text = DoctorDGV.SelectedRows[0].Cells[8].Value.ToString(); // Doctor's Password

                // Check if a row is selected and set the 'key' value
                if (DNameTb.Text == "")
                {
                    key = 0; // Reset 'key' if no row is selected
                }
                else
                {
                    // Convert the key (usually an ID) from the DataGridView row to an integer
                    key = Convert.ToInt32(DoctorDGV.SelectedRows[0].Cells[0].Value.ToString());
                }
            }
        }


        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (key == 0)
            {
                MessageBox.Show("Select the Doctor");
            }
            else if (DNameTb.Text == "" || DocPassTb.Text == "" || DocPhoneTb.Text == "" || DocAddTb.Text == "" || DocGenCb.SelectedIndex == -1 || DocSpecCb.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                // Create a custom confirmation dialog with "OK" and "Cancel" buttons
                DialogResult result = MessageBox.Show("Are you sure you want to edit this Doctor's information?", "Confirmation", MessageBoxButtons.OKCancel);

                if (result == DialogResult.OK)
                {
                    try
                    {
                        Con.Open();
                        SqlCommand cmd = new SqlCommand("UPDATE DoctorTb1 SET DocName = @DN, DOCDOB = @DD, DOCGEN = @DG, DOCSPEC = @DS, DOCEXP = @DE, DOCPHONE = @DP, DOCADD = @DA, DOCPASS = @DPA WHERE DOCID = @DKey", Con);
                        cmd.Parameters.AddWithValue("@DN", DNameTb.Text);
                        cmd.Parameters.AddWithValue("@DD", DocDOB.Value.Date);
                        cmd.Parameters.AddWithValue("@DG", DocGenCb.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@DS", DocSpecCb.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@DE", DocExpTb.Text);
                        cmd.Parameters.AddWithValue("@DP", DocPhoneTb.Text);
                        cmd.Parameters.AddWithValue("@DA", DocAddTb.Text);
                        cmd.Parameters.AddWithValue("@DPA", DocPassTb.Text);
                        cmd.Parameters.AddWithValue("@DKey", key);

                        cmd.ExecuteNonQuery();
                       
                        Con.Close();
                        DisplayDoc();
                        Clear();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                // If "Cancel" is clicked or the dialog is closed, do nothing (doctor won't be edited)
            }
        }



        private void DelBtn_Click(object sender, EventArgs e)
        {
            if (key == 0)
            {
                MessageBox.Show("Select the Doctor");
            }
            else
            {
                // Display a confirmation dialog with "OK" and "Cancel" buttons
                DialogResult result = MessageBox.Show("Are you sure you want to delete this doctor's information?", "Confirmation", MessageBoxButtons.OKCancel);

                if (result == DialogResult.OK)
                {
                    try
                    {
                        Con.Open();
                        SqlCommand cmd = new SqlCommand("DELETE FROM DoctorTb1 WHERE DocId = @DKey", Con);
                        cmd.Parameters.AddWithValue("@DKey", key);
                        cmd.ExecuteNonQuery();
                        
                        Con.Close();
                        DisplayDoc();
                        Clear();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                // If "Cancel" or close button (cross icon) is clicked, do nothing (doctor won't be deleted)
            }
        }



        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Doctors_Load(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {
            Patients obj = new Patients();
            obj.Show();
            this.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Nurse obj = new Nurse();
            obj.Show();
            this.Hide();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            LabTests obj = new LabTests();
            obj.Show();
            this.Hide();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Receptionists obj = new Receptionists();
            obj.Show();
            this.Hide();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Login obj = new Login();
            obj.Show();
            this.Hide();
        }

        private void DocGenCb_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Disable the key press event to stop input in the middle of the ComboBox
            e.Handled = true;
        }

        private void DocSpecCb_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void DocSpecCb_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Disable the key press event to stop input in the middle of the ComboBox
            e.Handled = true;
        }

        private void DocDOB_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void DocGenCb_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Home_Click(object sender, EventArgs e)
        {
            Home obj = new Home();
            obj.Show();
            this.Hide();
        }

        private void ResetBtn_Click(object sender, EventArgs e)
        {
            // Set the ComboBox to the first item (assuming it's a gender and Specialiasion selection)
            DocGenCb.SelectedIndex = 0;
            DocSpecCb.SelectedIndex = 0;


            // Clear all other fields
            DNameTb.Text = "";
            DocExpTb.Text = "";
            DocAddTb.Text = "";
            DocPassTb.Text = "";
            DocDOB.Value = DateTime.Now; // Set the DateOfBirth picker to the current date
            DocPhoneTb.Text = "";
        }
    }
}
