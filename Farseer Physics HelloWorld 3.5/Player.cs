using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;

namespace FarseerPhysics.Samples
{
    class Player
    {
        public enum States { Idle, Running, Jumping };

        public Body Body;
        public Vector2 Origin { get; private set; }

        public int State;

        public void Initialize(World world, Vector2 worldPosition, Vector2 worldOrigin)
        {
            Body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(96 / 2f), ConvertUnits.ToSimUnits((96 + 16) / 2f), 1f, worldPosition);
            Body.BodyType = BodyType.Dynamic;

            Body.Restitution = 0.0f;
            Body.Friction = 1.0f;
            Body.Mass = 50.0f;      // 60kg
            Body.FixedRotation = true;
            Body.LinearDamping = 1.0f;

            Body.OnCollision += Body_OnCollision;

            Origin = worldOrigin;
            State = (int) States.Idle;
        }

        public void Jump(float value)
        {
            Body.ApplyLinearImpulse(new Vector2(0, -value));
            State = (int)States.Jumping;
        }

        public void Move(float value)
        {
            if (State == (int)Player.States.Jumping)
            {
                Body.LinearVelocity = new Vector2(value, Body.LinearVelocity.Y);
            }
            else
            {
                Body.LinearVelocity = new Vector2(value, Body.LinearVelocity.Y);
                State = (int)States.Running;
            }

        }

        public void Stop()
        {
            if (State != (int) States.Jumping)
            {
                Body.LinearVelocity = Vector2.Zero;
                State = (int)States.Idle;
            }
        }

        private bool Body_OnCollision(Fixture fA, Fixture fB, Contact contact)
        {
            Body.LinearVelocity = Vector2.Zero;
            State = (int)States.Idle;
            
            return true;
        }

        public void PassThrough()
        {
            List<ForegroundElement> fgelements = Game1.GameMap.GetFGElementsInAABB(Body.Position, 3, 2);
            foreach (ForegroundElement f in fgelements)
            {
                f.Body.Enabled = false;
            }
        }
    }
}
