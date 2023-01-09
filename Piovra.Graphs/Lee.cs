using System.Collections.Generic;
using System.Linq;

namespace Piovra.Graphs;
public class Lee {
    public class Request {
        public int[,] A { get; set; }
        public int StartX { get; set; }
        public int StartY { get; set; }
        public int EndX { get; set; }
        public int EndY { get; set; }
    }

    public class Response {
        public List<Cell> Route { get; } = new List<Cell>();
        public bool HasRoute() => Route.NonEmpty();
    }

    public class Cell {
        public int X { get; set; }
        public int Y { get; set; }
        public int N { get; set; }
        public bool Discovered { get; set; }
        public bool Visited { get; set; }
    }

    const int DIRS = 4;

    static readonly int[] dx = { 1, 0, -1, 0 };
    static readonly int[] dy = { 0, -1, 0, 1 };

    public static Response Execute(Request request) {
        var res = new Response();

        var rows = request.A.Rows();
        var cols = request.A.Cols();

        var cc = new Cell[rows, cols];

        int bx = request.StartX;
        int by = request.StartY;

        int ex = request.EndX;
        int ey = request.EndY;

        var bc = cc[bx, by];
        bc.N = 1;
        bc.X = bx;
        bc.Y = by;
        bc.Visited = true;

        var q = new Queue<Cell>();

        bool stop = false;
        int x, y, num, nx, ny;

        while (!stop && q.Any()) {
            var c = q.Dequeue();
            x = c.X;
            y = c.Y;
            num = c.N + 1;

            for (int i = 0; i < DIRS; ++i) {
                nx = x + dx[i];
                ny = y + dy[i];

                if (nx >= 0 && nx < cols && ny >= 0 && ny < rows && !cc[nx, ny].Visited &&
                    request.A[nx, ny] == 0) {
                    cc[nx, ny].X = nx;
                    cc[nx, ny].Y = ny;
                    cc[nx, ny].N = num;
                    cc[nx, ny].Visited = true;

                    if (nx == ex && ny == ey) {
                        stop = true;
                        break;
                    }

                    q.Enqueue(cc[nx, ny]);
                }
            }
        }

        if (stop) {
            x = ex;
            y = ey;
            num = cc[x, y].N;
            while (x != bx || y != by) {
                for (int i = 0; i < DIRS; ++i) {
                    nx = x + dx[i];
                    ny = y + dy[i];
                    if (cc[nx, ny].N == num - 1) {
                        num = cc[nx, ny].N;
                        res.Route.Add(cc[nx, ny]);
                        x = nx;
                        y = ny;
                        break;
                    }
                }
            }
        }

        return res;
    }
}