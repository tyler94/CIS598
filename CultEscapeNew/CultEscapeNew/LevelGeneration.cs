using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using CultEscapeNew.Core;
using CultEscapeNew.Sprites;
using System;
using System.Threading.Tasks;
using MonoGame.Extended.Graphics;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

namespace CultEscapeNew
{
    public class LevelGeneration
    {
        Vector2 worldSize = new Vector2(4, 4);
        public Room[,] rooms;
        List<Vector2> takenPositions = new List<Vector2>();
        int gridSizeX, gridSizeY, numberOfRooms = 5;
        //public GameObject roomWhiteObj;
        //public Transform mapRoot;

        public void Start()
        {
            takenPositions = new List<Vector2>();
            if (numberOfRooms >= (worldSize.X * 2) * (worldSize.Y * 2))
            { // make sure we dont try to make more rooms than can fit in our grid
                numberOfRooms = (int)((worldSize.X * 2) * (worldSize.Y * 2));
            }
            gridSizeX = (int)worldSize.X; //note: these are half-extents
            gridSizeY = (int)worldSize.Y;
            CreateRooms(); //lays out the actual map
            SetRoomDoors(); //assigns the doors where rooms would connect
            //DrawMap(); //instantiates objects to make up a map
            //GetComponent<SheetAssigner>().Assign(rooms); //passes room info to another script which handles generatating the level geometry
        }
        float Lerp(float firstFloat, float secondFloat, float by)
        {
            return firstFloat * (1 - by) + secondFloat * by;
        }
        void CreateRooms()
        {
            takenPositions = new List<Vector2>();
            //setup
            rooms = new Room[gridSizeX * 2, gridSizeY * 2];
            //center, starting room
            rooms[gridSizeX, gridSizeY] = new Room(Vector2.Zero, 1);
            takenPositions.Insert(0, Vector2.Zero);
            Vector2 checkPos = Vector2.Zero;
            //magic numbers
            float randomCompare = 0.2f, randomCompareStart = 0.2f, randomCompareEnd = 0.01f;
            //add rooms
            for (int i = 0; i < numberOfRooms - 1; i++)
            {
                float randomPerc = ((float)i) / (((float)numberOfRooms - 1));
                randomCompare =Lerp(randomCompareStart, randomCompareEnd, randomPerc);
                //grab new position
                checkPos = NewPosition();
                //test new position
                Random random = new Random();
                double rand;
                rand = random.NextDouble();
                if (NumberOfNeighbors(checkPos, takenPositions) > 1 && rand > randomCompare)
                {
                    int iterations = 0;
                    do
                    {
                        checkPos = SelectiveNewPosition();
                        iterations++;
                    } while (NumberOfNeighbors(checkPos, takenPositions) > 1 && iterations < 100);
                    /*if (iterations >= 50)
                        print("error: could not create with fewer neighbors than : " + NumberOfNeighbors(checkPos, takenPositions));*/
                }
                //finalize position
                rooms[(int)checkPos.X + gridSizeX, (int)checkPos.Y + gridSizeY] = new Room(checkPos, 0);
                takenPositions.Insert(0, checkPos);
            }
        }
        Vector2 NewPosition()
        {
            int x = 0, y = 0;
            Vector2 checkingPos = Vector2.Zero;
            Random random = new Random();
            double rand;
            do
            {
                rand = random.NextDouble();
                int index = (int)(rand * (takenPositions.Count - 1)); // pick an existing room
                x = (int)takenPositions[index].X;
                y = (int)takenPositions[index].Y;
                rand = random.NextDouble();
                bool UpDown = (rand < 0.5f);//randomly pick wether to look on horizontal or vertical axis
                rand = random.NextDouble();
                bool positive = (rand < 0.5f);//pick whether to be positive or negative on that axis
                if (UpDown)
                { //find the position based on the above bools
                    if (positive)
                    {
                        y += 1;
                    }
                    else
                    {
                        y -= 1;
                    }
                }
                else
                {
                    if (positive)
                    {
                        x += 1;
                    }
                    else
                    {
                        x -= 1;
                    }
                }
                checkingPos = new Vector2(x, y);
            } while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY); //make sure the position is valid
            return checkingPos;
        }
        Vector2 SelectiveNewPosition()
        { // method differs from the above in the two commented ways
            int index = 0, inc = 0;
            int x = 0, y = 0;
            Vector2 checkingPos = Vector2.Zero;
            Random random = new Random();
            double rand;
            rand = random.NextDouble();
            do
            {
                inc = 0;
                do
                {
                    //instead of getting a room to find an adject empty space, we start with one that only 
                    //as one neighbor. This will make it more likely that it returns a room that branches out
                    index = (int)(rand * (takenPositions.Count - 1));
                    inc++;
                } while (NumberOfNeighbors(takenPositions[index], takenPositions) > 1 && inc < 100);
                x = (int)takenPositions[index].X;
                y = (int)takenPositions[index].Y;
                rand = random.NextDouble();
                bool UpDown = (rand < 0.5f);
                rand = random.NextDouble();
                bool positive = (rand < 0.5f);
                if (UpDown)
                {
                    if (positive)
                    {
                        y += 1;
                    }
                    else
                    {
                        y -= 1;
                    }
                }
                else
                {
                    if (positive)
                    {
                        x += 1;
                    }
                    else
                    {
                        x -= 1;
                    }
                }
                checkingPos = new Vector2(x, y);
            } while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY);
            if (inc >= 100)
            { // break loop if it takes too long: this loop isnt garuanteed to find solution, which is fine for this
                //print("Error: could not find position with only one neighbor");
            }
            return checkingPos;
        }
        int NumberOfNeighbors(Vector2 checkingPos, List<Vector2> usedPositions)
        {
            int ret = 0; // start at zero, add 1 for each side there is already a room
            Vector2 checkingPosRight = checkingPos;
            checkingPosRight.X += 1;
            Vector2 checkingPosLeft = checkingPos;
            checkingPosLeft.X -= 1;
            Vector2 checkingPosUp = checkingPos;
            checkingPosUp.Y += 1;
            Vector2 checkingPosDown = checkingPos;
            checkingPosDown.Y -= 1;
            if (usedPositions.Contains(checkingPosRight))
            { //using Vector.[direction] as short hands, for simplicity
                ret++;
            }
            if (usedPositions.Contains(checkingPosLeft))
            {
                ret++;
            }
            if (usedPositions.Contains(checkingPosUp))
            {
                ret++;
            }
            if (usedPositions.Contains(checkingPosDown))
            {
                ret++;
            }
            return ret;
        }
        void SetRoomDoors()
        {
            for (int x = 0; x < ((gridSizeX * 2)); x++)
            {
                for (int y = 0; y < ((gridSizeY * 2)); y++)
                {
                    if (rooms[x, y] == null)
                    {
                        continue;
                    }
                    Vector2 gridPosition = new Vector2(x, y);
                    if (y - 1 < 0)
                    { //check above
                        rooms[x, y].doorBot = false;
                    }
                    else
                    {
                        rooms[x, y].doorBot = (rooms[x, y - 1] != null);
                    }
                    if (y + 1 >= gridSizeY * 2)
                    { //check below
                        rooms[x, y].doorTop = false;
                    }
                    else
                    {
                        rooms[x, y].doorTop = (rooms[x, y + 1] != null);
                    }
                    if (x - 1 < 0)
                    { //check left
                        rooms[x, y].doorLeft = false;
                    }
                    else
                    {
                        rooms[x, y].doorLeft = (rooms[x - 1, y] != null);
                    }
                    if (x + 1 >= gridSizeX * 2)
                    { //check right
                        rooms[x, y].doorRight = false;
                    }
                    else
                    {
                        rooms[x, y].doorRight = (rooms[x + 1, y] != null);
                    }
                }
            }
        }
    }
}