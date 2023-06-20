using System.Collections.Generic;
using System;
using System.Linq;  
using System.Text;  
using System.Threading.Tasks;  
using UnityEngine.Tilemaps;
using UnityEngine;
namespace Astar
{
    class Location  
    {  
        public int X;  
        public int Y;  
        public int F;  
        public int G;  
        public int H;  
        public Location Parent;  
    }
    class Program  
    {  
        public static List<Vector3Int> Solve(Tilemap tilemap, Vector3 startPosition, Vector3 finishPosition, string walkableGround, bool walking)  
        {  
            BoundsInt bounds = tilemap.cellBounds;
            int height = bounds.size.y;
            int width = bounds.size.x;
            
            Vector3 finishVector = tilemap.WorldToCell(startPosition);
            finishVector = finishVector + new Vector3(width/2 + 1, height/2 - 1, 0);
            Vector3 startVector = tilemap.WorldToCell(finishPosition);
            startVector = startVector + new Vector3(width/2 + 1, height/2 - 1, 0);
            string[] map = Transformations.getMatrixString(tilemap, walkableGround);

            var start = new Location { X = (int)startVector.x, Y = (int)startVector.y };
            var target = new Location { X = (int)finishVector.x, Y = (int)finishVector.y };
            // algorithm  
            Location current = null;  
            var openList = new List<Location>();  
            var closedList = new List<Location>();  
            int g = 0;  
     
            // start by adding the original position to the open list  
            openList.Add(start);  
    
            while (openList.Count > 0)  
            {  
                // get the square with the lowest F score  
                var lowest = openList.Min(l => l.F);  
                current = openList.First(l => l.F == lowest);  
    
                // add the current square to the closed list  
                closedList.Add(current);  
                openList.Remove(current);  
    
                // if we added the destination to the closed list, we've found a path  
                if (closedList.FirstOrDefault(l => l.X == target.X && l.Y == target.Y) != null)  
                    break;  

                var adjacentSquares = GetWalkableAdjacentSquares(current.X, current.Y, map, openList);  
                g = current.G + 1;  

                foreach(var adjacentSquare in adjacentSquares)  
                {  
                    // if this adjacent square is already in the closed list, ignore it  
                    if (closedList.FirstOrDefault(l => l.X == adjacentSquare.X && l.Y == adjacentSquare.Y) != null)  
                        continue;  
    
                    // if it's not in the open list
                    if (openList.FirstOrDefault(l => l.X == adjacentSquare.X && l.Y == adjacentSquare.Y) == null)  
                    {  
                        // compute its score, set the parent  
                        adjacentSquare.G = g;  
                        adjacentSquare.H = ComputeHScore(adjacentSquare.X, adjacentSquare.Y, target.X, target.Y);  
                        adjacentSquare.F = adjacentSquare.G + adjacentSquare.H;  
                        adjacentSquare.Parent = current;  
    
                        // and add it to the open list  
                        openList.Insert(0, adjacentSquare);  
                    }  
                    else  
                    {  
                    // test if using the current G score makes the adjacent square's F score  
                    // lower, if yes update the parent because it means it's a better path  
                        if (g + adjacentSquare.H < adjacentSquare.F)  
                        {  
                            adjacentSquare.G = g;  
                            adjacentSquare.F = adjacentSquare.G + adjacentSquare.H;  
                            adjacentSquare.Parent = current;  
                        }  
                    }  
                }  
            }  
            Location end = current;
            int lastx = 0, lasty = 0;
            bool first = true;
            var result = new List<Vector3Int>();
            while (current != null)  
            {
                int positionx,positiony;
                positionx=current.X;
                positiony=current.Y;
                bool nearWall = NearWall(positionx,positiony,map);
                if(walking && !nearWall)
                {
                    //go to closest tile near ground
                    while(map[positiony-1][positionx] == ' ' || map[positiony-1][positionx] == 'B')
                    {
                        positiony-=1;
                    }

                    if(first)
                    {
                        first = false;
                    }
                    else
                    {
                        if(lasty != positiony)
                        {
                            //add corner tile
                            if(lasty < positiony)
                            {
                                result.Add(new Vector3Int(lastx, positiony, nearWall == true ? 1 : 0) - new Vector3Int(width/2 + 1, height/2 - 1, nearWall == true ? 1 : 0));
                            }
                            else
                            {
                                result.Add(new Vector3Int(positionx, lasty, nearWall == true ? 1 : 0) - new Vector3Int(width/2 + 1, height/2 - 1, nearWall == true ? 1 : 0));
                            }
                        }
                    }
                }
                result.Add(new Vector3Int(positionx, positiony, nearWall == true ? 1 : 0) - new Vector3Int(width/2 + 1, height/2 - 1, nearWall == true ? 1 : 0));
                lastx = positionx;
                lasty = positiony;
                current = current.Parent;  
            }  
            return result;
        }  
        static bool NearWall(int x, int y, string[] map)
        {
            if (map[y - 1][x - 1] == 'X' || 
            map[y - 1][x] == 'X' || 
            map[y - 1][x + 1] == 'X' || 
            map[y][x - 1] == 'X' || 
            map[y][x + 1] == 'X' || 
            map[y + 1][x - 1] == 'X' || 
            map[y + 1][x] == 'X' || 
            map[y + 1][x + 1] == 'X')
            {
            return true;
            }
            return false;
        }
        
