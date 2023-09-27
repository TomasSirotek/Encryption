using System.Text;

namespace encryption;

public abstract class Helper
{
    public static string EncryptMessage(string message, string passphrase)
    {
        byte[] binary = Encoding.UTF8.GetBytes(message);
        string encodedMessage = Convert.ToBase64String(binary);
        return encodedMessage;
    }
    public static string DecryptMessage(string encodedMessage, string passphrase)
    {
        try
        {
            byte[] binary = Convert.FromBase64String(encodedMessage);
            string message = Encoding.UTF8.GetString(binary);
            return message;
        }
        catch (Exception)
        { 
            return null;
        }
    }

}