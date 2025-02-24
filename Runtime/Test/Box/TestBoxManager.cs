using System.Collections.Generic;
using MizukiTool.Box;

namespace MizukiTool.Test.Box
{
    public class TestBoxManager : BoxManager
    {
        private static TestBoxManager instance;
        public new static TestBoxManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TestBoxManager();
                }
                return instance;
            }
        }
        protected override Dictionary<string, string> GetBoxPathDict()
        {
            return TestBoxDict.BoxPathDic;
        }
        protected override Dictionary<System.Type, string> GetBoxTypeDict()
        {
            return TestBoxDict.BoxTypeDic;
        }
    }
}