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
    public partial class LabTests : Form
    {
        public LabTests()
        {
            InitializeComponent();
            DisplayTest();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Dulith\Documents\HealthCare.Plus.mdf;Integrated Security=True;Connect Timeout=30");
        private void DisplayTest()
        {
            Con.Open();
            string Query = "Select * from TestTb1";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            LabDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        int key = 0;
        private void Clear()
        {
            LabTestTb.Text = "";
            LabCostTb.Text = "";

            key = 0;
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            try
            {
                // Create a custom dialog box with "OK" and "Cancel" buttons
                DialogResult result = MessageBox.Show("Are you sure you want to Add this Test?", "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if (result == DialogResult.OK)
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("Insert into TestTb1(TestName, TestCost) values (@TN, @TC)", Con);
                    cmd.Parameters.AddWithValue("@TN", LabTestTb.Text);

                    // Convert LabCostTb.Text to an integer
                    if (int.TryParse(LabCostTb.Text, out int testCost))
                    {
                        cmd.Parameters.AddWithValue("@TC", testCost);
                    }
                    else
                    {
                        MessageBox.Show("Invalid Test Cost. Please enter a valid integer.");
                        Con.Close();
                        return; // Exit the method without executing the query
                    }

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Test Added");
                    Con.Close();
                    DisplayTest();
                    Clear();
                }
                // If "Cancel" is clicked or the dialog is closed, do nothing (test won't be added)
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }




        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void LabDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure a valid row is clicked
            {
                DataGridViewRow row = LabDGV.Rows[e.RowIndex];

                // Populate textboxes with data from the selected row
                LabTestTb.Text = row.Cells[1].Value.ToString();
                LabCostTb.Text = row.Cells[2].Value.ToString();
               

                // Set the 'key' variable to the Doctor's ID
                if (row.Cells[0].Value != null)
                {
                    key = Convert.ToInt32(row.Cells[0].Value);
                }
            }
        }

        private void LabCostTb_TextChanged(object sender, EventArgs e)
        {

        }

        private void LabTestTb_TextChanged(object sender, EventArgs e)
        {

        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            try
            {
                // Create a custom dialog box with "OK" and "Cancel" buttons
                DialogResult result = MessageBox.Show("Are you sure you want to edit this test?", "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if (result == DialogResult.OK)
                {
                    if (LabCostTb.Text == "" || LabTestTb.Text == "")
                    {
                        MessageBox.Show("Missing Information");
                        return; // Exit the method without further execution
                    }

                    Con.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE TestTb1 SET TestName = @TN, TestCost = @TC where TestNum=@TKey", Con);
                    cmd.Parameters.AddWithValue("@TN", LabTestTb.Text);

                    // Convert LabCostTb.Text to an integer
                    if (int.TryParse(LabCostTb.Text, out int testCost))
                    {
                        cmd.Parameters.AddWithValue("@TC", testCost);
                    }
                    else
                    {
                        MessageBox.Show("Invalid Test Cost. Please enter a valid integer.");
                        Con.Close();
                        return; // Exit the method without executing the query
                    }

                    cmd.Parameters.AddWithValue("@TKey", key);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Test Edited");
                    Con.Close();
                    DisplayTest();
                    Clear();
                }
                // If "Cancel" is clicked or the dialog is closed, do nothing (test won't be edited)
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void DelBtn_Click(object sender, EventArgs e)
        {
            if (key == 0)
            {
                MessageBox.Show("Select the Test");
            }
            else
            {
                // Display a confirmation dialog with "OK" and "Cancel" buttons
                DialogResult result = MessageBox.Show("Are you sure you want to delete this test?", "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if (result == DialogResult.OK)
                {
                    try
                    {
                        Con.Open();
                        SqlCommand cmd = new SqlCommand("DELETE FROM TestTb1 WHERE TestNum = @TKey", Con);
                        cmd.Parameters.AddWithValue("@TKey", key);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Test Deleted");
                        Con.Close();
                        DisplayTest();
                        Clear();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                // If "Cancel" is clicked or the dialog is closed, do nothing (test won't be deleted)
            }
        }



        private void label5_Click(object sender, EventArgs e)
        {
            Login obj = new Login();
            obj.Show();
            this.Hide();
        }

        private void Home_Click(object sender, EventArgs e)
        {
            Home obj = new Home();
            obj.Show();
            this.Hide();
        }

        private void ResetBtn_Click(object sender, EventArgs e)
        {
           
            // Clear all other fields
            LabCostTb.Text = "";
            LabTestTb.Text = "";
           
        }
    }
}