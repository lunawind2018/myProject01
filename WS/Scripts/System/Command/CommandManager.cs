using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WS
{
    public class CommandManager
    {
        private static CommandManager mInstance;

        public static CommandManager Instance
        {
            get { return mInstance ?? (mInstance = new CommandManager()); }
        }

        public void DoCommand(CommandType command, Hashtable param)
        {
            Debug.Log("do command " + command);
        }
    }
}