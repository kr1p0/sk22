using System.Text;

namespace SK.Models
{
    public class CustomStr
    {
        public CustomStr() { }

        public static string Adjust(string input, int length = 14)
        {
            if (string.IsNullOrEmpty(input)) return "┅";
            var result = input.Length > length ? input.Substring(0, length) + "..." : input;
            return result;
        }

        public static string GenerateRandomString(int stringLength)
        {
            StringBuilder sb = new StringBuilder();
            char[] alphabet = Enumerable.Range('A', 26).Select(x => (char)x).ToArray();
            char[] alphabet2 = Enumerable.Range('a', 26).Select(x => (char)x).ToArray();
            alphabet = alphabet.Concat(alphabet2).ToArray();
            Random rnd = new Random();
            for (int i = 0; i < stringLength; i++)
            {
                int rndIndex = rnd.Next(0, alphabet.Length - 1);
                sb.Append(alphabet[rndIndex]);
            }
            return sb.ToString();
        }

        public static int getRandomNumber()
        {
            //look for unique number
            int count = 0;
            int min = 10000000;
            int max = 100000000;
            int result;
            Random rnd = new Random();
            do
            {
                result = rnd.Next(min, max);
                count++;
                if (count > 1000) //if there will be  not enough unique numbers
                {
                    max = 2000000000;
                }

            }
            while (Case.CheckIfUnique(result) == false);
            return result;
        }
    }
}
