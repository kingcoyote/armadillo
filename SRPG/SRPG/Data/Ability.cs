using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SRPG.Abilities;
using Torch;
using Game = Microsoft.Xna.Framework.Game;

namespace SRPG.Data
{
    abstract public class Ability : GameComponent
    {
        public string Name;
        public int ManaCost;
        /// <summary>
        /// The character who this ability is tied to. This is used to generate the hit, especially base damage.
        /// </summary>
        public Combatant Character;
        /// <summary>
        /// Indicating whether this is a passive or active ability.
        /// </summary>
        public AbilityType AbilityType;
        /// <summary>
        /// Indicating what type of item this ability is tied to, to ensure it can only be used if the character is
        /// equipped with the right item.
        /// </summary>
        public ItemType ItemType;
        /// <summary>
        /// An indication of who this ability can target - friend or foe.
        /// </summary>
        public AbilityTarget AbilityTarget;

        public SpriteObject Icon;

        protected Ability(Game game) : base(game) { }

        public string ImageName = "ability";

        /// <summary>
        /// Generate one or more hits to be used to damage the targets.
        /// </summary>
        /// <returns>A non-zero list of hits to be applied to a target.</returns>
        public virtual List<Hit> GenerateHits(BattleBoard board, Point target)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Generate a 25x25 grid showing what squares are targetable by this ability. Square 12,12 is the orientation square, where
        /// the character is located, and it is always oriented up. The caller is responsible for proper orientation. A value of 1
        /// indicates that this square can be targetted, 0 indicates it cannot be.
        /// </summary>
        /// <returns>A grid indicating where this ability can be cast.</returns>
        public virtual Grid GenerateTargetGrid(Grid board)
        {
            var targetGrid = GenerateTargetGrid();

            for(var x = 0; x < board.Size.Width; x++)
            {
                for(var y = 0; y < board.Size.Height; y++)
                {
                    var checkPoint = new Point(
                        (int)(x - Character.Avatar.Location.X + targetGrid.Size.Width/2),
                        (int)(y - Character.Avatar.Location.Y + targetGrid.Size.Height/2)
                    );

                    if(checkPoint.X < 0 || checkPoint.X > targetGrid.Size.Width - 1 || checkPoint.Y < 0 || checkPoint.Y > targetGrid.Size.Height - 1)
                    {
                        board.Weight[x, y] = 0;
                        continue;
                    }

                    board.Weight[x, y] = (byte)(board.Weight[x, y] > 0 && targetGrid.Weight[checkPoint.X, checkPoint.Y] > 0 ? 255 : 0);
                }
            }

            return board;
        }

        public virtual Grid GenerateTargetGrid()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Generate a 25x25 grid showing what squares are impacted by this ability. The value is a weight corresponding to how much each
        /// square is impacted (damaged or healed). Square 12,12 is the orientation square, where the ability is targeted, and it is always oriented
        /// up. The caller is responsible for proper orientation.
        /// </summary>
        /// <returns>A grid indicating the impact zone of an ability.</returns>
        public virtual Grid GenerateImpactGrid()
        {
            return new Grid(1, 1, 1);
        }

        public static Ability Factory(Microsoft.Xna.Framework.Game game, string name)
        {
            // todo : there has to be a less tedious way to do this...

            switch(name)
            {
                case "lunge": return new Lunge(game);
                case "cleave": return new Cleave(game);
                // todo : 3rd sword ability

                case "headshot": return new Headshot(game);
                case "drill": return new Drill(game);
                // todo : 3rd gun ability

                case "healing": return new Healing(game);
                case "protect": return new Protect(game);
                case "revive": return new Revive(game);

                case "fire": return new Fire(game);
                case "lightning": return new Lightning(game);
                case "quake": return new Quake(game);

                case "cobra punch": return new CobraPunch(game);
                case "flying knee": return new FlyingKnee(game);
                case "whip kick": return new WhipKick(game);

                case "target": return new Target(game);
                case "focus": return new Focus(game);
                case "serenity": return new Serenity(game);

                case "sprint": return new Sprint(game);
                case "untouchable": return new Untouchable(game);
                case "blur": return new Blur(game);

                case "awareness": return new Awareness(game);
                case "vengeance": return new Vengeance(game);

                case "deflection": return new Deflection(game);
                case "steel wall": return new SteelWall(game);

                case "move": return new Move(game);
                case "attack": return new Attack(game);

                default: throw new Exception(string.Format("Unknown ability {0}", name));

            }
        }

        public override bool Equals(object a)
        {
            if (!(a is Ability)) return false;

            return ((Ability) a).Name == Name;
        }

        protected void SetIcon(string iconName, int i)
        {
            Icon = new SpriteObject(Game, null, "Abilities/" + iconName) {Height = 50, Width = 50};
            Icon.AddAnimation(Name,
                              new SpriteAnimation
                                  {
                                      StartRow = 0,
                                      StartCol = i * 50,
                                      FrameCount = 1,
                                      FrameRate = 1,
                                      Size = new Rectangle(0, 0, 50, 50)
                                  });
            Icon.SetAnimation(Name);
        }

        public bool CanTarget(int faction)
        {
            switch(AbilityTarget)
            {
                case AbilityTarget.Any: return true;
                case AbilityTarget.Friendly: return faction == Character.Faction;
                case AbilityTarget.Enemy: return faction != Character.Faction && faction != -1;
                case AbilityTarget.Unoccupied: return faction == -1;
            }

            throw new Exception("unknown ability target");
        }

        protected bool CanHit(Grid grid, Point target)
        {
            if (!grid.Contains(target.X, target.Y)) return false;

            var newGrid = grid.OverlayGridFromCenter(GenerateTargetGrid(), TorchHelper.Vector2ToPoint(Character.Avatar.Location));

            return newGrid.Weight[target.X, target.Y] > 0;
        }
    }
}
