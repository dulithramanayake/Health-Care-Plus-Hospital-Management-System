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
    public partial class Prescription : Form
    {
        public Prescription()
        {
            InitializeComponent();

            if(Login.Role=="Receptionist")
            {
                label3.Enabled = false;
                label4.Enabled = false;
                label5.Enabled = false;

            }
            else if(Login.Role=="Doctor")
            {
                label3.Enabled = false;
                label4.Enabled = false;
                label5.Enabled = false;

            }

            DisplayPrescription();
            GetDocId();
            GetPatId();
            GetTestId();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Dulith\Documents\HealthCare.Plus.mdf;Integrated Security=True;Connect Timeout=30");
        
        
        private void Clear()
        {
            DocIdCb.SelectedIndex = 0;
            DocNameTb.Text = "";
            PatIdCb.SelectedIndex = 0;
            PatNameTb.Text = "";
            TestIdCb.SelectedIndex = 0;
            TestNameTb.Text = "";
            CostTb.Text = "";
            Medicines.Text = "";

            //key = 0;
        }
        private void GetDocName()
        {
            Con.Open();
            string Query = "Select * from DoctorTb1 where DocId=" + DocIdCb.SelectedValue.ToString() + "";
            SqlCommand cmd=new SqlCommand(Query, Con);
            DataTable dt=new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            foreach(DataRow dr in dt.Rows)
            {
                DocNameTb.Text = dr["DocName"].ToString();
            }
            Con.Close();
        }
        private void GetPatName()
        {
            Con.Open();
            string Query = "Select * from PatientTb1 where PatId=" + PatIdCb.SelectedValue.ToString() + "";
            SqlCommand cmd = new SqlCommand(Query, Con);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                PatNameTb.Text = dr["PatName"].ToString();
            }
            Con.Close();
        }
        private void GetTestName()
        {
            Con.Open();
            string Query = "Select * from TestTb1 where TestNum=" + TestIdCb.SelectedValue.ToString() + "";
            SqlCommand cmd = new SqlCommand(Query, Con);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                TestNameTb.Text = dr["TestName"].ToString();
                CostTb.Text = dr["TestCost"].ToString();
            }
            Con.Close();
        }
        private void GetDocId()
        {
            try
            {
                Con.Open();
                SqlCommand cmd = new SqlCommand("Select DocId from DoctorTb1", Con);
                SqlDataReader rdr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Columns.Add("DocId", typeof(int));
                dt.Load(rdr);
                DocIdCb.ValueMember = "DocID";
                DocIdCb.DataSource = dt;
                Con.Close();
            }
            catch (Exception ex)
            {
                // Handle and display the exception message
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            finally
            {
                // Make sure to close the connection even if an error occurs
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
            }

        }
        private void GetPatId()
        {
            try
            {
                Con.Open();
                SqlCommand cmd = new SqlCommand("Select PatId from PatientTb1", Con);
                SqlDataReader rdr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Columns.Add("PatId", typeof(int));
                dt.Load(rdr);
                PatIdCb.ValueMember = "PatID";
                PatIdCb.DataSource = dt;
                Con.Close();
            }
            catch (Exception ex)
            {
                // Handle and display the exception message
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            finally
            {
                // Make sure to close the connection even if an error occurs
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
            }

        }
        private void GetTestId()
        {
            try
            {
                Con.Open();
                SqlCommand cmd = new SqlCommand("Select TestNum from TestTb1", Con);
                SqlDataReader rdr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Columns.Add("TestNum", typeof(int));
                dt.Load(rdr);
                TestIdCb.ValueMember = "TestNum";
                TestIdCb.DataSource = dt;
                Con.Close();
            }
            catch (Exception ex)
            {
                // Handle and display the exception message
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            finally
            {
                // Make sure to close the connection even if an error occurs
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
            }

        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            PrescSumTxt.Text = "";
            PrescSumTxt.Text = "                                            HealthCare Plus\n\n" + "                                            PRESCRIPTION             " + "\n*******************************************************" + "\n" + DateTime.Today.Date + "\n\n       Doctor: " + PrescriptionDGV.SelectedRows[0].Cells[2].Value.ToString() + "                                Patient: " + PrescriptionDGV.SelectedRows[0].Cells[4].Value.ToString() + "\n\n       Test: " + PrescriptionDGV.SelectedRows[0].Cells[6].Value.ToString() + "        " + "                 Medicines: " + PrescriptionDGV.SelectedRows[0].Cells[7].Value.ToString() + "\n\n\n                                           HealthCare Plus";
        }
        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void DisplayPrescription()
        {
            Con.Open();
            string Query = "Select * from PreceptionTb1";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            PrescriptionDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        int key = 0;
        private void AddBtn_Click(object sender, EventArgs e)
        {
            if (DocNameTb.Text == "" || PatNameTb.Text == "" || TestNameTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                // Display a confirmation dialog
                DialogResult result = MessageBox.Show("Are you sure you want to Add this Prescription?", "Confirmation", MessageBoxButtons.OKCancel);

                if (result == DialogResult.OK)
                {
                    try
                    {
                        Con.Open();
                        SqlCommand cmd = new SqlCommand("Insert into PreceptionTb1(DocId,DocName,PatId,PatName,LabTestId,LabTestName,Medicines,Cost)values(@DI,@DN,@PI,@PN,@TI,@TN,@MED,@CO)", Con);
                        cmd.Parameters.AddWithValue("@DI", DocIdCb.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@DN", DocNameTb.Text);
                        cmd.Parameters.AddWithValue("@PI", PatIdCb.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@PN", PatNameTb.Text);
                        cmd.Parameters.AddWithValue("@TI", TestIdCb.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@TN", TestNameTb.Text);
                        cmd.Parameters.AddWithValue("@MED", Medicines.Text);
                        cmd.Parameters.AddWithValue("@CO", CostTb.Text);

                        cmd.ExecuteNonQuery();
                       
                        Con.Close();
                        DisplayPrescription();
                        Clear();
                    }
                    catch (Exception EX)
                    {
                        MessageBox.Show(EX.Message);
                    }
                }
            }
        }


        private void DocIdCb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            GetDocName();
        }

        private void PatIdCb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            GetPatName();
        }

        private void TestIdCb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            GetTestName();
        }

        private void PrescSumTxt_TextChanged(object sender, EventArgs e)
        {

        }

        private void DelBtn_Click(object sender, EventArgs e)
        {
            if(printPreviewDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            float yPos = 80; // Vertical position for the text

            // Draw the text from the PrescSumTxt textbox
            e.Graphics.DrawString(PrescSumTxt.Text, new Font("Averia", 18, FontStyle.Regular), Brushes.Black, new PointF(95, yPos));

            // Add some additional text below
            yPos += 40; // Increase the vertical position
            e.Graphics.DrawString("\n\t" + "", new Font("Averia", 15, FontStyle.Bold), Brushes.Red, new PointF(200, yPos));
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Login obj = new Login();
            obj.Show();
            this.Hide();
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

        private void Prescription_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            {
                if (key == 0)
                {
                    MessageBox.Show("Select the Preseption");
                }
                else
                {
                    try
                    {
                        Con.Open();
                        SqlCommand cmd = new SqlCommand("Delete from PreceptionTb1 where PrescId=@Prkey", Con);

                        cmd.Parameters.AddWithValue("@Prkey", key);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Prescription Deleted");
                        Con.Close();
                        DisplayPrescription();
                        Clear();

                    }
                    catch (Exception EX)
                    {
                        MessageBox.Show(EX.Message);
                    }
                }
            }
        }

        private void DocNameTb_TextChanged(object sender, EventArgs e)
        {

        }

        private void DocIdCb_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
