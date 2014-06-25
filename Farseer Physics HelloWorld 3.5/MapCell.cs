using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Common;
using FarseerPhysics.Common.TextureTools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace FarseerPhysics.Samples
{
    public class MapCell
    {
        public Guid guid;

        public ForegroundElement FGElement;
        int CellType;
        // public Ledge Ledge
        // public Destructable Dest;

        public const int CELL_FOREGROUND    = 1;
        public const int CELL_DESTRUCTABLE  = 2;
        public const int CELL_LEDGE         = 3;

        public void Initialize(int cellType, int bodyType, int texId, Vector2 position, Vector2 optionals)
        { 
            // Note : position must be in world coords
            //      : optionals are size for rect and params (radius and shizzle) for circle all in world coords

            guid = Guid.NewGuid();
            CellType = cellType;

            switch (CellType)
            {
                case CELL_FOREGROUND:
                    CreateForeground(bodyType, texId, position, optionals);
                    break;
                case CELL_DESTRUCTABLE:
                    CreateDestructable(texId, position, optionals);
                    break;
                case CELL_LEDGE:
                    CreateLedge(texId, position);
                    break;
            }
        }

        private void CreateForeground(int bodyType, int texId, Vector2 position, Vector2 size) 
        {
            FGElement = new ForegroundElement();
            FGElement.Initialize(bodyType, texId, position, size);
        }

        private void CreateDestructable(int texId, Vector2 position, Vector2 parameters)
        {

        }

        private void CreateLedge(int texId, Vector2 position)
        {

        }



        public void Draw()
        {
            switch (CellType)
            {
                case CELL_FOREGROUND:
                    FGElement.Draw();
                    break;
                case CELL_LEDGE:
                    
                    break;
                case CELL_DESTRUCTABLE:
                    
                    break;
            }
        }
    }
}
