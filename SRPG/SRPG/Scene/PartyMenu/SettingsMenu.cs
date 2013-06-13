﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Torch;

namespace SRPG.Scene.PartyMenu
{
    class SettingsMenu : Layer
    {
        public SettingsMenu(Torch.Scene scene) : base(scene)
        {
            Objects.Add("settings", new TextObject()
                {
                    Color = Color.White, 
                    X = 50, 
                    Y = 50, 
                    Font = Torch.Game.GetInstance().Content.Load<SpriteFont>("menu"), Value="Settings"
                });
        }
    }
}