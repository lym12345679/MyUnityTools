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
        public static Dictionary<System.Type, string> BoxTypeDic = new Dictionary<System.Type, string>()
        {
            //{typeof(MessageBox),BoxEnum.MessageBox.ToString()}
        };
        //用字典存储所有的UI预制体路径
        public static Dictionary<string, string> BoxPathDic = new Dictionary<string, string>
        {
            //{BoxEnum.MessageBox.ToString(),"UIPrefab/MessageBox"}
        };
        /*public static Dictionary<System.Type, BoxEnum> boxTypeDic = new Dictionary<System.Type, BoxEnum>
        {

        };
        //用字典存储所有的UI预制体路径
        public static Dictionary<BoxEnum, string> BoxPathDic = new Dictionary<BoxEnum, string>
        {

        };*/
    }
}

