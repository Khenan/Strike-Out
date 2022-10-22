using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class SelecterController : UIManager
{
    [HideInInspector] public MasterPlayerController m_masterPlayerController;
    [HideInInspector] public PlayerManager m_playerManager;

    [SerializeField, Tooltip("Selecters spriteRenderer")] public SpriteRenderer m_spriteRendererSelecter;
    [SerializeField, Tooltip("Selecters Shadow spriteRenderer")] public SpriteRenderer m_spriteRendererSelecterShadow;

    [SerializeField, Tooltip("First Sponsor")] public int m_firstSponsor = -1;
    [SerializeField, Tooltip("Second Sponsor")] public int m_secondSponsor = -1;

    private bool m_completeSponsor = false;
    
    public enum States
    {
        NULL,
        SPONSOR,
        GAMEPLAY
    }

    public States m_state;

    public bool m_isReady;
    
    private int m_lineButtonSelected = 0;
    private int m_rowButtonSelected = 0;
    private bool m_onPress = false;
    private bool m_onMove = false;
    
    protected override void OnEnable()
    {
        m_state = States.SPONSOR;
        m_playerInput = GetComponent<PlayerInput>();
        Init();

        if (SceneManager.Instance.m_idCurrentScene != 3) return;
        SponsorMenu.m_sponsorButtonList[m_lineButtonSelected][m_rowButtonSelected].FirstSelected();
        SetPos();
        
        if (m_masterPlayerController.m_id == 1)
        {
            m_spriteRendererSelecter.flipX = true;
            m_spriteRendererSelecterShadow.flipX = true;
            m_spriteRendererSelecter.transform.position += Vector3.right;
        }
    }

    private void SetPos()
    {
        transform.position = SponsorMenu.m_sponsorButtonList[m_lineButtonSelected][m_rowButtonSelected].transform
        .position;
    }

    protected override void OnDisable()
    {
        if (m_playerInput == null || m_playerInput.currentActionMap == null) return;
        base.OnDisable();
    }

    private void VerifComplete()
    {
        if (m_firstSponsor < 0 || m_secondSponsor < 0)
        {
            m_completeSponsor = false;
            SponsorManager.Instance.PressToReady(m_masterPlayerController.m_id, false);
            return;
        }
        m_completeSponsor = true;
        SponsorManager.Instance.PressToReady(m_masterPlayerController.m_id, true);
    }

    private void Ready()
    {
        m_isReady = true;
        SponsorManager.Instance.Ready(m_masterPlayerController.m_id);
    }
    private void Unready()
    {
        if(!m_isReady) return;
        m_isReady = false;
        SponsorManager.Instance.UnreadyVisible(m_masterPlayerController.m_id);
        VerifComplete();
    }
    
    private void GetSponsor()
    {
        SponsorButton button = SponsorMenu.m_sponsorButtonList[m_lineButtonSelected][m_rowButtonSelected];
        if (m_firstSponsor > -1 && m_secondSponsor < 0)
        {
            int bId = button.m_id;
            if (bId == 100) bId = Random.Range(0, DataManager.Instance.m_listAllSponsor.Count);
            m_secondSponsor = bId;
            SponsorManager.Instance.SetSecondSponsor(m_masterPlayerController.m_id, button.m_sprite);
            SponsorManager.Instance.AnimWait(m_masterPlayerController.m_id);
        }
        else if (m_firstSponsor < 0)
        {
            int bId = button.m_id;
            if (bId == 100) bId = Random.Range(0, DataManager.Instance.m_listAllSponsor.Count);
            m_firstSponsor = bId;
            SponsorManager.Instance.SetFirstSponsor(m_masterPlayerController.m_id, button.m_sprite);
            SponsorManager.Instance.AnimSecond(m_masterPlayerController.m_id);
        }
    }
    private void RemoveSponsor()
    {
        if (m_firstSponsor > -1 && m_secondSponsor > -1)
        {
            m_secondSponsor = -1;
            SponsorManager.Instance.RemoveSecondSponsor(m_masterPlayerController.m_id);
            SponsorManager.Instance.AnimSecond(m_masterPlayerController.m_id);
        }
        else if (m_firstSponsor > -1 && m_secondSponsor < 0)
        {
            m_firstSponsor = -1;
            SponsorManager.Instance.RemoveFirstSponsor(m_masterPlayerController.m_id);
            SponsorManager.Instance.AnimFirst(m_masterPlayerController.m_id);
        }
        VerifComplete();
    }
    protected override void Select_Started(InputAction.CallbackContext ctx)
    {
        if (!SceneManager.Instance.CanPlay) return;
        
        switch (m_state)
        {
            case States.NULL:
                break;
            case States.SPONSOR:
                m_onPress = true;
                if(!m_completeSponsor)
                    SponsorMenu.m_sponsorButtonList[m_lineButtonSelected][m_rowButtonSelected].Pressed();
                break;
            case States.GAMEPLAY:
                GameManager.Instance.StartReadyButton(m_masterPlayerController.m_id);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    protected override void Select_Canceled(InputAction.CallbackContext ctx)
    {
        if (!SceneManager.Instance.CanPlay) return;
        
        switch (m_state)
        {
            case States.NULL:
                break;
            case States.SPONSOR:
                if (m_isReady) break;
                if (m_firstSponsor < 0 || m_secondSponsor < 0)
                {
                    if(m_onMove)
                        SponsorMenu.m_sponsorButtonList[m_lineButtonSelected][m_rowButtonSelected].Released();
                    else
                    {
                        SponsorMenu.m_sponsorButtonList[m_lineButtonSelected][m_rowButtonSelected].Released();
                        SponsorMenu.m_sponsorButtonList[m_lineButtonSelected][m_rowButtonSelected].Interact();
                    }
                    m_onMove = false;
                    m_onPress = false;
                    GetSponsor();
                    VerifComplete();
                    break;
                }

                if (!m_completeSponsor) break;
                Ready();
                break;
            case States.GAMEPLAY:
                m_masterPlayerController.ActiveGameplayPlayer();
                GameManager.Instance.OnPlayerJoin(m_masterPlayerController);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    protected override void Up_Started(InputAction.CallbackContext ctx)
    {
        if (!SceneManager.Instance.CanPlay) return;
        
        switch (m_state)
        {
            case States.SPONSOR:
                if (m_isReady || m_completeSponsor) break;
                if (m_lineButtonSelected - 1 < 0) return;
        
                if(m_onPress)
                {
                    m_onMove = true;
                }
                SponsorMenu.m_sponsorButtonList[m_lineButtonSelected][m_rowButtonSelected].Unselected();
                m_lineButtonSelected--;
                if (m_rowButtonSelected >= SponsorMenu.m_sponsorButtonList[m_lineButtonSelected].Count)
                    m_rowButtonSelected = SponsorMenu.m_sponsorButtonList[m_lineButtonSelected].Count - 1;
                SponsorMenu.m_sponsorButtonList[m_lineButtonSelected][m_rowButtonSelected].Selected();
                SetPos();
                break;
        }
    }
    protected override void Down_Started(InputAction.CallbackContext ctx)
    {
        if (!SceneManager.Instance.CanPlay) return;
        switch (m_state)
        {
            case States.SPONSOR:
                if (m_isReady || m_completeSponsor) break;
                if (m_lineButtonSelected + 1 >= SponsorMenu.m_sponsorButtonList.Count) return;
                if (m_onPress)
                {
                    m_onMove = true;
                }
                SponsorMenu.m_sponsorButtonList[m_lineButtonSelected][m_rowButtonSelected].Unselected();
                m_lineButtonSelected++;
                if (m_rowButtonSelected >= SponsorMenu.m_sponsorButtonList[m_lineButtonSelected].Count)
                    m_rowButtonSelected = SponsorMenu.m_sponsorButtonList[m_lineButtonSelected].Count - 1;
                SponsorMenu.m_sponsorButtonList[m_lineButtonSelected][m_rowButtonSelected].Selected();
                SetPos();
                break;
        }

    }
    protected override void Left_Started(InputAction.CallbackContext ctx)
    {
        if (!SceneManager.Instance.CanPlay) return;
        switch (m_state)
        {
            case States.SPONSOR:
                if (m_isReady || m_completeSponsor) break;
                if (m_rowButtonSelected - 1 < 0) return;
                if(m_onPress)
                {
                    m_onMove = true;
                }
                SponsorMenu.m_sponsorButtonList[m_lineButtonSelected][m_rowButtonSelected].Unselected();
                m_rowButtonSelected--;
                SponsorMenu.m_sponsorButtonList[m_lineButtonSelected][m_rowButtonSelected].Selected();
                SetPos();
                break;
        }
        
    }
    protected override void Right_Started(InputAction.CallbackContext ctx)
    {
        if (!SceneManager.Instance.CanPlay) return;
        switch (m_state)
        {
            case States.SPONSOR:
                if (m_isReady || m_completeSponsor) break;
                if (m_rowButtonSelected + 1 >= SponsorMenu.m_sponsorButtonList[m_lineButtonSelected].Count) return;

                if (m_onPress)
                {
                    m_onMove = true;
                }
                SponsorMenu.m_sponsorButtonList[m_lineButtonSelected][m_rowButtonSelected].Unselected();
                m_rowButtonSelected++;
                SponsorMenu.m_sponsorButtonList[m_lineButtonSelected][m_rowButtonSelected].Selected();
                SetPos();
                break;
        }
    }

    protected override void Back_Started(InputAction.CallbackContext ctx)
    {
        if (!SceneManager.Instance.CanPlay) return;
        switch (m_state)
        {
            case States.SPONSOR:
                break;
        }
    }
    protected override void Back_Canceled(InputAction.CallbackContext ctx)
    {
        if (!SceneManager.Instance.CanPlay) return;
        switch (m_state)
        {
            case States.SPONSOR:
                if (!m_isReady)
                {
                    RemoveSponsor();
                    break;
                }

                Unready();
                break;
        }
    }
}
