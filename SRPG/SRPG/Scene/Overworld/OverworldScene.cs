using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SRPG.Data;
using Torch;
using Game = Torch.Game;

namespace SRPG.Scene.Overworld
{
    class OverworldScene : Torch.Scene
    {
        public Character Avatar;
        public Zone Zone;

        public OverworldScene(Game game) : base(game) { }

        public override void Initialize()
        {
            base.Initialize();

            Avatar = CharacterClass.GenerateCharacter("fighter");
            Avatar.Direction = Direction.Down;
            Avatar.Sprite.SetAnimation("standing down");
            Avatar.Location.X = 200;
            Avatar.Location.Y = 200;

            Layers.Add("keyboardinput", new KeyboardInput(this));
            Layers.Add("environment", new Environment(this));
            Layers.Add("hud", new HUD(this));
        }

        public override void Update(GameTime gameTime, Input input)
        {
            base.Update(gameTime, input);

            var dt = (150 * ((float)gameTime.ElapsedGameTime.Milliseconds / 1000));

            float yRevert = 0, xRevert = 0;

            Avatar.Velocity.X = MathHelper.Clamp(Avatar.Velocity.X, -1, 1);
            Avatar.Location.X += Avatar.Velocity.X * dt;

            // if the location hits a black square on the sandbag
            if(!IsValidLocation(new Rectangle((int)Avatar.Location.X, (int)Avatar.Location.Y, Avatar.Sprite.Width, Avatar.Sprite.Height)))
            {
                Avatar.Location.X -= Avatar.Velocity.X * dt;
                xRevert = Avatar.Velocity.X;
                Avatar.Velocity.X = 0;
            }

            Avatar.Velocity.Y = MathHelper.Clamp(Avatar.Velocity.Y, -1, 1);
            Avatar.Location.Y += Avatar.Velocity.Y * dt;

            // if the location hits a black square on the sandbag
            if (!IsValidLocation(new Rectangle((int)Avatar.Location.X, (int)Avatar.Location.Y, Avatar.Sprite.Width, Avatar.Sprite.Height)))
            {
                Avatar.Location.Y -= Avatar.Velocity.Y * dt;
                yRevert = Avatar.Velocity.Y;
                Avatar.Velocity.Y = 0;
            }

            UpdateAnimation();

            Avatar.Velocity.X += xRevert;
            Avatar.Velocity.Y += yRevert;
        }

        private bool IsValidLocation(Rectangle rect)
        {
            for(var x = rect.X; x < rect.X + rect.Width; x += 6)
            {
                for(var y = rect.Y; y < rect.Y + rect.Height; y += 6)
                {
                    // sandbag grids have a 1:6 scaling
                    if (Zone.Sandbag.Weight[x / 6, y / 6] < 64)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public void ChangeDirection(Direction direction, bool enabled)
        {
            switch (direction)
            {
                case Direction.Up: Avatar.Velocity.Y += enabled ? -1 : 1; break;
                case Direction.Down: Avatar.Velocity.Y += enabled ? 1 : -1; break;
                case Direction.Right: Avatar.Velocity.X += enabled ? 1 : -1; break;
                case Direction.Left: Avatar.Velocity.X += enabled ? -1 : 1; break;
            }

            
        }

        public void SetZone(Zone zone)
        {
            Zone = zone;
            ((Environment)Layers["environment"]).SetZone(zone);
        }

        private void UpdateAnimation()
        {
            // if they are currently moving
            if (Math.Abs(Avatar.Velocity.X) > 0 || Math.Abs(Avatar.Velocity.Y) > 0)
            {
                // find out what directions they are actually moving...
                var actualDir = ParseActualDirection(Avatar.Velocity.X, Avatar.Velocity.Y);
                // ... and what direction the animation is facing
                var currentDir = StringToDirection(Avatar.Sprite.GetAnimation().Split(' ')[1]);

                // if they aren't facing a valid direction, correct it
                if (!actualDir.Contains(currentDir))
                {
                    Avatar.Sprite.SetAnimation(String.Format("walking {0}", actualDir[0].ToString().ToLower()));
                }

                // if the animation is standing, change it to moving
                if (Avatar.Sprite.GetAnimation().Split(' ')[0] == "standing")
                {
                    Avatar.Sprite.SetAnimation(Avatar.Sprite.GetAnimation().Replace("standing", "walking"));
                }
            }
            else
            {
                // make sure they are standing if they have no velocity
                Avatar.Sprite.SetAnimation(Avatar.Sprite.GetAnimation().Replace("walking", "standing"));
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
    }
}
