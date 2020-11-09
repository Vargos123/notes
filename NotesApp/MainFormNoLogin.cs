using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;

namespace NotesApp
{
    public partial class MainFormNoLogin : Form
    {
        public MainFormNoLogin()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
        }               
        private void CloseButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount > 0)
            {
                if (MessageBox.Show("Вы действительно хотите выйти? Несохранённые данные будут утеряны!", "Выход", MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.OK)
                {
                    Application.Exit();
                }
            }
            else
            {
                Application.Exit();
            }
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
        private void MainFormNoLogin_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void MainFormNoLogin_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }



        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            if (textBoxSearch.TextLength == 50)
            {
                MessageBox.Show("Достигнуто максимальное количество символов: 50!");
                return;
            }
        }

        private void bttFind_Click(object sender, EventArgs e)
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

        private void bttRead_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount > 0)
            {
                int n = dataGridView1.CurrentCell.RowIndex;

                if (n > -1)
                {
                    nameBox.Text = (string)dataGridView1.Rows[n].Cells[0].Value;
                    messageBox.Text = (string)dataGridView1.Rows[n].Cells[1].Value;
                }
            }
            else
            {
                MessageBox.Show("Нет записей для чтения. Добавьте записи!");
                return;
            }
        }

        private void bttDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount > 0)
            {
                if (MessageBox.Show("Вы действительно хотите удалить выделенную запись?", "Удаление", MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.OK)
                {
                    int index = dataGridView1.SelectedCells[0].RowIndex;
                    dataGridView1.Rows.RemoveAt(index);
                }
            }
            else
            {
                MessageBox.Show("Нет записей для удаления!");
                return;
            }
        }

        private void bttNew_Click(object sender, EventArgs e)
        {
            nameBox.Clear();
            messageBox.Clear();
        }
        private void bttExit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount > 0)
            {
                if (MessageBox.Show("Вы действительно хотите выйти? Несохранённые данные будут утеряны!", "Выход", MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.OK)
                {
                    this.Hide();
                    LoginForm logF = new LoginForm();
                    logF.Show();
                }
            }
            else
            {
                this.Hide();
                LoginForm logF = new LoginForm();
                logF.Show();
            }            
        }    


        private void bttSave_Click(object sender, EventArgs e)
        {          
            try
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
                else if (messageBox.Text == "")
                {
                    MessageBox.Show("Вы не ввели Сообщение");
                    return;
                }
                else if (messageBox.TextLength < 4)
                {
                    MessageBox.Show("Длина Сообщения меньше допустимой нормы. Минимальная длина 4 символа.");
                    return;
                }
                else
                {

                    string Title = nameBox.Text;
                    string Message = messageBox.Text;
                    dataGridView1.Rows.Add(Title, Message);
                    MessageBox.Show("Вы успешно добавили данные");
                    nameBox.Clear();
                    messageBox.Clear();
                }
            }
            catch
            {
                MessageBox.Show("Не удалось сохранить данные.");
                return;
            }
        }

        private void bttDelAll_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount > 0)
            {
                    if (MessageBox.Show("Вы действительно хотите удалить все записи?", "Удаление", MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.OK)
                    {
                        dataGridView1.Rows.Clear();
                        MessageBox.Show("Все записи были успешно удаленны!");
                        return;
                    }
            }
            else
            {
                MessageBox.Show("Нет записей для удаления!");
                return;
            }
        }

        public void OpenF()
        {
            Stream myStream = null;
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                if ((myStream = openFileDialog1.OpenFile()) != null)
                {
                    StreamReader myReader = new StreamReader(myStream);
                    string[] str;
                    int num;
                    try
                    {
                        string[] str1 = myReader.ReadToEnd().Split('\n');
                        num = str1.Count() - 1;
                        dataGridView1.RowCount = num;
                        for (int i = 0; i < num; i++)
                        {
                            str = str1[i].Split(' ');
                            for (int j = 0; j < dataGridView1.ColumnCount; j++)
                            {
                                dataGridView1.Rows[i].Cells[j].Value = str[j];
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        myReader.Close();
                    }
                }
            }
        }

        private void openFile_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount > 0)
            {
                if (MessageBox.Show("У вас уже открыт файл. После открытия нового файла, старый файл будет закрыт. Проверьте его сохранение! Продолжить?", "Открытие", MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.OK)
                {
                    OpenF();
                }
            }
            else
            {
                OpenF();

            }
        }

        private void saveFile_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount > 0)
            {
                dataGridView1.AllowUserToAddRows = false;
                Stream myStream;

                saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                saveFileDialog1.FilterIndex = 2;
                saveFileDialog1.RestoreDirectory = true;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if ((myStream = saveFileDialog1.OpenFile()) != null)
                    {
                        StreamWriter myWritet = new StreamWriter(myStream);
                        try
                        {
                            for (int i = 0; i < dataGridView1.RowCount; i++)
                            {
                                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                                {
                                    myWritet.Write(dataGridView1.Rows[i].Cells[j].Value.ToString() + " ");
                                }
                                myWritet.WriteLine();
                            }

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        finally
                        {
                            myWritet.Close();
                        }

                        myStream.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Нет записей для сохранения. Добавьте записи!");
                return;
            }
        }
    }
}
