using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace WS
{
    public class FieldMapDataManager
    {
        private static FieldMapDataManager mInstance;

        public static FieldMapDataManager Instance
        {
            get { return mInstance ?? (mInstance = new FieldMapDataManager()); }
        }

        public Dictionary<Vector2, int[][]> MapDataDic = new Dictionary<Vector2, int[][]>();

        private FieldMapDataManager()
        {
        }

        public int[][] GetMapData(int x, int y)
        {
            return GetMapData(new Vector2(x, y));
        }
        private int[][] GetMapData(Vector2 v)
        {
            return MapDataDic.ContainsKey(v) ? MapDataDic[v] : null;
        }


        public void GenerateFieldMapData()
        {
            var data = GenerateSingleMapData();
            MapDataDic.Add(new Vector2(0, 0), data);
        }

        private int[][] GenerateSingleMapData()
        {
            Debug.Log("generate single map");
            var result = new int[FieldMap.Map_Size][];
            var wood = Random.Range(200, 300);
            for (int i = 0; i < wood; i++)
            {
                var x = (int)(Random.Range(0.1f, 0.9f) * FieldMap.Map_Size);
                var y = (int)(Random.Range(0.1f, 0.9f) * FieldMap.Map_Size);
                if (result[x] == null)
                {
                    result[x] = new int[100];
                }
                result[x][y] = 1;
                //Debug.Log("generate wood " + x + "," + y);
            }
            var stone = Random.Range(100, 180);
            for (int i = 0; i < stone; i++)
            {
                var x = (int)(Random.Range(0.1f, 0.9f) * FieldMap.Map_Size);
                var y = (int)(Random.Range(0.1f, 0.9f) * FieldMap.Map_Size);
                if (result[x] == null)
                {
                    result[x] = new int[100];
                }
                if (result[x][y] == 0)
                {
                    result[x][y] = 2;
                }
                else
                {
                    
                }
            }

            return result;
        }

        public Hashtable GetMapSaveData()
        {
            var result = new Hashtable();
            foreach (KeyValuePair<Vector2, int[][]> keyValuePair in MapDataDic)
            {
                var vecKey = keyValuePair.Key;
                var key = vecKey.x + "," + vecKey.y;
                var pointList = new List<string>();
                var pointarr = keyValuePair.Value;
                for (int i = 0; i < FieldMap.Map_Size; i++)
                {
                    if (pointarr[i] == null) continue;
                    for (int j = 0; j < FieldMap.Map_Size; j++)
                    {
                        var t = pointarr[i][j];
                        if (t > 0)
                        {
                            pointList.Add(i + "," + j + "," + t);
                        }
                    }
                }
                result.Add(key, pointList.ToArray());
            }
            return result;
        }

        public void SaveMapData(string filepath)
        {
            Debug.Log("save map");
            byte[] data = MyMsgPacker.Pack(GetMapSaveData());
            File.WriteAllBytes(filepath, data);
        }

        public void LoadFromData(byte[] data)
        {
            var result = MyMsgPacker.Unpack(data);
            var hash = result as Hashtable;
            if (hash == null)
            {
                Debug.LogError("map data error!");
            }
            foreach (var key in hash.Keys)
            {
                var tarr = key.ToString().Split(',');
                var x = int.Parse(tarr[0]);
                var y = int.Parse(tarr[1]);
                var points = hash[key] as object[];
                if (points == null)
                {
                    Debug.LogError("map data error: " + x + "," + y);
                    continue;
                }
                var mapkey = new Vector2(x, y);
                int[][] mapdata;
                if (MapDataDic.ContainsKey(mapkey))
                {
                    mapdata = MapDataDic[mapkey];
                }
                else
                {
                    mapdata = new int[FieldMap.Map_Size][];
                    MapDataDic.Add(mapkey, mapdata);
                }  
                foreach (var pstr in points)
                {
                    var parr = pstr.ToString().Split(',');
                    var px = int.Parse(parr[0]);
                    var py = int.Parse(parr[1]);
                    var pv = int.Parse(parr[2]);
                    if (mapdata[px] == null)
                    {
                        mapdata[px] = new int[FieldMap.Map_Size];
                     }
                    mapdata[px][py] = pv;
                }
            }
        }
    }
}
