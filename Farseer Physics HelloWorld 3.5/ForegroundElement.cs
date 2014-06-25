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
    public class ForegroundElement
    {
        public Body Body;
        private int BodyType;
        private int TexId;

        public const int BODY_RECT = 1;
        public const int BODY_CIRCLE = 2;
        public const int BODY_CUSTOM = 3;

        public void Initialize(int bodyType, int texId, Vector2 position, Vector2 optionals)
        {
            // Note : position must be in world coords
            //      : optionals are size for rect and params (radius and shizzle) for circle all in world coords

            BodyType = bodyType;
            TexId = texId;

            switch (BodyType)
            {
                case BODY_RECT:
                    CreateRectangleBody(texId, position, optionals);
                    break;
                case BODY_CIRCLE:
                    CreateCircleBody(texId, position, optionals);
                    break;
                case BODY_CUSTOM:
                    CreateCustomBody(texId, position);
                    break;
            }
        }

        private void CreateRectangleBody(int texId, Vector2 position, Vector2 size)
        {
            Body = BodyFactory.CreateRectangle(Game1.World, size.X, size.Y, 1, position);
            Body.Friction = 1.0f;
            Body.IsStatic = true;
        }

        private void CreateCircleBody(int texId, Vector2 position, Vector2 parameters)
        {

        }

        private void CreateCustomBody(int texId, Vector2 position)
        {
            uint[] texData = new uint[AssetManager.Instance.GetLevelObjectsById(TexId).Width * AssetManager.Instance.GetLevelObjectsById(TexId).Height];
            AssetManager.Instance.GetLevelObjectsById(TexId).GetData<uint>(texData);

            Vertices vertices;
            vertices = TextureConverter.DetectVertices(texData, AssetManager.Instance.GetLevelObjectsById(TexId).Width);
            List<Vertices> vertexList = FarseerPhysics.Common.Decomposition.Triangulate.ConvexPartition(vertices, FarseerPhysics.Common.Decomposition.TriangulationAlgorithm.Bayazit);

            Vector2 vertScale = new Vector2(ConvertUnits.ToSimUnits(1));
            foreach (Vertices vert in vertexList)
                vert.Scale(ref vertScale);

            Body = BodyFactory.CreateCompoundPolygon(Game1.World, vertexList, 1, position);
        }

         /**************************************
         * 
         * Drawing Methods
         * 
         * ***********************************/
        public void Draw()
        {
            switch (BodyType)
            {
                case BODY_RECT:
                    DrawRectangle();
                    break;
                case BODY_CIRCLE:
                    DrawCircle();
                    break;
                case BODY_CUSTOM:
                    DrawCustom();
                    break;
            }
        }

        private void DrawRectangle()
        {
            float width = AssetManager.Instance.GetLevelObjectsById(TexId).Width / 2f;
            float height = AssetManager.Instance.GetLevelObjectsById(TexId).Height / 2f;

            Game1.batch.Draw(AssetManager.Instance.GetLevelObjectsById(TexId),
                ConvertUnits.ToDisplayUnits(Body.Position),
                null,
                Color.White,
                0f,
                new Vector2(width, height),
                1f,
                SpriteEffects.None,
                0f);
        }

        private void DrawCircle()
        {

        }

        private void DrawCustom()
        {
            float width = ConvertUnits.ToSimUnits(AssetManager.Instance.GetLevelObjectsById(TexId).Width / 2f);
            float height = ConvertUnits.ToSimUnits(AssetManager.Instance.GetLevelObjectsById(TexId).Height / 2f);

            Game1.batch.Draw(AssetManager.Instance.GetLevelObjectsById(TexId),
                ConvertUnits.ToDisplayUnits(Body.Position),
                null,
                Color.White,
                0f,
                new Vector2(width, height),
                1f,
                SpriteEffects.None,
                0f);
        }
    }
}
