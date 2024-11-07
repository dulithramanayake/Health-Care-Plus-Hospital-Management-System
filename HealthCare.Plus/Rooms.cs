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
    public partial class Rooms : Form
    {
        public Rooms()
        {
            InitializeComponent();
            DisplayRoom();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Dulith\Documents\HealthCare.Plus.mdf;Integrated Security=True;Connect Timeout=30");
        private void DisplayRoom()
        {
            Con.Open();
            string Query = "Select * from RoomsTb1";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            RoomDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        int key = 0;
        private void Clear()
        {
            RoomNumTb.Text = "";
            RoomAva.SelectedIndex = 0;
            RoomTypeCb.SelectedIndex = 0;
            RoomPriceTb.Text = "";
            RoomBedCb.SelectedIndex = 0;

            key = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Set the ComboBox to the first item (assuming it's a gender and Specialiasion selection)
            RoomTypeCb.SelectedIndex = 0;
            RoomBedCb.SelectedIndex = 0;
            RoomAva.SelectedIndex = 0;


            // Clear all other fields
            RoomNumTb.Text = "";
            RoomPriceTb.Text = "";
            
            
        }

       

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Disable the key press event to stop input in the middle of the ComboBox
            e.Handled = true;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void DelBtn_Click(object sender, EventArgs e)
        {
            if (key == 0)
            {
                MessageBox.Show("Select the Room");
            }
            else
            {
                // Create a custom dialog box with "OK" and "Cancel" buttons
                DialogResult result = MessageBox.Show("Are you sure you want to delete this Room?", "Confirmation", MessageBoxButtons.OKCancel);

                if (result == DialogResult.OK)
                {
                    try
                    {
                        Con.Open();
                        SqlCommand cmd = new SqlCommand("Delete from RoomsTb1 where RoomId=@ROKey", Con);

                        cmd.Parameters.AddWithValue("@ROKey", key);
                        cmd.ExecuteNonQuery();
                        
                        Con.Close();
                        DisplayRoom();
                        Clear();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                // If "Cancel" is clicked or the dialog is closed, do nothing (Room won't be deleted)
            }
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            if (RoomNumTb.Text == "" || RoomPriceTb.Text == ""  || RoomTypeCb.SelectedIndex == -1 || RoomBedCb.SelectedIndex == -1 || RoomAva.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                // Create a custom dialog box with "Ok" and "Cancel" buttons
                DialogResult result = MessageBox.Show("Do you want to add this Room?", "Confirmation", MessageBoxButtons.OKCancel);

                if (result == DialogResult.OK)
                {
                    try
                    {
                        Con.Open();
                        SqlCommand cmd = new SqlCommand("Insert into RoomsTb1(RoomNum,RoomType,RoomPrice,RoomBed,RoomAva)values(@RON,@ROT,@ROP,@ROB,@ROA)", Con);
                        cmd.Parameters.AddWithValue("@RON", RoomNumTb.Text);
                        cmd.Parameters.AddWithValue("@ROT", RoomTypeCb.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@ROP", RoomPriceTb.Text);
                        cmd.Parameters.AddWithValue("@ROB", RoomBedCb.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@ROA", RoomAva.SelectedItem.ToString());
                        cmd.ExecuteNonQuery();
                        Con.Close();
                        DisplayRoom();
                        Clear();
                    }
                    catch (Exception EX)
                    {
                        MessageBox.Show(EX.Message);
                    }
                }
                // If "Cancel" is clicked, do nothing (Room won't be added)
            }
        }

        private void RoomDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Populate the textboxes and ComboBox with data from the selected DataGridView row
            RoomNumTb.Text = RoomDGV.SelectedRows[0].Cells[1].Value.ToString();
            RoomTypeCb.Text = RoomDGV.SelectedRows[0].Cells[2].Value.ToString();
            RoomPriceTb.Text = RoomDGV.SelectedRows[0].Cells[3].Value.ToString();
            RoomBedCb.Text = RoomDGV.SelectedRows[0].Cells[4].Value.ToString();
            RoomAva.Text = RoomDGV.SelectedRows[0].Cells[5].Value.ToString();


            if (RoomNumTb.Text == "")
            {
                key = 0;
            }
            else
            { // Convert the key (usually an ID) from the DataGridView row to an integer
                key = Convert.ToInt32(RoomDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (RoomNumTb.Text == "" || RoomPriceTb.Text == ""  || RoomTypeCb.SelectedIndex == -1 || RoomBedCb.SelectedIndex == -1 || RoomAva.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                // Create a custom dialog box with "OK" and "Cancel" buttons
                DialogResult result = MessageBox.Show("Are you sure you want to edit this Rooms information?", "Confirmation", MessageBoxButtons.OKCancel);

                if (result == DialogResult.OK)
                {
                    try
                    {
                        Con.Open();
                        SqlCommand cmd = new SqlCommand("Update RoomsTb1 SET RoomNum = @RON, RoomType = @ROT, RoomPrice = @ROP, RoomBed = @ROB, RoomAva = @ROA WHERE RoomId = @ROKey", Con);
                        cmd.Parameters.AddWithValue("@RON", RoomNumTb.Text);
                        cmd.Parameters.AddWithValue("@ROP", RoomPriceTb.Text);
                        cmd.Parameters.AddWithValue("@ROT", RoomTypeCb.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@ROB", RoomBedCb.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@ROA", RoomAva.SelectedItem.ToString());

                        cmd.Parameters.AddWithValue("@ROKey", key);

                        cmd.ExecuteNonQuery();
                        
                        Con.Close();
                        DisplayRoom();
                        Clear();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                // If "Cancel" is clicked or the dialog is closed, do nothing (Room won't be edited)
            }
        }

        private void Logout_Click(object sender, EventArgs e)
        {
            Login obj = new Login();
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

        private void label3_Click(object sender, EventArgs e)
        {
            Nurse obj = new Nurse();
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

        private void Home_Click(object sender, EventArgs e)
        {
            Home obj = new Home();
            obj.Show();
            this.Hide();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void RoomAva_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void RoomTypeCb_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void RoomBedCb_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
