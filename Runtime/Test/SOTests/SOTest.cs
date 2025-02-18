using System.Collections.Generic;
using UnityEngine;

namespace MizukiTool.GeneralSO
{

    [CreateAssetMenu(fileName = "TestSO", menuName = "MizukiTool/ScriptableObjectTools/TestSO")]
    public class TestSO : TwoValueSO<SOTestEnum, List<int>>
    {

    }

    public enum SOTestEnum
    {
        Test1,
        Test2,
        Test3,
        Other
    }

}