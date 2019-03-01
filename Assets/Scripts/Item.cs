using UnityEngine;

public class Item : MonoBehaviour
{
    #region Constants

    private const string TAG_CHARACTER = "Character";

    #endregion

    #region Fields

    public item item;

    #endregion

    #region Unity Method

    private void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.CompareTag(TAG_CHARACTER))
        {
            collider.GetComponent<Character>();
        }
    }

    #endregion
}