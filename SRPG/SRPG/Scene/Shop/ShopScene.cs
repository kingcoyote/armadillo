using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SRPG.Data;
using Torch;
using Game = Torch.Game;

namespace SRPG.Scene.Shop
{
    class ShopScene : Torch.Scene
    {
        private List<Item> _inventory = new List<Item>();
        
        public ShopScene(Game game) : base(game)
        {
        }

        public override void Initialize()
        {
            var keyboard = new KeyboardInputLayer(this);
            keyboard.AddKeyDownBinding(Keys.Escape, () => Game.ChangeScenes("overworld"));
            Layers.Add("keyboard", keyboard);

            Layers.Add("background", new Background(this));

            // store inventory layer
            Layers.Add("player inventory", new PlayerInventory(this, ((SRPGGame)Game).Inventory) { X = Game.GetInstance().GraphicsDevice.Viewport.Width - 350, Y = 50 });

            // player inventory layer
            Layers.Add("shop inventory", new ShopInventory(this, _inventory) { X = 50, Y = 50 });

            // buy selected
            // sell selected

            // try on
            // buy and equip
        }

        public void RefreshShop()
        {
            
        }

        public void SetInventory(List<Item> inventory)
        {
            _inventory = inventory;
        }
    }
}
