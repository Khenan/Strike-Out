using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SplashScreen : UIManager
{
    [SerializeField, Tooltip("Animator List")] private List<Animator> m_animatorList = new List<Animator>();
    private int m_pressedAnimation = Animator.StringToHash("Pressed");
    private int m_releasedAnimation = Animator.StringToHash("Released");

    protected override void OnEnable()
    {
        m_playerInput = SceneManager.Instance.GetPlayerInput();
        Init();
    }

    protected override void Select_Started(InputAction.CallbackContext ctx)
    {
        if (!SceneManager.Instance.CanPlay) return;
        SoundManager.Instance.PlayUIButtonPress();
        m_animatorList?.ForEach(a => a?.SetTrigger(m_pressedAnimation));
    }
    protected override void Select_Canceled(InputAction.CallbackContext ctx)
    {
        if (!SceneManager.Instance.CanPlay) return;
        SoundManager.Instance.PlayUIButtonRelease();
        m_animatorList?.ForEach(a => a?.SetTrigger(m_releasedAnimation));
        // SceneManager qui nous envoi vers le menu
        StartCoroutine(ChargeScene());
    }

    private IEnumerator ChargeScene()
    {
        yield return new WaitForSeconds(0.5f);
        // Envoi au menu
        SceneManager.Instance.GoToScene(2);
    }
}
