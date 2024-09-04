using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Calc
{
    public partial class MainForm : MetroFramework.Forms.MetroForm
    {
        // Приватные поля
        private int accountRowIndex = 0;
        private List<Control> accountControls = new List<Control>();
        private DatabaseHelper dbHelper;

        // Конструктор
        public MainForm()
        {
            InitializeComponent();
            string databasePath = "accounts.db"; // Укажите путь к вашей базе данных
            dbHelper = new DatabaseHelper(databasePath);
            LoadAccountsFromDatabase();
        }

        // Обработчик события нажатия кнопки создания счета
        private void button1_Click(object sender, EventArgs e)
        {
            var createAccountsForm = new createAccounts(this);
            createAccountsForm.Show();
        }

        // Метод для добавления строки счета
        public void AddAccountRow(string accountName, double value1 = 0, double value2 = 0, double balance = 0)
        {
            if (dbHelper.AccountExists(accountName))
            {
                MessageBox.Show("Счет с таким именем уже существует.");
                return;
            }

            AddAccountRowToInterface(accountName, value1, value2, balance);

            try
            {
                dbHelper.AddAccount(accountName, value1, value2, balance);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении счета: {ex.Message}");
            }
        }

        // Метод для добавления строки счета в интерфейс
        public void AddAccountRowToInterface(string accountName, double value1 = 0, double value2 = 0, double balance = 0)
        {
            int yOffset = 30 * accountRowIndex + 90; // Смещение по Y для новой строки

            Label accountLabel = new Label
            {
                Text = accountName,
                Location = new System.Drawing.Point(10, yOffset),
                AutoSize = true
            };
            this.Controls.Add(accountLabel);
            accountControls.Add(accountLabel);

            TextBox textBox1 = new TextBox
            {
                Location = new System.Drawing.Point(150, yOffset),
                Width = 100,
                Text = value1.ToString()
            };
            textBox1.KeyPress += TextBox_KeyPress;
            this.Controls.Add(textBox1);
            accountControls.Add(textBox1);

            TextBox textBox2 = new TextBox
            {
                Location = new System.Drawing.Point(300, yOffset), // Увеличьте значение X для увеличения расстояния
                Width = 100,
                Text = value2.ToString()
            };
            textBox2.KeyPress += TextBox_KeyPress;
            this.Controls.Add(textBox2);
            accountControls.Add(textBox2);

            Label balanceLabel = new Label
            {
                Location = new System.Drawing.Point(450, yOffset), // Увеличьте значение X для увеличения расстояния
                AutoSize = true,
                Text = balance.ToString()
            };
            this.Controls.Add(balanceLabel);
            accountControls.Add(balanceLabel);

            textBox1.TextChanged += (s, e) => UpdateBalance(textBox1, textBox2, balanceLabel);
            textBox2.TextChanged += (s, e) => UpdateBalance(textBox1, textBox2, balanceLabel);

            accountRowIndex++;
        }

        // Обработчик события нажатия клавиши в текстовом поле
        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Разрешить только цифры и управляющие символы
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        // Метод для обновления баланса
        private void UpdateBalance(TextBox textBox1, TextBox textBox2, Label balanceLabel)
        {
            if (double.TryParse(textBox1.Text, out double value1) && double.TryParse(textBox2.Text, out double value2))
            {
                balanceLabel.Text = (value2 - value1).ToString();
                string accountName = ((Label)accountControls[accountControls.IndexOf(balanceLabel) - 3]).Text;
                dbHelper.UpdateAccount(accountName, accountName, value1, value2, value2 - value1);
            }
            else
            {
                balanceLabel.Text = "Ошибка";
            }
        }

        // Обработчик события нажатия кнопки удаления счета
        private void button2_Click(object sender, EventArgs e)
        {
            var deleteAccountsForm = new deleteAccounts(this);
            deleteAccountsForm.Show();
        }

        // Метод для получения имен счетов
        public List<string> GetAccountNames()
        {
            List<string> accountNames = new List<string>();
            for (int i = 0; i < accountControls.Count; i += 4)
            {
                if (accountControls[i] is Label label)
                {
                    accountNames.Add(label.Text);
                }
            }
            return accountNames;
        }

        // Метод для удаления счета
        public void DeleteAccount(string accountName)
        {
            for (int i = 0; i < accountControls.Count; i++)
            {
                if (accountControls[i] is Label label && label.Text == accountName)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        this.Controls.Remove(accountControls[i]);
                        accountControls.RemoveAt(i);
                    }
                    accountRowIndex--;
                    RepositionAccounts(i);
                    dbHelper.DeleteAccount(accountName);
                    break;
                }
            }
        }

        // Метод для переупорядочивания счетов
        private void RepositionAccounts(int startIndex)
        {
            for (int i = startIndex; i < accountControls.Count; i += 4)
            {
                int yOffset = 30 * (i / 4) + 90;
                accountControls[i].Location = new System.Drawing.Point(10, yOffset);
                accountControls[i + 1].Location = new System.Drawing.Point(150, yOffset);
                accountControls[i + 2].Location = new System.Drawing.Point(260, yOffset);
                accountControls[i + 3].Location = new System.Drawing.Point(370, yOffset);
            }
        }

        // Метод для переименования счета
        public void RenameAccount(string oldName, string newName)
        {
            foreach (var control in accountControls)
            {
                if (control is Label label && label.Text == oldName)
                {
                    label.Text = newName;
                    dbHelper.UpdateAccount(oldName, newName, double.Parse(((TextBox)accountControls[accountControls.IndexOf(label) + 1]).Text), double.Parse(((TextBox)accountControls[accountControls.IndexOf(label) + 2]).Text), double.Parse(((Label)accountControls[accountControls.IndexOf(label) + 3]).Text));
                    break;
                }
            }
        }

        // Метод для загрузки счетов из базы данных
        private void LoadAccountsFromDatabase()
        {
            var accounts = dbHelper.GetAccounts();
            foreach (var account in accounts)
            {
                AddAccountRowToInterface(account.Name, account.Value1, account.Value2, account.Balance);
            }
        }
    }
}
