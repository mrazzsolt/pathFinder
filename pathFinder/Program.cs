using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pathFinder
{
    
    class Program
    {
        private static char pathway = '.';
        private static char blocked = 'B';
        private static char path = 'p';

        public static int[,] ways = new int[4,2] { { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 } };
        public static Queue<Node> Q = new Queue<Node>();
        public struct Node
        {
            public int x, y;
            public int steps;
        }


        public static bool findMazePath(int x, int y, char[][] maze, int N)
        {
            if (x < 0 || y < 0 || x >= N || y >= N) { return false; }
            else if (maze[x][y] != pathway) { return false; }
            else if (x == N - 1 && y == N - 1)
            {
                maze[x][y] = path;
                return true;
            }
            else
            {
                maze[x][y] = path;
                if (findMazePath(x - 1, y, maze, N) ||
                  findMazePath(x, y + 1, maze, N) ||
                  findMazePath(x + 1, y, maze, N) ||
                  findMazePath(x, y - 1, maze, N))
                {
                    return true;
                }
                maze[x][y] = blocked;
                return false;
            }

        }
        public static bool pathFinder(string maze1)
        {
            //pálya átalakítása, hogy bejárható legyen
            int N = 0;
            int count = 0;
            char[] chs = maze1.ToCharArray();

            for (int i = 0; i < chs.Length; i++)
            {
                if (chs[i] == '\n') { N = i; break; }
            }
            if (N == 0) { return true; }
            char[][] maze;
            maze = new char[N][];
            for (int i = 0; i < N; i++)
            {
                maze[i] = new char[N];
            }
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (chs[count] == '\n')
                        count++;
                    maze[i][j] = chs[count];
                    count++;
                }
            }
            //útvonal keresése
            findMazePath(0, 0, maze, N);

            if (maze[N - 1][N - 1] == path) { return true; }
            else { return false; }
        }
        public static int findShortestPath(int length, string maze)
        {
            while (Q.Count != 0)
            {
                Node now = Q.Dequeue();
                for (int i = 0; i < 4; i++)
                {
                    int nowX = now.x + ways[i, 0];
                    int nowY = now.y + ways[i, 1];
                    if (nowX == length - 1 && nowY == length - 1)
                    {
                        return ++now.steps;
                    }
                    if (nowX < 0 || nowX >= length || nowY < 0 || nowY >= length)
                    {
                        continue;
                    }
                    if (maze[nowX * (length + 1) + nowY] == 'W')
                    {
                        continue;
                    }
                    maze = maze.Remove(nowX * (length + 1) + nowY, 1).Insert(nowX * (length + 1) + nowY, "W");
                    Node tmp;
                    tmp.x = nowX;
                    tmp.y = nowY;
                    tmp.steps = now.steps + 1;
                    Q.Enqueue(tmp);
                }
            }
            return -1;
        }
        public static int pathFind(string maze)
        {
            int length = (int)Math.Sqrt(maze.Length);
            if(length == 1)
            {
                return 0;
            }
            while (Q.Count != 0)
            {
                Q.Dequeue();
            }
            maze = maze.Remove(0, 1).Insert(0, "W");
            Node tmp;
            tmp.x = tmp.y = tmp.steps = 0;
            Q.Enqueue(tmp);
            return findShortestPath(length, maze);
        }
        static void Main(string[] args)
        {

            string a = ".....\n" +
                       ".WWW.\n" +
                       "..W..\n" +
                       "W.W.W\n" +
                       ".....";
            //Végig lehet menni ?
            Console.WriteLine("Végig tudunk menni?");
            Console.WriteLine(pathFinder(a));
            Console.WriteLine("Legrövidebb út hossza {0} lépés. ",pathFind(a));


            Console.ReadKey();
        }
    }
}
