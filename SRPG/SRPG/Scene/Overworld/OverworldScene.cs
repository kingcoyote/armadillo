using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SRPG.Data;
using SRPG.Data.Layers;
using Torch;
using Game = Torch.Game;

namespace SRPG.Scene.Overworld
{
    public class OverworldScene : Torch.Scene
    {
        public Character Avatar;
        public Zone Zone;

        private bool _isPaused = false;

        public OverworldScene(Game game) : base(game) { }

        public override void Initialize()
        {
            base.Initialize();

            Avatar = CharacterClass.GenerateCharacter("link");
            Avatar.Direction = Direction.Down;
            Avatar.Sprite.SetAnimation("standing down");
            Avatar.Location.X = 600;
            Avatar.Location.Y = 1300;

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

            if(!IsValidLocation(Avatar.GetFeet()))
            {
                xRevert = Avatar.Velocity.X;
                Avatar.Location.X -= Avatar.Velocity.X*dt;
                Avatar.Velocity.X = 0;

            }

            Avatar.Velocity.Y = MathHelper.Clamp(Avatar.Velocity.Y, -1, 1);
            Avatar.Location.Y += Avatar.Velocity.Y * dt;

            if (!IsValidLocation(Avatar.GetFeet()))
            {
                yRevert = Avatar.Velocity.Y;
                Avatar.Location.Y -= Avatar.Velocity.Y*dt;
                Avatar.Velocity.Y = 0;
            }

            Avatar.UpdateAnimation();

            Avatar.Sprite.Z = Avatar.Sprite.Y + Avatar.Sprite.Height;

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
                    if (Zone.Sandbag.Weight[x / 6, y / 6] < 250)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public void SetZone(Zone zone)
        {
            Zone = zone;
            ((Environment)Layers["environment"]).SetZone(zone);
        }

        public void ChangeDirection(Direction direction, bool enabled)
        {
            if (_isPaused) return;

            switch (direction)
            {
                case Direction.Up: Avatar.Velocity.Y += enabled ? -1 : 1; break;
                case Direction.Down: Avatar.Velocity.Y += enabled ? 1 : -1; break;
                case Direction.Right: Avatar.Velocity.X += enabled ? 1 : -1; break;
                case Direction.Left: Avatar.Velocity.X += enabled ? -1 : 1; break;
            }
        }

        public void Interact()
        {
            if (_isPaused) return;

            var eventArgs = new InteractEventArgs() {Character = Avatar, Scene = this};
            Rectangle scanBox;

            Rectangle feet = Avatar.GetFeet();

            switch(Avatar.Direction)
            {
                case Direction.Up:
                    scanBox = new Rectangle(feet.X, feet.Y - 5, feet.Width, 5);
                    break;
                case Direction.Down:
                    scanBox = new Rectangle(feet.X, feet.Y + feet.Height + 5, feet.Width, 5);
                    break;
                case Direction.Left:
                    scanBox = new Rectangle(feet.X - 5, feet.Y, 5, feet.Height);
                    break;
                case Direction.Right:
                    scanBox = new Rectangle(feet.X + feet.Width, feet.Y, 5, feet.Height);
                    break;
                default:
                    throw new Exception("unable to process interaction without a valid avatar direction");

            }

            foreach(InteractiveObject obj in Zone.Objects)
            {
                if (obj.Location.Intersects(scanBox))
                {
                    obj.Interact.Invoke(this, eventArgs);
                }
            }

            // scan the area in front of the player
            // if there is a valid interaction object
            //    execute the interact callback for that object

            // dialog would need to pause the controls and create a dialog layer, which will unpause when done
            // doors would need to change the zone
            // chests would need to pause the controls, create a dialog layer, give the player a new item, and unpause when done
            // levers would need to update some part of the zone (i have zero ways to do this!)
        }

        public void StartDialog(Dialog dialog)
        {
            Avatar.Velocity.X = 0;
            Avatar.Velocity.Y = 0;

            _isPaused = true;
            Layers.Add("dialog", new DialogLayer(this, dialog));
        }

        public void EndDialog()
        {
            Layers.Remove("dialog");
            _isPaused = false;
        }
    }
}
