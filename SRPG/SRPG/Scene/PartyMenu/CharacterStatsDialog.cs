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
            _defText.Text = character.Stats[Stat.Defense].ToString();
            _attText.Text = character.Stats[Stat.Attack].ToString();
            _wisText.Text = character.Stats[Stat.Wisdom].ToString();
            _intText.Text = character.Stats[Stat.Intelligence].ToString();
            _spdText.Text = character.Stats[Stat.Speed].ToString();
            _hitText.Text = character.Stats[Stat.Hit].ToString();

            _defProg.Progress = (character.StatExperienceLevels[Stat.Defense] % 100) / 100f;
            _attProg.Progress = (character.StatExperienceLevels[Stat.Attack] % 100) / 100f;
            _wisProg.Progress = (character.StatExperienceLevels[Stat.Wisdom] % 100) / 100f;
            _intProg.Progress = (character.StatExperienceLevels[Stat.Intelligence] % 100) / 100f;
            _spdProg.Progress = (character.StatExperienceLevels[Stat.Speed] % 100) / 100f;
            _hitProg.Progress = (character.StatExperienceLevels[Stat.Hit] % 100) / 100f;

            _defLevel.Text = (character.StatExperienceLevels[Stat.Defense] % 100).ToString();
            _attLevel.Text = (character.StatExperienceLevels[Stat.Attack] % 100).ToString();
            _wisLevel.Text = (character.StatExperienceLevels[Stat.Wisdom] % 100).ToString();
            _intLevel.Text = (character.StatExperienceLevels[Stat.Intelligence] % 100).ToString();
            _spdLevel.Text = (character.StatExperienceLevels[Stat.Speed] % 100).ToString();
            _hitLevel.Text = (character.StatExperienceLevels[Stat.Hit] % 100).ToString();
        }
    }
}
