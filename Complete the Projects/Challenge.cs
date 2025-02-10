namespace Challenge
{
    class Pair
    {
        public int X;
        public int Y;
        public Pair(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
 
    class Challenge
    {
        static int result, n, r;
        static List<Pair> positive = new();
        static List<Pair> negative = new();
 
        static void Main(string[] args)
        {
            Input();
            Solve();
            Console.WriteLine(result);
        }
 
        static void Input()
        {
            var parts = Console.ReadLine().Split();
            n = int.Parse(parts[0]);
            r = int.Parse(parts[1]);
 
            for (int i = 0; i < n; i++)
            {
                var line = Console.ReadLine().Split();
                int x = int.Parse(line[0]);
                int y = int.Parse(line[1]);
 
                if (y < 0)
                    negative.Add(new Pair(x, y));
                else
                    positive.Add(new Pair(x, y));
            }
        }
 
        static void MaxPos()
        {
            positive.Sort((a, b) => a.X.CompareTo(b.X));
            foreach (var challenge in positive)
            {
                if (r >= challenge.X)
                {
                    r += challenge.Y;
                    result++;
                }
            }
        }
 
        class MultiSet
        {
            private SortedDictionary<int, int> dictionary = new();
            public bool IsEmpty => dictionary.Count == 0;
 
            public void Insert(int key)
            {
                if (dictionary.ContainsKey(key))
                    dictionary[key]++;
                else
                    dictionary[key] = 1;
            }
 
            public int GetMin()
            {
                return dictionary.First().Key;
            }
 
            public void Remove(int key)
            {
                if (dictionary.ContainsKey(key))
                {
                    dictionary[key]--;
                    if (dictionary[key] == 0)
                        dictionary.Remove(key);
                }
            }
        }
 
        static void Schedule()
        {
            MultiSet multiSet = new MultiSet();
 
            for (int i = negative.Count - 1; i >= 0; i--)
            {
                int current = Math.Max(0, negative[i].X);
                int previous = i > 0 ? Math.Max(negative[i - 1].X, 0) : 0;
                int diference = current - previous;
 
                multiSet.Insert(-negative[i].Y);
 
                while (!multiSet.IsEmpty && diference > 0)
                {
                    int x = multiSet.GetMin();
                    multiSet.Remove(x);
 
                    if (diference >= x)
                    {
                        result++;
                        diference -= x;
                    }
                    else
                    {
                        x -= diference;
                        diference = 0;
                        multiSet.Insert(x);
                    }
                }
            }
        }
        static void Solve()
        {
            MaxPos();
            
            foreach (var p in negative)
            {
                p.X = r - p.X;
                p.X -= p.Y;
                p.X = Math.Min(p.X, r);
            }
            
            negative.Sort((a, b) => a.X.CompareTo(b.X));
            Schedule();
        }
    }
}