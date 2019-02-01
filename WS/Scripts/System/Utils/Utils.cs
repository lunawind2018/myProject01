using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace WS
{
    public partial class Utils
    {

        public static ResourceType GetResourceType(string str)
        {
            switch (str)
            {
                case "wood":
                    return ResourceType.Wood;
                case "stone":
                    return ResourceType.Stone;
            }
            return ResourceType.None;
        }

        public static K SafeGet<T, K>(Dictionary<T, K> dic, T key, K def)
        {
            return dic.ContainsKey(key) ? dic[key] : def;
        }

        enum CSVParserState
        {
            Begin,
            Plane,
            Quote,
            End,
        }

        public static ArrayList ConvertCSV(string csvText, bool removeTitle = true)
        {
            int len = csvText.Length;
            int begin = 0;
            while (Convert.ToInt32(csvText[begin]) == 0xFEFF && begin < len)
            {
                begin += 1;
            }
            ArrayList rows = new ArrayList();
            ArrayList cols = new ArrayList();

            int startIdx = begin;
            int lastIdx = begin;
            bool skipRow = removeTitle;
            bool needsQuoteEscape = false;

            CSVParserState state = CSVParserState.Begin;

            for (int i = begin; i < len; ++i)
            {
                char c = csvText[i];
                switch (c)
                {
                    case ',':
                        switch (state)
                        {
                            case CSVParserState.Begin:
                                cols.Add("");
                                break;
                            case CSVParserState.Plane:
                            case CSVParserState.End:
                                string item = csvText.Substring(startIdx, lastIdx + 1 - startIdx);
                                cols.Add(needsQuoteEscape ? item.Replace("\"\"", "\"") : item);
                                needsQuoteEscape = false;
                                state = CSVParserState.Begin;
                                break;
                            case CSVParserState.Quote:
                                lastIdx = i;
                                break;
                        }
                        break;
                    case ' ':
                    case '\t':
                        switch (state)
                        {
                            case CSVParserState.Plane:
                                break;
                            case CSVParserState.Quote:
                                lastIdx = i;
                                break;
                            case CSVParserState.Begin:
                            case CSVParserState.End:
                                break;
                        }
                        break;
                    case '\r':
                        if (i < len - 1 && csvText[i + 1] == '\n')
                        {
                            i += 1;
                        }
                        goto case '\n';
                    // Fallthrough
                    case '\n':
                        switch (state)
                        {
                            case CSVParserState.Plane:
                            case CSVParserState.End:
                                string item = csvText.Substring(startIdx, lastIdx + 1 - startIdx);
                                cols.Add(needsQuoteEscape ? item.Replace("\"\"", "\"") : item);
                                needsQuoteEscape = false;
                                if (!skipRow)
                                {
                                    cols.TrimToSize();
                                    rows.Add(cols);
                                }
                                else
                                {
                                    skipRow = false;
                                }
                                cols = new ArrayList(cols.Count);
                                state = CSVParserState.Begin;
                                break;
                            case CSVParserState.Begin:
                                if (cols.Count > 0)
                                {
                                    cols.Add("");
                                    if (!skipRow)
                                    {
                                        cols.TrimToSize();
                                        rows.Add(cols);
                                    }
                                    else
                                    {
                                        skipRow = false;
                                    }
                                    cols = new ArrayList(cols.Count);
                                }
                                break;
                            case CSVParserState.Quote:
                                // NOTE:
                                //  ダブルクォート中に改行が入っていたらCSVとして壊れているはずだが、
                                //  互換性を持たせるために従来のアルゴリズムと同じく許容する。 - ykst
                                lastIdx = i;
                                break;
                        }
                        break;
                    case '"':
                        switch (state)
                        {
                            case CSVParserState.Begin:
                                startIdx = i + 1;
                                lastIdx = i;
                                state = CSVParserState.Quote;
                                break;
                            case CSVParserState.End:
                            case CSVParserState.Plane:
                                throw new System.ApplicationException("error CSV");
                            case CSVParserState.Quote:
                                if (i < len - 1)
                                {
                                    if (csvText[i + 1] == '"')
                                    {
                                        i += 1;
                                        needsQuoteEscape = true;
                                        lastIdx = i;
                                    }
                                    else
                                    {
                                        state = CSVParserState.End;
                                    }
                                }
                                else
                                {
                                    state = CSVParserState.End;
                                }
                                break;
                        }
                        break;
                    default:
                        switch (state)
                        {
                            case CSVParserState.Begin:
                                startIdx = i;
                                lastIdx = i;
                                state = CSVParserState.Plane;
                                break;
                            case CSVParserState.End:
                                Debug.LogError("Unanalyzable CSV, using legacy one");
                                return LegacyConvertCSV(csvText, removeTitle);

                            case CSVParserState.Plane:
                            case CSVParserState.Quote:
                                lastIdx = i;
                                break;
                        }
                        break;
                }
            }

            switch (state)
            {
                case CSVParserState.Begin:
                    if (cols.Count > 0 && !skipRow)
                    {
                        cols.Add("");
                        cols.TrimToSize();
                        rows.Add(cols);
                    }
                    break;
                case CSVParserState.End:
                case CSVParserState.Plane:
                    if (!skipRow)
                    {
                        cols.Add(csvText.Substring(startIdx, lastIdx + 1 - startIdx));
                        cols.TrimToSize();
                        rows.Add(cols);
                    }
                    break;
                case CSVParserState.Quote:
                    throw new System.ApplicationException("error CSV");
            }
            return rows;
        }

        //----------------------------------------------------------------------------
        // CSV関連
        //----------------------------------------------------------------------------
        /*!
         * @brief CSVをArrayListに変換
         * CSV形式に準拠して変換する
         * "答えは""1""です"⇒答えは"1"です
         * "\1,000"⇒\1,000
         * 
         * @param[in] csvText CSVテキスト
         * @return 置換後の行単位のリスト
         */
        public static ArrayList LegacyConvertCSV(string csvText, bool removeTitle = true)
        {
            // 正規表現:一行取り出す
            const string REGEX_ROW = "^.*(?:\\n|$)";
            // 正規表現:1行のCSVから各フィールドを取得する
            const string REGEX_FIELD = "\\s*(\"(?:[^\"]|\"\")*\"|[^,]*)\\s*,";

            ArrayList csvRecords = new ArrayList();

            //前後の改行を削除しておく
            csvText = csvText.Trim(new char[] { '\r', '\n' });

            //一行取り出す
            Regex regLine = new Regex(REGEX_ROW, RegexOptions.Multiline);

            //1行のCSVから各フィールドを取得する
            Regex regCsv = new Regex(REGEX_FIELD, RegexOptions.None);

            Match mLine = regLine.Match(csvText);
            while (mLine.Success)
            {
                //一行取り出す
                string line = mLine.Value;
                //改行記号が"で囲まれているか調べる
                while ((CountString(line, "\"") % 2) == 1)
                {
                    mLine = mLine.NextMatch();
                    if (!mLine.Success)
                    {
                        throw new System.ApplicationException("不正なCSV");
                    }
                    line += mLine.Value;
                }
                //行の最後の改行記号を削除
                line = line.TrimEnd(new char[] { '\r', '\n' });
                //最後に「,」をつける
                line += ",";

                //1つの行からフィールドを取り出す
                ArrayList csvFields = new ArrayList();
                Match m = regCsv.Match(line);
                while (m.Success)
                {
                    string field = m.Groups[1].Value;
                    //前後の空白を削除
                    field = field.Trim();
                    //"で囲まれている時
                    if (field.StartsWith("\"") && field.EndsWith("\""))
                    {
                        //前後の"を取る
                        field = field.Substring(1, field.Length - 2);
                        //「""」を「"」にする
                        field = field.Replace("\"\"", "\"");
                    }
                    csvFields.Add(field);
                    m = m.NextMatch();
                }

                csvFields.TrimToSize();
                csvRecords.Add(csvFields);

                mLine = mLine.NextMatch();
            }

            // 1行目のカラム行を削除する.
            if (removeTitle)
            {
                csvRecords.RemoveAt(0);
            }

            csvRecords.TrimToSize();
            return csvRecords;
        }

        public static int CountString(string strInput, string strFind)
        {
            int foundCount = 0;
            int sPos = strInput.IndexOf(strFind);
            while (sPos > -1)
            {
                foundCount++;
                sPos = strInput.IndexOf(strFind, sPos + 1);
            }

            return foundCount;
        }

        private static Dictionary<string,Color> colorDic = new Dictionary<string, Color>();
        public static Color GetColor(string colorstr)
        {
            if (colorDic.ContainsKey(colorstr)) return colorDic[colorstr];
            Color color;
            ColorUtility.TryParseHtmlString("#" + colorstr + "FF", out color);
            colorDic.Add(colorstr, color);
            return color;
        }
        
    }
}
