using UnityEngine;

public class FloatingTextController : MonoBehaviour
{
    public FloatingText PopupText;
    public GameObject Canvas;
    public static FloatingText popupText;
    public static GameObject canvas;

    private void Start()
    {
        popupText = PopupText;
        canvas = Canvas;
    }

    public static void CreateFloatingText(string text,Transform location)
    {
        FloatingText Instance = Instantiate(popupText);
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(new Vector2(location.position.x + Random.Range(-0.5f,0.5f),location.position.y+ Random.Range(-0.5f, 0.5f)));

        Instance.transform.SetParent(canvas.transform, false);
        Instance.transform.position = screenPosition;
        Instance.SetText(text);
    }
}