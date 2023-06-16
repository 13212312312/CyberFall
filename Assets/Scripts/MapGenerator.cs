using System;
using UnityEngine;
using System.Text;

namespace MapGenerator
{
    public class Stack<T>
    {
        static readonly int MAX = 1000;
        int top;
        T[] stack = new T[MAX];
  
        public bool IsEmpty()
        {
            return (top < 0);
        }
        public int Count()
        {
            return top;
        }
        public Stack()
        {
            top = -1;
        }
        public bool Push(T data)
        {
            if (top >= MAX)
            {
                return false;
            }
            else
            {
                stack[++top] = data;
                return true;
            }
        }
  
        public T Pop()
        {
            if (top < 0)
            {
                return default(T);
            }
            else
            {
                T value = stack[top--];
                return value;
            }
        }
  
        public T First()
        {
            if (top < 0)
            {
                return default(T);
            }
            else
            {
                return stack[top];
            }
        }
    }
    public class Room
    {
        public int RoomType { get; set; }
        public bool[] HasNeighbours { get; set; }
        public Room[] NeighBour { get; set; }
        public Room(int roomType)
        {
            RoomType = roomType;
            HasNeighbours = new bool[4];
            for (int i = 0; i < 4; i++)
            {
                HasNeighbours[i] = false;
            }
            NeighBour = new Room[4];
        }
    }
    public class RoomInQueue
    {
        public Room Room { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Index { get; set; }
        public RoomInQueue(Room room, int x, int y, int index)
        {
            Room = room;
            X = x;
            Y = y;
            Index = index;
        }
    }
    public class Map
    {
        public Room[,] Matrix;
        public int Height;
        public int Width;
        public int StartX;
        public int StartY;
        public int MaxIndex;
        public int NrOfTypesOfRoom;
        public static int[] directionX = { -1, 0, 1, 0 };
        public static int[] directionY = { 0, 1, 0, -1 };
        public System.Random rnd;
        public RoomInQueue exitRoom;
        public static int[] OpposingRoom = { 2, 3, 0, 1 };

        public Map(int width, int height, int startX, int startY, int maxIndex, int nrOfTypesOfRoom, System.Random random)
        {
            Height = height;
            Width = width;
            Matrix = new Room[Height, Width];
            StartX = startX;
            StartY = startY;
            MaxIndex = maxIndex;
            NrOfTypesOfRoom = nrOfTypesOfRoom;
            rnd = random;
        }
        public void GenerateMap()
        {
            var roomQueue = new Stack<RoomInQueue>();
            var room = new Room(0);
            var firstRoom = new RoomInQueue(room, StartX, StartY, 1);
            roomQueue.Push(firstRoom);
            Matrix[StartY, StartX] = room;
            while (!roomQueue.IsEmpty())
            {
                var currentRoom = roomQueue.First();
                int nrOfNb = 0;
                if (currentRoom.Index == MaxIndex)
                {
                    roomQueue.Pop();
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (0 <= currentRoom.X + directionX[i] && currentRoom.X + directionX[i] < Width &&
                               0 <= currentRoom.Y + directionY[i] && currentRoom.Y + directionY[i] < Height &&
                              Matrix[currentRoom.Y + directionY[i], currentRoom.X + directionX[i]] == null)
                        {
                            nrOfNb++;
                        }
                    }
                    Console.WriteLine(nrOfNb);
                    if (nrOfNb == 0)
                    {
                        roomQueue.Pop();
                    }
                    else
                    {
                        int roomIndex;
                        while (true)
                        {
                            roomIndex = rnd.Next(0, 4);
                            if (0 <= currentRoom.X + directionX[roomIndex] && currentRoom.X + directionX[roomIndex] < Width &&
                               0 <= currentRoom.Y + directionY[roomIndex] && currentRoom.Y + directionY[roomIndex] < Height &&
                              Matrix[currentRoom.Y + directionY[roomIndex], currentRoom.X + directionX[roomIndex]] == null)
                            {
                                break;
                            }
                        }
                        currentRoom.Room.NeighBour[roomIndex] = new Room(rnd.Next(2, NrOfTypesOfRoom));
                        currentRoom.Room.NeighBour[roomIndex].NeighBour[OpposingRoom[roomIndex]] = currentRoom.Room.NeighBour[roomIndex];
                        currentRoom.Room.HasNeighbours[roomIndex] = true;
                        currentRoom.Room.NeighBour[roomIndex].HasNeighbours[OpposingRoom[roomIndex]] = true;
                        Matrix[currentRoom.Y + directionY[roomIndex], currentRoom.X + directionX[roomIndex]] = currentRoom.Room.NeighBour[roomIndex];
                        var nextRoom = new RoomInQueue(currentRoom.Room.NeighBour[roomIndex], currentRoom.X + directionX[roomIndex], currentRoom.Y + directionY[roomIndex], currentRoom.Index + 1);
                        if (exitRoom == null || nextRoom.Index > exitRoom.Index)
                        {
                            exitRoom = nextRoom;
                        }
                        roomQueue.Push(nextRoom);
                    }
                }
            }
            Matrix[exitRoom.Y, exitRoom.X].RoomType = 1;
        }
        public String Print()
        {
            StringBuilder toPrint = new StringBuilder();
            for (int i = 0; i < Height; i++)
            {
                StringBuilder row = new StringBuilder();
                for (int j = 0; j < Width; j++)
                {
                    StringBuilder element = new StringBuilder("{");
                    if (Matrix[i, j] != null)
                    {
                        element.Append(Matrix[i, j].RoomType);
                    }
                    else
                    {
                        element.Append("0");
                    }

                    for (int z = 0; z < 4; z++)
                    {
                        if (Matrix[i, j] != null)
                        {
                            if (Matrix[i, j].HasNeighbours[z])
                                element.Append("@");
                            else
                                element.Append("U");
                        }
                        else
                        {
                            element.Append("0");
                        }
                    }
                    element.Append("}");
                    row.Append(element.ToString()).Append(",");
                }
                toPrint.Append(row.ToString()).Append("\n");
            }
            return toPrint.ToString();
        }
    }
}