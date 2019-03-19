using System;
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
            commandDic = new Dictionary<CommandType, Command_Base>();
//            commandDic = new Dictionary<CommandType, Command_Base>()
//            {
//                {CommandType.Collect, new Command_Collect()},
//                {CommandType.Craft, new Command_Craft()},
//            };
            Debug.Log(new Command_Collect().GetType().ToString());
            var names = System.Enum.GetValues(typeof (CommandType));
            foreach (CommandType t in names)
            {
                var className = "WS.Command_" + t;
                Debug.Log(className);
                System.Type tt = System.Type.GetType(className);
                var obj = System.Activator.CreateInstance(tt)as Command_Base;
                commandDic.Add(t, obj);
            }
        }


        public bool CommandSuccess;
        public IEnumerator DoCommand(CommandType command, object param, System.Action<bool> callback = null)
        {
            Debug.Log("do command " + command);
            CommandSuccess = true;
            yield return commandDic[command].DoCommand(param);
            if (callback != null) callback.Invoke(CommandSuccess);
        }
    }
}