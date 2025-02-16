using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace MizukiTool.Box
{
    public class MessageBox : GeneralBox<MessageBox, Message, string>
    {
        public TextMeshProUGUI title;
        public TextMeshProUGUI content;
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
            title.text = param.title;
            content.text = param.content;
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
