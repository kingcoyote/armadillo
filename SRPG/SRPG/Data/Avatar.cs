using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Torch;
using Game = Microsoft.Xna.Framework.Game;

namespace SRPG.Data
{
    public class Avatar : DrawableGameComponent
    {
        /// <summary>
        /// Printable name for the character.
        /// </summary>
        public string Name;

        /// <summary>
        /// The SpriteObject that acts as a visual representation of this character on the battlefield.
        /// </summary>
        /// 
        public SpriteObject Sprite;
        /// <summary>
        /// An X,Y pair indicating where on the current battlefield this character is located. Outside of a battle scene,
        /// this is not applicable and can be ignored.
        /// </summary>
        public Vector2 Location;


        public Direction Direction;


        private Vector2 Velocity;


        public int FeetWidth = 0;


        public int FeetHeight = 0;


        public EventHandler Interact = (sender, args) => { };


        public int Speed = 150;


        public Avatar(Game game) : base(game) { }

        public Rectangle GetFeet()
        {
            return new Rectangle(
                (int)(Location.X + Sprite.Width / 2 - FeetWidth / 2),
                (int)(Location.Y + Sprite.Height - FeetHeight),
                FeetWidth,
                FeetHeight
            );
        }


        public void UpdateVelocity(float x, float y)
        {
            Velocity.X = x;
            Velocity.Y = y;
            UpdateAnimation();
        }


        private void UpdateAnimation()
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
                Sprite.SetAnimation("standing " + Direction.ToString().ToLower());
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

