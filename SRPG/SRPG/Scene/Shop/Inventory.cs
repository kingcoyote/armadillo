using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SRPG.Data;
using Torch;
using Game = Torch.Game;

namespace SRPG.Scene.Shop
{
    class Inventory : Layer
    {
        private List<Item> _inventory;
        private List<int> _selectedItems = new List<int>();

        public Inventory(Torch.Scene scene, List<Item> inventory) : base(scene)
        {
            _inventory = inventory;

            Objects.Add("background", new TextureObject() { Color = Color.Black, Width = 350, Z = -1, Height = Game.GetInstance().GraphicsDevice.Viewport.Height - 100});

            RefreshItems();

            MouseClick += SelectItems;
        }

        public List<Item> GetSelectedInventory()
        {
            return _selectedItems.Select(index => _inventory[index]).ToList();
        }

        private void RefreshItems()
        {
            ClearByName("inventory");

            var font = Game.GetInstance().Content.Load<SpriteFont>("Menu");

            for (var i = 0; i < _inventory.Count; i++)
            {
                var item = _inventory[i];

                Objects.Add("inventory/" + i, new TextObject
                    {
                        Font = font,
                        Color = _selectedItems.Contains(i) ? Color.Yellow : Color.White,
                        Value = item.Name,
                        Y = 25 + i*40,
                        X = 25
                    });
            }
        }

        private void SelectItems(object sender, MouseEventArgs args)
        {
            for(var i = 0; i < _inventory.Count; i++)
            {
                if(Objects["inventory/" + i].Rectangle.Contains(new Point((int)(args.X - X), (int)(args.Y - Y))))
                {
                    if(_selectedItems.Contains(i))
                    {
                        _selectedItems.Remove(i);
                    }
                    else
                    {
                        _selectedItems.Add(i);
                    }
                }
            }

            RefreshItems();
        }
    }
}
