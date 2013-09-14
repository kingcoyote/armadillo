using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;
using Nuclex.UserInterface.Controls.Desktop;
using Torch;
using Game = Torch.Game;

namespace SRPG.Data.Layers
{
    partial class DialogLayer : WindowControl
    {
        private LabelControl _dialogText;

        private void InitializeComponent()
        {
            _dialogText = new LabelControl();
            Children.Add(_dialogText);

            Bounds = new UniRectangle(
                new UniScalar(0.0f, 0.0f), new UniScalar(1.0f, -250.0f),      
                new UniScalar(1.0f, 0.0f), new UniScalar(0.0f, 225.0f) 
            );

            

            /*Objects.Add("dialog window", new TextureObject()
            {
                Color = Color.Blue,
                Z = 100000,
                X = (int)(Game.Window.ClientBounds.Width * 0.05),
                Y = (int)(Game.Window.ClientBounds.Height * 0.75),
                Width = (int)(Game.Window.ClientBounds.Width * 0.9),
                Height = (int)(Game.Window.ClientBounds.Height * 0.2)
            });
            Objects.Add("dialog text", new TextObject()
            {
                Color = Color.White,
                Z = 100002,
                Value = "",
                Font = FontManager.Get("Dialog"),
                X = Objects["dialog window"].X + 20,
                Y = Objects["dialog window"].Y + 10,
                Width = Objects["dialog window"].Width - 40,
                Height = Objects["dialog window"].Height - 20
            });
            Objects.Add("dialog highlight", new TextureObject()
            {
                Color = Color.Black,
                Z = 100001,
                Y = -9999,
                Width = Objects["dialog window"].Width - 20,
                Height = ((TextObject)Objects["dialog text"]).Font.LineSpacing,
                X = Objects["dialog window"].X + 10
            });*/
        }
    }
}
