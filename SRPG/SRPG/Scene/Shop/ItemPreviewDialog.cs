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
            _defLabel.Text = "DEF";
            _defText.Text = String.Format("x{0} / {1}", item.StatMultipliers[Stat.Defense], item.StatBoosts[Stat.Defense]);
            _attLabel.Text = "ATT";
            _attText.Text = String.Format("x{0} / {1}", item.StatMultipliers[Stat.Attack], item.StatBoosts[Stat.Attack]);
            _wisLabel.Text = "WIS";
            _wisText.Text = String.Format("x{0} / {1}", item.StatMultipliers[Stat.Wisdom], item.StatBoosts[Stat.Wisdom]);
            _intLabel.Text = "INT";
            _intText.Text = String.Format("x{0} / {1}", item.StatMultipliers[Stat.Intelligence], item.StatBoosts[Stat.Intelligence]);
            _spdLabel.Text = "SPD";
            _spdText.Text = String.Format("x{0} / {1}", item.StatMultipliers[Stat.Speed], item.StatBoosts[Stat.Speed]);
            _hitLabel.Text = "HIT";
            _hitText.Text = String.Format("x{0} / {1}", item.StatMultipliers[Stat.Hit], item.StatBoosts[Stat.Hit]);
            _abilityLabel.Text = item.Ability != null ? item.Ability.Name : "---";
            _priceLabel.Text = item.Cost + "g";
        }

        public void ClearItem()
        {
            _nameLabel.Text = "";
            _typeLabel.Text = "";
            _defLabel.Text = "";
            _defText.Text = "";
            _attLabel.Text = "";
            _attText.Text = "";
            _wisLabel.Text = "";
            _wisText.Text = "";
            _intLabel.Text = "";
            _intText.Text = "";
            _spdLabel.Text = "";
            _spdText.Text = "";
            _hitLabel.Text = "";
            _hitText.Text = "";
            _abilityLabel.Text = "";
            _priceLabel.Text = "";
        }
    }
}
