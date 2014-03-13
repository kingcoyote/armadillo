using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface.Controls.Desktop;
using SRPG.Data;

namespace SRPG.Scene.Shop
{
    partial class ItemPreviewDialog : WindowControl
    {
        public ItemPreviewDialog()
        {
            InitializeComponent();
        }

        public void SetItem(Item item)
        {
            _nameLabel.Text = item.Name;
            _typeLabel.Text = item.ItemType.ToString();
            _defText.Text = String.Format("x{0} / {1}", item.StatMultipliers[Stat.Defense], item.StatBoosts[Stat.Defense]);
            _attText.Text = String.Format("x{0} / {1}", item.StatMultipliers[Stat.Attack], item.StatBoosts[Stat.Attack]);
            _wisText.Text = String.Format("x{0} / {1}", item.StatMultipliers[Stat.Wisdom], item.StatBoosts[Stat.Wisdom]);
            _intText.Text = String.Format("x{0} / {1}", item.StatMultipliers[Stat.Intelligence], item.StatBoosts[Stat.Intelligence]);
            _spdText.Text = String.Format("x{0} / {1}", item.StatMultipliers[Stat.Speed], item.StatBoosts[Stat.Speed]);
            _hitText.Text = String.Format("x{0} / {1}", item.StatMultipliers[Stat.Hit], item.StatBoosts[Stat.Hit]);
            _abilityLabel.Text = item.Ability != null ? item.Ability.Name : "---";
            _priceLabel.Text = item.Cost + "g";
        }
    }
}
