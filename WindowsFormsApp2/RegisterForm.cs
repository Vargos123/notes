using MySql.Data.MySqlClient;
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

namespace WindowsFormsApp2
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void CloseButton_MouseEnter(object sender, EventArgs e)
        {
            CloseButton.ForeColor = Color.Black;
        }

        private void CloseButton_MouseLeave(object sender, EventArgs e)
        {
            CloseButton.ForeColor = Color.White;
        }

        Point lastPoint;
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }


        private void butRegister_Click(object sender, EventArgs e)
        {

            DataB db = new DataB();

            if (loginF.Text == "")
            {
                MessageBox.Show("Вы не ввели логин");
                return;
            }
            else if (loginF.TextLength < 4)
            {
                MessageBox.Show("Длина логина меньше допустимой нормы. Минимальная длина 4 символа.");
                return;
            }
            else if (passF.Text == "")
            {
                MessageBox.Show("Вы не ввели пароль");
                return;
            }
            else if (passF.TextLength < 4)
            {
                MessageBox.Show("Длина пароля меньше допустимой нормы. Минимальная длина 4 символа.");
                return;
            }

            if (isUser())
                return;
            try
            {
                using (MySqlCommand crtdata = new MySqlCommand("CREATE TABLE " + loginF.Text + " LIKE PrimerTable", db.getConn()))
                {
                    db.openConn();
                    crtdata.ExecuteNonQuery();
                    db.closeConn();
                }
            }
            catch
            {
                MessageBox.Show("Данный логин не может быть использован :( ");
                return;
            }

            MySqlCommand command = new MySqlCommand("INSERT INTO `AllUsersLogPass` (`login`, `pass`) VALUES (@login, @pass)", db.getConn());

            command.Parameters.Add("@login", MySqlDbType.VarChar).Value = loginF.Text;
            command.Parameters.Add("@pass", MySqlDbType.VarChar).Value = passF.Text;                                 
            
            db.openConn();
            if (command.ExecuteNonQuery() == 1)
            {

                MessageBox.Show("Вы успешно зарегистрировались");                
            }
            else
                MessageBox.Show("Вы не зарегистрировались, проверьте ввод даных");
            db.closeConn();
        }        

        public Boolean isUser()
        {
            DataB db = new DataB();

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * FROM `AllUsersLogPass` WHERE `login` = @userL", db.getConn());
            command.Parameters.Add("@userL", MySqlDbType.VarChar).Value = loginF.Text;


            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                MessageBox.Show("Даный логин уже зарегистрирован");
                return true;
            }
            else
                return false;
        }

        private void label3_Click(object sender, EventArgs e)
        {
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void label4_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm logF = new LoginForm();
            logF.Show();
        }

        private void loginF_TextChanged(object sender, EventArgs e)
        {
            richTextBox2.Text = loginF.Text.Length.ToString();
            if (loginF.TextLength == 16)
            {
                MessageBox.Show("Достигнуто максимальное количество символов: 16");
                return;
            }
        }

        private void passF_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.Text = passF.Text.Length.ToString();
            if (passF.TextLength == 32)
            {
                MessageBox.Show("Достигнуто максимальное количество символов: 32");
                return;
            }
        }
    }
}
