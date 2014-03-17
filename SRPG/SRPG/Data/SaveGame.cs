using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SRPG.Data
{
    public class SaveGame
    {
        public string Name;
        public long GameTime;

        public string Zone;
        public string Door;

        public List<Combatant> Party = new List<Combatant>();
        public List<Item> Inventory = new List<Item>();
        public int Money;
        public Dictionary<string, int> BattlesCompleted = new Dictionary<string, int>();
        public long TutorialsCompleted;
        public Dictionary<string, byte[]> RoomDetails = new Dictionary<string, byte[]>();

        private static readonly byte[] Guid = new byte[] { 0x04, 0x18, 0xB4, 0xD8, 0xD3, 0xE3, 0x42, 0x5F, 0x9B, 0xB8, 0x13, 0xF7, 0xFD, 0x05, 0xF7, 0x5D };

        /// <summary>
        /// Convert the save data into a file.
        /// </summary>
        public void Save(int fileNumber)
        {
            var filename = string.Format(
                "{0}\\Armadillo\\Save\\save{1}.asg",
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                fileNumber
                );

            if (File.Exists(filename)) File.Delete(filename);

            var binaryWriter = new BinaryWriter(File.OpenWrite(filename));

            // GUID for the game - 0418B4D8-D3E3-425F-9BB8-13F7FD05F75D
            binaryWriter.Write(Guid);
            // ID for a save file (16-bit int)
            binaryWriter.Write((short)1);
            // save file version (16-bit int)
            binaryWriter.Write((short)1);
            // save date (64-bit timestamp)
            binaryWriter.Write(DateTime.Now.Ticks);
            // save file name
            binaryWriter.Write(Name);
            // elapsed game time
            binaryWriter.Write(GameTime);
            // zone/door names (length-prefixed strings)
            binaryWriter.Write(Zone);
            binaryWriter.Write(Door);

            // how many party members (16-bit int)
            binaryWriter.Write((short)Party.Count);
            // foreach party member
            foreach (var character in Party)
            {
                // name (length-prefixed string)
                binaryWriter.Write(character.Name);
                
                // stat xp levels - dawish + hp/mp (32-bit int)
                binaryWriter.Write(character.ReadStat(Stat.Health));
                binaryWriter.Write(character.ReadStat(Stat.Mana));
                binaryWriter.Write(character.ReadStat(Stat.Defense));
                binaryWriter.Write(character.ReadStat(Stat.Attack));
                binaryWriter.Write(character.ReadStat(Stat.Wisdom));
                binaryWriter.Write(character.ReadStat(Stat.Intelligence));
                binaryWriter.Write(character.ReadStat(Stat.Speed));
                binaryWriter.Write(character.ReadStat(Stat.Hit));

                // number of ability xp levels (16-bit int)
                binaryWriter.Write((short)character.AbilityExperienceLevels.Count);
                // foreach ability xp level
                foreach (var ability in character.AbilityExperienceLevels.Keys)
                {
                    // ability name (length-prefixed string)
                    binaryWriter.Write(ability.Name);
                    
                    // xp level (32-bit int)
                    binaryWriter.Write(character.AbilityExperienceLevels[ability]);
                }

                binaryWriter.Write(character.GetEquippedWeapon().Key);
                binaryWriter.Write(character.GetEquippedArmor().Key);
                // binaryWriter.Write(character.GetEquippedAccessory().Key);
            }
            
            
            // money (32-bit int)
            binaryWriter.Write(Money);
            
            
            // number of items in inventory (16-bit int)
            binaryWriter.Write((short)Inventory.Count);
            // foreach item in inventory
            foreach (var item in Inventory)
            {
                // name (length-prefixed string)
                binaryWriter.Write(item.Key);
                // quantity (16-bit int)
                binaryWriter.Write((short)1);
            }
            
            // number of stored rooms (32-bit int)
            binaryWriter.Write(RoomDetails.Count);
            // foreach room
            foreach (var room in RoomDetails.Keys)
            {
                //   name of room (length-prefixed string)
                binaryWriter.Write(room);
                //   room details (length-prefixed byte array)
                binaryWriter.Write(RoomDetails[room].Length);
                binaryWriter.Write(RoomDetails[room]);
            }


            // number of battles completed (16-bit int)
            binaryWriter.Write(BattlesCompleted.Count);
            // foreach battle
            foreach (var battle in BattlesCompleted)
            {
                //   battle name (length-prefixed string)
                binaryWriter.Write(battle.Key);
                //   score (32-bit int)
                binaryWriter.Write(battle.Value);

            }
            // 64 bit tutorial details
            binaryWriter.Write(TutorialsCompleted);

            binaryWriter.Close();
        }

        /// <summary>
        /// Load a specified file number.
        /// </summary>
        /// <param name="game">the game being loaded</param>
        /// <param name="fileNumber">The file number to be loaded.</param>
        public static SaveGame Load(SRPGGame game, int fileNumber)
        {
            var save = new SaveGame();

            var filename = string.Format(
                "{0}\\Armadillo\\Save\\save{1}.asg",
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                fileNumber
                );
            
            if (File.Exists(filename) == false)
            {
                throw new Exception("file does not exist, can't load");
            }

            var r = new BinaryReader(File.OpenRead(filename));

            var guid = r.ReadBytes(16);
            var filetype = r.ReadInt16();
            var version = r.ReadInt16();

            // confirm the file is correct
            if (!guid.SequenceEqual(Guid) || filetype != 1 || version != 1) 
                throw new Exception("file is not in correct format");

            // skip save date
            r.ReadInt64();
            save.Name = r.ReadString();
            save.GameTime = r.ReadInt64();
            save.Zone = r.ReadString();
            save.Door = r.ReadString();

            // how many party members (16-bit int)
            var partyCount = r.ReadInt16();
            for (var i = 0; i < partyCount; i++ )
            {
                // name (length-prefixed string)
                var character = Combatant.FromTemplate(game, "party/" + r.ReadString().ToLower());

                // stat xp levels - dawish + hp/mp (32-bit int)
                character.Stats[Stat.Health] = r.ReadInt32();
                character.Stats[Stat.Mana] = r.ReadInt32();
                character.Stats[Stat.Defense] = r.ReadInt32();
                character.Stats[Stat.Attack] = r.ReadInt32();
                character.Stats[Stat.Wisdom] = r.ReadInt32();
                character.Stats[Stat.Intelligence] = r.ReadInt32();
                character.Stats[Stat.Speed] = r.ReadInt32();
                character.Stats[Stat.Hit] = r.ReadInt32();

                character.AbilityExperienceLevels.Clear();

                var abilityCount = r.ReadInt16();
                for (var j = 0; j < abilityCount; j++)
                {
                    var ability = Ability.Factory(game, r.ReadString());
                    character.AbilityExperienceLevels.Add(ability, r.ReadInt32());
                }

                character.EquipItem(Item.Factory(game, r.ReadString()));
                character.EquipItem(Item.Factory(game, r.ReadString()));
                //character.EquipItem(Item.Factory(game, r.ReadString()));

                save.Party.Add(character);
            }

            save.Money = r.ReadInt32();


            var inventoryCount = r.ReadInt16();
            for (var i = 0; i < inventoryCount; i++)
            {
                var item = Item.Factory(game, r.ReadString());
                // todo process quantity properly
                r.ReadInt16();
                save.Inventory.Add(item);
            }
            
            var roomCount = r.ReadInt32();
            for (var i = 0; i < roomCount; i++)
            {
                var roomName = r.ReadString();
                var roomDataSize = r.ReadInt32();
                var roomData = r.ReadBytes(roomDataSize);
                save.RoomDetails.Add(roomName, roomData);
            }

            /*
            // number of battles completed (16-bit int)
            binaryWriter.Write(BattlesCompleted.Count);
            // foreach battle
            foreach (var battle in BattlesCompleted)
            {
                //   battle name (length-prefixed string)
                binaryWriter.Write(battle.Key);
                //   score (32-bit int)
                binaryWriter.Write(battle.Value);

            }
            // 64 bit tutorial details
            binaryWriter.Write(TutorialsCompleted);
            */
            r.Close();

            return save;
        }

        public static List<SaveGame> FetchAll(SRPGGame game)
        {
            var files = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Armadillo\\Save").GetFiles("*.asg", SearchOption.TopDirectoryOnly);
            var savegames = new List<SaveGame>();
            foreach (var f in files)
            {
                int number;
                if(int.TryParse(f.Name.Replace("save", "").Replace(".asg", ""), out number))
                {
                    savegames.Add(Load(game, number));
                }
            }
            return savegames;
        }
    }
}
