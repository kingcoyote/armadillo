using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nuclex.Input;
using Nuclex.UserInterface.Visuals.Flat;
using SRPG.Data;
using SRPG.Data.Layers;
using Torch;
using Game = Torch.Game;

namespace SRPG.Scene.Overworld
{
    public class OverworldScene : Torch.Scene
    {
        public Avatar Avatar;
        public Zone Zone;

        private bool _isPaused = false;

        private readonly Dictionary<string, Vector2[]> _characterMovements = new Dictionary<string, Vector2[]>();

        private string _startDoor = "";

        private Environment _environment;
        private DialogLayer _dialog;

        public OverworldScene(Game game) : base(game) { }

        public override void Initialize()
        {
            base.Initialize();

            Components.Add(new KeyboardInput(this, null));
            _environment = new Environment(this, null) { DrawOrder = 1 };
            Components.Add(new HUD(this, null) { DrawOrder = 5 });
            Components.Add(_environment);

            Avatar = Avatar.GenerateAvatar(Game, _environment, "fighter");

            Gui.DrawOrder = 1000;
            Gui.Visualizer = FlatGuiVisualizer.FromFile(Game.Services, "Content/Gui/main_menu.xml");
        }

        public override void Update(GameTime gametime)
        {
            base.Update(gametime);

            var dt = (((float)gametime.ElapsedGameTime.Milliseconds / 1000));

            var mouse = ((IInputService)Game.Services.GetService(typeof(IInputService))).GetMouse().GetState();
            var keyboard = ((IInputService)Game.Services.GetService(typeof(IInputService))).GetKeyboard().GetState();

            // update the player's avatar
            UpdateAvatarMovement(dt, keyboard, mouse);
            CheckAvatarDoor();

            // update all characters in the zone
            UpdateCharacterMovements(dt);
        }

        private void UpdateAvatarMovement(float dt, KeyboardState keyboard, MouseState mouse)
        {
            float newX = 0;
            float newY = 0;

            if (keyboard.IsKeyDown(Keys.A) && !keyboard.IsKeyDown(Keys.D))
            {
                newX = -1;
                Avatar.Direction = Direction.Left;
            }
            else if (keyboard.IsKeyDown(Keys.D) && !keyboard.IsKeyDown(Keys.A))
            {
                newX = 1;
                Avatar.Direction = Direction.Right;
            }

            if (keyboard.IsKeyDown(Keys.W) && !keyboard.IsKeyDown(Keys.S))
            {
                newY = -1;
                Avatar.Direction = Direction.Up;
            }
            else if (keyboard.IsKeyDown(Keys.S) && !keyboard.IsKeyDown(Keys.W))
            {
                newY = 1;
                Avatar.Direction = Direction.Down;
            }

            Avatar.Location.X += newX * dt * Avatar.Speed;

            if (!IsValidLocation(Avatar.GetFeet()))
            {
                Avatar.Location.X -= (newX * dt * Avatar.Speed);
                newX = 0;
            }

            Avatar.Location.Y += newY*dt*Avatar.Speed;

            if (!IsValidLocation(Avatar.GetFeet()))
            {
                Avatar.Location.Y -= (newY * dt * Avatar.Speed);
                newY = 0;
            }

            Avatar.UpdateVelocity(newX, newY);
        }

        private void CheckAvatarDoor()
        {
            var door =
                (from d in Zone.Doors where d.Location.Intersects(Avatar.GetFeet()) && !String.IsNullOrEmpty(d.Zone) select d);
            if (door.Any() && door.First().Name != _startDoor)
            {
                SetZone(Zone.Factory(Game, null, door.First().Zone), door.First().ZoneDoor);
            }
            else if (!door.Any())
            {
                _startDoor = "";
            }
        }

        private void UpdateCharacterMovements(float dt)
        {
            foreach (var character in _characterMovements.Keys.ToList())
            {
                var vector = _characterMovements[character][0];
                float x = 0;
                float y = 0;

                if (vector.X > 1) x = Zone.Characters[character].Speed;
                else if (vector.X < -1) x = 0 - Zone.Characters[character].Speed;

                _characterMovements[character][0].X -= x*dt;
                Zone.Characters[character].Location.X += x*dt;

                if (vector.Y > 1) y = Zone.Characters[character].Speed;
                else if (vector.Y < -1) y = 0 - Zone.Characters[character].Speed;

                _characterMovements[character][0].Y -= y*dt;
                Zone.Characters[character].Location.Y += y*dt;

                if (Math.Abs(vector.X) <= 1 && Math.Abs(vector.Y) <= 1)
                {
                    var movements = _characterMovements[character].ToList();
                    movements.RemoveAt(0);
                    if (movements.Count == 0) _characterMovements.Remove(character);
                    else _characterMovements[character] = movements.ToArray();
                }

                Zone.Characters[character].UpdateVelocity(x, y);
            }
        }

        private bool IsValidLocation(Rectangle rect)
        {
            for(var x = rect.X; x < rect.X + rect.Width; x++)
            {
                for(var y = rect.Y; y < rect.Y + rect.Height; y++)
                {
                    // sandbag grids have a 1:6 scaling
                    if (Zone.Sandbag.Weight[x / 6, y / 6] < 250)
                    {
                        return false;
                    }
                }
            }

            foreach(var character in Zone.Characters.Values)
            {
                if (rect.Intersects(character.GetFeet()))
                {
                    return false;
                }
            }

            return true;
        }

        public void SetZone(Zone zone, string doorName)
        {
            Zone = zone;
            _environment.SetZone(zone);

            var door = (from d in zone.Doors where d.Name == doorName select d).First();
            Avatar.Sprite.SetAnimation(string.Format("standing {0}", door.Orientation.ToString().ToLower()));
            Avatar.Location.X = door.Location.X + (door.Location.Width/2) - (Avatar.Sprite.Width/2);
            Avatar.Location.Y = door.Location.Y + (door.Location.Height / 2) - (Avatar.Sprite.Height / 2) - (Avatar.Sprite.Height - Avatar.GetFeet().Height) / 2;
            Avatar.Direction = door.Orientation;

            _startDoor = doorName;
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

            foreach(var character in Zone.Characters.Values)
            {
                if(scanBox.Intersects(character.GetFeet()))
                {
                    character.Interact(this, new EventArgs());
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

        public void MoveCharacter(string name, Vector2[] directions)
        {
            if (_characterMovements.Keys.Contains(name)) _characterMovements.Remove(name);
            _characterMovements.Add(name, directions);
        }

        public void StartDialog(Dialog dialog)
        {
            StopCharacter();

            _isPaused = true;
            dialog.OnExit += EndDialogEvent;
            _dialog = new DialogLayer(this, dialog);
            Gui.Screen.Desktop.Children.Add(_dialog);
        }

        private void StopCharacter()
        {
            Avatar.UpdateVelocity(0, 0);
        }

        public void EndDialog()
        {
            Gui.Screen.Desktop.Children.Remove(_dialog);
            _dialog = null;
            _isPaused = false;
        }

        public void EndDialogEvent(object sender, EventArgs args)
        {
            EndDialog();
        }

        public void OpenPartyMenu()
        {
            if (_isPaused) return;
            StopCharacter();

            Game.ChangeScenes("party menu");
        }
    }
}
