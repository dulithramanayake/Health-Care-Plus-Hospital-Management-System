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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            RoleCb.SelectedIndex = 0;
            UnameTb.Text = "";
            PassTb.Text = "";
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Dulith\Documents\HealthCare.Plus.mdf;Integrated Security=True;Connect Timeout=30");

        public static string Role { get; internal set; }

        private void LoginBtn_Click(object sender, EventArgs e)
        {
            // Check if a role is selected in the ComboBox
            if (RoleCb.SelectedIndex == -1)
            {
                MessageBox.Show("Select Your Position");
            }
            // If "Admin" is selected
            else if (RoleCb.SelectedIndex == 0)
            {
                // Check if both the Admin Name and Password are entered
                if (UnameTb.Text == "" || PassTb.Text == "")
                {
                    MessageBox.Show("Enter Both Admin Name and Password");
                }
                // Check if the entered Admin Name and Password match
                else if (UnameTb.Text == "Admin" && PassTb.Text == "1234")
                {
                    // Set the user's role to "Admin" and open the "Patients" form
                    Role = "Admin";
                    Patients obj = new Patients();
                    obj.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Wrong Admin Name and Password");
                }
            }
            // If "Doctor" is selected
            else if (RoleCb.SelectedIndex == 1)
            {
                // Check if both the Doctor Name and Password are entered
                if (UnameTb.Text == "" || PassTb.Text == "")
                {
                    MessageBox.Show("Enter Both Doctor Name and Password");
                }
                else
                {
                    Con.Open(); // Open the database connection
                    SqlDataAdapter sda = new SqlDataAdapter("Select Count(*) from DoctorTb1 where DocName='" + UnameTb.Text + "' and DocPass='" + PassTb.Text + "'", Con);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    // If a match is found in the database, set the user's role to "Doctor" and open the "Prescription" form
                    if (dt.Rows[0][0].ToString() == "1")
                    {
                        Role = "Doctor";
                        Prescription obj = new Prescription();
                        obj.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Doctor Not found");
                    }
                    Con.Close(); // Close the database connection
                }
            }
            // If "Receptionist" is selected or any other case
            else
            {
                // Check if both the Receptionist Name and Password are entered
                if (UnameTb.Text == "" || PassTb.Text == "")
                {
                    MessageBox.Show("Enter Both Receptionist Name and Password");
                }
                else
                {
                    Con.Open(); // Open the database connection
                    SqlDataAdapter sda = new SqlDataAdapter("Select Count(*) from ReceptionistTb1 where RecepName='" + UnameTb.Text + "' and RecepPass='" + PassTb.Text + "'", Con);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);

                    // If a match is found in the database, set the user's role to "Receptionist" and open the "Home" form
                    if (dt.Rows[0][0].ToString() == "1")
                    {
                        Role = "Receptionist";
                        Home obj = new Home();
                        obj.Show();
                        this.Hide();
                    }
                    else
                    {
                        // If no match is found, display an error message.
                        MessageBox.Show("Receptionist Not found");
                    }

                    Con.Close(); // Close the database connection
                }
            }
        }


        private void RoleCb_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void UnameTb_TextChanged(object sender, EventArgs e)
        {

        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void PassTb_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                PassTb.PasswordChar = '\0'; // Show Password
                checkBox1.Text = "Hide";
            }
            else
            {
                PassTb.PasswordChar = '*'; // Hide Password
                checkBox1.Text = "Show";
            }
        }

        private void RoleCb_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Disable the key press event to stop input in the middle of the ComboBox
            e.Handled = true;
        }
    }
}
