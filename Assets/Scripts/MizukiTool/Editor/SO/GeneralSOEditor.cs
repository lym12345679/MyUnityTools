#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MizukiTool.GeneralSO
{
    [CustomEditor(typeof(TwoValueSO<,>), true)]
    public class TwoValueSOEditor : Editor
    {
        private string numText = "0";
        private string searchText = "";
        private string deleteEnum = "";
        private SerializedProperty searchList = null;

        //重写OnInspectorGUI类(刷新Inspector面板)
        public override void OnInspectorGUI()
        {
            // 更新序列化对象
            serializedObject.Update();

            OneElement();
            MutiElement();
            RemoveOneElementByEnum();
            Search();
            SearchList();

            // 应用对序列化对象的修改
            serializedObject.ApplyModifiedProperties();

            //继承基类方法
            base.OnInspectorGUI();
        }

        private void OneElement()
        {
            GUILayout.BeginHorizontal(
                GUILayout.MinWidth(100),
                GUILayout.MaxWidth(400)
            );
            if (GUILayout.Button("添加一个"))
            {
                target.GetType().GetMethod("AddOneElement").Invoke(target, null);
            }
            if (GUILayout.Button("删除一个"))
            {
                target.GetType().GetMethod("RemoveOneElement").Invoke(target, null);
            }
            GUILayout.EndHorizontal();
        }

        private void MutiElement()
        {
            GUILayout.BeginHorizontal(
                GUILayout.MinWidth(100),
                GUILayout.MaxWidth(400)
            );

            int num;
            numText = GUILayout.TextField(numText,
                10,
                GUILayout.Height(20),
                GUILayout.MaxWidth(100),
                GUILayout.MinWidth(20)
            );
            if (!int.TryParse(numText, out num))
            {
                num = 0;
            }
            if (GUILayout.Button("添加多个"))
            {
                target.GetType().GetMethod("AddMutiElement").Invoke(target, new object[] { num });
            }
            if (GUILayout.Button("删除多个"))
            {
                target.GetType().GetMethod("RemoveMutiElement").Invoke(target, new object[] { num });
            }
            GUILayout.EndHorizontal();
        }
        private void RemoveOneElementByEnum()
        {
            GUILayout.BeginHorizontal(
                GUILayout.MinWidth(100),
                GUILayout.MaxWidth(400)
            );
            deleteEnum = GUILayout.TextField(deleteEnum,
                GUILayout.Height(20),
                GUILayout.MaxWidth(100),
                GUILayout.MinWidth(20)
            );
            if (GUILayout.Button("删除这个"))
            {
                target.GetType().GetMethod("RemoveOneElementByEnum").Invoke(target, new object[] { deleteEnum });
            }
            GUILayout.EndHorizontal();
        }
        private void Search()
        {
            GUILayout.BeginHorizontal(
                GUILayout.MinWidth(100),
                GUILayout.MaxWidth(400)
            );
            searchText = GUILayout.TextField(searchText,
                GUILayout.MaxWidth(100)
            );
            GUILayout.Label("这是搜索框");
            GUILayout.EndHorizontal();

        }

        private void SearchList()
        {
            searchList = serializedObject.FindProperty("SelfList");
            GUILayout.BeginVertical();
            GUILayout.Label("当前结果:");

            for (int i = 0; i < searchList.arraySize; i++)
            {
                var item = searchList.GetArrayElementAtIndex(i);
                var enumProperty = item.FindPropertyRelative("EnumValue");
                var value2Property = item.FindPropertyRelative("Value2");
                if (searchText.Length > 1 && !enumProperty.enumNames[enumProperty.enumValueIndex].Contains(searchText))
                {
                    //Debug.Log(enumProperty.enumNames[enumProperty.enumValueIndex]);
                    continue;
                }
                GUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(enumProperty,
                    GUIContent.none,
                    GUILayout.MaxWidth(100)
                );
                GUILayout.Space(15);
                EditorGUILayout.PropertyField(value2Property,
                    GUIContent.none
                );
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }
    }
}
#endif