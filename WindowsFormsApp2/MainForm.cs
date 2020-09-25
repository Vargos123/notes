using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class MainForm : Form
    {
        MySqlConnection connection = new MySqlConnection("server = remotemysql.com; port = 3306; Username = ed5dW7gcoL; Password = 0Gm5En5jkl; database = ed5dW7gcoL; charset = utf8");


        DataB db = new DataB();
        MySqlCommand command;
        MySqlDataReader reader;

       


        public MainForm(string log, string pas)
        {
            // Стартовая позиция по центру экрана
            this.StartPosition = FormStartPosition.CenterScreen;

            InitializeComponent();
            this.log = log;
            this.pas = pas;
            LoadData();
        }
        string log, pas;

        private void LoadData()
        {
            connection.Open();
            string query = "SELECT * FROM  " + log + " ORDER BY `id`";            
            command = new MySqlCommand(query, connection);                    
            reader = command.ExecuteReader();

            List<string[]> data = new List<string[]>();
            while (reader.Read())
            {
                data.Add(new string[3]);
                data[data.Count - 1][0] = reader[0].ToString();
                data[data.Count - 1][1] = reader[1].ToString();
                data[data.Count - 1][2] = reader[2].ToString();

            }
                reader.Close();
                connection.Close();
                foreach (string[] s in data)
                dataGridView1.Rows.Add(s);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 100)
            {
                MessageBox.Show("Вы можете добавить не больше 100 записей :( ");
                return;
            }
            else if (textBox1.Text == "")
            {
                MessageBox.Show("Вы не ввели Название");
                return;
            }
            if (textBox1.TextLength < 4)
            {
                MessageBox.Show("Длина Названия меньше допустимой нормы. Минимальная длина 4 символа.");
                return;
            }
            else if (textBox1.TextLength > 50)
            {
                MessageBox.Show("Длина Названия превышает допустимую норму. Максимальная длина 50 символов.");
                return;
            }
            else if (textBox2.Text == "")
            {
                MessageBox.Show("Вы не ввели Сообщение");
                return;
            }
            else if (textBox2.TextLength < 4)
            {
                MessageBox.Show("Длина Сообщения меньше допустимой нормы. Минимальная длина 4 символа.");
                return;
            }
            else
            {
                int n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells[2].Value = textBox1.Text;
                dataGridView1.Rows[n].Cells[1].Value = textBox2.Text;
            }
            
            MySqlCommand command = new MySqlCommand("INSERT INTO " + log +  " (`Title`, `Massage`) VALUES (@Title, @Massage)", db.getConn());
           
            command.Parameters.Add("@Title", MySqlDbType.VarChar).Value = textBox1.Text;
            command.Parameters.Add("@Massage", MySqlDbType.VarChar).Value = textBox2.Text;

            db.openConn();

            if (command.ExecuteNonQuery() == 1)
                MessageBox.Show("Вы успешно добавили данные");
            else
                MessageBox.Show("Не удалось добавить данные");

            db.closeConn();
            textBox1.Clear();
            textBox2.Clear();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        Point lastPoint;

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void bttNew_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
        }    
        
        private void button3_Click(object sender, EventArgs e)
        {
            int n = dataGridView1.CurrentCell.RowIndex;

            if (n > -1)
            {
                textBox1.Text = (string)dataGridView1.Rows[n].Cells[1].Value;
                textBox2.Text = (string)dataGridView1.Rows[n].Cells[2].Value;
            }
        }

        private void bttFind_Click_1(object sender, EventArgs e)
        {
            if (textBoxSearch.Text == "")
            {
                MessageBox.Show("Вы не ввели данные для поиска");
                return;
            }
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                dataGridView1.Rows[i].Selected = false;
                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                    if (dataGridView1.Rows[i].Cells[j].Value != null)
                        if (dataGridView1.Rows[i].Cells[j].Value.ToString().Contains(textBoxSearch.Text))
                        {
                            dataGridView1.Rows[i].Selected = true;
                            break;
                        }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.Text = textBox2.Text.Length.ToString();

            if (textBox2.TextLength == 500)
            {
                MessageBox.Show("Достигнуто максимальное количество символов: 500");
                return;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox2.Text = textBox1.Text.Length.ToString();
            if (textBox1.TextLength == 50)
            {
                MessageBox.Show("Достигнуто максимальное количество символов: 50");
                return;
            }
        }

        private void bttDelete_Click(object sender, EventArgs e)
        {

        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            if (textBoxSearch.TextLength == 50)
            {
                MessageBox.Show("Достигнуто максимальное количество символов: 50");
                return;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Вы действительно хотите удалить все записи?", "Удаление", MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.OK)
                {

                    dataGridView1.Rows.Clear();

                    using (MySqlCommand commanS = new MySqlCommand(" TRUNCATE TABLE " + log, db.getConn()))
                    {
                        db.openConn();
                        commanS.ExecuteNonQuery();
                        db.closeConn();
                    }
                }
            }
            catch
            {
                MessageBox.Show("Все записи были успешно удаленны");
            }            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm logF = new LoginForm();
            logF.Show();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
                        
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Вы действительно хотите удалить свой аккаунт? ", "Удаление", MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.OK)
            {
                if (MessageBox.Show("Аккаунт восстановлению не принадлежит.                           Вы действительно хотите продолжить?! ", "Удаление", MessageBoxButtons.OKCancel,
                 MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.OK)
                {
                        MySqlCommand comman1 = new MySqlCommand(" DELETE FROM AllUsersLogPass WHERE login = '" + log + "' AND pass = '" + pas + "'", db.getConn());
                        MySqlCommand comman2 = new MySqlCommand(" DROP TABLE " + log, db.getConn());
                        db.openConn();
                        comman1.ExecuteNonQuery();
                        comman2.ExecuteNonQuery();
                        db.closeConn();

                    this.Hide();
                    LoginForm logF = new LoginForm();
                    logF.Show();
                    MessageBox.Show("Ваш аккаунт был успешно удален!");
                }
            }            
        }
    }
}
