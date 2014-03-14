using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface.Controls.Desktop;
using SRPG.Data;

namespace SRPG.Scene.Shop
{
    partial class CharacterDialog : WindowControl
    {
        private Combatant _character;

        public CharacterDialog(Combatant character)
        {
            InitializeComponent();

            _character = character;

            ResetCharacter();
        }

        public void PreviewItem(Item item)
        {
            if (_character.CanEquipItem(item) == false) return;

            _defLabel.Text = DisplayStat(Stat.Defense, item);
            _attLabel.Text = DisplayStat(Stat.Attack, item);
            _wisLabel.Text = DisplayStat(Stat.Wisdom, item);
            _intLabel.Text = DisplayStat(Stat.Intelligence, item);
            _spdLabel.Text = DisplayStat(Stat.Speed, item);
            _hitLabel.Text = DisplayStat(Stat.Hit, item);
        }

        public void ResetCharacter()
        {
            _nameLabel.Text = _character.Name;
            _classLabel.Text = _character.Class;

            _defLabel.Text = _character.ReadStat(Stat.Defense).ToString();
            _attLabel.Text = _character.ReadStat(Stat.Attack).ToString();
            _wisLabel.Text = _character.ReadStat(Stat.Wisdom).ToString();
            _intLabel.Text = _character.ReadStat(Stat.Intelligence).ToString();
            _spdLabel.Text = _character.ReadStat(Stat.Speed).ToString();
            _hitLabel.Text = _character.ReadStat(Stat.Hit).ToString();
        }

        private string DisplayStat(Stat stat, Item item)
        {
            var oldStat = _character.ReadStat(stat);
            var change = _character.CompareStat(stat, item);

            if (change < 0)
            {
                return string.Format("{0}   -{1}", oldStat, Math.Abs(change));
            } if (change > 0)
            {
                return string.Format("{0}   +{1}", oldStat, change);
            } else
            {
                return oldStat.ToString();
            }
        }
    }
}
