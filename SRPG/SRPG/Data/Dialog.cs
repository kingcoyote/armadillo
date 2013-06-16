using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace SRPG.Data
{
    public class Dialog
    {
        public Dictionary<int, DialogNode> Nodes;
        public DialogNode CurrentNode { get; private set; }
        public EventHandler OnEnter;
        public EventHandler OnExit;

        private int _currentOption = -1;

        public void SetOption(int optionNumber)
        {
            _currentOption = optionNumber;
        }

        public void Continue()
        {
            if (CurrentNode == null)
            {
                CurrentNode = Nodes[1];
            }
            else if(CurrentNode.Options.Count == 1)
            {
                CurrentNode = (from node in Nodes.Values where node.Identifier == CurrentNode.Options.First().Value select node).First();
            }
            else if (CurrentNode.Options.Count > 1)
            {
                CurrentNode = (from node in Nodes.Values where node.Identifier == CurrentNode.Options.ElementAt(_currentOption).Value select node).First();
            }
            else
            {
                CurrentNode = new DialogNode() { Identifier = -1 };
            }
        }

        public static Dialog Fetch(string filename, string objectname)
        {
            var dialog = new Dialog();
            dialog.Nodes = new Dictionary<int, DialogNode>();
            
            string settingString = String.Join("\r\n", File.ReadAllLines("Content/Dialog/" + filename + ".js"));

            var nodeList = Newtonsoft.Json.Linq.JObject.Parse(settingString);

            foreach(var node in nodeList[objectname]["nodes"])
            {
                var dialogNode = new DialogNode
                    {
                        Identifier = (int) (node["identifier"]),
                        Text = node["text"].ToString(),
                        Options = new Dictionary<string, int>()
                    };

                if (node.SelectToken("options") != null && node["options"].Any())
                {
                    foreach(var option in node["options"])
                    {
                        dialogNode.Options.Add(option["text"].ToString(), (int)option["node"]);
                    }
                }

                dialog.Nodes.Add(dialogNode.Identifier, dialogNode);
            }

            dialog.Continue();

            return dialog;

        }
    }
}
