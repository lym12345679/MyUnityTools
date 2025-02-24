using MizukiTool.Test.Box;
using UnityEngine;
namespace MizukiTool.Test
{
    public class UITestScene : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            MessageBox.Open(new Message("Title", "Content"));
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
