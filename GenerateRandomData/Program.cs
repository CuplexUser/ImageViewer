using System.Security.Cryptography;
using System.Text;

namespace GenerateRandomData
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Generating secure random bytes");
            if (args.Length == 0)
            {
                DisplayErrorAndExit("Length required");
            }
            string lengthArgument = args[0];

            if (string.IsNullOrWhiteSpace(lengthArgument))
            {
                DisplayErrorAndExit("Missing length argument");
            }

            if (!int.TryParse(lengthArgument, out int length))
            {
                DisplayErrorAndExit("failed to parse length");
            }

            string randomHexData = GenerateRandomBytes(length);
            Console.WriteLine(randomHexData);
        }

        private static string GenerateRandomBytes(int length)
        {
            var generator = RandomNumberGenerator.Create();
            if (length < 1)
                return "Zero length random";

            var rndData = new byte[length];
            generator.GetBytes(rndData);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("private static readonly byte[] HeaderBytes =");
            sb.AppendLine("{");

            for (int i = 0; i < rndData.Length; i++)
            {
                sb.Append("0x"+rndData[i].ToString("x2").ToUpper() + ", ");
            }

            sb.Remove(sb.Length - 3, 2);
            sb.AppendLine("");
            sb.AppendLine("};");

            return sb.ToString();
        }

        private static void DisplayErrorAndExit(string message)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(message);
            Console.ForegroundColor = originalColor;
            Environment.Exit(1);
        }
    }
}
