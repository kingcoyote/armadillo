using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Torch;

namespace SRPG.Data
{
    public class Avatar
    {
        /// <summary>
        /// Printable name for the character.
        /// </summary>
        public string Name;

        /// <summary>
        /// The SpriteObject that acts as a visual representation of this character on the battlefield.
        /// </summary>
        public SpriteObject Sprite;
        /// <summary>
        /// An X,Y pair indicating where on the current battlefield this character is located. Outside of a battle scene,
        /// this is not applicable and can be ignored.
        /// </summary>
        public Vector2 Location;


        public Direction Direction;

        public Vector2 Velocity;



        public int FeetWidth = 0;
        public int FeetHeight = 0;

        public EventHandler Interact = (sender, args) => { };



        public void UpdateAnimation()
        {
            // if they are currently moving
            if (Math.Abs(Velocity.X) > 0 || Math.Abs(Velocity.Y) > 0)
            {
                // find out what directions they are actually moving...
                var actualDir = ParseActualDirection(Velocity.X, Velocity.Y);
                // ... and what direction the animation is facing
                var currentDir = StringToDirection(Sprite.GetAnimation().Split(' ')[1]);

                Direction = actualDir[0];

                // if they aren't facing a valid direction, correct it
                if (!actualDir.Contains(currentDir))
                {
                    Sprite.SetAnimation(String.Format("walking {0}", actualDir[0].ToString().ToLower()));
                }

                // if the animation is standing, change it to moving
                if (Sprite.GetAnimation().Split(' ')[0] == "standing")
                {
                    Sprite.SetAnimation(Sprite.GetAnimation().Replace("standing", "walking"));
                }
            }
            else
            {
                // make sure they are standing if they have no velocity
                Sprite.SetAnimation(Sprite.GetAnimation().Replace("walking", "standing"));
            }
        }

        private static List<Direction> ParseActualDirection(float x, float y)
        {
            var dirs = new List<Direction>();

            if (x > 0) dirs.Add(Direction.Right);
            if (x < 0) dirs.Add(Direction.Left);

            if (y > 0) dirs.Add(Direction.Down);
            if (y < 0) dirs.Add(Direction.Up);

            return dirs;
        }

        private static Direction StringToDirection(string str)
        {
            switch (str.ToLower())
            {
                case "up": return Direction.Up;
                case "down": return Direction.Down;
                case "left": return Direction.Left;
                case "right": return Direction.Right;
                default: throw new Exception();
            }
        }

        public Rectangle GetFeet()
        {
            return new Rectangle(
                (int)(Location.X + Sprite.Width / 2 - FeetWidth / 2),
                (int)(Location.Y + Sprite.Height - FeetHeight),
                FeetWidth,
                FeetHeight
            );
        }
    }
}
