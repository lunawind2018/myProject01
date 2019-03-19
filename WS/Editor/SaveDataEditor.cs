using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace WS
{
    public class SaveDataEditor : EditorWindow
    {

        [MenuItem("Tools/SaveDataEditor")]
        private static void SaveDataEditorWindow()
        {
            SaveDataEditor window = EditorWindow.GetWindow<SaveDataEditor>();
            window.Init();
            //window.maxSize = new Vector2(1200,1200);
            window.Show();
        }

        private string saveDataPath;

        private const int iLF = 20;
        private int iXPos;
        private int iYPos;

        private Color tmpButtonColor, tmpBackColor;

        private Hashtable dataHash;

        Rect GetRect(int w, int h)
        {
            return new Rect(iXPos, iYPos, w, h);
        }

        public void Init()
        {
            EditorWindow.GetWindowWithRect<SaveDataEditor>(new Rect(0, 0, 420, 1000), true, "SaveDataEditor");
            saveDataPath = Application.persistentDataPath + "/" + SaveDataManager.SaveFile;
            Load();
            hasChanged = false;
        }

        void OnGUI()
        {
            iXPos = iYPos = 10;
//            GUI.Label(new Rect(iXPos, iYPos, 800, 200), "Savedata Editor");
//            iYPos += iLF;
            // 背景

            tmpBackColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.cyan;
            GUI.Box(new Rect(iXPos, iYPos, 700, 50), "");
            GUI.backgroundColor = tmpBackColor;
            iYPos += 5;
            iXPos += 5;

            GUI.Label(GetRect(600, 20), "Data Path :");
            iYPos += iLF;

            GUI.Label(GetRect(600, 20), saveDataPath);
            //saveDataPath = EditorGUI.TextField(GetRect(200, 20), saveDataPath);

            if (GUI.Button(new Rect(iXPos + 600 + 10, iYPos, 80, 20), "Delete"))
            {
                if (File.Exists(saveDataPath))
                {
                    File.Delete(saveDataPath);
                    Debug.Log("delete save file");
                }
            }
            iYPos += 2 * iLF;
            if (hasChanged)
            {
                if (GUI.Button(new Rect(iXPos, iYPos, 80, 20), "save"))
                {
                    Save();
                }
                if (GUI.Button(new Rect(iXPos + 100, iYPos, 80, 20), "revert"))
                {
                    Load();
                }
                iYPos += 2 * iLF;
            }

            var space = 0;
            if (this.dataHash.Count > 0)
            {
                Draw(dataHash, space);
            }

            iYPos += 2 * iLF;

        }

        private void Draw(object obj, int space)
        {
            if (obj is Hashtable)
            {
                Draw(obj as Hashtable, space);
            }
            else if (obj is Array)
            {
                Draw(obj as Array, space);
            }
            else
            {
                int tab = GetTab(space);
                iXPos += tab;
                GUI.Label(GetRect(200, 20), GetPrefix(space)+obj);
                iXPos -= tab;
                iYPos += iLF + 5;
            }

        }

        private bool hasChanged;
        private Dictionary<string,bool> foldDic = new Dictionary<string, bool>();
        private void Draw(Hashtable data, int space)
        {
            int tab = GetTab(space);
            var pre = GetPrefix(space);
            iXPos += tab;
            var keys = new object[data.Keys.Count];
            data.Keys.CopyTo(keys, 0);
            foreach (var key in keys)
            {
                var v = data[key];
                if (v == null)
                {
                    GUI.Label(GetRect(200, 20), pre + key);
                    if (GUI.Button(new Rect(iXPos + 200 + 10, iYPos, 80, 20), "Delete"))
                    {
                        Debug.Log("delete " + key);
                        data.Remove(key);
                        hasChanged = true;
                    }
                }
                else
                {
                    var foldKey = space + "_" + key;
                    bool foldout = Utils.SafeGet(foldDic, foldKey, false);
                    foldout = EditorGUI.Foldout(GetRect(200, 20), foldout, pre + key);
                    Utils.SafeAdd(foldDic, foldKey, foldout);
                    //GUI.Label(GetRect(200, 20), key + "");
                    if (GUI.Button(new Rect(iXPos + 200 + 10, iYPos, 80, 20), "Delete"))
                    {
                        Debug.Log("delete " + key);
                        data.Remove(key);
                        hasChanged = true;
                    }
                    iYPos += iLF + 5;
                    if (foldout)
                    {
                        Draw(v, space + 1);
                    }
                }
            }
            iXPos -= tab;
        }

        private void Draw(Array data, int space)
        {
            int tab = GetTab(space);
            iXPos += tab;
            for (int i = 0; i < data.Length; i++)
            {
                Draw(data.GetValue(i), space);
            }
            iXPos -= tab;

        }

        private string GetPrefix(int space)
        {
            if (space == 0) return "";
            var sb = new StringBuilder();
            sb.Append("");
            return sb.ToString();
        }

        private int GetTab(int space)
        {
            return 10*space;
        }

        private void Save()
        {
            var data = MyMsgPacker.Pack(this.dataHash);
            File.WriteAllBytes(saveDataPath,data);
            Debug.Log("saved");
            hasChanged = false;
        }

        private void Load()
        {
            if (File.Exists(saveDataPath))
            {
                var data = File.ReadAllBytes(saveDataPath);
                this.dataHash = MyMsgPacker.Unpack(data) as Hashtable;
            }
            hasChanged = false;
            Debug.Log("loaded");
        }
    }
}