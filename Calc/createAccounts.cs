using System;
using System.Windows.Forms;

namespace Calc
{
    public partial class createAccounts : MetroFramework.Forms.MetroForm
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
        public createAccounts(MainForm form)
        {
            InitializeComponent();
            MainForm = form;
        }

        // Обработчик события нажатия кнопки
        private void button1_Click(object sender, EventArgs e)
        {
            string accountName = textBox1.Text;
            if (IsValidAccountName(accountName))
            {
                MainForm.AddAccountRow(accountName);
                this.Close();
            }
            else
            {
                MessageBox.Show("Введите имя счёта.");
            }
        }

        // Метод для валидации имени счета
        private bool IsValidAccountName(string accountName)
        {
            return !string.IsNullOrEmpty(accountName);
        }
    }
}
