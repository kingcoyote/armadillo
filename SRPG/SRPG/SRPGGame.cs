using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SRPG.Data;
using SRPG.Data.Items;
using SRPG.Scene.Battle;
using SRPG.Scene.Intro;
using SRPG.Scene.PartyMenu;
using SRPG.Scene.Options;
using SRPG.Scene.Overworld;
using SRPG.Scene.Shop;
using Torch;

namespace SRPG
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SRPGGame : Torch.Game
    {
        public List<Character> Party;
        public List<Item> Inventory;
        public int Money;

        public const int TileSize = 64;

        public SRPGGame()
        {
            IsFullScreen = false;

            Scenes.Add("intro", new IntroScene(this));
            Scenes.Add("party menu", new PartyMenuScene(this));
            Scenes.Add("overworld", new OverworldScene(this));
            Scenes.Add("battle", new BattleScene(this));
            Scenes.Add("shop", new ShopScene(this));
            Scenes.Add("options", new OptionsScene(this));

            CurrentScene = "intro";
        }

        protected override void Initialize()
        {
            base.Initialize();

            
        }

        public void StartGame()
        {
            ChangeScenes("overworld");
            
            ((OverworldScene)Scenes["overworld"]).SetZone(Zone.Factory("kakariko/village"), "arch");

            BeginNewGame();
        }

        public void EquipCharacter(Character character, Item newItem)
        {
            if(Inventory.Contains(newItem))
            {
                var oldItem = character.EquipItem(newItem);
                Inventory.Remove(newItem);
                if(oldItem != null) Inventory.Add(oldItem);
            }
            else
            {
                throw new Exception("this item cannot be equipped because it is not in the inventory");
            }
        }

        private void BeginNewGame()
        {
            Party = new List<Character>();
            Inventory = new List<Item>();

            var character = CharacterClass.GenerateCharacter("fighter");
            character.Name = "Jaha";
            character.MaxHealth = 20;
            Party.Add(character);

            character = CharacterClass.GenerateCharacter("cleric");
            character.Name = "Aeris";
            character.MaxHealth = 12;
            Party.Add(character);

            character = CharacterClass.GenerateCharacter("ranger");
            character.Name = "Meera";
            character.MaxHealth = 14;
            Party.Add(character);

            character = CharacterClass.GenerateCharacter("wizard");
            character.Name = "Raistlin";
            character.MaxHealth = 12;
            Party.Add(character);

            character = CharacterClass.GenerateCharacter("monk");
            character.Name = "Gratho";
            character.MaxHealth = 18;
            Party.Add(character);

            Inventory.Add(Item.Factory("Shortsword"));
            Inventory.Add(Item.Factory("Longsword"));
            Inventory.Add(Item.Factory("Greatsword"));
            Inventory.Add(Item.Factory("Pistol"));
            Inventory.Add(Item.Factory("Revolver"));
            Inventory.Add(Item.Factory("Big Iron"));
            Inventory.Add(Item.Factory("Scroll"));
            Inventory.Add(Item.Factory("Holy Book"));
            Inventory.Add(Item.Factory("Anthology"));
            Inventory.Add(Item.Factory("Fire Wand"));
            Inventory.Add(Item.Factory("Spark Staff"));
            Inventory.Add(Item.Factory("Ground Staff"));
            Inventory.Add(Item.Factory("Brass Knuckles"));
            Inventory.Add(Item.Factory("Tiger Claws"));
            Inventory.Add(Item.Factory("Old Shoes"));

            Inventory.Add(Item.Factory("Vest"));
            Inventory.Add(Item.Factory("Robe"));
            Inventory.Add(Item.Factory("Kimono"));
            Inventory.Add(Item.Factory("Jerkin"));
            Inventory.Add(Item.Factory("Studded Leather"));
            Inventory.Add(Item.Factory("Brigandine"));
            Inventory.Add(Item.Factory("Hauberk"));
            Inventory.Add(Item.Factory("Scale Mail"));
            Inventory.Add(Item.Factory("Dragonskin"));
            Inventory.Add(Item.Factory("Breastplate"));
            Inventory.Add(Item.Factory("Full Plate"));
            Inventory.Add(Item.Factory("Gothic Plate"));


        }
    }
}