        static List<Location> GetWalkableAdjacentSquares(int x, int y, string[] map, List<Location> openList)  
        {  
            List<Location> list = new List<Location>();  
    
            if (map[y - 1][x] == ' ' || map[y - 1][x] == 'B')  
            {  
                Location node = openList.Find(l => l.X == x && l.Y == y - 1);  
                if (node == null) list.Add(new Location() { X = x, Y = y - 1 });  
                else list.Add(node);  
            }  
    
            if (map[y + 1][x] == ' ' || map[y + 1][x] == 'B')  
            {  
                Location node = openList.Find(l => l.X == x && l.Y == y + 1);  
                if (node == null) list.Add(new Location() { X = x, Y = y + 1 });  
                else list.Add(node);  
            }  
    
            if (map[y][x - 1] == ' ' || map[y][x - 1] == 'B')  
            {  
                Location node = openList.Find(l => l.X == x - 1 && l.Y == y);  
                if (node == null) list.Add(new Location() { X = x - 1, Y = y });  
                else list.Add(node);  
            }  
    
            if (map[y][x + 1] == ' ' || map[y][x + 1] == 'B')  
            {  
                Location node = openList.Find(l => l.X == x + 1 && l.Y == y);  
                if (node == null) list.Add(new Location() { X = x + 1, Y = y });  
                else list.Add(node);  
            }  
    
            return list;  
        }  
    
        static int ComputeHScore(int x, int y, int targetX, int targetY)  
        {  
            return Math.Abs(targetX - x) + Math.Abs(targetY - y);  
        }  
    }
    class Transformations
    {
        public static string[] getMatrixString(Tilemap tilemap,string walkableGround)
        {
            BoundsInt bounds = tilemap.cellBounds;
            TileBase[] matrix = tilemap.GetTilesBlock(bounds);
            int height = bounds.size.y;
            int width = bounds.size.x;
            string[] matrixString = new string[height + 2];
            string message = "\n";
            string fullLine = "X";
            for (int x = 0; x < width; x++)
            {
                fullLine += "X";
            }
            fullLine += "X";
            matrixString[0] = fullLine;
            for (int y = height - 1; y > 0; y--) 
            {
                message = "X";
                for (int x = 0; x < width; x++) 
                {
                    TileBase tile = matrix[x + y * width];
                    if (tile != null) 
                    {
                        if(tile.name == walkableGround)
                        {
                            message += " ";
                        }
                        else
                        {
                            message += "X";
                        }
                    } 
                    else 
                    {
                        message += " ";
                    }
                }
                message += "X";
                matrixString[y + 1] = message;
            }
            matrixString[height + 1] = fullLine;
            return matrixString;
        }
    }
}