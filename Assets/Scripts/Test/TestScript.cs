using MizukiTool.Box;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MessageBox.Open(new Message("Hello", "The World!"));
    }
}
