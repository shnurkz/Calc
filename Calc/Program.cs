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

            // ������������� � ������ ����������
            InitializeAndRun();
        }

        // ����� ��� ������������� ���� ������ � ������� ������� �����
        private static void InitializeAndRun()
        {
            // ������������� ���� ������
            var dbHelper = new DatabaseHelper("accounts.db");

            // ������ ������� �����
            Application.Run(new MainForm());
        }
    }
}
