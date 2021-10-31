using UnityEngine;
using System.Collections.Generic;

public class RoadsManager : MonoBehaviour // отвечает за дороги
{
    public bool[,] roadGrid;

    public GameObject moveResource;

    public Dictionary<Resource, Warehouse[,]> resourceGrid = new Dictionary<Resource, Warehouse[,]>();

    void Start()
    {
        roadGrid = new bool[PlayerStats.gridSize*2, PlayerStats.gridSize*2];
    }


    public void InstantRoad(Vector3Int position)
    {
        roadGrid[position.x + PlayerStats.gridSize, position.y + PlayerStats.gridSize] = true;
    }
    
    public void InstantTower(Vector3Int position, GameObject towerObject)
    {
        Warehouse warehouse = towerObject.GetComponent<Warehouse>();
        if (warehouse)
        {
            
            foreach (Export export in warehouse.exports)
            {
                if(!resourceGrid.ContainsKey(export.resource))
                {
                    resourceGrid.Add(export.resource, new Warehouse[PlayerStats.gridSize*2 + 1, PlayerStats.gridSize*2 + 1]);
                }
                Warehouse[,] grid;
                grid = resourceGrid[export.resource];
                int x = position.x + PlayerStats.gridSize;
                int y = position.y + PlayerStats.gridSize;
                grid[x, y] = towerObject.GetComponent<Warehouse>();
            }
        }
    }

    public bool Order(Vector3Int position, Resource resource, Warehouse warehouse) //заказ
    {
        Vector2Int start = new Vector2Int(position.x + PlayerStats.gridSize, position.y + PlayerStats.gridSize);
        // Vector2 end = new Vector2(position.x + PlayerStats.gridSize - 0.5f, position.y + PlayerStats.gridSize - 0.5f);

        var path = BFS(start, resource);
        if (path != null)
        {
            GameObject obj = Instantiate(moveResource);
            obj.GetComponent<MoveResource>().SetAll(PointToPath(path), resource, warehouse);
            return true;
        }
        return false;
    }

    public Point BFS(Vector2Int start, Resource resource) //поиск в ширину
    {
        
        if(!resourceGrid.ContainsKey(resource))
            return null;

        List<Point> extremePoints = new List<Point>();
        Point[,] Pointsgrid = new Point[PlayerStats.gridSize*2, PlayerStats.gridSize*2];
        Vector2Int[] around = new Vector2Int[4]{new Vector2Int(1, 0), new Vector2Int(0, 1), new Vector2Int(-1, 0), new Vector2Int(0, -1)};
        Vector2Int[] square = new Vector2Int[4]{new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(1, 1)};

        for (int v = 0; v < square.Length; v++)
        {
            Point startPoint = new Point();
            int x = start.x - square[v].x;
            int y = start.y - square[v].y;
            if (0 <= x && x < PlayerStats.gridSize*2 && 0 <= y && y < PlayerStats.gridSize*2)
                if (roadGrid[x, y])
                {
                    startPoint.SetAllBFS(new Vector2Int(x, y), 0, null);
                    Pointsgrid[x, y] = startPoint;
                    extremePoints.Add(startPoint);
                }
        }

        int i = 0;

        while (extremePoints.Count > 0 && i < 500)
        {
            Point minPoint = GetMinPoint(extremePoints);

            for (int v = 0; v < around.Length; v++)
            {
                Vector2Int newPos = minPoint.position + around[v];
                if (0 <= newPos.x && newPos.x < PlayerStats.gridSize*2 && 0 <= newPos.y && newPos.y < PlayerStats.gridSize*2)
                {
                    if (roadGrid[newPos.x, newPos.y] && (Pointsgrid[newPos.x, newPos.y] == null))
                    {
                        Point newPoint = new Point();
                        newPoint.SetAllBFS(newPos, minPoint.distanceToStart + 1, minPoint);
                        extremePoints.Add(newPoint);
                        Pointsgrid[newPos.x, newPos.y] = newPoint;

                        // MyDraw(newPoint, Color.green);

                        for (int s = 0; s < square.Length; s++)
                        {
                            int x = newPos.x + square[s].x;
                            int y = newPos.y + square[s].y;
                            if (0 <= x && x <= PlayerStats.gridSize*2 && 0 <= y && y <= PlayerStats.gridSize*2)
                                if (resourceGrid[resource][x, y])
                                    if (resourceGrid[resource][x, y].Receive(resource, 1))
                                        return newPoint;
                        }
                    }
                }
            }
            minPoint.freez = true;
            extremePoints.Remove(minPoint);

            i++;
        }

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


    public void MyDraw(Point point, Color col)
    {
        if (point.beforPoint != null)
        {
            Vector3 pos1 = PointToPosition(point);
            Vector3 pos2 = PointToPosition(point.beforPoint);

            Debug.DrawLine(pos1, pos2, col, 5f, false);
        }
    }

    
    public List<Vector3> PointToPath(Point point)
    {
        List<Vector3> dots = new List<Vector3>();

        while (point != null)
        {
            dots.Add(PointToPosition(point));
            point = point.beforPoint;
        }

        return dots;
    }


    public Vector3 PointToPosition(Point point)
    {
        return new Vector3(point.position.x - PlayerStats.gridSize + 0.5f, point.position.y - PlayerStats.gridSize + 0.5f, 0);
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

    public void SetAllStar(Vector2Int pos, int dis, Point bP, Vector2 end)
    {
        position = pos;
        distanceToStart = dis;
        beforPoint = bP;
        endPosition = end;
        
        distanceToEnd = (end - position).sqrMagnitude;
        price = distanceToEnd + distanceToStart;
    }

    
    public void SetAllBFS(Vector2Int pos, int dis, Point bP)
    {
        position = pos;
        distanceToStart = dis;
        beforPoint = bP;
        price = distanceToStart;
    }
}

// public class Path
// {
    // public List<Vector3> dots = new List<Vector3>();
//     public List<Point> points = new List<Point>();

//     public void MyDrawPath()
//     {
//         foreach (Vector3 dot in dots)
//         {
//             MyDraw(point, Color.red);
//         }
//     }

    // public void PointToPath(Point point)
    // {
    //     while (point.beforPoint != null)
    //     {
    //         point = point.beforPoint;
    //         dots.Add();
    //     }
    // }
// }