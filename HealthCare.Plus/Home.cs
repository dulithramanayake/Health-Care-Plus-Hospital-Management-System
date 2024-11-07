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
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
            if (Login.Role == "Receptionist")
            {
                RecepLbl.Enabled = false;
                DoctorLbl.Enabled = false;
                LabLbl.Enabled = false;
                label3.Enabled = false;
                label4.Enabled = false;

            }
            CountPatients();
            CountDoctors();
            CountLabTest();
            CountHIV();
            CountNurse();
            CountRoom();
            

        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Dulith\Documents\HealthCare.Plus.mdf;Integrated Security=True;Connect Timeout=30");
        private void CountPatients()
        {
            Con.Open();
            SqlDataAdapter sda =new SqlDataAdapter("Select count(*) from PatientTb1", Con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            PatNumLbl.Text = dt.Rows[0][0].ToString();
            Con.Close();
        }
        private void CountDoctors()
        {
            Con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("Select count(*) from DoctorTb1", Con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            DocNumLbl.Text = dt.Rows[0][0].ToString();
            Con.Close();
        }
        private void CountLabTest()
        {
            Con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("Select count(*) from TestTb1", Con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            TestNumLbl.Text = dt.Rows[0][0].ToString();
            Con.Close();
        }
        private void CountHIV()
        {
            string Status = "Positive";
            Con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("Select count(*) from PatientTb1 where PatHIV='"+Status+"'", Con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            HIVNumLbl.Text = dt.Rows[0][0].ToString();
            Con.Close();
        }
        private void CountRoom()
        {
            string Status = "Available";
            Con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("Select count(*) from RoomsTb1 where RoomAva='" + Status + "'", Con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            RoomNumLbl.Text = dt.Rows[0][0].ToString();
            Con.Close();
        }

        private void CountNurse()
        {
            Con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("Select count(*) from NurseTb1", Con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            NurseNumLbl.Text = dt.Rows[0][0].ToString();
            Con.Close();
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            Application.Exit();
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

        private void label6_Click(object sender, EventArgs e)
        {
            Login obj = new Login();
            obj.Show();
            this.Hide();
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Nurse obj = new Nurse();
            obj.Show();
            this.Hide();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Rooms obj = new Rooms();
            obj.Show();
            this.Hide();
        }

        private void NurseNumLbl_Click(object sender, EventArgs e)
        {

        }

        private void HIVNumLbl_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            Appointment obj = new Appointment();
            obj.Show();
            this.Hide();
        }
    }
}
