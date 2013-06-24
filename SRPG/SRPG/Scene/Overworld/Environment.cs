using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SRPG.Data;
using Torch;
using Game = Torch.Game;

namespace SRPG.Scene.Overworld
{
    class Environment : Layer
    {
        private int _width;
        private int _height;

        private Zone _zone;

        public Environment(Torch.Scene scene) : base(scene) { }

        public void SetZone(Zone zone)
        {
            _zone = zone;

            Objects.Clear();

            Objects.Add("avatar", ((OverworldScene)Scene).Avatar.Sprite);

            var i = 0;
            foreach (var image in zone.ImageLayers)
            {
                Objects.Add("zone" + i, image);
                i++;
            }

            foreach(var character in zone.Characters.Keys)
            {
                Objects.Add("character/" + character, zone.Characters[character].Sprite);
            }

            // test code to show interactive objects. this should not make it into release.
            i = 0;
            foreach (var obj in zone.Objects)
            {
                //Objects.Add("interact" + i, new TextureObject { Color = Color.Red, X = obj.Location.X, Y = obj.Location.Y, Width = obj.Location.Width, Height = obj.Location.Height, Z = 9999 });
                i++;
            }

            // sandbags are down scaled 1:6
            _width = zone.Sandbag.Size.Width * 6;
            _height = zone.Sandbag.Size.Height * 6;
        }

        public override void Update(GameTime gameTime, Input input)
        {
            base.Update(gameTime, input);

            Objects["avatar"].X = (int)((OverworldScene) Scene).Avatar.Location.X;
            Objects["avatar"].Y = (int)((OverworldScene)Scene).Avatar.Location.Y;

            const float deadzone = 0.2F;
            int screenWidth = Game.GetInstance().Window.ClientBounds.Width;
            var deadzoneWidth = (int)(screenWidth*(1 - deadzone)/2);

            int screenHeight = Game.GetInstance().Window.ClientBounds.Height;
            var deadzoneHeight = (int)(screenHeight * (1 - deadzone) / 2);

            // if the screen can go to the left and if the avatar is left of the deadzone
            if(X < 0 && Objects["avatar"].X < 0 - X + deadzoneWidth)
            {
                // slide the screen to the left
                X -= Objects["avatar"].X - (0 - X + deadzoneWidth);
            }
            // same thing for the right side
            else if (0 - X < _width - screenWidth && Objects["avatar"].X > 0 - X + screenWidth - deadzoneWidth)
            {
                X -= Objects["avatar"].X - (0 - X + screenWidth - deadzoneWidth);
            }

            // bottom
            if (Y < 0 && Objects["avatar"].Y < 0 - Y + deadzoneHeight)
            {
                // slide the screen to the left
                Y -= Objects["avatar"].Y - (0 - Y + deadzoneHeight);
            }
            // top
            else if (0 - Y < _height - screenHeight && Objects["avatar"].Y > 0 - Y + screenHeight - deadzoneHeight)
            {
                Y -= Objects["avatar"].Y - (0 - Y + screenHeight - deadzoneHeight);
            }

            // ensure that there is no black area at the edge of the screen. 
            // this can happen when the start point is near the bottom or right edge
            if (_width > screenWidth)
            {
                if (X < 0 - _width + screenWidth) X = 0 - _width + screenWidth;
                if (X > 0) X = 0;
            }
            else
            {
                X = screenWidth/2 - _width/2;
            }

            if (_height > screenHeight)
            {
                if (Y < 0 - _height + screenHeight) Y = 0 - _height + screenHeight;
            }
            else
            {
                Y = screenHeight / 2 - _height / 2;
            }

            foreach(var character in _zone.Characters.Keys)
            {
                Objects["character/" + character].X = (int)_zone.Characters[character].Location.X;
                Objects["character/" + character].Y = (int)_zone.Characters[character].Location.Y;
                Objects["character/" + character].Z = Objects["character/" + character].Y + ((SpriteObject)(Objects["character/" + character])).Height;
            }
        }
    }
}
