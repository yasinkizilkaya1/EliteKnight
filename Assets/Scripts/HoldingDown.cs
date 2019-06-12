using UnityEngine;
using UnityEngine.EventSystems;

public class HoldingDown : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    #region Constants

    public const float HoldingDownTime = 30;

    #endregion

    #region Fields

    public GameManager gameManager;

    public bool IsHoldingDown;
    public float HoldingTime;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        HoldingTime = HoldingDownTime;
    }

    public void Update()
    {
        if (HoldingTime > 0 && IsHoldingDown && gameManager.Character.Guns.Count > 1 && gameManager.IsPause == false)
        {
            HoldingTime -= 1;
        }
        else if (HoldingTime <= 0 && gameManager.GunSlot.Items[0] != null)
        {
            gameManager.GunSlot.GunDrop(gameManager.Character);
            HoldingTime = HoldingDownTime;
        }
    }
   
    #endregion

    #region Public Methods

    public void OnPointerDown(PointerEventData eventData)
    {
        IsHoldingDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        IsHoldingDown = false;
        HoldingTime = HoldingDownTime;
    }

    #endregion
}