        public static Avatar GenerateAvatar(Microsoft.Xna.Framework.Game game, Torch.Object parent, string className)
        {
            Avatar character;

            int spriteWidth;
            int spriteHeight;

            switch (className)
            {
                case "fighter":
                    spriteWidth = 50;
                    spriteHeight = 100;
                    character = new Avatar(game)
                    {
                        Sprite = new SpriteObject(game, parent, "Characters/fighter"),
                        FeetWidth = 40,
                        FeetHeight = 25
                    };
                    character.Sprite.AddAnimation("walking down", new SpriteAnimation { FrameCount = 7, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 1, FrameRate = 12 });
                    character.Sprite.AddAnimation("standing down", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 1, FrameRate = 1 });
                    character.Sprite.AddAnimation("walking up", new SpriteAnimation { FrameCount = 7, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 101, FrameRate = 12 });
                    character.Sprite.AddAnimation("standing up", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 101, FrameRate = 1 });
                    character.Sprite.AddAnimation("walking left", new SpriteAnimation { FrameCount = 7, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 201, FrameRate = 12 });
                    character.Sprite.AddAnimation("standing left", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 201, FrameRate = 1 });
                    character.Sprite.AddAnimation("walking right", new SpriteAnimation { FrameCount = 7, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 301, FrameRate = 12 });
                    character.Sprite.AddAnimation("standing right", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 301, FrameRate = 1 });
                    character.Sprite.SetAnimation("standing down");
                    break;
                case "ranger":
                    spriteWidth = 50;
                    spriteHeight = 100;
                    character = new Avatar(game)
                    {
                        Sprite = new SpriteObject(game, parent, "Characters/Ranger"),
                        FeetWidth = 40,
                        FeetHeight = 25
                    };
                    character.Sprite.AddAnimation("walking down", new SpriteAnimation { FrameCount = 7, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 1, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing down", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 1, FrameRate = 1 });
                    character.Sprite.AddAnimation("walking up", new SpriteAnimation { FrameCount = 7, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 76, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing up", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 76, FrameRate = 1 });
                    character.Sprite.AddAnimation("walking left", new SpriteAnimation { FrameCount = 7, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 151, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing left", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 151, FrameRate = 1 });
                    character.Sprite.AddAnimation("walking right", new SpriteAnimation { FrameCount = 7, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 226, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing right", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 226, FrameRate = 1 });
                    character.Sprite.SetAnimation("standing down");
                    break;
                case "cleric":
                    spriteWidth = 50;
                    spriteHeight = 100;
                    character = new Avatar(game)
                    {
                        Sprite = new SpriteObject(game, parent, "Characters/cleric"),
                        FeetWidth = 40,
                        FeetHeight = 25
                    };
                    character.Sprite.AddAnimation("walking down", new SpriteAnimation { FrameCount = 7, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 0, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing down", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 0, FrameRate = 1 });
                    character.Sprite.AddAnimation("walking up", new SpriteAnimation { FrameCount = 7, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 100, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing up", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 100, FrameRate = 1 });
                    character.Sprite.AddAnimation("walking left", new SpriteAnimation { FrameCount = 7, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 200, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing left", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 200, FrameRate = 1 });
                    character.Sprite.AddAnimation("walking right", new SpriteAnimation { FrameCount = 7, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 300, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing right", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 300, FrameRate = 1 });
                    character.Sprite.SetAnimation("standing down");
                    break;
                case "wizard":
                    spriteWidth = 50;
                    spriteHeight = 100;
                    character = new Avatar(game)
                    {
                        Sprite = new SpriteObject(game, parent, "Characters/wizard"),
                        FeetWidth = 40,
                        FeetHeight = 25
                    };
                    character.Sprite.AddAnimation("walking down", new SpriteAnimation { FrameCount = 7, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 1, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing down", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 1, FrameRate = 1 });
                    character.Sprite.AddAnimation("walking up", new SpriteAnimation { FrameCount = 7, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 101, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing up", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 101, FrameRate = 1 });
                    character.Sprite.AddAnimation("walking left", new SpriteAnimation { FrameCount = 7, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 201, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing left", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 201, FrameRate = 1 });
                    character.Sprite.AddAnimation("walking right", new SpriteAnimation { FrameCount = 7, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 301, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing right", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 301, FrameRate = 1 });
                    character.Sprite.SetAnimation("standing down");
                    break;
                case "monk":
                    spriteWidth = 65;
                    spriteHeight = 75;
                    character = new Avatar(game)
                    {
                        Sprite = new SpriteObject(game, parent, "Characters/TempMonk"),
                        FeetWidth = 40,
                        FeetHeight = 25
                    };
                    character.Sprite.AddAnimation("walking down", new SpriteAnimation { FrameCount = 8, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 1, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing down", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 1, FrameRate = 1 });
                    character.Sprite.AddAnimation("walking up", new SpriteAnimation { FrameCount = 8, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 76, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing up", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 76, FrameRate = 1 });
                    character.Sprite.AddAnimation("walking left", new SpriteAnimation { FrameCount = 8, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 151, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing left", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 151, FrameRate = 1 });
                    character.Sprite.AddAnimation("walking right", new SpriteAnimation { FrameCount = 8, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 226, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing right", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 226, FrameRate = 1 });
                    character.Sprite.SetAnimation("standing down");
                    break;
                case "enemy":
                    spriteWidth = 50;
                    spriteHeight = 100;
                    character = new Avatar(game)
                    {
                        Sprite = new SpriteObject(game, parent, "Characters/guard"),
                        FeetWidth = 40,
                        FeetHeight = 25
                    };
                    character.Sprite.AddAnimation("walking down", new SpriteAnimation { FrameCount = 6, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 1, StartCol = 50, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing down", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 1, FrameRate = 1 });
                    character.Sprite.AddAnimation("walking up", new SpriteAnimation { FrameCount = 6, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 101, StartCol = 50, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing up", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 101, FrameRate = 1 });
                    character.Sprite.AddAnimation("walking left", new SpriteAnimation { FrameCount = 6, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 201, StartCol = 50, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing left", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 201, FrameRate = 1 });
                    character.Sprite.AddAnimation("walking right", new SpriteAnimation { FrameCount = 6, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 301, StartCol = 50, FrameRate = 8 });
                    character.Sprite.AddAnimation("standing right", new SpriteAnimation { FrameCount = 1, Size = new Rectangle(0, 0, spriteWidth, spriteHeight), StartRow = 301, FrameRate = 1 });
                    character.Sprite.SetAnimation("standing down");
                    break;
                default:
                    throw new Exception(string.Format("unknown avatar {0}", className));
            }

            return character;
        }
    }
}
