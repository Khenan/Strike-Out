using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenu : UIManager
{
    [SerializeField, Tooltip("Menu Button List")] private List<MenuButton> m_menuButtonList = new List<MenuButton>();

    private int m_idButtonSelected = 0;
    private bool m_onPress = false;
    private bool m_onMove = false;

    protected override void OnEnable()
    {
        m_playerInput = SceneManager.Instance.GetPlayerInput();
        Init();
    }

    private void Start()
    {
        m_menuButtonList[m_idButtonSelected].FirstSelected();
    }

    protected override void Up_Started(InputAction.CallbackContext ctx)
    {
        if (!SceneManager.Instance.CanPlay) return;
        if (m_idButtonSelected - 1 < 0) return;
        
        if(m_onPress)
        {
            m_onMove = true;
        }
        m_menuButtonList[m_idButtonSelected].Unselected();
        m_idButtonSelected--;
        m_menuButtonList[m_idButtonSelected].Selected();
    }
    protected override void Down_Started(InputAction.CallbackContext ctx)
    {
        if (!SceneManager.Instance.CanPlay) return;
        if (m_idButtonSelected + 1 >= m_menuButtonList.Count) return;

        if (m_onPress)
        {
            m_onMove = true;
        }
        m_menuButtonList[m_idButtonSelected].Unselected();
        m_idButtonSelected++;
        m_menuButtonList[m_idButtonSelected].Selected();
    }
    protected override void Select_Started(InputAction.CallbackContext ctx)
    {
        if (!SceneManager.Instance.CanPlay) return;
        m_onPress = true;
        m_menuButtonList[m_idButtonSelected].Pressed();
    }
    protected override void Select_Canceled(InputAction.CallbackContext ctx)
    {
        if (!SceneManager.Instance.CanPlay) return;
        if(m_onMove)
            m_menuButtonList[m_idButtonSelected].Released();
        else
        {
            m_menuButtonList[m_idButtonSelected].Released();
            m_menuButtonList[m_idButtonSelected].Interact();
        }
        m_onMove = false;
        m_onPress = false;
    }
    protected override void Back_Started(InputAction.CallbackContext ctx)
    {
        
    }
    protected override void Back_Canceled(InputAction.CallbackContext ctx)
    {
        
    }
}
