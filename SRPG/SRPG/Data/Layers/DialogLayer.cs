using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nuclex.Input;
using Nuclex.Input.Devices;
using Torch;
using Game = Torch.Game;

namespace SRPG.Data.Layers
{
    public partial class DialogLayer
    {
        private readonly Dialog _dialog;
        private int _currentOption;
        private int _charCount;
        private bool _optionsDisplayed;
        private IKeyboard _keyboard;


        public DialogLayer(Torch.Scene scene, Dialog dialog)
        {
            InitializeComponent();

            _dialog = dialog;

            _keyboard = ((InputManager)scene.Game.Services.GetService(typeof(IInputService))).GetKeyboard();
            _keyboard.KeyPressed += OnKeyPress;

            InitializeDialog();
        }

        public void OnKeyPress(Keys key)
        {
            var dialogContinues = true;

            switch (key)
            {
                case (Keys.Escape):
                    ExitDialog();
                    return;
                case (Keys.Enter):
                case (Keys.E):
                case (Keys.Space):
                    dialogContinues = UpdateDialog();
                    break;
                case (Keys.Up):
                case (Keys.W):
                    if (_optionsDisplayed == false) break;
                    _currentOption++;
                    if (_currentOption >= _dialog.CurrentNode.Options.Count) _currentOption = 0;
                    _dialog.SetOption(_currentOption);
                    UpdateOptionHighlight();
                    break;
                case (Keys.Down):
                case (Keys.S):
                    if (_optionsDisplayed == false) break;
                    _currentOption--;
                    if (_currentOption < 0) _currentOption = _dialog.CurrentNode.Options.Count - 1;
                    _dialog.SetOption(_currentOption);
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
            // todo fix the option highlighting
            if (_currentOption == -1)
            {
                //Objects["dialog highlight"].Y = -1000;
            }
            else
            {
                //Objects["dialog highlight"].Y = Objects["dialog text"].Y + Objects["dialog text"].Height*_currentOption;
            }
        }

        private void InitializeDialog()
        {
            UpdateText(_dialog.CurrentNode.Text);
        }

        private void UpdateText(string text)
        {
       
            _dialogText.Text = text.Trim();

            _charCount += text.Length;

            // todo allow for multi-line / multi window text
            /*while (dialogText.Height > dialogWindow.Height - 20)
            {
                // find the last space
                // trim everything after it
                // adjust char count down by trimmed length

                dialogText.Value = dialogText.Value.Substring(0, dialogText.Value.Length - 1);
                _charCount--;
            }*/
        }

        private bool UpdateDialog()
        {
            /*
             * if there is more text on the node
             *   display it
             * if there is an option menu to display that has not been displayed
             *   display it
             * else
             *   continue
             */


            // if the current node has leftover text
            if (_charCount < _dialog.CurrentNode.Text.Length)
            {
                // display the next page
                UpdateText(_dialog.CurrentNode.Text.Substring(_charCount));
                return true;
            }

            // if there is more than 1 option to choose from
            if (_dialog.CurrentNode.Options.Count > 1 && _optionsDisplayed == false)
            {
                // todo change this from text to buttons
                _dialogText.Text = String.Join(" \n", _dialog.CurrentNode.Options.Keys);
                _currentOption = 0;
                UpdateOptionHighlight();
                _dialog.SetOption(_currentOption);
                _optionsDisplayed = true;
                return true;
            }

            // invoke the old node's OnExit method
            _dialog.CurrentNode.OnExit.Invoke(_dialog, new DialogNodeEventArgs());

            // advance to the next node
            _dialog.Continue();

            _currentOption = -1;
            _optionsDisplayed = false;
            UpdateOptionHighlight();

            // if there is no next node, return false to indicate as much
            if (_dialog.CurrentNode.Identifier == -1)
            {
                return false;
            }

            // invoke the new node's OnEnter method
            _dialog.CurrentNode.OnEnter.Invoke(_dialog, new DialogNodeEventArgs());

            // display the first page of the new node
            _charCount = 0;
            UpdateText(_dialog.CurrentNode.Text);

            return true;
        }

        private void ExitDialog()
        {
            _dialog.OnExit.Invoke(this, new EventArgs());
            _keyboard.KeyPressed -= OnKeyPress;
        }
    }
}
