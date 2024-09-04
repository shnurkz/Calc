namespace Calc
{
    public class Account
    {
        // Приватные поля
        private int id;
        private string name;
        private string labelText;
        private string textBox1Value;
        private string textBox2Value;
        private double balance;

        // Публичные свойства для доступа к полям
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string LabelText
        {
            get { return labelText; }
            set { labelText = value; }
        }

        public string TextBox1Value
        {
            get { return textBox1Value; }
            set { textBox1Value = value; }
        }

        public string TextBox2Value
        {
            get { return textBox2Value; }
            set { textBox2Value = value; }
        }

        public double Balance
        {
            get { return balance; }
            set { balance = value; }
        }

        // Конструкторы
        public Account() { }

        public Account(int id, string name, string labelText, string textBox1Value, string textBox2Value, double balance)
        {
            this.id = id;
            this.name = name;
            this.labelText = labelText;
            this.textBox1Value = textBox1Value;
            this.textBox2Value = textBox2Value;
            this.balance = balance;
        }

        // Метод для обновления баланса
        public void UpdateBalance(double value1, double value2)
        {
            this.textBox1Value = value1.ToString();
            this.textBox2Value = value2.ToString();
            this.balance = value2 - value1;
        }

        // Переопределение метода ToString для удобного отображения информации об аккаунте
        public override string ToString()
        {
            return $"Account: {name}, Balance: {balance}";
        }
    }
}
