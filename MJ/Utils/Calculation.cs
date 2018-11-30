using System;
using System.Collections;
using System.Collections.Generic;
using MJ;
using UnityEngine;

public class Calculation
{
    private const int MAX_C = 42;
    private const int NUM_C = 34;
    private static int[] YaoJiu = new[] {0, 8, 10, 18, 20, 28, 30, 32, 34, 36, 38, 40, 42};
    private static int[] CIndex = new[] {0,1,2,3,4,5,6,7,8,10,11,12,13,14,15,16,17,18,20,21,22,23,24,25,26,27,28,30,32,34,36,38,40,42};
    // 0-8   M1-M9
    // 10-18 S1-S9
    // 20-28 P1-P9
    // 30 32 34 36
    // 38 40 42
    public static int CheckRong(int[] c, out List<RongData> rdList )
    {
        rdList = new List<RongData>();
        //guoshi
        if (CheckGuoShi(c)) return 13;
        //qitui
        if (CheckQiDui(c)) return 7;
        //
        var start = 0;
        for (int i = 0; i < MAX_C; i++)
        {
            if (c[i] == 0)
            {
                start++;
                continue;
            }
            if (c[i] >= 2)
            {
                var cc = DeepCopy(c);
                cc[i] -= 2;
                var rongData = new RongData();
                rongData.atama = i;
                if (CheckMenZi(cc, start, 0, ref rongData) > 0)
                {
                    rdList.Add(rongData);
                }
            }
        }
        return rdList.Count > 0 ? 1 : 0;
    }

    private static bool CheckGuoShi(int[] c)
    {
        //return (c[0]*c[8]*c[10]*c[18]*c[20]*c[28]*c[30]*c[32]*c[34]*c[36]*c[38]*c[40]*c[42] == 2);
        var t = 1;
        for (int i = 0; i < 13; i++)
        {
            if (c[YaoJiu[i]] == 0)
            {
                return false;
            }
            t = t << (c[YaoJiu[i]] - 1);
            if (t > 2) return false;
        }
        return t == 2;
    }

    private static bool CheckQiDui(int[] c)
    {
        var t = 0;
        for (int i = 0; i < MAX_C; i++)
        {
            if (c[i] == 1) break;
            t += Convert.ToInt32(c[i] == 2);
        }
        return t == 7;
    }

    public static bool CheckRong(int[] c)
    {
        //guoshi
        if (CheckGuoShi(c)) return true;
        //qitui
        if (CheckQiDui(c)) return true;
        //
        var start = 0;
        for (int i = 0; i < MAX_C; i++)
        {
            if (c[i] == 0)
            {
                start++;
                continue;
            }
            if (c[i] >= 2)
            {
                var cc = DeepCopy(c);
                cc[i] -= 2;
                if (CheckMenZi(cc, start, 0))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private static int CheckMenZi(int[] c, int start, int n, ref RongData rd)
    {
        var s = start;
        for (int i = start; i < MAX_C; i++)
        {
            if (c[i] == 0)
            {
                s++;
                continue;
            }
            if (c[i] >= 3)
            {
                c[i] -= 3;
                rd.mens[n] = new[] {i, i, i};
                n++;
                return CheckMenZi(c, s, n, ref rd);
            }
            if (i <= 26 && c[i + 1] > 0 && c[i + 2] > 0)
            {
                c[i]--;
                c[i + 1]--;
                c[i + 2]--;
                rd.mens[n] = new[] {i, i + 1, i + 2};
                n++;
                return CheckMenZi(c, s, n, ref rd);
            }
            return 0;
        }
        return 1;
    }

    private static bool CheckMenZi(int[] c, int start, int n)
    {
        var s = start;
        for (int  i= start; i < NUM_C; i++)
        {
            var index = CIndex[i];
            if (c[index] == 0)
            {
                s++;
                continue;
            }
            if (c[index] >= 3)
            {
                c[index] -= 3;
                n++;
                return CheckMenZi(c, s, n);
            }
            if (index <= 26 && c[index + 1] > 0 && c[index + 2] > 0)
            {
                c[index]--;
                c[index + 1]--;
                c[index + 2]--;
                n++;
                return CheckMenZi(c, s, n);
            }
            return false;
        }
        return true;
    }

    public static List<int> CheckTing(int[] cardArray)
    {
        Debug.Log("===check ting");
        var result = new List<int>();
        for (int i = 0; i < NUM_C; i++)
        {
            var index = CIndex[i];
            if (cardArray[index] <= 3)
            {
                var c = DeepCopy(cardArray);
                c[index]++;
                if (CheckRong(c)) result.Add(index);
            }
        }
        return result;
    }

    private static int[] DeepCopy(int[] arr)
    {
        var l = arr.Length;
        var result = new int[l];
        for (int i = 0; i < l; i++)
        {
            result[i] = arr[i];
        }
        return result;
    }


    public class RongData
    {
        public int atama;
        public int[][] mens;

        public RongData()
        {
            atama = 0;
            mens = new int[4][];
            for (int i = 0; i < 4; i++)
            {
                mens[i] = new[] { 0, 0, 0 };
            }
        }
    }

    public static string IndexToName(int cindex)
    {
        return NameDic[cindex];
    }

    private static Dictionary<int,string > NameDic = new Dictionary<int, string>()
    {
        {0,"M1"},{1,"M2"},{2,"M3"},{3,"M4"},{4,"M5"},{5,"M6"},{6,"M7"},{7,"M8"},{8,"M9"},
        {10,"S1"},{11,"S2"},{12,"S3"},{13,"S4"},{14,"S5"},{15,"S6"},{16,"S7"},{17,"S8"},{18,"S9"},
        {20,"P1"},{21,"P2"},{22,"P3"},{23,"P4"},{24,"P5"},{25,"P6"},{26,"P7"},{27,"P8"},{28,"P9"},
        {30,"D"},{32,"N"},{34,"X"},{36,"B"},{38,"P"},{40,"F"},{42,"Z"}
    };
}
