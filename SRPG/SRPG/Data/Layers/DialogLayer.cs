using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Torch;
using Game = Torch.Game;

namespace SRPG.Data.Layers
{
    class DialogLayer : Layer
    {
        private readonly Dialog _dialog;
        private int _currentOption;
        private int _charCount;

        public DialogLayer(Torch.Scene scene, Dialog dialog) : base(scene)
        {
            _dialog = dialog;

            Objects.Add("dialog window", new TextureObject()
                {
                    Color = Color.Blue, 
                    Z = 100000, 
                    X = (int)(Game.GetInstance().Window.ClientBounds.Width * 0.05),
                    Y = (int)(Game.GetInstance().Window.ClientBounds.Height* 0.75),
                    Width = (int)(Game.GetInstance().Window.ClientBounds.Width * 0.9),
                    Height = (int)(Game.GetInstance().Window.ClientBounds.Height * 0.2)
                });
            Objects.Add("dialog highlight", new TextureObject() { Color = Color.Yellow, Z = 100001 });
            Objects.Add("dialog text", new TextObject()
                {
                    Color = Color.White, 
                    Z = 100002, 
                    Value = "", 
                    Font = Game.GetInstance().Content.Load<SpriteFont>("dialogfont"),
                    X = Objects["dialog window"].X + 10,
                    Y = Objects["dialog window"].Y + 10,
                    Width = Objects["dialog window"].Width - 20,
                    Height = Objects["dialog window"].Height - 20
                });

            KeyDown += OnKeyPress;
        }

        public void OnKeyPress(object sender, KeyboardEventArgs args)
        {
            var dialogContinues = true;

            switch (args.WhichKey)
            {
                case (Keys.Escape):
                    ExitDialog();
                    return;
                case (Keys.Enter):
                    dialogContinues = UpdateDialog();
                    break;
                case (Keys.Up):
                    _currentOption++;
                    if (_currentOption >= _dialog.CurrentNode.Options.Count) _currentOption = 0;
                    UpdateOptionHighlight();
                    break;
                case (Keys.Down):
                    _currentOption--;
                    if (_currentOption < 0) _currentOption = _dialog.CurrentNode.Options.Count - 1;
                    UpdateOptionHighlight();
                    break;
            }

            if (!dialogContinues)
            {
                ExitDialog();
            }
        }

        private void UpdateOptionHighlight()
        {
            // move the highlight boxes to surround the option object whose key matches _currentOption
            // todo highlight the current option
        }

        private bool UpdateDialog()
        {
            // if the current node has leftover text
            if (_charCount < _dialog.CurrentNode.Text.Length)
            {
                // todo update the dialog to the next page of text
                // clear the text
                // while text can fit in the window and there is more text
                //   add the next word to the window
                // update _charCount
                return true;
            }

            // invoke the old node's OnExit method
            _dialog.CurrentNode.OnExit.Invoke(_dialog, new DialogNodeEventArgs());

            // advance to the next node
            _dialog.Continue();

            _charCount = 0;

            // if there is no next node, return false to indicate as much
            if (_dialog.CurrentNode.Identifier == -1)
            {
                return false;
            }

            // invoke the new node's OnEnter method
            _dialog.CurrentNode.OnEnter.Invoke(_dialog, new DialogNodeEventArgs());
            
            // todo add the new text to the page

            // take the text in the current node and put it into the layer
            //   if there is leftover text
            //     return false

            // todo add options to the page
            // take each option and put them into the layer 
            // set the first option as current
            // update option highlight

            return true;
        }

        private void ExitDialog()
        {
            Objects.Clear();
            _dialog.OnExit.Invoke(this, new EventArgs());
        }
    }
}
