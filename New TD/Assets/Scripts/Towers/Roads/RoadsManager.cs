using UnityEngine;
using System.Collections.Generic;

public class RoadsManager : MonoBehaviour // отвечает за дороги
{
    public bool[,] grid;

    void Start()
    {
        grid = new bool[PlayerStats.gridSize*2, PlayerStats.gridSize*2];
    }


    public void InstantRoad(Vector3Int position)
    {
        grid[position.x + PlayerStats.gridSize, position.y + PlayerStats.gridSize] = true;
    }


    public void Order(Vector3Int position)
    {
        Vector2Int start = new Vector2Int(PlayerStats.gridSize, PlayerStats.gridSize);
        Vector2 end = new Vector2(position.x + PlayerStats.gridSize - 0.5f, position.y + PlayerStats.gridSize - 0.5f);

        Point path = Astar(start, end);
        if (path != null)
        {
            Debug.Log("yes");
            MyDrawPath(path);
        }
        else
        {
            Debug.Log("no");
        }
    }


    public Point Astar(Vector2Int start, Vector2 end)
    {
        List<Point> extremePoints = new List<Point>();
        Point[,] Pointsgrid = new Point[PlayerStats.gridSize*2, PlayerStats.gridSize*2];
        Vector2Int[] around = new Vector2Int[4]{new Vector2Int(1, 0), new Vector2Int(0, 1), new Vector2Int(-1, 0), new Vector2Int(0, -1)};

        Point startPoint = new Point();
        startPoint.SetAll(start, 0, null, end);

        extremePoints.Add(startPoint);
        Pointsgrid[start.x, start.y] = startPoint;
        
        int i = 0;

        while (extremePoints.Count > 0 && i < 500)
        {
            Point minPoint = GetMinPoint(extremePoints);

            for (int v = 0; v < around.Length; v++)
            {
                Vector2Int newPos = minPoint.position + around[v];
                if (0 <= newPos.x && newPos.x < PlayerStats.gridSize*2 && 0 <= newPos.y && newPos.y < PlayerStats.gridSize*2)
                {
                    if (grid[newPos.x, newPos.y] && (Pointsgrid[newPos.x, newPos.y] == null))
                    {
                        Point newPoint = new Point();
                        newPoint.SetAll(newPos, minPoint.distanceToStart + 1, minPoint, end);
                        extremePoints.Add(newPoint);
                        Pointsgrid[newPos.x, newPos.y] = newPoint;

                        MyDraw(newPoint, Color.green);

                        if (newPoint.distanceToEnd < 1)
                            return newPoint;
                    }
                }
            }
            minPoint.freez = true;
            extremePoints.Remove(minPoint);

            i++;
        }
        if (i == 500)
            Debug.Log("error");

        return null;
    }


    public Point GetMinPoint(List<Point> extremePoints)
    {
        int min = 0;
        for (int i = 0; i < extremePoints.Count; i++)
        {
            if (extremePoints[i].price < extremePoints[min].price)
                min = i;
        }
        return extremePoints[min];
    }


    public void MyDrawPath(Point p)
    {
        while (p.beforPoint != null)
        {
            MyDraw(p, Color.red);
            p = p.beforPoint;
        }
    }


    public void MyDraw(Point p, Color col)
    {
        if (p.beforPoint != null)
        {
            Vector3 pos1 = new Vector3(p.position.x - PlayerStats.gridSize + 0.5f, p.position.y - PlayerStats.gridSize + 0.5f, 0);
            Vector3 pos2 = new Vector3(p.beforPoint.position.x - PlayerStats.gridSize + 0.5f, p.beforPoint.position.y - PlayerStats.gridSize + 0.5f, 0);

            Debug.DrawLine(pos1, pos2, col, 5f, false);
        }
        else
        {
            Debug.Log("error2");
        }
    }
}



public class Point
{
    public bool freez = false; // не крайня ли эта точка

    public Vector2Int position;

    public int distanceToStart; 
    public float distanceToEnd;

    public Vector2 endPosition;

    public Point beforPoint;

    public float price;

    public void SetAll(Vector2Int pos, int dis, Point bP, Vector2 end)
    {
        position = pos;
        distanceToStart = dis;
        beforPoint = bP;
        endPosition = end;
        
        distanceToEnd = (end - position).sqrMagnitude;
        price = distanceToEnd + distanceToStart;
    }
}