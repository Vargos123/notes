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

namespace NotesApp
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
            LoadData();
        }
        string log;

        private void LoadData()
        {
            connection.Open();
            string query = "SELECT * FROM  `" + log + "` ORDER BY `id`";            
            command = new MySqlCommand(query, connection);                    
            reader = command.ExecuteReader();

            List<string[]> data = new List<string[]>();
            while (reader.Read())
            {
                data.Add(new string[2]);
                data[data.Count - 1][0] = reader[1].ToString();
                data[data.Count - 1][1] = reader[2].ToString();

            }
                reader.Close();
                connection.Close();
                foreach (string[] s in data)
                dataGridView1.Rows.Add(s);
        }

        private void bttSave_Click(object sender, EventArgs e)
        {
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                if (dataGridView1.Rows.Count == 100)
                {
                    MessageBox.Show("Вы можете добавить не больше 100 записей :( ");
                    return;
                }
                else if (nameBox.Text == "")
                {
                    MessageBox.Show("Вы не ввели Название");
                    return;
                }
                if (nameBox.TextLength < 4)
                {
                    MessageBox.Show("Длина Названия меньше допустимой нормы. Минимальная длина 4 символа.");
                    return;
                }
                else if (nameBox.TextLength > 50)
                {
                    MessageBox.Show("Длина Названия превышает допустимую норму. Максимальная длина 50 символов.");
                    return;
                }
                else if (massageBox.Text == "")
                {
                    MessageBox.Show("Вы не ввели Сообщение");
                    return;
                }
                else if (massageBox.TextLength < 4)
                {
                    MessageBox.Show("Длина Сообщения меньше допустимой нормы. Минимальная длина 4 символа.");
                    return;
                }
                else
                {
                    int n = dataGridView1.Rows.Add();
                    dataGridView1.Rows[n].Cells[0].Value = nameBox.Text;
                    dataGridView1.Rows[n].Cells[1].Value = massageBox.Text;
                }

                MySqlCommand command = new MySqlCommand("INSERT INTO " + log + " (`Title`, `Massage`) VALUES (@Title, @Massage)", db.getConn());

                command.Parameters.Add("@Title", MySqlDbType.VarChar).Value = nameBox.Text;
                command.Parameters.Add("@Massage", MySqlDbType.VarChar).Value = massageBox.Text;

                db.openConn();

                if (command.ExecuteNonQuery() == 1)
                    MessageBox.Show("Вы успешно добавили данные");
                else
                    MessageBox.Show("Не удалось добавить данные");

                db.closeConn();
                nameBox.Clear();
                massageBox.Clear();
            }
            else
            {
                MessageBox.Show("Не удалось сохранить данные. Проверьте доступ к интернету!");
            }            
        }
        private void CloseButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void CloseButton_MouseEnter(object sender, EventArgs e)
        {
            CloseButton.ForeColor = Color.Black; // черный крестик при наводе мышкой
        }
        private void CloseButton_MouseLeave(object sender, EventArgs e)
        {
            CloseButton.ForeColor = Color.White; // белый крестик
        }
        private void hide_MouseEnter(object sender, EventArgs e)
        {
            hide.ForeColor = Color.Black; // черный - при наводе мышкой на 
        }
        private void hide_MouseLeave(object sender, EventArgs e)
        {
            hide.ForeColor = Color.White; // белый -
        }
        private void hide_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
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
            nameBox.Clear();
            massageBox.Clear();
        }

        private void bttRead_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount > 0)
            {
                int n = dataGridView1.CurrentCell.RowIndex;

                if (n > -1)
                {
                    nameBox.Text = (string)dataGridView1.Rows[n].Cells[0].Value;
                    massageBox.Text = (string)dataGridView1.Rows[n].Cells[1].Value;
                }
            }
            else
            {
                MessageBox.Show("Нет записей для чтения. Добавьте записи!");
                return;
            }
        }

        private void bttFind_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount > 0)
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
            else
            {
                MessageBox.Show("Нет записей для поиска. Добавьте записи!");
                return;
            }
        }

        private void massageBox_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.Text = massageBox.Text.Length.ToString();

            if (massageBox.TextLength == 500)
            {
                MessageBox.Show("Достигнуто максимальное количество символов: 500");
                return;
            }
        }

        private void nameBox_TextChanged(object sender, EventArgs e)
        {
            richTextBox2.Text = nameBox.Text.Length.ToString();
            if (nameBox.TextLength == 50)
            {
                MessageBox.Show("Достигнуто максимальное количество символов: 50");
                return;
            }
        }

        private void bttDelete_Click(object sender, EventArgs e)
        {
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                if (dataGridView1.RowCount > 0)
                {
                    if (MessageBox.Show("Вы действительно хотите удалить выделенную запись?", "Удаление", MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.OK)
                    {
                        if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                        {
                            int index = dataGridView1.SelectedCells[0].RowIndex + 1;
                            dataGridView1.Rows.RemoveAt(dataGridView1.SelectedCells[0].RowIndex);

                            MySqlCommand comman2 = new MySqlCommand("DELETE FROM `" + log + "` WHERE id = " + index + "", db.getConn()); // Удаляем выделенную строку по индексу
                            MySqlCommand comman1 = new MySqlCommand("ALTER TABLE `" + log + "` DROP id;" +
                            "ALTER TABLE `" + log + "`" +
                            "ADD id INT UNSIGNED NOT NULL AUTO_INCREMENT FIRST," +
                            "ADD PRIMARY KEY(id)", db.getConn()); // Обновляем ид от 1
                            db.openConn();
                            comman2.ExecuteNonQuery();
                            comman1.ExecuteNonQuery();
                            db.closeConn();
                        }
                        else
                        {
                            MessageBox.Show("Не удалось удалить данные. Проверьте доступ к интернету!");
                            return;
                        }
                    }                    
                }
                else
                {
                    MessageBox.Show("Нет записей для удаления!");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Не удалось удалить данные. Проверьте доступ к интернету!");
                return;
            }
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            if (textBoxSearch.TextLength == 50)
            {
                MessageBox.Show("Достигнуто максимальное количество символов: 50!");
                return;
            }
        }

        private void bttDelAll_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount > 0)
            {
                if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                {
                    if (MessageBox.Show("Вы действительно хотите удалить все записи?", "Удаление", MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.OK)
                    {
                        if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                        {
                            dataGridView1.Rows.Clear();
                            using (MySqlCommand commanS = new MySqlCommand("TRUNCATE TABLE " + log, db.getConn()))
                            {
                                db.openConn();
                                commanS.ExecuteNonQuery();
                                db.closeConn();
                            }
                            MessageBox.Show("Все записи были успешно удаленны!");
                            return;
                        }
                        else
                        {
                            MessageBox.Show("Не удалось удалить данные. Проверьте доступ к интернету!");
                            return;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Не удалось удалить данные. Проверьте доступ к интернету!");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Нет записей для удаления!");
                return;
            }
        }

        private void bttExit_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm logF = new LoginForm();
            logF.Show();
        }

        private void bttDelAcc_Click(object sender, EventArgs e)
        {
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                if (MessageBox.Show("Вы действительно хотите удалить свой аккаунт? ", "Удаление", MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.OK)
                {
                    if (MessageBox.Show("Аккаунт восстановлению не принадлежит!                           Вы действительно хотите продолжить?! ", "Удаление", MessageBoxButtons.OKCancel,
                     MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.OK)
                    {
                        if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                        {
                            MySqlCommand comman1 = new MySqlCommand(" DELETE FROM `AllUsersLogPass` WHERE `login` = @log", db.getConn());
                            MySqlCommand comman2 = new MySqlCommand(" DROP TABLE `" + log + "`", db.getConn());
                            comman1.Parameters.Add("@log", MySqlDbType.VarChar).Value = log;


                            db.openConn();
                            comman1.ExecuteNonQuery();
                            comman2.ExecuteNonQuery();
                            db.closeConn();

                            this.Hide();
                            LoginForm logF = new LoginForm();
                            logF.Show();
                            MessageBox.Show("Ваш аккаунт был успешно удален!");
                        }
                        else
                        {
                            MessageBox.Show("Не удалось удалить аккаунт. Проверьте доступ к интернету!");
                            return;
                        }
                    }
                }                
            }
            else
            {
                MessageBox.Show("Не удалось удалить аккаунт. Проверьте доступ к интернету!");
                return;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}
