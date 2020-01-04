using System;
using System.Threading.Tasks;

namespace Lab8
{
    class Program
    {
        private int v;
        private int[,] graph;

        void findHamiltonianCycle(int[,] g)
        {
            v = g.Length;
            int[] path = new int[v];

            for (int i = 0; i < path.Length; i++)
                path[i] = -1;

            graph = g;
            try
            {
                path[0] = 0;
                solve(0,path,1);
                Console.WriteLine("Solution not found");

            }catch(Exception e)
            {
                if (!e.Message.Contains("Solution found!"))
                    throw new Exception(e.Message);
            }
        }

        private void solve(int vertex, int[] path, int count)
        {
            if (graph[vertex,0] == 1 && count == v)
            {
                display(path);
                throw new Exception("Solution found!");
            }
            if (count == v)
                return;

            for (int i = 0; i < v; i++) {

                if (graph[vertex, i] == 1) {
                    /** add to path **/
                    path[count++] = i;
                    /** remove connection **/
                    graph[vertex, i] = 0;
                    graph[i, vertex] = 0;

                    /** if vertex not already selected  solve recursively **/
                    if (!isPresent(i, path, count)) {
                        int current = i;
                        int currentCount = count;

                        Task task1 = Task.Run(() =>
                        {
                            try {
                                solve(current, path, currentCount);
                            } catch (Exception e) {
                                throw new Exception(e.Message);
                            }
                        });
                        task1.Wait();
                    }

                    /** restore connection **/
                    graph[vertex, i] = 1;
                    graph[i, vertex] = 1;
                    /** remove path **/
                    path[--count] = -1;
                }
            }
        }

        private bool isPresent(int v, int[] path, int count)
        {
            for (int i = 0; i < count - 1; i++)
                if (path[i] == v)
                    return true;
            return false;
        }
        private void display(int[] path)
        {
            Console.WriteLine("Path: \n");
            for(int i =0; i< v ;i++)
                Console.WriteLine(path[i%v] + " ");
        }
        static void Main(string[] args)
        {
            int[,] g = new int[,]{
                        {0, 1, 0, 1, 1},
                        {0, 0, 1, 1, 0},
                        {0, 1, 0, 1, 1},
                        {1, 1, 1, 0, 1},
                        {0, 1, 1, 0, 0},
                    };
            Program P = new Program();
            P.findHamiltonianCycle(g);
            Console.WriteLine();
        }
    }
}
