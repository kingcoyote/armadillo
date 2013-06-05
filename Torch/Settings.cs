using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Input;

namespace Torch
{

    public class AppSettings
    {
        public List<HighScore> HighScores;
        public string[] ControlStrings;
        public Keys[] ControlKeys;

        public struct HighScore
        {
            public string Name;
            public int Cleared;
            public int Score;
        }

        public AppSettings()
        {
            HighScores = new List<HighScore>();
            
            ControlStrings = new [] {
                 "left",
                 "right", 
                 "down",
                 "ccw", 
                 "cw",
                 "swap", 
                 "pause", 
                 "drop",
            };

            ControlKeys = new [] {
                Keys.A,
                Keys.D,
                Keys.S,
                Keys.Q,
                Keys.E,
                Keys.W,
                Keys.Escape,
                Keys.Space 
            };
        }
    }

    static class SettingsManager
    {
        private const string FileName = "settings.xml";
        public static AppSettings Settings = new AppSettings();

        public static void LoadSettings()
        {
            // Create our exposed settings class. This class gets serialized to load/save the settings.
            Settings = new AppSettings();

            //Obtain a virtual store for application
            var fileStorage = IsolatedStorageFile.GetUserStoreForDomain();

            // Check if file is there
            if (fileStorage.FileExists(FileName))
            {
                var serializer = new XmlSerializer(Settings.GetType());
                var stream = new StreamReader(new IsolatedStorageFileStream(FileName, FileMode.Open, fileStorage));

                try
                {
                    Settings = (AppSettings)serializer.Deserialize(stream);
                    stream.Close();
                }
                catch
                {
                    // An error occurred so let's use the default settings.
                    stream.Close();
                    Settings = new AppSettings();
                    // Saving is optional - in this sample we assume it works and the error is due to the file not being there.
                    SaveSettings();
                    // Handle other errors here
                }
            }
            else
            {
                SaveSettings();
            }
        }


        public static void SaveSettings()
        {
            //Obtain a virtual store for application
            var fileStorage = IsolatedStorageFile.GetUserStoreForDomain();
            var serializer = new XmlSerializer(Settings.GetType());
            var stream = new StreamWriter(new IsolatedStorageFileStream(FileName, FileMode.Create, fileStorage));

            try
            {
                serializer.Serialize(stream, Settings);
            }
            catch
            {
                // Handle your errors here
            }
            stream.Close();
        }
    }
}