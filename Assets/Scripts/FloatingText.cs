using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{
    public Animator Animator;
    public Text DamageText;

    private void Start()
    {
        AnimatorClipInfo[] clipInfos = Animator.GetCurrentAnimatorClipInfo(0);
        Destroy(this.gameObject,clipInfos[0].clip.length);
        DamageText = Animator.GetComponent<Text>();
    }

    public void SetText(string text)
    {
        Animator.GetComponent<Text>().text = text;
    }
}