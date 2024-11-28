using System.Text.Json;

namespace ATM
{
    class UserList
    {
        private List<User> users;
        private const string FilePath = "users.json";

        public UserList()
        {
            users = LoadUsersFromFile(); // Load users from file
        }

        // Add a new user to the system
        public void AddUser(string name, int age, int pin, double money)
        {
            // Check if username already exists
            if (users.Any(u => u.UserName == name))
            {
                Console.WriteLine("This user name already exists.");
                return;
            }

            // Validate inputs
            if (string.IsNullOrWhiteSpace(name) || age <= 0)
            {
                throw new ArgumentException("All fields must contain valid information.");
            }
            if (pin < 1000 || pin > 9999)
            {
                throw new ArgumentException("PIN must be a 4-digit number.");
            }

            users.Add(new User(name, age, pin, money)); // Add new user
            SaveUsersToFile(); // Save updated user list
            Console.WriteLine("User added successfully!");
        }
        // Change PIN for a given account
        public void ChangePin(User account)
        {
            var foundAccount = users.FirstOrDefault(u => u.Id == account.Id);
            if (foundAccount != null)
            {
                Console.WriteLine("Enter old PIN Code");
                int.TryParse(Console.ReadLine(), out int oldPin);

                // Check if old PIN is correct
                if (oldPin == account.UserPin)
                {
                    Console.WriteLine("Enter new PIN Code");
                    int.TryParse(Console.ReadLine(), out int newPin);
                    if (newPin > 9999 || newPin < 1000)
                    {
                        Console.WriteLine("Pin must be 4 digits");
                        return;
                    }
                    foundAccount.UserPin = newPin; // Update PIN
                    SaveUsersToFile(); // Save updated list
                }
                else
                {
                    Console.WriteLine("Wrong Pin");
                }
            }
            else
            {
                Console.WriteLine("No Account found");
            }
        }

        // Withdraw money from account
        public void WithdrawMoney(User account)
        {
            var foundAccount = users.FirstOrDefault(u => u.Id == account.Id);
            if (foundAccount != null)
            {
                Console.WriteLine("Write the amount to withdraw");
                string input = Console.ReadLine();

                if (!int.TryParse(input, out int withdrawMoney)) // Validate input
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                    return;
                }

                if (foundAccount.UserMoney < withdrawMoney)
                {
                    Console.WriteLine("Insufficient balance.");
                }
                else
                {
                    foundAccount.UserMoney -= withdrawMoney; // Deduct money
                    SaveUsersToFile(); // Save changes
                    Console.WriteLine($"Successfully withdrawn {withdrawMoney}. New balance: {foundAccount.UserMoney}");
                }
            }
            else
            {
                Console.WriteLine("No account found.");
            }
        }
        // Deposit money into the account
        public void DepositMoney(User account)
        {
            var foundAccount = users.FirstOrDefault(u => u.Id == account.Id);
            if (foundAccount != null)
            {
                Console.WriteLine("Write the amount to Deposit");
                string input = Console.ReadLine();

                if (!int.TryParse(input, out int depositMoney)) // Validate input
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                    return;
                }

                foundAccount.UserMoney += depositMoney; // Add money
                SaveUsersToFile(); // Save changes
                Console.WriteLine($"Successfully deposited {depositMoney}. New balance: {foundAccount.UserMoney}");
            }
        }

        // Transfer money to another user
        public void TransferMoney(User account, string friendName)
        {
            var foundAccount = users.FirstOrDefault(u => u.Id == account.Id);
            var friendAccount = users.FirstOrDefault(u => u.UserName == friendName);

            if (friendAccount == null)
            {
                Console.WriteLine("No account found with this name.");
                return;
            }

            if (foundAccount != null)
            {
                Console.WriteLine("How much do you want to transfer?");
                string userInp = Console.ReadLine();

                if (!int.TryParse(userInp, out int transferMoney)) // Validate input
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                    return;
                }

                if (transferMoney > foundAccount.UserMoney)
                {
                    Console.WriteLine("You have not enough money to transfer");
                    return;
                }

                foundAccount.UserMoney -= transferMoney; // Deduct from sender
                friendAccount.UserMoney += transferMoney; // Add to recipient
                SaveUsersToFile(); // Save changes
                Console.WriteLine($"Successfully transferred {transferMoney} to {friendAccount.UserName}. New balance: {foundAccount.UserMoney}");
            }
        }
        // Account management menu
        public void AccountManagment(string name, int pin)
        {
            var foundAccount = users.FirstOrDefault(u =>
                u.UserName.Contains(name, StringComparison.OrdinalIgnoreCase) &&
                u.UserPin == pin);

            if (foundAccount != null)
            {
                Console.WriteLine("Account found: ");
                Console.WriteLine(foundAccount);

                Console.WriteLine("Select Option");
                Console.WriteLine("1 - Change Pin");
                Console.WriteLine("2 - Withdraw Money");
                Console.WriteLine("3 - Deposit money");
                Console.WriteLine("4 - Transfer Money");
                Console.WriteLine("5 - Exit");

                string userInp = Console.ReadLine();
                switch (userInp)
                {
                    case "1": ChangePin(foundAccount); break;
                    case "2": WithdrawMoney(foundAccount); break;
                    case "3": DepositMoney(foundAccount); break;
                    case "4":
                        Console.WriteLine("Who to transfer money to? (User Name)");
                        string transferPerson = Console.ReadLine();
                        TransferMoney(foundAccount, transferPerson);
                        break;
                    default: Console.WriteLine("Thanks, Have a good day."); break;
                }
            }
            else
            {
                Console.WriteLine("No account found with the given name and PIN.");
            }
        }

        // Display list of users
        public void DisplayUsers()
        {
            if (!users.Any()) // If no users exist
            {
                Console.WriteLine("The user list is empty.");
                return;
            }

            Console.WriteLine("User List:");
            foreach (var user in users)
            {
                Console.WriteLine(user); // Print user info
            }
        }
        // Load users from JSON file
        private List<User> LoadUsersFromFile()
        {
            if (!File.Exists(FilePath))
            {
                return new List<User>(); // If file doesn't exist, return empty list
            }

            try
            {
                string json = File.ReadAllText(FilePath);
                return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
            }
            catch
            {
                Console.WriteLine("Error loading users from file.");
                return new List<User>();
            }
        }

        // Save updated user list to JSON file
        private void SaveUsersToFile()
        {
            try
            {
                string json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(FilePath, json);
                Console.WriteLine("Users saved to file.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving users: {ex.Message}");
            }
        }
    }
}

