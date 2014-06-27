using System;
using System.Collections.Generic;
using System.Reflection;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Common;
using FarseerPhysics.Common.TextureTools;
using FarseerPhysics.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace FarseerPhysics.Samples
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        public static SpriteBatch batch;
        private KeyboardState _oldKeyState;

        public static World World;

        private Player Player;

        private Texture2D _circleSprite;
        private Texture2D _groundSprite;
        private Texture2D _tileSprite;
        private Texture2D _vertSprite;

        ControlHelper runHelper;
        ControlHelper jumpHelper;

        public static Camera Camera;

        public static GameMap GameMap;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1024;    // 12.5m
            graphics.PreferredBackBufferHeight = 640;   // 7.5m

            Content.RootDirectory = "Content";

            //Create a world with gravity.
            World = new World(new Vector2(0, 30));
            Camera = new Camera();

            Player = new Player();

            runHelper = new ControlHelper((int)ControlHelper.Types.run);
            jumpHelper = new ControlHelper((int)ControlHelper.Types.jump);

            GameMap = new GameMap(20, 10, 64.0f);
        }

        protected override void LoadContent()
        {
            // Initialize camera controls
            Camera.Initialize(graphics);
            batch = new SpriteBatch(Camera.Graphics.GraphicsDevice);

            AssetManager.Instance.Load(Content);

            // Load sprites
            //_circleSprite = Content.Load<Texture2D>("CircleSprite"); // 96px x 96px => 1.5m x 1.5m
            _groundSprite = Content.Load<Texture2D>("GroundSprite"); // 512px x 64px =>   8m x 1m
            _tileSprite = Content.Load<Texture2D>("ground_tile_1"); // 512px x 64px =>   8m x 1m
            _vertSprite = Content.Load<Texture2D>("vert");

            // 1 meters equals 64 pixels here
            ConvertUnits.SetDisplayUnitToSimUnitRatio(64f);

            GameMap.Load("map01.sav");

            /* We need XNA to draw the ground and circle at the center of the shapes */
            Vector2 groundOrigin = new Vector2(_tileSprite.Width / 2f, _tileSprite.Height / 2f);
            Vector2 playerOrigin = new Vector2(AssetManager.Instance._circleSprite.Width / 2f, AssetManager.Instance._circleSprite.Height / 2f);

            
            

            /* Player */
            Vector2 playerInitialPosition = ConvertUnits.ToSimUnits(Camera.ScreenCenter) + new Vector2(0, 4.5f);
            Player.Initialize(World, playerInitialPosition, playerOrigin);

            uint[] texData = new uint[_vertSprite.Width * _vertSprite.Height];
            _vertSprite.GetData<uint>(texData);

            Vertices vertices;
            vertices = TextureConverter.DetectVertices(texData, _vertSprite.Width);
            List<Vertices> vertexList = FarseerPhysics.Common.Decomposition.Triangulate.ConvexPartition(vertices, FarseerPhysics.Common.Decomposition.TriangulationAlgorithm.Bayazit);

            Vector2 vertScale = new Vector2(ConvertUnits.ToSimUnits(1));
            foreach (Vertices vert in vertexList)
                vert.Scale(ref vertScale);

            //BodyFactory.CreateCompoundPolygon(World, vertexList, 1, new Vector2(5, 5));

            //Body ground = BodyFactory.CreateRectangle(World, 30, 2, 1, new Vector2(0, 14));
            //ground.Friction = 1.0f;
        }

        protected override void Update(GameTime gameTime)
        {
            HandleKeyboard((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);

            World.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);

            GameMap.Update((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);

            base.Update(gameTime);

            //DEBUG
            //Console.WriteLine("State : " + Player.State);
        }

        private void HandleKeyboard(float dt)
        {
            KeyboardState state = Keyboard.GetState();
            Keys[] pressedKeys = state.GetPressedKeys();

            MoveCamera(state);

            runHelper.Update(dt, state);
            jumpHelper.Update(dt, state);

            if (state.IsKeyDown(Keys.A))
                Player.Move(-1*runHelper.GetValue());

            if (state.IsKeyDown(Keys.D))
                Player.Move(runHelper.GetValue());

            if (pressedKeys.Length == 0 && Player.State != (int)Player.States.Idle)
                Player.Stop();

            if (state.IsKeyDown(Keys.S) && state.IsKeyDown(Keys.Space) && _oldKeyState.IsKeyUp(Keys.Space))
            {
                Player.PassThrough();
            }
            else if (state.IsKeyDown(Keys.Space) && _oldKeyState.IsKeyUp(Keys.Space) && Player.State != (int)Player.States.Jumping)
            {
                Player.Jump(jumpHelper.GetValue());
            }



            if (state.IsKeyDown(Keys.Escape))
                Exit();

            _oldKeyState = state;
        }

        private void MoveCamera(KeyboardState state)
        {
            if (state.IsKeyDown(Keys.Left))
                Camera.TranslateX(1.5f);

            if (state.IsKeyDown(Keys.Right))
                Camera.TranslateX(-1.5f);

            if (state.IsKeyDown(Keys.Up))
                Camera.TranslateY(1.5f);

            if (state.IsKeyDown(Keys.Down))
                Camera.TranslateY(-1.5f);

            Camera.Update();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //Draw circle and ground
            batch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Camera.View);

            GameMap.Draw();

            //batch.Draw(_vertSprite, ConvertUnits.ToDisplayUnits(new Vector2(5, 5)), null, Color.White, 0f, new Vector2(5/2f, 5/2f), 1f, SpriteEffects.None, 0f);

            batch.Draw(AssetManager.Instance._circleSprite, ConvertUnits.ToDisplayUnits(Player.Body.Position), null, Color.White, Player.Body.Rotation, Player.Origin, 1f, SpriteEffects.None, 0f);


            batch.End();

            base.Draw(gameTime);
        }
    }
}