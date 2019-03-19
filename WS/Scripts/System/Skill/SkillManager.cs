using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WS
{
    public class SkillManager : MonoBehaviour
    {

        public static SkillManager Instance { get; private set; }

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

        private List<Skill> skillList = new List<Skill>();

        public void UseSkill(int skillid, FieldObject obj, System.Action cb )
        {
            Debug.Log("use skill " + obj.name + " " + skillid);
            var skilldata = MasterDataManager.Skill.GetData(skillid);
            var path = "Effect/Attack/" + skilldata.effect;
//            Debug.Log("effect " + path);
            var prefab = Resources.Load<GameObject>(path);
            var effectObj = Instantiate(prefab);
            var effectScript = effectObj.GetComponent<EffectBase>();
            effectScript.Play();
            if (obj is FieldPlayer)
            {
                Utils.SetParent(effectObj.transform, (obj as FieldPlayer).BodyTransform);
            }
            else
            {
                Utils.SetParent(effectObj.transform, obj.transform);
            }
            effectObj.transform.localPosition = skilldata.effect_offset;

            var skill = new Skill();
            skill.Init(skilldata, obj, effectScript, cb);
            skillList.Add(skill);
        }

        void Update()
        {
            var t = Time.deltaTime;
            var l = skillList.Count;
            for (int i = 0; i < l; i++)
            {
                var skill = skillList[i];
                skill.Update(t);
                if (skill.Dead)
                {
                    skillList.RemoveAt(i);
                    skill.DestroySelf();
                    i--;
                    l--;
                }
            }
        }
    }
}