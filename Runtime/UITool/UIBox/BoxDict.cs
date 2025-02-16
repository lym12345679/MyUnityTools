using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MizukiTool.Box
{
    public enum BoxEnum
    {
        MessageBox,
    }
    public static class BoxDict
    {
        //用字典存储所有的UIEnum和类型
        public static Dictionary<System.Type, BoxEnum> boxTypeDic = new Dictionary<System.Type, BoxEnum>
        {

        };
        //用字典存储所有的UI预制体路径
        public static Dictionary<BoxEnum, string> BoxPathDic = new Dictionary<BoxEnum, string>
        {

        };
    }
}

