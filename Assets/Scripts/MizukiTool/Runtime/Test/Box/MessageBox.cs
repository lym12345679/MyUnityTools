using UnityEngine;
namespace MizukiTool.Test.Box
{
    public class MessageBox : TestGeneralBox<MessageBox, Message, string>
    {
        public override void GetParams(Message param)
        {
            this.param = param;
        }
        public override string SendParams()
        {
            return "关闭UI";
        }
        void Start()
        {
            Debug.Log("GetTitle:" + param.title);
            Debug.Log("GetContent:" + param.content);
        }
    }
    public class Message
    {
        public Message(string title, string content)
        {
            this.title = title;
            this.content = content;
        }
        public string title;
        public string content;
    }
}
