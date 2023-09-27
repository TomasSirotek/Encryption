using encryption;

// Colorz
const ConsoleColor DefaultForegroundColor = ConsoleColor.White;
const ConsoleColor ErrorForegroundColor = ConsoleColor.Red;
const ConsoleColor SuccessForegroundColor = ConsoleColor.Green;

Console.WriteLine("Passphrase:");
string passphrase = Console.ReadLine();

while (true)
{
    Console.Clear();
    Console.ForegroundColor = DefaultForegroundColor;

    Console.WriteLine("1: Safely store message");
    Console.WriteLine("2: Read message");
    Console.WriteLine("0: Exit");

    if (int.TryParse(Console.ReadLine(), out int choice))
    {
        switch (choice)
        {
            case 1:
                Console.Write("Type a message to encrypt: ");
                string message = Console.ReadLine();
                string encryptedMessage = Helper.EncryptMessage(message, passphrase);
                Console.ForegroundColor = SuccessForegroundColor;
                Console.WriteLine("Encrypted Message: " + encryptedMessage);
                Console.ResetColor();
                break;

            case 2:
                Console.Write("Enter the encrypted message: ");
                string encryptedInput = Console.ReadLine();
                string decryptedMessage = Helper.DecryptMessage(encryptedInput, passphrase);
                if (decryptedMessage != null)
                {
                    Console.ForegroundColor = SuccessForegroundColor;
                    Console.WriteLine("Decrypted Message: " + decryptedMessage);
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ErrorForegroundColor;
                    Console.WriteLine("Invalid encrypted message or passphrase.");
                    Console.ResetColor();
                }

                break;

            case 0:
                return;

            default:
                Console.ForegroundColor = ErrorForegroundColor;
                Console.WriteLine("Invalid choice. Please try again.");
                Console.ResetColor();
                break;
        }
    }
    else
    {
        Console.ForegroundColor = ErrorForegroundColor;
        Console.WriteLine("Invalid input. Please enter a valid option.");
        Console.ResetColor();
    }

    Console.WriteLine("Press any key to continue...");
    Console.ReadKey(true);
}