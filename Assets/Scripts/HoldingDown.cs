using UnityEngine;
using UnityEngine.EventSystems;

public class HoldingDown : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public const float HoldingDownTime = 30;
    public bool IsHoldingDown;
    public float HoldingTime;
    public GameManager gameManager;

    public void OnPointerDown(PointerEventData eventData)
    {
        IsHoldingDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        IsHoldingDown = false;
        HoldingTime = HoldingDownTime;
    }

    private void Awake()
    {
        HoldingTime = HoldingDownTime;
    }

    public void Update()
    {
        if (HoldingTime > 0 && IsHoldingDown)
        {
            HoldingTime -= 1;
        }
        else if (HoldingTime <= 0 && gameManager.gunSlot.Items[0] != null)
        {
            gameManager.gunSlot.ItemDrop(gameManager.spawn.CharacterList[0]);
            HoldingTime = HoldingDownTime;
        }
    }
}