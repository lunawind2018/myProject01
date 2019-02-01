using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
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

        // テキストフィールド
        exportPath = EditorGUI.TextField(GetRect(200, 20), exportPath);
        // 参照ボタン
        if (GUI.Button(new Rect(iXPos + 200 + 10, iYPos, 80, 20), "Browse..."))
        {
            // フォルダ選択ダイアログ（パスを取得する）
            string strTempFile = EditorUtility.SaveFolderPanel("Excel Path", exportPath, "");

            // パス情報が入力されていたら反映（キャンセルの場合は空文字で戻ってくる）
            if (strTempFile.Length != 0)
            {
                exportPath = strTempFile;
                PlayerPrefs.SetString("ExportPath", exportPath);
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
            //string strTempFile = EditorUtility.SaveFolderPanel("Excel Path", exportPath, "");
            var files = Directory.GetFiles(exportPath);
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].EndsWith(".xlsx")) ConvertToCsv(files[i]);
            }
        }
    }

    private void ConvertToCsv(string selectPath)
    {
        FileStream stream = File.Open(selectPath, FileMode.Open, FileAccess.Read);
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
                if (int.TryParse(nvalue, out v))
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
            var p = Path.GetDirectoryName(selectPath);
            var n = Path.GetFileNameWithoutExtension(selectPath);
            var filename = exportPath + "/" + n + ".csv";
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
            File.WriteAllLines(filename, lines.ToArray(), Encoding.UTF8);
            Debug.Log("output: " + filename);
        }

    }
}
