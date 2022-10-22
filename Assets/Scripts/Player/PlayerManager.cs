using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerManager : MonoBehaviour
{
    //--------------------------Components----------------------------//
    [Header("Components")] 
    [FormerlySerializedAs("m_controls")] [SerializeField, Tooltip("Player Input du joueur")] private PlayerInput m_playerInput;
    [SerializeField, Tooltip("Circle collider 2D du joueur")] private BoxCollider2D m_collider;
    [SerializeField, Tooltip("Sprite Renderer du feedback grab")] private SpriteRenderer m_spriteRenderer;
    
    //--------------------------Layer Mask----------------------------//
    [Header("Mesh Shadow")] 
    [SerializeField, Tooltip("Shadow du mesh player")] public Transform m_shadowTransform;
    
    //--------------------------Layer Mask----------------------------//
    [Header("Layer")] 
    [SerializeField, Tooltip("Layer de la ball")] private LayerMask m_layerBall;

    //--------------------------Controller Variables----------------------------//
    [Header("Controller")] 
    [SerializeField, Tooltip("Vitesse des mouvements du joueur")] private float m_speedMovement = 10;
    
    //--------------------------Interaction Variables----------------------------//
    [Header("Interaction")] 
    [SerializeField, Tooltip("Vitesse des mouvements du joueur")] private float m_timeHoldingMax = 1f;
    
    //--------------------------Public Hide----------------------------//
    [HideInInspector] public Goal m_goal;
    
    //--------------------------OTHER SCRIPT----------------------------//
    [HideInInspector] public PlayerController m_playerController;
    [HideInInspector] public PlayerInteraction m_playerInteraction;
    [HideInInspector] public MasterPlayerController m_masterPlayerController;

    private Vector3 m_dirShadow = new Vector3(0,-0.05f);
    private Vector3 m_scaleMultiple = new Vector3(0.25f,0.23f);

    private void Awake()
    {
        // Create Component
        m_playerController = gameObject.AddComponent<PlayerController>();
        m_playerInteraction = gameObject.AddComponent<PlayerInteraction>();
        
        // Init variables component
        m_playerInput = GetComponent<PlayerInput>();

        //Controller
        InitControllerScript();
        
        //Interact
        InitInteractionScript();
    }

    private void InitControllerScript()
    {
        m_playerController.m_playerInteraction = m_playerInteraction;
        m_playerController.m_speedMovement = m_speedMovement;
        m_playerController.m_controls = m_playerInput;
        m_playerController.InitInputAction();
    }

    private void InitInteractionScript()
    {
        m_shadowTransform.SetParent(null);
        m_playerInteraction.m_spriteRenderer = m_spriteRenderer;
        m_playerInteraction.m_collider = m_collider;
        m_playerInteraction.m_timeHoldingMax = m_timeHoldingMax;
        m_playerInteraction.m_layerBall = m_layerBall;
        m_playerInteraction.m_controls = m_playerInput;
        m_playerInteraction.InitInputAction();
    }
    
    public void InitGoalScript()
    {
        m_goal.m_playerManager = this;
    }
    
    private void Update()
    {
        m_shadowTransform.position = transform.position + m_dirShadow;
        m_shadowTransform.rotation = transform.rotation;
        m_shadowTransform.localScale = Vector3.Scale(transform.localScale, m_scaleMultiple);
        
        m_playerController.DoUpdate();
    }

    public void ResetPlayerVariables()
    {
        if (GameManager.Instance.m_ballInGame != null)
        {
            GameManager.Instance.m_ballInGame.GetComponent<BallManager>().m_listSmol.ForEach(Destroy);
            GameManager.Instance.m_ballInGame.GetComponent<BallManager>().m_listSmol = new List<GameObject>();
        }
        
        m_shadowTransform.SetParent(null);
        
        m_playerInteraction.transform.localScale = Vector2.one * 0.5f;
        m_playerInteraction.m_spriteRenderer = m_spriteRenderer;
        m_playerInteraction.m_timeHoldingMax = m_timeHoldingMax;
        m_playerInteraction.m_ghostBall = 0;
        m_playerController.m_speedMovement = m_speedMovement;
    }
}
