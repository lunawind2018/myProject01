using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMsgPacker
{
    private const byte FIX_INT = 0x00;
    private const byte FIX_INT_END = 0x7f;
    private const byte FIX_MAP = 0x80;
    private const byte FIX_MAP_END = 0x8f;
    private const byte FIX_ARR = 0x90;
    private const byte FIX_ARR_END = 0x9f;
    private const byte FIX_STR = 0xa0;
    private const byte FIX_STR_END = 0xbf;
    private const byte INT8 = 0xd0;
    private const byte INT16 = 0xd1;
    private const byte INT32 = 0xd2;

    private const byte STR8 = 0xd9;
    private const byte STR16 = 0xda;
    private const byte STR32 = 0xdb;

    private const byte ARR16 = 0xdc;
    private const byte ARR32 = 0xdd;

    private const byte MAP16 = 0xde;
    private const byte MAP32 = 0xdf;

    public static object Unpack(byte[] data)
    {
        long index = 0;
        return Unpack(data, ref index);
    }

    public static object Unpack(byte[] data, ref long index)
    {
        var head = data[index];
//        Debug.Log("head " + head.ToString("X"));
        var t = index;
        if (head <= FIX_INT_END)
        {
            // 0xxxxxxx 0x00-0x7f :positive fixint
            index++;
            return (int)head;
        }
        if (head == INT8)
        {
            //0xd0|xxxxxxxx :int8 = byte
            index += 2;
            return (int)data[t + 1];
        }
        if (head == INT16)
        {
            //0xd1|xxxxxxxx|xxxxxxxx :int16 = short
            index += 3;
            return UnpackInt16(data, t);
        }
        if (head == INT32)
        {
            //0xd2|xxxxxxxx|xxxxxxxx|xxxxxxxx|xxxxxxxx :int32 = int
            index += 5;
            return UnpackInt32(data, t);
        }
        if (head >= FIX_STR && head <= FIX_STR_END)
        {
            //101xxxxx 0xa0-0xbf :fixstring
            var len = head - 0xa0;
            index += 1 + len;
            return UnpackString(data, len, t + 1);
        }
        if (head == STR8)
        {
            //0xd9  |YYYYYYYY|  data  |
            var len = (int)(data[t + 1]);
            index += 2 + len;
            return UnpackString(data, len, t + 2);
        }
        if (head == STR16)
        {
            //0xda  |YYYYYYYY|YYYYYYYY|    N objects    |
            var len = UnpackInt16(data, t);
            index += 3 + len;
            return UnpackString(data, len, t + 3);
        }
        if (head == STR32)
        {
            //0xdb  |ZZZZZZZZ|ZZZZZZZZ|ZZZZZZZZ|ZZZZZZZZ|    N objects    |
            var len = UnpackInt32(data, t);
            index += 5 + len;
            return UnpackString(data, len, t + 5);
        }
        if (head >= FIX_MAP && head <= FIX_MAP_END)
        {
            //0x80-0x8f |1000XXXX|   N*2 objects |
            index++;
            return UnpackHashTable(data, (head & 0x0f), ref index);
        }
        if (head == MAP16)
        {
            //0xde  |YYYYYYYY|YYYYYYYY|   N*2 objects |
            index += 3;
            return UnpackHashTable(data, UnpackInt16(data, t), ref index);
        }
        if (head == MAP32)
        {
            //0xdf  |ZZZZZZZZ|ZZZZZZZZ|ZZZZZZZZ|ZZZZZZZZ|   N*2 objects|
            index += 5;
            return UnpackHashTable(data, UnpackInt32(data, t), ref index);
        }
        if (head >= FIX_ARR && head <= FIX_ARR_END)
        {
            //0x90 - 0x9f |1001XXXX|    N objects    |
            index++;
            return UnpackArray(data, (head & 0x0f), ref index);
        }
        if (head == ARR16)
        {
            //0xdc  |YYYYYYYY|YYYYYYYY|    N objects    |
            index += 3;
            return UnpackArray(data, UnpackInt16(data, t), ref index);
        }
        if (head == ARR32)
        {
            //0xdd  |ZZZZZZZZ|ZZZZZZZZ|ZZZZZZZZ|ZZZZZZZZ|    N objects    |
            index += 5;
            return UnpackArray(data, UnpackInt32(data, t), ref index);
        }
        Debug.LogError("unpack error");
        return null;
    }

    private static int UnpackInt16(byte[]data, long t)
    {
        return ((data[t + 1] << 8) | data[t + 2]);
    }

    private static int UnpackInt32(byte[] data, long t)
    {
        return ((data[t + 1] << 24) | (data[t + 2] << 16) | (data[t + 3] << 8) | data[t + 4]);
    }

    private static Array UnpackArray(byte[] data, int len, ref long index)
    {
        object[] result = new object[len];
        for (int i = 0; i < len; i++)
        {
            result[i] = Unpack(data, ref index);
        }
        return result;
    }

    private static string UnpackString(byte[] data, int len, long index)
    {
        var temp = new byte[len];
        for (int i = 0; i < len; i++) temp[i] = data[index + i];
        return System.Text.Encoding.Default.GetString(temp);
    }

    private static Hashtable UnpackHashTable(byte[] data, int len, ref long index)
    {
        var hashtable = new Hashtable();
        for (int i = 0; i < len; i++)
        {
            var kk = Unpack(data, ref index);
            var vv = Unpack(data, ref index);
            hashtable.Add(kk, vv);
        }
        return hashtable;
    }

    //
    //=========================================
    //
    public static byte[] Pack(object data)
    {
        if (data is int)
        {
            return Pack((int)data);
        }
        var str = data as string;
        if (str != null)
        {
            return Pack(str);
        }
        var hash = data as Hashtable;
        if (hash != null)
        {
            return Pack(hash);
        }
        var arr = data as Array;
        if (arr != null)
        {
            return Pack(arr);
        }
        Debug.LogError("pack error");
        return null;
    }

    public static byte[] Pack(Array data)
    {
        long index;
        byte[] result;
        var len = data.Length;
        if (len <= 15)
        {
            //0x90 - 0x9f |1001XXXX|    N objects    |
            result = new byte[1 + len];
            result[0] = (byte)(FIX_ARR + len);
            index = 1;
        }
        else if (len <= short.MaxValue)
        {
            //0xdc  |YYYYYYYY|YYYYYYYY|    N objects    |
            result = new byte[3 + len];
            result[0] = ARR16;
            PackInt16(result, len);
            index = 3;
        }
        else
        {
            //0xdd  |ZZZZZZZZ|ZZZZZZZZ|ZZZZZZZZ|ZZZZZZZZ|    N objects    |
            result = new byte[5 + len];
            result[0] = ARR32;
            PackInt32(result, len);
            index = 5;
        }
        for (int i = 0; i < len; i++)
        {
            result = Join(result, Pack(data.GetValue(i)), ref index);
        }
        var temp = new byte[index];
        for (int i = 0; i < index; i++)
        {
            temp[i] = result[i];
        }
        return temp;
    }


    /*
     * hashtable
     */
    public static byte[] Pack(Hashtable data)
    {
        long index = 0;
        byte[] result;
        var keys = data.Keys;
        var len = keys.Count;
        if (len <= 15)
        {
            //0x80-0x8f 1000XXXX|   N*2 objects  
            result = new byte[1 + (len << 1)];
            result[0] = (byte)(0x80 + len);
            index = 1;
        }
        else if (len <= short.MaxValue)
        {
            //0xde  |YYYYYYYY|YYYYYYYY|   N*2 objects 
            result = new byte[3 + (len << 1)];
            result[0] = MAP16;
            PackInt16(result, len);
            index = 3;
        }
        else
        {
            //0xdf  |ZZZZZZZZ|ZZZZZZZZ|ZZZZZZZZ|ZZZZZZZZ|   N*2 objects
            result = new byte[5 + (len << 1)];
            result[0] = MAP32;
            PackInt32(result, len);
            index = 5;
        }
        foreach (var key in keys)
        {
            var vv = data[key];
            result = Join(result, Pack(key), ref index);
            result = Join(result, Pack(vv), ref index);
        }
        var temp = new byte[index];
        for (int i = 0; i < index; i++)
        {
            temp[i] = result[i];
        }
        return temp;
    }



    /* 
     * string:
     */
    public static byte[] Pack(string data)
    {
        long index;
        byte[] result;
        var len = data.Length;
        var strbytes = System.Text.Encoding.Default.GetBytes(data);
        //            for (int i = 0; i < len; i++)
        //            {
        //                strbytes[i] = (byte)(~strbytes[i]);
        //            }
        if (len <= 31)
        {
            //101xxxxx 0xa0-0xbf :fixstring
            result = new byte[len + 1];
            result[0] = (byte)(len + 0xa0);
            index = 1;
        }
        else if (len <= byte.MaxValue)
        {
            //11011001 0xd9|xxxxxxxx|data :str8
            result = new byte[len + 2];
            result[0] = STR8;
            result[1] = (byte)len;
            index = 2;
        }
        else if (len <= short.MaxValue)
        {
            //11011010 0xda|xxxxxxxx|xxxxxxxx|data :str16
            result = new byte[len + 3];
            result[0] = STR16;
            PackInt16(result, len);
            index = 3;
        }
        else
        {
            //11011011 0xdb|xxxxxxxx|xxxxxxxx|xxxxxxxx|xxxxxxxx|data :str32
            result = new byte[len + 5];
            result[0] = 0xdb;
            PackInt32(result, len);
            index = 5;
        }
        strbytes.CopyTo(result, index);
        return result;

    }

    /* 
     * int:
     */
    public static byte[] Pack(int data)
    {
        // 0xxxxxxx 0x00-0x7f :positive fixint
        if (data >= 0 && data <= 0x7f) return new[] { (byte)data };

        byte[] result;
        if (data <= byte.MaxValue && data >= byte.MinValue)
        {
            // 11010000 0xd0|xxxxxxxx :int8 = byte
            result = new byte[2];
            result[0] = INT8;
            result[1] = (byte)data;
        }
        else if (data <= short.MaxValue && data >= short.MinValue)
        {
            // 11010001 0xd1|xxxxxxxx|xxxxxxxx :int16 = short
            result = new byte[3];
            result[0] = INT16;
            PackInt16(result, data);
        }
        else
        {
            // 11010002 0xd2|xxxxxxxx|xxxxxxxx|xxxxxxxx|xxxxxxxx :int32 = int
            result = new byte[5];
            result[0] = INT32;
            PackInt32(result,data);
        }
        return result;
    }

    private static void PackInt16(byte[] result, int v)
    {
        result[1] = (byte)((v >> 8) & 0xff);
        result[2] = (byte)(v & 0xff);
    }

    private static void PackInt32(byte[] result, int v)
    {
        result[1] = (byte)((v >> 24) & 0xff);
        result[2] = (byte)((v >> 16) & 0xff);
        result[3] = (byte)((v >> 8) & 0xff);
        result[4] = (byte)(v & 0xff);
    }

    private static byte[] Join(byte[] a, byte[] b, ref long index)
    {
        var al = a.LongLength;
        var bl = b.LongLength;
        var newl = al;
        if (bl + index > newl)
        {
            while (bl + index > newl)
            {
                newl = newl << 1;
            }
            //            newl = bl + index;
            var result = new byte[newl];
            a.CopyTo(result, 0);
            b.CopyTo(result, index);
            index += bl;
            return result;
        }
        else
        {
            b.CopyTo(a, index);
            index += bl;
            return a;
        }
    }

}
