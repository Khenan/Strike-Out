using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    protected PlayerInput m_playerInput;

    protected virtual void OnEnable()
    {
        Init();
    }

    protected virtual void OnDisable()
    {
        Uninit();
    }
    
    protected void Init()
    {
        m_playerInput.currentActionMap["Select"].started += Select_Started;
        m_playerInput.currentActionMap["Select"].canceled += Select_Canceled;
        m_playerInput.currentActionMap["Back"].started += Back_Started;
        m_playerInput.currentActionMap["Back"].canceled += Back_Canceled;
        m_playerInput.currentActionMap["HoldBack"].started += HoldBack_Started;
        m_playerInput.currentActionMap["HoldBack"].canceled += HoldBack_Canceled;
        m_playerInput.currentActionMap["Up"].started += Up_Started;
        m_playerInput.currentActionMap["Up"].canceled += Up_Canceled;
        m_playerInput.currentActionMap["Down"].started += Down_Started;
        m_playerInput.currentActionMap["Down"].canceled += Down_Canceled;
        m_playerInput.currentActionMap["Left"].started += Left_Started;
        m_playerInput.currentActionMap["Left"].canceled += Left_Canceled;
        m_playerInput.currentActionMap["Right"].started += Right_Started;
        m_playerInput.currentActionMap["Right"].canceled += Right_Canceled;
        m_playerInput.currentActionMap["Down"].started += Down_Started;
        m_playerInput.currentActionMap["Down"].canceled += Down_Canceled;
        m_playerInput.currentActionMap["Option"].started += Option_Started;
        m_playerInput.currentActionMap["Option"].canceled += Option_Canceled;
    }
    protected void Uninit()
    {
        if (m_playerInput == null || m_playerInput.currentActionMap == null) return;
        
        m_playerInput.currentActionMap["Select"].started -= Select_Started;
        m_playerInput.currentActionMap["Select"].canceled -= Select_Canceled;
        m_playerInput.currentActionMap["Back"].started -= Back_Started;
        m_playerInput.currentActionMap["Back"].canceled -= Back_Canceled;
        m_playerInput.currentActionMap["HoldBack"].started -= HoldBack_Started;
        m_playerInput.currentActionMap["HoldBack"].canceled -= HoldBack_Canceled;
        m_playerInput.currentActionMap["Up"].started -= Up_Started;
        m_playerInput.currentActionMap["Up"].canceled -= Up_Canceled;
        m_playerInput.currentActionMap["Down"].started -= Down_Started;
        m_playerInput.currentActionMap["Down"].canceled -= Down_Canceled;
        m_playerInput.currentActionMap["Left"].started -= Left_Started;
        m_playerInput.currentActionMap["Left"].canceled -= Left_Canceled;
        m_playerInput.currentActionMap["Right"].started -= Right_Started;
        m_playerInput.currentActionMap["Right"].canceled -= Right_Canceled;
        m_playerInput.currentActionMap["Option"].started -= Option_Started;
        m_playerInput.currentActionMap["Option"].canceled -= Option_Canceled;
    }
    protected virtual void Select_Started(InputAction.CallbackContext ctx) { }
    protected virtual void Select_Canceled(InputAction.CallbackContext ctx) { }
    protected virtual void Back_Started(InputAction.CallbackContext ctx) { }
    protected virtual void Back_Canceled(InputAction.CallbackContext ctx) { }
    protected virtual void HoldBack_Started(InputAction.CallbackContext ctx) { }
    protected virtual void HoldBack_Canceled(InputAction.CallbackContext ctx) { }
    protected virtual void Up_Started(InputAction.CallbackContext ctx) { }
    protected virtual void Up_Canceled(InputAction.CallbackContext ctx) { }
    protected virtual void Down_Started(InputAction.CallbackContext ctx) { }
    protected virtual void Down_Canceled(InputAction.CallbackContext ctx) { }
    protected virtual void Left_Started(InputAction.CallbackContext ctx) { }
    protected virtual void Left_Canceled(InputAction.CallbackContext ctx) { }
    protected virtual void Right_Started(InputAction.CallbackContext ctx) { }
    protected virtual void Right_Canceled(InputAction.CallbackContext ctx) { }
    protected virtual void Option_Started(InputAction.CallbackContext ctx) { }
    protected virtual void Option_Canceled(InputAction.CallbackContext ctx) { }
}