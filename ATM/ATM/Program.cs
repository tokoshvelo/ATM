
namespace ATM
{

    internal class Program
    {
        static void Main(string[] args)
        {
            UserList user = new UserList();
            bool isOpen = true;


            while (isOpen)
            {
                Console.WriteLine("\n1 - Add Client");
                Console.WriteLine("2 - Sign in to your account");
                Console.WriteLine("3 - See all Clients");
                Console.WriteLine("4 - Exit");

                Console.Write("Enter your choice: ");
                string userInp = Console.ReadLine();
                Console.Clear();
                switch (userInp)
                {
                    case "1":
                        Console.WriteLine("Add Client\n");
                        Console.Write("Enter User name: ");
                        string name = Console.ReadLine();

                        try
                        {
                            Console.Write("Enter user's age: ");
                            if (!int.TryParse(Console.ReadLine(), out int age) || age <= 0)
                            {
                                Console.WriteLine("Invalid age. Please enter a positive number.");
                                break;
                            }

                            Console.Write("Enter user's pin: ");
                            if (!int.TryParse(Console.ReadLine(), out int pin) || pin < 1000 || pin > 9999)
                            {
                                Console.WriteLine("Invalid PIN. It must be a 4-digit number.");
                                break;
                            }

                            Console.Write("Enter user's money: ");
                            if (!double.TryParse(Console.ReadLine(), out double money) || money < 0)
                            {
                                Console.WriteLine("Invalid money amount. Please enter a non-negative value.");
                                break;
                            }

                            user.AddUser(name, age, pin, money);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                        break;
                    case "2":
                        Console.WriteLine("Client Information");
                        Console.WriteLine("Enter your name: ");
                        string userName = Console.ReadLine();
                        Console.WriteLine("Enter your pin: ");
                        try
                        {
                            int.TryParse(Console.ReadLine(), out int userPin);
                            user.AccountManagment(userName, userPin);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;

                    case "3":
                        user.DisplayUsers();
                        break;

                    case "4":
                        isOpen = false;
                        Console.WriteLine("Program exited.");
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }

        }
    }


}