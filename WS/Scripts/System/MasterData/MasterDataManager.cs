using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WS
{
    public class MasterDataManager : MonoBehaviour
    {
        public static MasterDataManager Instance;

        [SerializeField]
        private TextAsset data_field_object;
        [SerializeField]
        private TextAsset data_collect;
        [SerializeField]
        private TextAsset data_item;
        [SerializeField]
        private TextAsset data_const_text;
        [SerializeField]
        private TextAsset data_craft;
        [SerializeField]
        private TextAsset data_skill;
        [SerializeField]
        private TextAsset data_monster;


        public static MasterDataTable<MasterDataFieldObject> FieldObject = new MasterDataTable<MasterDataFieldObject>();
        public static MasterDataCollectTable Collect = new MasterDataCollectTable();
        public static MasterDataTable<MasterDataItem> Item = new MasterDataTable<MasterDataItem>();
        public static MasterDataTable<MasterDataConstText> ConstText = new MasterDataTable<MasterDataConstText>();
        public static MasterDataTable<MasterDataCraft> Craft = new MasterDataTable<MasterDataCraft>();
        public static MasterDataTable<MasterDataSkill> Skill = new MasterDataTable<MasterDataSkill>();
        public static MasterDataTable<MasterDataMonster> Monster = new MasterDataTable<MasterDataMonster>();


        public Dictionary<GameObject, string> testdic = new Dictionary<GameObject, string>();

        void Awake()
        {

            if (Instance != null)
            {
                Debug.LogError("multi Instance");
                Destroy(this.gameObject);
                return;
            }
            Instance = this;
        }

        public IEnumerator Init()
        {
            FieldObject.Init(data_field_object);
            Collect.Init(data_collect);
            Item.Init(data_item);
            ConstText.Init(data_const_text);
            Craft.Init(data_craft);
            Skill.Init(data_skill);
            Monster.Init(data_monster);
            yield return 0;
        }

    }
}
