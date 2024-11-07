using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace HealthCare.Plus
{
    public partial class Receptionists : Form
    {
        public Receptionists()
        {
            InitializeComponent();
            DisplayRec();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Dulith\Documents\HealthCare.Plus.mdf;Integrated Security=True;Connect Timeout=30");
        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void Receptionists_Load(object sender, EventArgs e)
        {

        }

        private void DelBtn_Click(object sender, EventArgs e)
        {
            if (key == 0)
            {
                MessageBox.Show("Select the Receptionist");
            }
            else
            {
                // Create a custom confirmation dialog with "OK" and "Cancel" buttons
                DialogResult result = MessageBox.Show("Are you sure you want to delete this receptionist?", "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if (result == DialogResult.OK)
                {
                    try
                    {
                        Con.Open();
                        SqlCommand cmd = new SqlCommand("Delete from ReceptionistTb1 where RecepId=@Rkey", Con);

                        cmd.Parameters.AddWithValue("@RKey", key);
                        cmd.ExecuteNonQuery();
                        
                        Con.Close();
                        DisplayRec();
                        Clear();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                // If "Cancel" is clicked or the dialog is closed, do nothing (receptionist won't be deleted)
            }
        }

        private void DisplayRec()
        {
            Con.Open();
            string Query = "Select * from ReceptionistTb1";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            ReceptionistDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
       private void AddBtn_Click(object sender, EventArgs e)
{
    if (RNameTb.Text == "" || RPassword.Text == "" || RPhoneTb.Text == "" || RAddressTb.Text == "")
    {
        MessageBox.Show("Missing Information");
    }
    else
    {
        // Create a custom confirmation dialog with "OK" and "Cancel" buttons
        DialogResult result = MessageBox.Show("Do you want to add this receptionist?", "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

        if (result == DialogResult.OK)
        {
            try
            {
                Con.Open();
                SqlCommand cmd = new SqlCommand("Insert into ReceptionistTb1(RecepName,RecepPhone,RecepAdd,RecepPass)values(@RN,@RP,@RA,@RPA)", Con);
                cmd.Parameters.AddWithValue("@RN", RNameTb.Text);
                cmd.Parameters.AddWithValue("@RP", RPhoneTb.Text);
                cmd.Parameters.AddWithValue("@RA", RAddressTb.Text);
                cmd.Parameters.AddWithValue("@RPA", RPassword.Text);
                cmd.ExecuteNonQuery();
                Con.Close();
                DisplayRec();
                Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        // If "Cancel" is clicked or the dialog is closed, do nothing (receptionist won't be added)
    }
}

        int key = 0;
       
        private void RAddressTb_TextChanged(object sender, EventArgs e)
        {

            
        }


        private void RPhoneTb_TextChanged(object sender, EventArgs e)
        {

        }

       
       

        private void EditBtn_Click_1(object sender, EventArgs e)
        {
            if (RNameTb.Text == "" || RPassword.Text == "" || RPhoneTb.Text == "" || RAddressTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                // Create a custom confirmation dialog with "OK" and "Cancel" buttons
                DialogResult result = MessageBox.Show("Do you want to update this receptionist?", "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if (result == DialogResult.OK)
                {
                    try
                    {
                        Con.Open();
                        SqlCommand cmd = new SqlCommand("Update ReceptionistTb1 set RecepName=@RN, RecepPhone=@RP, RecepAdd=@RA, RecepPass=@RPA where RecepId=@RKey", Con);
                        cmd.Parameters.AddWithValue("@RN", RNameTb.Text);
                        cmd.Parameters.AddWithValue("@RP", RPhoneTb.Text);
                        cmd.Parameters.AddWithValue("@RA", RAddressTb.Text);
                        cmd.Parameters.AddWithValue("@RPA", RPassword.Text);
                        cmd.Parameters.AddWithValue("@RKey", key);
                        cmd.ExecuteNonQuery();
                        Con.Close();
                        DisplayRec();
                        Clear();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                // If "Cancel" is clicked or the dialog is closed, do nothing (receptionist won't be updated)
            }

        }


        private void ReceptionistDGV_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure a valid row is clicked
            {
                DataGridViewRow row = ReceptionistDGV.Rows[e.RowIndex];

                // Populate textboxes with data from the selected row
                RNameTb.Text = row.Cells[1].Value.ToString();
                RPhoneTb.Text = row.Cells[2].Value.ToString();
                RAddressTb.Text = row.Cells[3].Value.ToString();
                RPassword.Text = row.Cells[4].Value.ToString();

                // Set the 'key' variable to the receptionist's ID
                if (row.Cells[0].Value != null)
                {
                    key = Convert.ToInt32(row.Cells[0].Value);
                }
            }

        }
        private void Clear()
        {
            RNameTb.Text="";
            RPassword.Text = "";
            RPhoneTb.Text = "";
            RAddressTb.Text = "";
            key = 0;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ReceptionistDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
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
            Doctors obj = new Doctors();
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

        private void ResetBtn_Click(object sender, EventArgs e)
        {
            

            // Clear all other fields
            RNameTb.Text = "";
            RAddressTb.Text = "";
            RPassword.Text = "";
            RPhoneTb.Text = "";
            
        }
    }
}
