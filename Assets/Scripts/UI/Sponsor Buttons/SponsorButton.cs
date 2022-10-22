using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SponsorButton : MonoBehaviour
{
    [SerializeField, Tooltip("Button Animator")] public Animator m_buttonAnimator;
    private int m_selectedAnimation = Animator.StringToHash("Selected");
    private int m_unselectedAnimation = Animator.StringToHash("Unselected");
    private int m_pressedAnimation = Animator.StringToHash("Pressed");
    private int m_releasedAnimation = Animator.StringToHash("Released");
    
    [SerializeField, Tooltip("Sponsor Id")] public int m_id = 0;
    [SerializeField, Tooltip("Sponsor Sprite")] public Sprite m_sprite;

    private int m_nbView = 0;
    
    public void FirstSelected()
    {
        m_nbView++;
        ResetAllTrigger();
        m_buttonAnimator.SetTrigger(m_selectedAnimation);
    }
    public void Selected()
    {
        m_nbView++;
        ResetAllTrigger();
        SoundManager.Instance.PlayUIButtonSelect();
        m_buttonAnimator.SetTrigger(m_selectedAnimation);
    }
    public void Unselected()
    {
        m_nbView--;
        if (m_nbView > 0) return;
        ResetAllTrigger();
        m_buttonAnimator.SetTrigger(m_unselectedAnimation);
    }
    public void Pressed()
    {
        if(m_nbView <= 0) return;
        ResetAllTrigger();
        SoundManager.Instance.PlayUIButtonPress();
        m_buttonAnimator.SetTrigger(m_pressedAnimation);
    }
    public void Released()
    {
        if(m_nbView <= 0) return;
        ResetAllTrigger();
        SoundManager.Instance.PlayUIButtonRelease();
        m_buttonAnimator.SetTrigger(m_releasedAnimation);
    }
    public virtual void Interact() { }
    private void ResetAllTrigger()
    {
        m_buttonAnimator.ResetTrigger(m_releasedAnimation);
        m_buttonAnimator.ResetTrigger(m_selectedAnimation);
        m_buttonAnimator.ResetTrigger(m_unselectedAnimation);
        m_buttonAnimator.ResetTrigger(m_pressedAnimation);
    }
}
