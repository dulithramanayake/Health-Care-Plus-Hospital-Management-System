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
    public partial class Appointment : Form
    {
        public Appointment()
        {
            InitializeComponent();

            if (Login.Role == "Receptionist")
            {
                RecepLbl.Enabled = false;
                DoctorLbl.Enabled = false;
                LabLbl.Enabled = false;
                label3.Enabled = false;
                label4.Enabled = false;
                PatientLbl.Enabled = false;

            }

            DisplayAppointment();
            GetDocId();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Dulith\Documents\HealthCare.Plus.mdf;Integrated Security=True;Connect Timeout=30");
        private void DisplayAppointment()
        {
            Con.Open();
            string Query = "Select * from AppointmentTb1";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            AppointmentDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        int key = 0;
        private void Clear()
        {
            NameTb.Text = "";
            GenCb.SelectedIndex = 0;
            DocId.SelectedIndex = 0;
            DocNameTb.Text = "";
            AddTb.Text = "";
            PhoneTb.Text = "";
            AppDate.Value = DateTime.Now;
            DOB.Value = DateTime.Now;

            //key = 0;
        }
        private void GetDocId()
        {
            Con.Open();
            SqlCommand cmd = new SqlCommand("Select DocId from DoctorTb1", Con);
            SqlDataReader rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("DocId", typeof(int));
            dt.Load(rdr);
            DocId.ValueMember = "DocID";
            DocId.DataSource = dt;
            Con.Close();
        }
        private void GetDocName()
        {
            Con.Open();
            string Query = "Select * from DoctorTb1 where DocId=" + DocId.SelectedValue.ToString() + "";
            SqlCommand cmd = new SqlCommand(Query, Con);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                DocNameTb.Text = dr["DocName"].ToString();
            }
            Con.Close();
        }


        private void AddBtn_Click(object sender, EventArgs e)
        {
            if (NameTb.Text == "" || DocNameTb.Text == "" || AddTb.Text == "" || PhoneTb.Text == "" || GenCb.SelectedIndex == -1 || DocId.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                // Create a custom dialog box with "Ok" and "Cancel" buttons
                DialogResult result = MessageBox.Show("Do you want to add this Appointment?", "Confirmation", MessageBoxButtons.OKCancel);

                if (result == DialogResult.OK)
                {
                    try
                    {
                        Con.Open();
                        SqlCommand cmd = new SqlCommand("INSERT INTO AppointmentTb1(Name, Gen, DocId, DocName, [Add], DOB, Phone, AppDate) VALUES (@N, @G, @DI, @DN, @ADD, @DOB, @PN, @AD)", Con);
                        cmd.Parameters.AddWithValue("@N", NameTb.Text);
                        cmd.Parameters.AddWithValue("@G", GenCb.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@DI", DocId.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@DN", DocNameTb.Text);
                        cmd.Parameters.AddWithValue("@ADD", AddTb.Text);
                        cmd.Parameters.AddWithValue("@DOB", DOB.Value.Date);
                        cmd.Parameters.AddWithValue("@PN", PhoneTb.Text);
                        cmd.Parameters.AddWithValue("@AD", AppDate.Value.Date);

                        cmd.ExecuteNonQuery();
                        Con.Close();
                        DisplayAppointment();
                        Clear();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                // If "Cancel" is clicked, do nothing (Appointment won't be added)
            }
        }



        private void DoctorLbl_Click_1(object sender, EventArgs e)
        {
            Doctors obj = new Doctors();
            obj.Show();
            this.Hide();
        }

        private void Home_Click_1(object sender, EventArgs e)
        {
            Home obj = new Home();
            obj.Show();
            this.Hide();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void PatientLbl_Click(object sender, EventArgs e)
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

        private void AppointmentDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            NameTb.Text = AppointmentDGV.SelectedRows[0].Cells["Name"].Value.ToString();
            GenCb.Text = AppointmentDGV.SelectedRows[0].Cells["Gen"].Value.ToString();
            AppDate.Value = Convert.ToDateTime(AppointmentDGV.SelectedRows[0].Cells["AppDate"].Value);
            DocNameTb.Text = AppointmentDGV.SelectedRows[0].Cells["Name"].Value.ToString();
            DOB.Value = Convert.ToDateTime(AppointmentDGV.SelectedRows[0].Cells["Dob"].Value);
            AddTb.Text = AppointmentDGV.SelectedRows[0].Cells["Add"].Value.ToString();
            PhoneTb.Text = AppointmentDGV.SelectedRows[0].Cells["Phone"].Value.ToString();
            DocId.Text = AppointmentDGV.SelectedRows[0].Cells["DocId"].Value.ToString();
           


            if (NameTb.Text == "")
            {
                key = 0;
            }
            else
            { // Convert the key (usually an ID) from the DataGridView row to an integer
                key = Convert.ToInt32(AppointmentDGV.SelectedRows[0].Cells["AppointmentId"].Value.ToString());
            }
        }

        private void SearchBtn_Click(object sender, EventArgs e)
        {
            string searchKeyword = searchTb.Text;

            if (!string.IsNullOrEmpty(searchKeyword))
            {
                try
                {
                    Con.Open();
                    bool isValidId = int.TryParse(searchKeyword, out int appointmentId);
                    string query;

                    if (isValidId)
                    {
                        query = "SELECT * FROM AppointmentTb1 WHERE AppointmentId = @SearchId";
                    }
                    else
                    {
                        query = "SELECT * FROM AppointmentTb1 WHERE Name LIKE @SearchName";
                    }

                    SqlCommand cmd = new SqlCommand(query, Con);

                    if (isValidId)
                    {
                        cmd.Parameters.AddWithValue("@SearchId", appointmentId);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@SearchName", "%" + searchKeyword + "%");
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    if (dataTable.Rows.Count == 0)
                    {
                        if (isValidId)
                        {
                            MessageBox.Show("Invalid ID", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("No results found for the given name.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        AppointmentDGV.DataSource = dataTable;
                    }

                    Con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    Con.Close();
                }
            }
            else
            {
                DisplayAppointment();
            }

        }

        private void DocId_SelectionChangeCommitted(object sender, EventArgs e)
        {
            GetDocName();
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
                DialogResult result = MessageBox.Show("Are you sure you want to delete this Appointment?", "Confirmation", MessageBoxButtons.OKCancel);

                if (result == DialogResult.OK)
                {
                    try
                    {
                        Con.Open();
                        SqlCommand cmd = new SqlCommand("DELETE FROM AppointmentTb1 WHERE AppointmentId = @APKey", Con);
                        cmd.Parameters.AddWithValue("@APKey", key);
                        cmd.ExecuteNonQuery();

                        Con.Close();
                        DisplayAppointment();
                        Clear();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                // If "Cancel" or close button (cross icon) is clicked, do nothing (Appointment won't be deleted)
            }
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (key == 0)
            {
                MessageBox.Show("Select the Appointment");
            }
            else if (NameTb.Text == "" || AddTb.Text == "" || PhoneTb.Text == "" || DocNameTb.Text == ""|| DocId.Text == "" || GenCb.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                // Create a custom confirmation dialog with "OK" and "Cancel" buttons
                DialogResult result = MessageBox.Show("Are you sure you want to edit this Appointment's information?", "Confirmation", MessageBoxButtons.OKCancel);

                if (result == DialogResult.OK)
                {
                    try
                    {
                        Con.Open();
                        SqlCommand cmd = new SqlCommand("UPDATE AppointmentTb1 SET Name = @N, Gen = @G, DocId = @DI, DocName = @DN, [Add] = @A,DOB=@D,Phone=@P,AppDate=@AD WHERE AppointmentId = @APKey", Con);
                        cmd.Parameters.AddWithValue("@N", NameTb.Text);
                        cmd.Parameters.AddWithValue("@G", GenCb.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@DI", DocId.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@DN", DocNameTb.Text);
                        cmd.Parameters.AddWithValue("@A", AddTb.Text);
                        cmd.Parameters.AddWithValue("@D", DOB.Value.Date);
                        cmd.Parameters.AddWithValue("@P", PhoneTb.Text);
                        cmd.Parameters.AddWithValue("@AD", AppDate.Value.Date);
                        
                        cmd.Parameters.AddWithValue("@APKey", key);

                        cmd.ExecuteNonQuery();

                        Con.Close();
                        DisplayAppointment();
                        Clear();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                // If "Cancel" is clicked or the dialog is closed, do nothing (Appointment won't be edited)
            }
        }

        private void ResetBtn_Click(object sender, EventArgs e)
        {
            // Set the ComboBox to the first item (assuming it's a gender and Specialiasion selection)
            GenCb.SelectedIndex = 0;
            DocId.SelectedIndex = 0;



            // Clear all other fields
            NameTb.Text = "";
            AddTb.Text = "";
            DOB.Value = DateTime.Now;
            AppDate.Value = DateTime.Now;// Set the DateOfBirth picker to the current date
            PhoneTb.Text = "";
            DocNameTb.Text = "";
        }

        private void searchTb_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
