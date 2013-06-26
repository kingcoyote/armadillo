using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
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
        private readonly Dictionary<Direction, bool> _directions = new Dictionary<Direction, bool>(4)
            {{ Direction.Up, false},{Direction.Down,false},{Direction.Left, false},{Direction.Right,false}};

        private readonly Dictionary<string, Vector2[]> _characterMovements = new Dictionary<string, Vector2[]>();

        private string _startDoor = "";

        public OverworldScene(Game game) : base(game) { }

        public override void Initialize()
        {
            base.Initialize();

            Avatar = CharacterClass.GenerateCharacter("ranger");

            Layers.Add("keyboardinput", new KeyboardInput(this));
            Layers.Add("environment", new Environment(this));
            Layers.Add("hud", new HUD(this));
        }

        public override void Update(GameTime gameTime, Input input)
        {
            base.Update(gameTime, input);

            var dt = (((float)gameTime.ElapsedGameTime.Milliseconds / 1000));
            var movementSpeed = 150;

            float yRevert = 0, xRevert = 0;

            Avatar.Velocity.X = MathHelper.Clamp(Avatar.Velocity.X, -1, 1);
            Avatar.Location.X += Avatar.Velocity.X * dt * movementSpeed;

            if(!IsValidLocation(Avatar.GetFeet()))
            {
                xRevert = Avatar.Velocity.X;
                Avatar.Location.X -= Avatar.Velocity.X*dt*movementSpeed;
                Avatar.Velocity.X = 0;

            }

            Avatar.Velocity.Y = MathHelper.Clamp(Avatar.Velocity.Y, -1, 1);
            Avatar.Location.Y += Avatar.Velocity.Y * dt * movementSpeed;

            if (!IsValidLocation(Avatar.GetFeet()))
            {
                yRevert = Avatar.Velocity.Y;
                Avatar.Location.Y -= Avatar.Velocity.Y*dt*movementSpeed;
                Avatar.Velocity.Y = 0;
            }

            Avatar.Sprite.Z = Avatar.Sprite.Y + Avatar.Sprite.Height;

            Avatar.Velocity.X += xRevert;
            Avatar.Velocity.Y += yRevert;

            Avatar.UpdateAnimation();

            // find a door that the avatar is in that leads elsewhere
            var door = (from d in Zone.Doors where d.Location.Intersects(Avatar.GetFeet()) && !String.IsNullOrEmpty(d.Zone) select d);
            if(door.Any() && door.First().Name != _startDoor)
            {
                SetZone(Zone.Factory(door.First().Zone), door.First().ZoneDoor);    
            }
            else if (!door.Any())
            {
                _startDoor = "";
            }

            foreach(var character in _characterMovements.Keys.ToList())
            {
                var vector = _characterMovements[character][0];
                float x = 0;
                float y = 0;

                if(vector.X > 1) x = movementSpeed;
                else if (vector.X < -1) x = 0 - movementSpeed;
                
                _characterMovements[character][0].X -= x*dt;
                Zone.Characters[character].Location.X += x * dt;

                if (vector.Y > 1) y = movementSpeed;
                else if (vector.Y < -1) y = 0 - movementSpeed;

                _characterMovements[character][0].Y -= y * dt;
                Zone.Characters[character].Location.Y += y * dt;

                if(Math.Abs(vector.X) <= 1 && Math.Abs(vector.Y) <= 1)
                {
                    var movements = _characterMovements[character].ToList();
                    movements.RemoveAt(0);
                    if (movements.Count == 0) _characterMovements.Remove(character);
                    else _characterMovements[character] = movements.ToArray();
                }

                Zone.Characters[character].Velocity.X = x;
                Zone.Characters[character].Velocity.Y = y;
                Zone.Characters[character].UpdateAnimation();
            }
            
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
            ((Environment)Layers["environment"]).SetZone(zone);

            var door = (from d in zone.Doors where d.Name == doorName select d).First();
            Avatar.Sprite.SetAnimation(string.Format("standing {0}", door.Orientation.ToString().ToLower()));
            Avatar.Location.X = door.Location.X + (door.Location.Width/2) - (Avatar.Sprite.Width/2);
            Avatar.Location.Y = door.Location.Y + (door.Location.Height / 2) - (Avatar.Sprite.Height / 2) - (Avatar.Sprite.Height - Avatar.GetFeet().Height) / 2;
            Avatar.Direction = door.Orientation;

            _startDoor = doorName;
        }

        public void ChangeDirection(Direction direction, bool enabled)
        {
            if (_isPaused) return;

            if (_directions[direction] == enabled) return;
            _directions[direction] = enabled;

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
            Layers.Add("dialog", new DialogLayer(this, dialog));
        }

        private void StopCharacter()
        {
            Avatar.Velocity.X = 0;
            Avatar.Velocity.Y = 0;
            _directions[Direction.Up] = false;
            _directions[Direction.Down] = false;
            _directions[Direction.Left] = false;
            _directions[Direction.Right] = false;
        }

        public void EndDialog()
        {
            Layers.Remove("dialog");
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
