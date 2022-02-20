using UnityEngine;
using UnityEngine.UI;

public class ResourcePanel : MonoBehaviour
{
    public Image image;
    public Text countText;
    public int need = 0;

    public void SetResource(Resource resource)
    {
        image.sprite = resource.icon;
    }

    public void SetCount(int count)
    {
        if (need != 0)
        {
            countText.text= count.ToString() + "/" + need.ToString();
        }
        else
        {
            countText.text = count.ToString();
        }
    }

    public void SetNeed(int newNeed)
    {
        need = newNeed;
    }
}
