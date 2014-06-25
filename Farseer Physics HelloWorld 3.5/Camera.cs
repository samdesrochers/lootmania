using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace FarseerPhysics.Samples
{
    public class Camera
    {
        public GraphicsDeviceManager Graphics;

        public Matrix View;
        public Vector2 Position;
        public Vector2 ScreenCenter;

        public float Zoom { get; set; }
        public float Rotation { get; set; }

        public void Initialize(GraphicsDeviceManager graphics)
        {
            Graphics = graphics;
            Rotation = 0.0f;
            Zoom = 0.5f;

            View = Matrix.Identity;
            Position = Vector2.Zero;

            ScreenCenter = new Vector2(Graphics.GraphicsDevice.Viewport.Width / Zoom / 2f, Graphics.GraphicsDevice.Viewport.Height / Zoom / 2f);
        }

        bool ascending = true;
        public void Update()
        {
            //if (ascending)
            //{
            //    Zoom += 0.005f;
            //    if (Zoom > 1.5f) { ascending = false; }
            //}
            //else
            //{
            //    Zoom -= 0.005f;
            //    if (Zoom < 0.5f) { ascending = true; }
            //}


            View = Matrix.CreateTranslation(new Vector3(Position - ScreenCenter, 0f)) * Matrix.CreateTranslation(new Vector3(ScreenCenter, 0f)) * 
                   Matrix.CreateRotationZ(Rotation) *
                   Matrix.CreateScale(Zoom);
        }

        public void TranslateX(float value)
        {
            Position.X += value;
        }

        public void TranslateY(float value)
        {
            Position.Y += value;
        }

        public Vector2 GetWorldPosition(Vector2 screenPosition)
        {
            return Vector2.Transform(screenPosition, Matrix.Invert(View));
        }

        public Vector2 GetScreenPosition(Vector2 worldPosition)
        {
            return Vector2.Transform(worldPosition, View);
        }

        public Vector2 ToWorld(Vector2 pos)
        {
            return new Vector2(pos.X / Zoom, pos.Y / Zoom);
        }

        public float ToData(float pos)
        {
            return pos * Zoom;
        }

        public float ToWorld(float pos)
        {
            return pos / Zoom;
        }
    }
}
