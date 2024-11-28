
namespace ATM
{
    internal class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int UserAge { get; set; }
        public int UserPin { get; set; }
        public double UserMoney { get; set; }

        // Parameterized constructor
        public User(string name, int age, int pin, double money)
        {
            UserName = name;
            UserAge = age;
            UserPin = pin;
            UserMoney = money;
            Random random = new Random();
            Id = random.Next(1, 90000) * 2;
        }

        // Default constructor (required for JSON deserialization)
        public User() { }

        public override string ToString()
        {
            return $"Name: {UserName}, Age: {UserAge}, PIN: {UserPin}, Money: {UserMoney}, Id: {Id}";
        }
    }
}