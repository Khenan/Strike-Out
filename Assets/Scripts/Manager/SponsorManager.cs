using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SponsorManager : Singleton<SponsorManager>
{
    [SerializeField, Tooltip("Panneaux des joueurs")] private List<SpriteRenderer> m_spriteRendererList = new List<SpriteRenderer>();
    [SerializeField, Tooltip("Labels Join des joueurs")] private List<GameObject> m_labelJoinList = new List<GameObject>();
    [SerializeField, Tooltip("Labels Ready des joueurs")] private List<GameObject> m_labelReadyList = new List<GameObject>();
    [SerializeField, Tooltip("Labels Back des joueurs")] private List<GameObject> m_labelBackList = new List<GameObject>();
    [SerializeField, Tooltip("First Sponsors des joueurs")] private List<SpriteRenderer> m_firstSponsorListPlayer = new List<SpriteRenderer>();
    [SerializeField, Tooltip("Second Sponsors des joueurs")] private List<SpriteRenderer> m_secondSponsorListPlayer = new List<SpriteRenderer>();

    [SerializeField, Tooltip("Sprite de la croix")] private Sprite m_spriteCross;

    private int m_animJoin = Animator.StringToHash("Join");
    private int m_animReady = Animator.StringToHash("Ready");
    private int m_animFirst = Animator.StringToHash("First");
    private int m_animSecond = Animator.StringToHash("Second");
    private int m_animWait = Animator.StringToHash("Wait");
    
    protected override string GetSingletonName()
    {
        return "SponsorManager";
    }

    public void Join(MasterPlayerController player)
    {
        if (!SceneManager.Instance.CanPlay) return;
        
        m_labelJoinList[player.m_id].SetActive(false);
        
        if (player.m_id >= 2) return;
        
        m_spriteRendererList[player.m_id].GetComponent<Animator>().SetTrigger(m_animJoin);

        if(player.m_id == 1) MasterInputManager.Instance.gameObject.SetActive(false);
    }

    public void Ready(int id)
    {
        ReadyVisible(id);
        
        if(DataManager.Instance.m_masterPlayerList.Count < 2) return;
        bool allReady = true;
        
        DataManager.Instance.m_masterPlayerList.ForEach(p => { if (!p.m_playerSelecter.m_isReady) {allReady = false;} });
        
        if(!allReady) return;
        DataManager.Instance.m_masterPlayerList.ForEach(p => { p.m_playerSelecter.m_state = SelecterController.States.NULL;});
        
        DataManager.Instance.m_masterPlayerList.ForEach(p =>
        {
            p.m_playerSelecter.m_spriteRendererSelecter.enabled = false;
            p.m_playerSelecter.m_spriteRendererSelecterShadow.enabled = false;
        });
        
        SceneManager.Instance.GoToScene(4);
    }

    public void PressToReady(int id, bool value)
    {
        m_labelReadyList[id].SetActive(value);
    }
    private void PressToBack(int id, bool value)
    {
        m_labelBackList[id].SetActive(value);
    }

    private void ReadyVisible(int id)
    {
        ResetTriggers(id);
        m_spriteRendererList[id].GetComponent<Animator>().SetTrigger(m_animReady);
        PressToBack(id, true);
        PressToReady(id, false);
    }

    public void UnreadyVisible(int id)
    {
        ResetTriggers(id);
        PressToBack(id, false);
        PressToReady(id, true);
        AnimWait(id);
    }

    public void SetFirstSponsor(int id, Sprite sprite)
    {
        m_firstSponsorListPlayer[id].sprite = sprite;
    }
    public void RemoveFirstSponsor(int id)
    {
        m_firstSponsorListPlayer[id].sprite = m_spriteCross;
    }
    public void SetSecondSponsor(int id, Sprite sprite)
    {
        m_secondSponsorListPlayer[id].sprite = sprite;
    }
    public void RemoveSecondSponsor(int id)
    {
        m_secondSponsorListPlayer[id].sprite = m_spriteCross;
    }

    public void AnimFirst(int id)
    {
        ResetTriggers(id);
        m_spriteRendererList[id].GetComponent<Animator>().SetTrigger(m_animFirst);
    }
    public void AnimSecond(int id)
    {
        ResetTriggers(id);
        m_spriteRendererList[id].GetComponent<Animator>().SetTrigger(m_animSecond);
    }
    public void AnimWait(int id)
    {
        ResetTriggers(id);
        m_spriteRendererList[id].GetComponent<Animator>().SetTrigger(m_animWait);
    }

    private void ResetTriggers(int id)
    {
        m_spriteRendererList[id].GetComponent<Animator>().ResetTrigger(m_animReady);
        m_spriteRendererList[id].GetComponent<Animator>().ResetTrigger(m_animFirst);
        m_spriteRendererList[id].GetComponent<Animator>().ResetTrigger(m_animSecond);
        m_spriteRendererList[id].GetComponent<Animator>().ResetTrigger(m_animWait);
    }
}