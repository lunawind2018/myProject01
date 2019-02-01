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

        private Dictionary<CommandType, Command_Base> commandDic;
        private CommandManager()
        {
            commandDic = new Dictionary<CommandType, Command_Base>()
            {
                {CommandType.Collect, new Command_Collect()},
            };
        }

        public IEnumerator DoCommand(CommandType command, Hashtable param, System.Action callback = null)
        {
            Debug.Log("do command " + command);
            yield return commandDic[command].DoCommand(param);
            if (callback != null) callback.Invoke();
        }
    }
}