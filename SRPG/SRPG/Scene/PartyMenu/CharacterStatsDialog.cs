using Nuclex.UserInterface.Controls.Desktop;
using SRPG.Data;

namespace SRPG.Scene.PartyMenu
{
    partial class CharacterStatsDialog : WindowControl
    {
        private Combatant _character;

        public CharacterStatsDialog()
        {
            InitializeComponent();
        }

        public void SetCharacter(Combatant character)
        {
            _character = character;

            RefreshComponents();
        }

        private void RefreshComponents()
        {
            _nameText.Text = _character.Name;
            _classText.Text = _character.Class;

            _healthText.Text = _character.MaxHealth.ToString();
            _manaText.Text = _character.MaxMana.ToString();

            _defText.Text = _character.Stats[Stat.Defense].ToString();
            _attText.Text = _character.Stats[Stat.Attack].ToString();
            _wisText.Text = _character.Stats[Stat.Wisdom].ToString();
            _intText.Text = _character.Stats[Stat.Intelligence].ToString();
            _spdText.Text = _character.Stats[Stat.Speed].ToString();
            _hitText.Text = _character.Stats[Stat.Hit].ToString();
        }
    }
}
