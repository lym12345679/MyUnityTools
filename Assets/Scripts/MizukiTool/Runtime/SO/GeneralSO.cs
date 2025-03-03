using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MizukiTool.GeneralSO
{
    public class TwoValueSO<T1, T2> : ScriptableObject where T1 : Enum
    {
        public virtual TwoValueSO<T1, T2> GetParentClass()
        {
            return this;
        }

        [HideInInspector]
        public List<TwoValueClass<T1, T2>> SelfList = new List<TwoValueClass<T1, T2>>();
        public T2 GetValueByKey(T1 key)
        {
            foreach (var item in SelfList)
            {
                if (item.EnumValue.Equals(key))
                {
                    return item.Value2;
                }
            }
            return default;
        }
        public bool TryGetValueByKey(T1 key, out T2 value)
        {
            foreach (var item in SelfList)
            {
                if (item.EnumValue.Equals(key))
                {
                    value = item.Value2;
                    return true;
                }
            }
            value = default;
            return false;
        }
        void OnValidate()
        {
            for (int i = 0; i < SelfList.Count; i++)
            {
                SelfList[i].Name = SelfList[i].EnumValue.ToString();
            }
            //Search(currentSearchText);
        }
        #region Func
        public virtual void TestFunc()
        {
            Debug.Log("TestFunc");
        }
        public virtual void AddOneElement()
        {
            SelfList.Add(new TwoValueClass<T1, T2>()
            {
                EnumValue = (T1)Enum.GetValues(typeof(T1)).GetValue(0),
                Name = Enum.GetValues(typeof(T1)).GetValue(0).ToString()
            });
        }
        public virtual void RemoveOneElement()
        {
            if (SelfList.Count > 0)
            {
                SelfList.RemoveAt(SelfList.Count - 1);
            }
        }
        public virtual void AddMutiElement(int num)
        {
            for (int i = 0; i < num; i++)
            {
                SelfList.Add(new TwoValueClass<T1, T2>()
                {
                    EnumValue = (T1)Enum.GetValues(typeof(T1)).GetValue(0),
                    Name = Enum.GetValues(typeof(T1)).GetValue(0).ToString()
                });
            }
        }
        public virtual void RemoveMutiElement(int num)
        {
            for (int i = 0; i < num; i++)
            {
                if (SelfList.Count > 0)
                {
                    SelfList.RemoveAt(SelfList.Count - 1);
                }
            }
        }
        public virtual void RemoveOneElementByEnum(string deleteEnum)
        {
            for (int i = 0; i < SelfList.Count; i++)
            {
                if (SelfList[i].EnumValue.ToString().Equals(deleteEnum))
                {
                    SelfList.RemoveAt(i);
                    Debug.Log("删除了" + deleteEnum);
                    break;
                }
                else
                {
                    Debug.Log(SelfList[i].EnumValue.ToString());
                }
            }
        }
        #endregion
    }


    [Serializable]
    public class TwoValueClass<T1, T2> where T1 : Enum
    {
        [HideInInspector]
        public string Name;
        public T1 EnumValue;
        public T2 Value2;
    }
}

