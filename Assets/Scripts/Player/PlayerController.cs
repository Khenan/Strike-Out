using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    //--------------------------Controller----------------------------//
    [HideInInspector]public float m_speedMovement = 10;
    
    //-------------------------Input System-----------------------------//
    [HideInInspector]public PlayerInput m_controls;
    
    //-------------------------Other Script-----------------------------//
    [HideInInspector]public PlayerInteraction m_playerInteraction;
    
    //---------------------------Private---------------------------//
    private Rigidbody2D m_rb;
    
    private InputAction m_actionMovement;
    private InputAction m_actionLook;
    
    private Vector3 m_dirPlayer;
    
    private float m_velocityAngle;
    private float m_smoothTime = 0.1f;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
    }

    public void InitInputAction()
    {
        m_actionMovement = m_controls.currentActionMap["Movement"];
        m_actionLook = m_controls.currentActionMap["Look"];
    }

    public void DoUpdate()
    {
        if(!m_playerInteraction.HasCatched()) m_rb.MovePosition( m_rb.position + m_actionMovement.ReadValue<Vector2>() * m_speedMovement);
        
        float angle = Mathf.Atan2(-m_actionLook.ReadValue<Vector2>().x, m_actionLook.ReadValue<Vector2>().y) * Mathf.Rad2Deg;
        m_dirPlayer.z =  Mathf.SmoothDampAngle(transform.eulerAngles.z, angle, ref m_velocityAngle, m_smoothTime);
        
        if (angle == 0) return;
        
        transform.rotation = Quaternion.Euler(m_dirPlayer);
    }
}