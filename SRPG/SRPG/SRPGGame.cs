using System;
using System.Collections.Generic;
using SRPG.Data;
using SRPG.Scene.Battle;
using SRPG.Scene.Intro;
using SRPG.Scene.PartyMenu;
using SRPG.Scene.Options;
using SRPG.Scene.Overworld;
using SRPG.Scene.Shop;

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
            character.Stats[Stat.Defense] = 22;
            character.Stats[Stat.Attack] = 18;
            character.Stats[Stat.Wisdom] = 6;
            character.Stats[Stat.Intelligence] = 8;
            character.Stats[Stat.Speed] = 12;
            character.Stats[Stat.Hit] = 15;
            Party.Add(character);

            character = CharacterClass.GenerateCharacter("cleric");
            character.Name = "Aeris";
            character.MaxHealth = 12;
            character.Stats[Stat.Defense] = 8;
            character.Stats[Stat.Attack] = 6;
            character.Stats[Stat.Wisdom] = 22;
            character.Stats[Stat.Intelligence] = 15;
            character.Stats[Stat.Speed] = 12;
            character.Stats[Stat.Hit] = 18;
            Party.Add(character);

            character = CharacterClass.GenerateCharacter("ranger");
            character.Name = "Meera";
            character.MaxHealth = 14;
            character.Stats[Stat.Defense] = 12;
            character.Stats[Stat.Attack] = 15;
            character.Stats[Stat.Wisdom] = 8;
            character.Stats[Stat.Intelligence] = 6;
            character.Stats[Stat.Speed] = 18;
            character.Stats[Stat.Hit] = 22;
            Party.Add(character);

            character = CharacterClass.GenerateCharacter("wizard");
            character.Name = "Raistlin";
            character.MaxHealth = 12;
            character.Stats[Stat.Defense] = 6;
            character.Stats[Stat.Attack] = 12;
            character.Stats[Stat.Wisdom] = 22;
            character.Stats[Stat.Intelligence] = 18;
            character.Stats[Stat.Speed] = 8;
            character.Stats[Stat.Hit] = 15;
            Party.Add(character);

            character = CharacterClass.GenerateCharacter("monk");
            character.Name = "Gratho";
            character.MaxHealth = 18;
            character.Stats[Stat.Defense] = 8;
            character.Stats[Stat.Attack] = 15;
            character.Stats[Stat.Wisdom] = 12;
            character.Stats[Stat.Intelligence] = 6;
            character.Stats[Stat.Speed] = 22;
            character.Stats[Stat.Hit] = 18;
            Party.Add(character);

            Inventory.Add(Item.Factory("sword/shortsword"));
            Inventory.Add(Item.Factory("sword/longsword"));
            Inventory.Add(Item.Factory("sword/greatsword"));



        }
    }
}
