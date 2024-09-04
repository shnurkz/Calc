using System;
using System.Windows.Forms;

namespace Calc
{
    public partial class deleteAccounts : MetroFramework.Forms.MetroForm
    {
        // Приватное поле
        private MainForm mainForm;

        // Публичное свойство для доступа к mainForm
        public MainForm MainForm
        {
            get { return mainForm; }
            private set { mainForm = value; }
        }

        // Конструктор
        public deleteAccounts(MainForm form)
        {
            InitializeComponent();
            MainForm = form;
            LoadAccountNames();
        }

        // Метод для загрузки имен счетов
        private void LoadAccountNames()
        {
            listBox1.Items.Clear();
            var accountNames = MainForm.GetAccountNames();
            foreach (var name in accountNames)
            {
                listBox1.Items.Add(name);
            }
        }

        // Обработчик события нажатия кнопки удаления
        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                string selectedAccount = listBox1.SelectedItem.ToString();
                MainForm.DeleteAccount(selectedAccount);
                LoadAccountNames();
            }
        }

        // Обработчик события нажатия кнопки переименования
        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null && IsValidAccountName(textBox1.Text))
            {
                string selectedAccount = listBox1.SelectedItem.ToString();
                string newAccountName = textBox1.Text;
                MainForm.RenameAccount(selectedAccount, newAccountName);
                LoadAccountNames();
            }
        }

        // Метод для валидации имени счета
        private bool IsValidAccountName(string accountName)
        {
            return !string.IsNullOrEmpty(accountName);
        }

        // Обработчик события клика по метке
        private void label1_Click(object sender, EventArgs e)
        {
            // Логика для клика по метке
        }

        // Обработчик события изменения выбранного элемента в списке
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Логика для изменения выбранного элемента в списке
        }

        // Обработчик события изменения текста в текстовом поле
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Логика для изменения текста в текстовом поле
        }
    }
}
