using System.Text;

namespace Xeovis.Base.Utilities
{
    public static class StringUtilities
    {
        public static int GetLevenshteinDistance(string a, string b)
        {
            int[,] dp = new int[a.Length + 1, b.Length + 1];

            for (int i = 0; i <= a.Length; i++)
                dp[i, 0] = i;
            for (int j = 0; j <= b.Length; j++)
                dp[0, j] = j;

            for (int i = 1; i <= a.Length; i++)
            {
                for (int j = 1; j <= b.Length; j++)
                {
                    int cost = (a[i - 1] == b[j - 1]) ? 0 : 1;                  // Substitution Cost
                    var deletion = dp[i - 1, j] + 1;                            // Deletion
                    var insertion = dp[i, j - 1] + 1;                           // Insertion
                    var substitution = dp[i - 1, j - 1] + cost;                 // Substitution
                    dp[i, j] = Math.Min(Math.Min(deletion, insertion), substitution);
                }
            }

            return dp[a.Length, b.Length];
        }

        public static string WrapTextToLines(string text, int maxLineWidth)
        {
            string[] words = text.Split(' ');
            StringBuilder newSentence = new StringBuilder();
            
            string line = "";
            foreach (string word in words)
            {
                if ((line + word).Length > maxLineWidth)
                {
                    newSentence.AppendLine(line);
                    line = "";
                }
            
                line += string.Format("{0} ", word);
            }
            
            if (line.Length > 0) newSentence.AppendLine(line);
            return newSentence.ToString();
        }
    }
}
