using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{
    #region Fields

    public Animator Animator;
    public Text DamageText;

    #endregion

    #region Unity Method

    private void Start()
    {
        AnimatorClipInfo[] clipInfos = Animator.GetCurrentAnimatorClipInfo(0);
        Destroy(this.gameObject,clipInfos[0].clip.length);
        DamageText = Animator.GetComponent<Text>();
    }

    #endregion

    #region Public Method

    public void SetText(string text)
    {
        Animator.GetComponent<Text>().text = text;
    }

    #endregion
}