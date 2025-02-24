using MizukiTool.Box;

namespace MizukiTool.Test.Box
{
    public class TestGeneralBox<T1, T2, T3> : GeneralBox<T1, T2, T3> where T1 : TestGeneralBox<T1, T2, T3> where T2 : class where T3 : class
    {
        public static new void Open(T2 param)
        {
            TestBoxManager.Instance.OpenBox<T1, T2, T3>(param);
        }

        public override void Close()
        {
            TestBoxManager.Instance.CloseBox<T1, T2, T3>(this.BoxID);
        }
    }
}