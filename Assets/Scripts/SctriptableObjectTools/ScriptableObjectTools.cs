using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace MizukiTool.ScriptableObjectTools
{
    public class ScriptableObjectTools : ScriptableObject
    {
        
    }
    [Serializable]
    public class TwoValueSOT<T1, T2> where T1 : Enum 
    {
        public T1 value1;
        public T2 value2;
    }    
}

