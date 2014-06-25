using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Microsoft.Xna.Framework;

namespace FarseerPhysics.Samples
{
    public class GameMap
    {
        MapCell[,] Map;

        private int Width;
        private int Height;
        private float WorldUnit;

        public GameMap(int width, int height, float worldUnit) 
        {
            Width = width;
            Height = height;
            WorldUnit = worldUnit;

            // TODO : remove when done
            MapHelper.Generate();
        }

        public void Load(string fileName)
        {
            
            // Read the file 
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader(@"Maps/" + fileName);

            // Parse first parameters (width, height, name, etc.)
            ParseMapInfo(file.ReadLine());
            Map = new MapCell[Height, Width];

            int xCounter = 0;
            int yCounter = 0;

            Vector2 bounds = Game1.Camera.ToWorld(new Vector2(Width, Height));

            // Parse each map cells information and create the approriate Cell
            // Format : CellType(99) - TexID(9999) - SizeX(99) - SizeY(99) - BodyType(99)
            while ((line = file.ReadLine()) != null)
            {
                string[] parameters = ParseMapCellsInfo(line);
                if (parameters != null)
                {
                    int cellType = Int32.Parse(parameters[0]);
                    int texID = Int32.Parse(parameters[1]);
                    int bodyType = Int32.Parse(parameters[4]);

                    Vector2 size = new Vector2(float.Parse(parameters[2]), float.Parse(parameters[3]));
                    Vector2 position = new Vector2(Game1.Camera.ToWorld(xCounter) + size.X/2, (int)bounds.Y - yCounter - size.Y/2);                    

                    MapCell c = new MapCell();
                    if (cellType != 0)
                    {
                        c.Initialize(cellType, bodyType, texID, position, size);
                    }

                    Map[yCounter, xCounter] = c;

                    xCounter++;
                    if (xCounter == Width)
                    {
                        yCounter++;
                        xCounter = 0;
                    }
                }
            }

            file.Close();
        }

        public void Save()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Map[y, x] = new MapCell();
                }
            }
        }

        // Get common values  *** Width = 0, Height = 1, Name = 2
        public void ParseMapInfo(string line)
        {
            if(String.IsNullOrEmpty(line))
                throw new FileNotFoundException(@"Map's first line was empty");

            char[] delimiterChars = { '-' };
            string[] parameters = line.Split(delimiterChars);

            
            Width = Int32.Parse(parameters[0]);
            Height = Int32.Parse(parameters[1]);
        }

        // Parse each cell's content
        private string[] ParseMapCellsInfo(string line)
        {
            if (line[0] == '#')
                return null;

            char[] delimiterChars = { '-' };
            string[] parameters = line.Split(delimiterChars);

            return parameters;
        }

        public void Draw()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    MapCell m = Map[y, x];
                    
                    if (m.guid != null)
                    {
                        m.Draw();
                    }
                }
            }
        }

        public void Update()
        { 
            
        }

        public List<ForegroundElement> GetFGElementsInAABB(Vector2 center, float width, float height)
        {
            int x = (int)Game1.Camera.ToData(center.X);
            int y = (int)Game1.Camera.ToData(center.Y);
            int w = (int)width;
            int h = (int)height;

            int startY = Height - (y + h); if (startY < 0) startY = 0;
            int endY = Height - (y - h); if (endY > Height - 1) endY = Height - 1;

            int startX = (x - w > 0) ? x - w : 0;
            int endX = (x + w < Width - 1) ? x + w : Width - 1;

            List<ForegroundElement> cells = new List<ForegroundElement>();

            for (int i = startY; i < endY; i++)
            {
                for (int j = startX; j < endX; j++)
                {
                    if (Map[i, j].FGElement != null)
                    {
                        cells.Add(Map[i, j].FGElement);
                    }
                }
            }

            if (cells.Count == 0)
                return null;

            return cells;
        }
    }
}
