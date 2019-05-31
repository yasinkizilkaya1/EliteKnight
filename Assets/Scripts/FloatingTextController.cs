using UnityEngine;

public class FloatingTextController : MonoBehaviour
{
    #region Fields

    public static FloatingText popupText;
    public static GameObject canvas;

    public FloatingText PopupText;
    public GameObject Canvas;

    #endregion

    #region Unity Method

    private void Start()
    {
        popupText = PopupText;
        canvas = Canvas;
    }

    #endregion

    #region Public Method

    public static void CreateFloatingText(string text, Transform location)
    {
        FloatingText Instance = Instantiate(popupText);
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(new Vector2(location.position.x + Random.Range(-0.5f, 0.5f), location.position.y + Random.Range(-0.5f, 0.5f)));

        Instance.transform.SetParent(canvas.transform, false);
        Instance.transform.position = screenPosition;
        Instance.SetText(text);
    }

    #endregion
}