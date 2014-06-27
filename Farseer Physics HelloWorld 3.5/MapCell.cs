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

        public CellObject CellObject;
        public int ObjectType;

        public void Initialize(int objectType, int bodyType, int texId, Vector2 position, Vector2 optionals)
        { 
            // Note : position must be in world coords
            //      : optionals are size for rect and params (radius and shizzle) for circle all in world coords

            guid = Guid.NewGuid();
            ObjectType = objectType;

            switch (ObjectType)
            {
                case CellObject.OBJ_FOREGROUND:
                    CreateForeground(bodyType, texId, position, optionals);
                    break;
                case CellObject.OBJ_DESTRUCTABLE:
                    CreateDestructable(bodyType, texId, position, optionals);
                    break;
                case CellObject.OBJ_LEDGE:
                    CreateLedge(bodyType, texId, position, optionals);
                    break;
            }
        }

        private void CreateForeground(int bodyType, int texId, Vector2 position, Vector2 size) 
        {
            CellObject = new ForegroundElement();
            CellObject.Initialize(bodyType, texId, position, size);
        }

        private void CreateDestructable(int bodyType, int texId, Vector2 position, Vector2 size)
        {
            //CellObject = new ForegroundElement();
            //CellObject.Initialize(bodyType, texId, position, size);
        }

        private void CreateLedge(int bodyType, int texId, Vector2 position, Vector2 size)
        {
            CellObject = new Ledge();
            CellObject.Initialize(bodyType, texId, position, size);
        }

        public void Draw()
        {
            if (CellObject != null)
                CellObject.Draw();
        }

        public void Update(float dt)
        {
            if (CellObject != null)
                CellObject.Update(dt);
        }
    }
}
