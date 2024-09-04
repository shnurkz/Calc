using System;
using System.Windows.Forms;

namespace Calc
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Инициализация и запуск приложения
            InitializeAndRun();
        }

        // Метод для инициализации базы данных и запуска главной формы
        private static void InitializeAndRun()
        {
            // Инициализация базы данных
            var dbHelper = new DatabaseHelper("accounts.db");

            // Запуск главной формы
            Application.Run(new MainForm());
        }
    }
}
