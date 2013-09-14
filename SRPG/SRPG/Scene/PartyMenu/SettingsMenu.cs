using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Torch;

namespace SRPG.Scene.PartyMenu
{
    class SettingsMenu : SubmenuLayer
    {
        public SettingsMenu(Torch.Scene scene) : base(scene)
        {
            //Objects.Add("settings", new TextObject(Game)
            //    {
            //        Color = Color.White, 
            //        X = 50, 
            //        Y = 125, 
            //        Font = FontManager.Get("Menu"), Value="Settings"
            //    });
        }
    }
}
