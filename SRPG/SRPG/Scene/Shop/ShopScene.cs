using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using SRPG.Data;
using Torch;

namespace SRPG.Scene.Shop
{
    class ShopScene : Torch.Scene
    {
        private List<Item> _inventory = new List<Item>();
        
        public ShopScene(Game game) : base(game)
        {
            var keyboard = new KeyboardInputLayer(this);
            keyboard.AddKeyDownBinding(Keys.Escape, () => Game.ChangeScenes("overworld"));
            Layers.Add("keyboard", keyboard);
        }

        public override void Initialize()
        {
            
        }

        public void SetInventory(List<Item> inventory)
        {
            _inventory = inventory;
        }
    }
}
