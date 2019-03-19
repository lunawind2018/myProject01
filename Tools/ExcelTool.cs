using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;
using UnityEngine;
using Excel;
public class ExcelTool : EditorWindow
{

    public void Init()
    {
        EditorWindow.GetWindowWithRect<ExcelTool>(new Rect(0, 0, 420, 920), true, "ExcelTool");
        exportPath = PlayerPrefs.GetString("ExportPath");
    }

    [MenuItem("Tools/ExcelTool")]
    private static void ExcelToolWindow()
    {
        ExcelTool window = EditorWindow.GetWindow<ExcelTool>();
        window.Init();
        window.Show();
    }

    private const int iLF = 20;
    private int iXPos;
    private int iYPos;

    private string exportPath;

    private Color tmpButtonColor, tmpBackColor;

    private bool hasReadMd5 = false;

    Rect GetRect(int w, int h)
    {
        return new Rect(iXPos, iYPos, w, h);
    }

    void OnGUI()
    {
        iXPos = iYPos = 10;
        GUI.Label(new Rect(iXPos, iYPos, 400, 200), "Excel Tool");
        iYPos += iLF;
        // 背景

        Rect settingBg = new Rect(iXPos, iYPos, 300, 200 + 30 + 30);
        tmpBackColor = GUI.backgroundColor;
        GUI.backgroundColor = Color.cyan;
        GUI.Box(settingBg, "");
        GUI.backgroundColor = tmpBackColor;
        iYPos += 5;
        iXPos += 5;

        GUI.Label(GetRect(200, 20), "Export Path :");
        iYPos += iLF;

        exportPath = EditorGUI.TextField(GetRect(200, 20), exportPath);
        if (GUI.Button(new Rect(iXPos + 200 + 10, iYPos, 80, 20), "Browse..."))
        {
            string strTempFile = EditorUtility.SaveFolderPanel("Excel Path", exportPath, "");

            if (strTempFile.Length != 0)
            {
                exportPath = strTempFile;
                PlayerPrefs.SetString("ExportPath", exportPath);
                ReadMd5File();
            }
        }
        iYPos += iLF;
        iYPos += iLF;

        if (GUI.Button(GetRect(250, 20), "Select Excel"))
        {
            string selectPath = EditorUtility.OpenFilePanel("storydata", "", "xlsx");
            if (string.IsNullOrEmpty(selectPath))
            {
                Debug.Log("empty");
                return;
            }
            ConvertToCsv(selectPath);
        }
        iYPos += iLF;
        iYPos += iLF;
        if (GUI.Button(GetRect(250, 20), "Convert All"))
        {
            if (!hasReadMd5)
            {
                ReadMd5File();
            }
            //string strTempFile = EditorUtility.SaveFolderPanel("Excel Path", exportPath, "");
            var files = Directory.GetFiles(exportPath);
            var count = 0;
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].EndsWith(".xlsx"))
                {
                    var b = ConvertToCsv(files[i]);
                    if (b) count++;
                }
            }
            Debug.Log("export " + count + " csv");
            if (count > 0) WriteMd5File();
        }
    }

    private Dictionary<string,string> Md5Dic =new Dictionary<string, string>();

    private void ReadMd5File()
    {
        var md5file = exportPath + "/md5.txt";
        Debug.Log("read " + md5file);
        if (File.Exists(md5file))
        {
            var lines = File.ReadAllLines(md5file);
            for (int i = 0; i < lines.Length; i++)
            {
                var arr = lines[i].Split(',');
                var k = arr[0];
                var v = arr[1];
                if (Md5Dic.ContainsKey(k))
                {
                    Md5Dic[k] = v;
                }
                else
                {
                    Md5Dic.Add(k, v);
                }
            }
            Debug.Log("read md5 complete " + lines.Length);
        }
        hasReadMd5 = true;
    }

    private void WriteMd5File()
    {
        var md5file = exportPath + "/md5.txt";
        var lines = new List<string>();
        foreach (KeyValuePair<string, string> keyValuePair in Md5Dic)
        {
            var k = keyValuePair.Key;
            var v = keyValuePair.Value;
            lines.Add(k + "," + v);
        }
        File.WriteAllLines(md5file, lines.ToArray(), Encoding.UTF8);
        Debug.Log("write md5 " + md5file + "  " + lines.Count + "lines");
    }

    private bool ConvertToCsv(string selectPath)
    {
        FileStream stream = File.Open(selectPath, FileMode.Open, FileAccess.Read);

        var md5provider = new MD5CryptoServiceProvider();
        md5provider.ComputeHash(stream);
        byte[] b = md5provider.Hash;
        StringBuilder sb = new StringBuilder(32);
        for (int i = 0; i < b.Length; i++)
        {
            sb.Append(b[i].ToString("X2"));
        }
        var md5str = sb.ToString();
//        Debug.Log("MD5 : " + selectPath + " |" + md5str);
        if (Md5Dic.ContainsKey(selectPath))
        {
            if (Md5Dic[selectPath] == md5str)
            {
//                Debug.Log("MD5 not change");
                return false;
            }
            else
            {
                Md5Dic[selectPath] = md5str;
            }
        }
        else
        {
            Md5Dic.Add(selectPath, md5str);
        }

        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

        DataSet result = excelReader.AsDataSet();

        int columns = result.Tables[0].Columns.Count;
        int rows = result.Tables[0].Rows.Count;

        var lines = new List<string>();
        for (int i = 0; i < rows; i++)
        {
            var line = "";
            for (int j = 0; j < columns; j++)
            {
                string nvalue = result.Tables[0].Rows[i][j].ToString();
                //Debug.Log(nvalue);
                int v;
                float f;
                if (nvalue.Contains(","))
                {
                    line += "\"" + nvalue + "\"";
                }
                else if (int.TryParse(nvalue, out v))
                {
                    line += v;
                }
                else if (float.TryParse(nvalue, out f))
                {
                    line += f;
                }
                else
                {
                    line += "\"" + nvalue + "\"";
                }
                if (j < columns - 1) line += ",";
            }
            lines.Add(line);
            //Debug.Log(i + " " + line);
        }
        if (lines.Count > 0)
        {
            //var p = Path.GetDirectoryName(selectPath);
            var n = Path.GetFileNameWithoutExtension(selectPath);
            var filename = exportPath + "/" + n + ".csv";
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
            File.WriteAllLines(filename, lines.ToArray(), Encoding.UTF8);
            Debug.Log("output: " + filename);
        }
        return true;
    }
}

