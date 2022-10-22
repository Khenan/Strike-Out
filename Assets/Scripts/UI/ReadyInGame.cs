using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyInGame : MonoBehaviour
{
    [SerializeField, Tooltip("Animator List")]
    private List<Animator> m_animatorList = new List<Animator>();
    [SerializeField, Tooltip("Animator List Shadow")]
    private List<Animator> m_animatorShadowList = new List<Animator>();
    
    private int m_pressedAnimation = Animator.StringToHash("Pressed");
    private int m_releasedAnimation = Animator.StringToHash("Released");
    private int m_fadedAnimation = Animator.StringToHash("Faded");
    
    public void StartButton()
    {
        SoundManager.Instance.PlayUIButtonPress();
        m_animatorList?.ForEach(a => a?.SetTrigger(m_pressedAnimation));
    }

    public void CancelButton()
    {
        SoundManager.Instance.PlayReadyPlayer();
        SoundManager.Instance.PlayUIButtonRelease();
        m_animatorList?.ForEach(a => a?.SetTrigger(m_releasedAnimation));
        // Lancer l'animation de fade out
        StartCoroutine(FadeOut());
    }
    
    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(0.5f);
        // Animation de fade Out
        m_animatorList?.ForEach(a => a?.SetTrigger(m_fadedAnimation));
        m_animatorShadowList?.ForEach(a => a?.SetTrigger(m_fadedAnimation));
        
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}