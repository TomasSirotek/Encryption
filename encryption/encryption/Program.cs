
using System.Security.Cryptography;
using System.Text;


class Program
{
    static void Main(string[] args)
    {
       
        Console.Write("Enter your passphrase: ");
        string passphrase = Console.ReadLine();


        int keySize = 32;
        if (passphrase.Length < keySize)
        {

            passphrase = passphrase.PadRight(keySize);
        }
        else if (passphrase.Length > keySize)
        {

            passphrase = passphrase.Substring(0, keySize);
        }

        using (AesGcm aesGcm = new AesGcm(Encoding.UTF8.GetBytes(passphrase)))
        {
            while (true)
            {
                Console.WriteLine("Menu:");
                Console.WriteLine("1: Safely store message");
                Console.WriteLine("2: Read message");
                Console.WriteLine("0: Exit");
                Console.Write("Enter your choice (1/2/0): ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        StoreMessage(aesGcm);
                        break;
                    case "2":
                        ReadMessage(aesGcm);
                        break;
                    case "0":
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("Exiting the application.");
                        Console.ResetColor();
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please enter 1, 2, or 0.");
                        break;
                }
            }
        }
    }

    static void StoreMessage(AesGcm aesGcm)
    {
        Console.Write("Enter the message to store: ");
        string message = Console.ReadLine();
        byte[] nonce = new byte[12];
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(nonce);
        }

        byte[] plaintext = Encoding.UTF8.GetBytes(message);

        // Encrypt the message
        byte[] ciphertext = new byte[plaintext.Length];
        byte[] tag = new byte[16];
        aesGcm.Encrypt(nonce, plaintext, ciphertext, tag, null);


        using (FileStream fs = new FileStream("encrypted_message.bin", FileMode.Create))
        {
            fs.Write(nonce);
            fs.Write(tag);
            fs.Write(ciphertext);
        }
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Message encrypted and saved to 'encrypted_message.bin'");
        Console.ResetColor();
    }

    static void ReadMessage(AesGcm aesGcm)
    {
        try
        {
            byte[] data = File.ReadAllBytes("encrypted_message.bin");
            byte[] nonce = new byte[12];
            byte[] tag = new byte[16];
            byte[] ciphertext = new byte[data.Length - nonce.Length - tag.Length];

            Array.Copy(data, nonce, nonce.Length);
            Array.Copy(data, nonce.Length, tag, 0, tag.Length);
            Array.Copy(data, nonce.Length + tag.Length, ciphertext, 0, ciphertext.Length);

            byte[] decryptedMessage = new byte[ciphertext.Length];
            aesGcm.Decrypt(nonce, ciphertext, tag, decryptedMessage);

            string decryptedText = Encoding.UTF8.GetString(decryptedMessage);
            
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Decrypted message: " + decryptedText);
            Console.ResetColor();
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("No message found. Please store a message first.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}