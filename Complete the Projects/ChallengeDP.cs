namespace ChallengeDP
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] firstLine = Array.ConvertAll(Console.ReadLine().Split(), int.Parse);
            int n = firstLine[0];
            int rating = firstLine[1];
            
            var positiveProjects = new List<(int a, int b)>();
            var negativeProjects = new List<(int a, int b)>();
 
            for (int i = 0; i < n; i++)
            {
                int[] projectData = Array.ConvertAll(Console.ReadLine().Split(), int.Parse);
                int a = projectData[0];
                int b = projectData[1];
                if (b >= 0)
                    positiveProjects.Add((a, b));
                else
                    negativeProjects.Add((a, b));
            }
            
            positiveProjects.Sort((p1, p2) => p1.a.CompareTo(p2.a));
            int countPositive = 0;
            foreach (var proj in positiveProjects)
            {
                if (rating >= proj.a)
                {
                    rating += proj.b;
                    countPositive++;
                }
            }
            
            negativeProjects.Sort((p1, p2) => (p2.a + p2.b).CompareTo(p1.a + p1.b));
            int m = negativeProjects.Count;
            
            const int NEG_INF = -1000000000;
            int[] dp = new int[m + 1];
            for (int i = 0; i <= m; i++)
                dp[i] = NEG_INF;
            dp[0] = rating;
 
            foreach (var proj in negativeProjects)
            {
                int req = proj.a;
                int change = proj.b;
                for (int j = m - 1; j >= 0; j--)
                {
                    if (dp[j] >= req && dp[j] + change >= 0)
                    {
                        dp[j + 1] = Math.Max(dp[j + 1], dp[j] + change);
                    }
                }
            }
 
            int bestNegative = 0;
            for (int j = 0; j <= m; j++)
            {
                if (dp[j] >= 0)
                    bestNegative = j;
            }
 
            Console.WriteLine(countPositive + bestNegative);
        }
    }
}