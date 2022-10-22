using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerTutorial : MonoBehaviour
{
    [SerializeField, Tooltip("left Shoulder Transform")] private Transform m_leftShoulderTransform;
    [SerializeField, Tooltip("right Shoulder Transform")] private Transform m_rightShoulderTransform;
    
    [SerializeField, Tooltip("left Stick Transform")] private Transform m_leftStickTransform;

    [SerializeField, Tooltip("right Stick Transform")] private Transform m_rightStickTransform;
    
    [SerializeField, Tooltip("down")] private float m_down;
    [SerializeField, Tooltip("up")] private float m_up;
    
    private InputAction m_leftStick;
    private InputAction m_rightStick;
    
    private PlayerInput m_playerInput;
    
    private void Start()
    {
        m_playerInput = SceneManager.Instance.GetPlayerInput();
        
        m_leftStick = m_playerInput.currentActionMap["Left Stick"];
        m_rightStick = m_playerInput.currentActionMap["Right Stick"];
        
        m_playerInput.currentActionMap["Left Shoulder"].started += _ => Shoulder_Started(m_leftShoulderTransform);
        m_playerInput.currentActionMap["Left Shoulder"].canceled += _ => Shoulder_Canceled(m_leftShoulderTransform);
        
        m_playerInput.currentActionMap["Right Shoulder"].started += _ => Shoulder_Started(m_rightShoulderTransform);
        m_playerInput.currentActionMap["Right Shoulder"].canceled += _ => Shoulder_Canceled(m_rightShoulderTransform);
    }

    private void Shoulder_Canceled(Transform tr)
    {
        tr.localPosition = new Vector3(tr.localPosition.x,m_up,tr.localPosition.z);
    }
    
    private void Shoulder_Started(Transform tr)
    {
        tr.localPosition = new Vector3(tr.localPosition.x,m_down,tr.localPosition.z);
    }

    private void Update()
    {
        m_leftStickTransform.localPosition = m_leftStick.ReadValue<Vector2>() * 0.25f;
        m_rightStickTransform.localPosition = m_rightStick.ReadValue<Vector2>() * 0.25f;
    }
}
