using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MizukiTool.ScriptableObjectTools;
using System;

public enum TestEnum
{
    Test1,
    Test2,
    Test3
}
[CreateAssetMenu(fileName = "ScriptableObjectTest", menuName = "ScriptableObject测试")]
public class ScriptableObjectTest : ScriptableObjectTools
{
    [Header("基础类型")]
    public string SearchString;
    [SerializeField]
    private List<TwoValueSOT<TestEnum, GameObject>> twoValueList = new List<TwoValueSOT<TestEnum, GameObject>>();
    private List<TwoValueSOT<TestEnum, GameObject>> SearchedTwoValueList = new List<TwoValueSOT<TestEnum, GameObject>>();
    public int[] intTest = new int[10];

    public List<TwoValueSOT<TestEnum, GameObject>> TwoValueList
    {
        get
        {
            if (twoValueList == null)
            {
                twoValueList = new List<TwoValueSOT<TestEnum, GameObject>>();
            }
            if (SearchString != null)
            {
                SearchedTwoValueList.Clear();
                string searchString = SearchString.ToLower();
                foreach (TwoValueSOT<TestEnum, GameObject> twoValue in twoValueList)
                {
                    if (twoValue.value1.ToString().ToLower().Contains(searchString))
                    {
                        SearchedTwoValueList.Add(twoValue);
                        break;
                    }
                }
                return SearchedTwoValueList;
            }
            return twoValueList;
        }
        set
        {
            twoValueList = value;
        }
    }
}
