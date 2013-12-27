using SRPG.Data;
using System.Collections.Generic;

namespace SRPG.Scene.Battle
{
    public partial class QueuedCommandsDialog
    {
        public readonly List<Command> Commands;

        public QueuedCommandsDialog(List<Command> commands)
        {
            Commands = commands;
            InitializeComponent();
        }
    }
}
