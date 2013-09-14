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

        private Avatar _avatar;
        private readonly Dictionary<string, Avatar> _characters = new Dictionary<string, Avatar>();

        public Environment(Torch.Scene scene) : base(scene) { }

        public void SetZone(Zone zone)
        {
            _zone = zone;

            _avatar = ((OverworldScene) Scene).Avatar;
            Components.Add(_avatar.Sprite);

            var i = 0;
            foreach (var image in zone.ImageLayers)
            {
                Components.Add(image);
                i++;
            }

            foreach(var character in zone.Characters.Keys)
            {
                _characters.Add(character, zone.Characters[character]);
                Components.Add(zone.Characters[character].Sprite);
            }

            // sandbags are down scaled 1:6
            _width = zone.Sandbag.Size.Width * 6;
            _height = zone.Sandbag.Size.Height * 6;
        }

        public override void Update(GameTime gametime)
        {
            base.Update(gametime);

            _avatar.Sprite.X = (int)((OverworldScene) Scene).Avatar.Location.X;
            _avatar.Sprite.Y = (int)((OverworldScene)Scene).Avatar.Location.Y;

            const float deadzone = 0.2F;
            int screenWidth = Game.Window.ClientBounds.Width;
            var deadzoneWidth = (int)(screenWidth*(1 - deadzone)/2);

            int screenHeight = Game.Window.ClientBounds.Height;
            var deadzoneHeight = (int)(screenHeight * (1 - deadzone) / 2);

            // if the screen can go to the left and if the avatar is left of the deadzone
            if(X < 0 && _avatar.Sprite.X < 0 - X + deadzoneWidth)
            {
                // slide the screen to the left
                X -= _avatar.Sprite.X - (0 - X + deadzoneWidth);
            }
            // same thing for the right side
            else if (0 - X < _width - screenWidth && _avatar.Sprite.X > 0 - X + screenWidth - deadzoneWidth)
            {
                X -= _avatar.Sprite.X - (0 - X + screenWidth - deadzoneWidth);
            }

            // bottom
            if (Y < 0 && _avatar.Sprite.Y < 0 - Y + deadzoneHeight)
            {
                // slide the screen to the left
                Y -= _avatar.Sprite.Y - (0 - Y + deadzoneHeight);
            }
            // top
            else if (0 - Y < _height - screenHeight && _avatar.Sprite.Y > 0 - Y + screenHeight - deadzoneHeight)
            {
                Y -= _avatar.Sprite.Y - (0 - Y + screenHeight - deadzoneHeight);
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
                _characters[character].Sprite.X = (int)_zone.Characters[character].Location.X;
                _characters[character].Sprite.Y = (int)_zone.Characters[character].Location.Y;
                _characters[character].Sprite.DrawOrder = _characters[character].Sprite.Y - ((_characters[character])).Sprite.Height;
            }
        }
    }
}
