using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRPG.Data
{
    public class SaveGame
    {
        public List<Combatant> Party;
        public List<Item> Inventory;
        public int Money;
        public List<Byte> BattlesCompleted;
        public List<Byte> TutorialsCompleted;
        
        /// <summary>
        /// Convert the save data into a file.
        /// </summary>
        public void Save(byte fileNumber)
        {
            
        }

        /// <summary>
        /// Load a specified file number.
        /// </summary>
        /// <param name="fileNumber">The file number to be loaded.</param>
        public void Load(byte fileNumber)
        {
            
        }
    }
}
