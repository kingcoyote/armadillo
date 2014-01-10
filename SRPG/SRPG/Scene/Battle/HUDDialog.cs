using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SRPG.Scene.Battle
{
    public partial class HUDDialog
    {
        public Action EndTurnPressed;

        public HUDDialog()
        {
            InitializeComponent();

            _endTurn.Enabled = false;
            _endTurn.Pressed += (s, a) => EndTurnPressed.Invoke();
        }

        public void SetStatus(int round, int faction)
        {
            _roundNumber.Text = round.ToString();
            _faction.Text = faction == 0 ? "Player Turn" : "Enemy Turn";
        }
    }
}
