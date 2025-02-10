public class Edge {
    public int To, Capacity, Cost, Rev, OrigCap;
    public Edge(int to, int capacity, int cost, int rev) {
        To = to;
        Capacity = capacity;
        Cost = cost;
        Rev = rev;
        OrigCap = capacity;
    }
}
 
public class MinCostMaxFlow {
    public int V;
    public List<Edge>[] graph;
    public MinCostMaxFlow(int V) {
        this.V = V;
        graph = new List<Edge>[V];
        for (int i = 0; i < V; i++) {
            graph[i] = new List<Edge>();
        }
    }
    public void AddEdge(int s, int t, int cap, int cost) {
        graph[s].Add(new Edge(t, cap, cost, graph[t].Count));
        graph[t].Add(new Edge(s, 0, -cost, graph[s].Count - 1));
    }
    public (int flow, int cost) MinCostFlow(int source, int sink) {
        int flow = 0, cost = 0;
        int[] dist = new int[V];
        int[] potential = new int[V];
        int[] prevV = new int[V];
        int[] prevE = new int[V];
        while (true) {
            for (int i = 0; i < V; i++)
                dist[i] = int.MaxValue;
            dist[source] = 0;
            bool[] inQueue = new bool[V];
            Queue<int> queue = new Queue<int>();
            queue.Enqueue(source);
            inQueue[source] = true;
            while (queue.Count > 0) {
                int v = queue.Dequeue();
                inQueue[v] = false;
                for (int i = 0; i < graph[v].Count; i++) {
                    Edge e = graph[v][i];
                    if (e.Capacity > 0 && dist[e.To] > dist[v] + e.Cost + potential[v] - potential[e.To]) {
                        dist[e.To] = dist[v] + e.Cost + potential[v] - potential[e.To];
                        prevV[e.To] = v;
                        prevE[e.To] = i;
                        if (!inQueue[e.To]) {
                            queue.Enqueue(e.To);
                            inQueue[e.To] = true;
                        }
                    }
                }
            }
            if (dist[sink] == int.MaxValue) break;
            for (int v = 0; v < V; v++) {
                if (dist[v] < int.MaxValue)
                    potential[v] += dist[v];
            }
            int addFlow = int.MaxValue;
            for (int v = sink; v != source; v = prevV[v])
                addFlow = Math.Min(addFlow, graph[prevV[v]][prevE[v]].Capacity);
            flow += addFlow;
            cost += addFlow * potential[sink];
            for (int v = sink; v != source; v = prevV[v]) {
                Edge e = graph[prevV[v]][prevE[v]];
                e.Capacity -= addFlow;
                graph[v][e.Rev].Capacity += addFlow;
            }
        }
        return (flow, cost);
    }
}
 
public class Program {
    public static void Main(string[] args) {
        string[] parts = Console.ReadLine().Split();
        int d = int.Parse(parts[0]);
        int n = int.Parse(parts[1]);
        
        int[] masks = new int[n];
        int[] popcounts = new int[n];
        for (int i = 0; i < n; i++) {
            string s = Console.ReadLine().Trim();
            int m = 0;
            int cnt = 0;
            for (int j = 0; j < d; j++) {
                if (s[j] == '1') {
                    m |= (1 << j);
                    cnt++;
                }
            }
            masks[i] = m;
            popcounts[i] = cnt;
        }
        
        int totalNodes = 2 * n + 2;
        int source = 0;
        int sink = totalNodes - 1;
        MinCostMaxFlow mcmf = new MinCostMaxFlow(totalNodes);
        
        for (int i = 0; i < n; i++) {
            mcmf.AddEdge(source, 1 + i, 1, 0);
        }
        for (int i = 0; i < n; i++) {
            mcmf.AddEdge(1 + n + i, sink, 1, 0);
        }
        for (int i = 0; i < n; i++) {
            for (int j = 0; j < n; j++) {
                if (i == j) continue;
                if ((masks[i] & masks[j]) == masks[i] && masks[i] != masks[j]) {
                    mcmf.AddEdge(1 + i, 1 + n + j, 1, -(popcounts[i] + 1));
                }
            }
        }

        var result = mcmf.MinCostFlow(source, sink);
        int totalSaving = -result.cost; // M = ahorro total
        
        int costSeparate = 0;
        for (int i = 0; i < n; i++) {
            costSeparate += (popcounts[i] + 1);
        }
        
        int[] matchLeft = new int[n];
        int[] matchRight = new int[n];
        for (int i = 0; i < n; i++) {
            matchLeft[i] = -1;
            matchRight[i] = -1;
        }
        for (int i = 0; i < n; i++) {
            int u = 1 + i;
            foreach (var edge in mcmf.graph[u]) {
                if (edge.To >= 1 + n && edge.To < 1 + n + n && edge.OrigCap == 1 && edge.Capacity == 0) {
                    int j = edge.To - (1 + n);
                    matchLeft[i] = j;
                    matchRight[j] = i;
                }
            }
        }
        
        bool[] used = new bool[n];
        List<List<int>> chains = new List<List<int>>();
        for (int i = 0; i < n; i++) {
            if (matchRight[i] == -1 && !used[i]) {
                List<int> chain = new List<int>();
                int cur = i;
                while (cur != -1 && !used[cur]) {
                    used[cur] = true;
                    chain.Add(cur);
                    cur = matchLeft[cur];
                }
                chains.Add(chain);
            }
        }
        for (int i = 0; i < n; i++) {
            if (!used[i]) {
                List<int> chain = new List<int> { i };
                chains.Add(chain);
            }
        }

        List<string> moves = new List<string>();
        bool firstChain = true;
        foreach (var chain in chains) {
            if (!firstChain) {
                moves.Add("R");
            }
            firstChain = false;
            int state = 0;
            foreach (int idx in chain) {
                int req = masks[idx] & (~state);
                for (int bit = 0; bit < d; bit++) {
                    if ((req & (1 << bit)) != 0) {
                        moves.Add(bit.ToString());
                        state |= (1 << bit);
                    }
                }
            }
        }
        
        Console.WriteLine(moves.Count);
        Console.WriteLine(string.Join(" ", moves));
    }
}