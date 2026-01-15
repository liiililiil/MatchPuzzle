using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    [SerializeField]
    private RawImage rawImage;
    
    [SerializeField]
    private Text text;

    public void Bind(Texture texture, int num)
    {
        //바인딩
        rawImage.texture = texture;
        text.text = num.ToString();
    }
}
