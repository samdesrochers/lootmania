using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace FarseerPhysics.Samples
{
    public class AssetManager
    {
        private static AssetManager instance;
        public static AssetManager Instance 
        {
            get 
            {
                if (instance == null)
                {
                    instance = new AssetManager();
                }
                return instance;
            }
        }

        /***************************
         * 
         * Textures Definition
         * 
         * *************************/

        Texture2D[] LevelObjects;

        public Texture2D _circleSprite;
        public Texture2D _groundSprite;
        public Texture2D _tileSprite;
        public Texture2D _vertSprite;
        public Texture2D platform_1;

        public void Load(ContentManager content)
        {
            _circleSprite = content.Load<Texture2D>("CircleSprite"); // 96px x 96px => 1.5m x 1.5m
            _groundSprite = content.Load<Texture2D>("GroundSprite"); // 512px x 64px =>   8m x 1m
            _tileSprite = content.Load<Texture2D>("ground_tile_1"); // 512px x 64px =>   8m x 1m
            _vertSprite = content.Load<Texture2D>("vert");
            platform_1 = content.Load<Texture2D>("platform-1");

            LevelObjects = new Texture2D[4];

            //Tex id = 0, 1, 2, ...
            LevelObjects[0] = _groundSprite;
            LevelObjects[1] = platform_1;
            LevelObjects[2] = _vertSprite;
            
        }

        public Texture2D GetLevelObjectsById(int id)
        {
            if (id < LevelObjects.Length)
            {
                return LevelObjects[id];
            }

            return null;
        }
    }
}
