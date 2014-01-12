using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SRPG.Data;

namespace SRPG.Scene.PartyMenu
{
    public partial class ItemDisplay
    {
        private Item _item;
        
        public ItemDisplay(Item item)
        {
            _item = item;
            InitializeComponent();
        }
    }
}
