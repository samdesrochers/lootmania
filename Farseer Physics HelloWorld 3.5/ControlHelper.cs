using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Input;

namespace FarseerPhysics.Samples
{
    class ControlHelper
    {
        public enum Types {run, jump};

        private const float MaxMoveValue = 6.5f;
        private const float MaxJumpValue = 15.0f;
        private float MoveValue = 0.0f;
        private int Type;

        public ControlHelper(int type)
        {
            Type = type;
        }

        public void Update(float dt, KeyboardState state)
        {
            switch (Type) 
            {
                case((int)Types.run) :
                   
                    if (state.IsKeyDown(Keys.A) || state.IsKeyDown(Keys.D))
                        MoveValue = Math.Min(MoveValue + (60 * dt), MaxMoveValue);
                    else Reset(); 

                    break;

                case ((int)Types.jump) :

                    if (state.IsKeyDown(Keys.Space))
                        MoveValue = Math.Min(MoveValue + (950 * dt), MaxJumpValue); // 550

                    else Reset();

                    break;
            }

        }

        private void Reset()
        {
            MoveValue = 0.0f;
        }

        public float GetValue()
        {
            return MoveValue;
        }
    }
}
