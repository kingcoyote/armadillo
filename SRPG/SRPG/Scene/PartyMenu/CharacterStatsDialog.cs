using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;
using SRPG.Data;

namespace SRPG.Scene.PartyMenu
{
    public partial class CharacterStatsDialog
    {
        public CharacterStatsDialog()
        {
            InitializeComponent();
        }

        public void UpdateCharacter(Combatant character)
        {
            _defLevel.Text = (character.Stats[Stat.Defense] / 100).ToString();
            _attLevel.Text = (character.Stats[Stat.Attack] / 100).ToString();
            _wisLevel.Text = (character.Stats[Stat.Wisdom] / 100).ToString();
            _intLevel.Text = (character.Stats[Stat.Intelligence] / 100).ToString();
            _spdLevel.Text = (character.Stats[Stat.Speed] / 100).ToString();
            _hitLevel.Text = (character.Stats[Stat.Hit] / 100).ToString();

            _defProg.Progress = (character.Stats[Stat.Defense] % 100) / 100f;
            _attProg.Progress = (character.Stats[Stat.Attack] % 100) / 100f;
            _wisProg.Progress = (character.Stats[Stat.Wisdom] % 100) / 100f;
            _intProg.Progress = (character.Stats[Stat.Intelligence] % 100) / 100f;
            _spdProg.Progress = (character.Stats[Stat.Speed] % 100) / 100f;
            _hitProg.Progress = (character.Stats[Stat.Hit] % 100) / 100f;

            _defExp.Text = (character.Stats[Stat.Defense] % 100).ToString();
            _attExp.Text = (character.Stats[Stat.Attack] % 100).ToString();
            _wisExp.Text = (character.Stats[Stat.Wisdom] % 100).ToString();
            _intExp.Text = (character.Stats[Stat.Intelligence] % 100).ToString();
            _spdExp.Text = (character.Stats[Stat.Speed] % 100).ToString();
            _hitExp.Text = (character.Stats[Stat.Hit] % 100).ToString();
        }
    }
}
