using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    //---------------------------Player Input---------------------------//
    public PlayerInput m_controls;
    public float m_timeHoldingMax = 0.5f;

    //---------------------------Layer---------------------------//
    public LayerMask m_layerBall;
    
    //---------------------------Component---------------------------//
    public BoxCollider2D m_collider;
    public SpriteRenderer m_spriteRenderer;

    //---------------------------Private---------------------------//
    private BallController m_currentBall;

    private Coroutine m_coroutineHold;
    
    private float m_timePressed;
    
    private float m_timeHolding;
    
    private bool m_hasCatched;
    
    private Coroutine m_coroutineTrans;
    
    private InputAction.CallbackContext m_emptyCallback;
    [HideInInspector]public int m_ghostBall;


    public void InitInputAction()
    {
        m_ghostBall = 0;
        m_emptyCallback = new InputAction.CallbackContext();
        m_controls.currentActionMap["Interact"].started += StartPropulse;
        m_controls.currentActionMap["Interact"].canceled += PropulseBall;
    }

    private void OnDisable()
    {
        if (m_controls == null || m_controls.currentActionMap == null) return;
        m_controls.currentActionMap["Interact"].started -= StartPropulse;
        m_controls.currentActionMap["Interact"].canceled -= PropulseBall;
    }

    public bool HasCatched()
    {
        return m_hasCatched;
    }

    /// <summary>
    /// Check if the player can propulse a ball
    /// </summary>
    private void StartPropulse(InputAction.CallbackContext context)
    {
        if (m_currentBall == null) return;
        SoundManager.Instance.PlayCatchBall();
        m_hasCatched = true;
        m_currentBall.m_isCatched = true;
        
        m_currentBall.StopBall();
        m_currentBall.transform.SetParent(transform);

        StartBallCoroutines();
    }

    private void StartBallCoroutines()
    {
        SoundManager.Instance.PlayChargeBall();
        
        //Transition de la ball vers le point de lancement
        if(m_coroutineTrans == null) m_coroutineTrans = StartCoroutine(TransBallInFront());
        
        //Faire lacher la balle si on la garde trop longtemps
        if(m_coroutineHold == null) m_coroutineHold = StartCoroutine(PropulseIfHoldIsToLong());
    }

    IEnumerator TransBallInFront()
    {
        float time = 0;
        while (time < 0.2f)
        {
            time += Time.deltaTime;
            m_currentBall.transform.position = Vector2.Lerp(m_currentBall.transform.position, transform.position + transform.up , time * 5);
            yield return null;
        }
    }

    IEnumerator PropulseIfHoldIsToLong()
    {
        float time = 0;
        while (time < m_timeHoldingMax)
        {
            time += Time.deltaTime;
            m_spriteRenderer.size += Vector2.one * Time.deltaTime / m_timeHoldingMax;
            yield return null;
        }
        
        if (m_hasCatched) PropulseBall(m_emptyCallback);
    }
    
    /// <summary>
    /// Propulse the ball after released input
    /// </summary>
    private void PropulseBall(InputAction.CallbackContext context)
    {
        if (!m_hasCatched) return;
        m_hasCatched = false;
        
        SoundManager.Instance.StopChargeBall();
        SoundManager.Instance.PlayPropulseBall();
        
        m_spriteRenderer.size = Vector2.zero;
        
        StopBallCoroutines();
        
        m_currentBall.m_isCatched = false;

        for (int i = 0; i < m_ghostBall; i++)
        {
            GameObject go = Instantiate(GameManager.Instance.m_prefabGhostBall ,GameManager.Instance.m_ballInGame.transform.position, Quaternion.identity);
            go.GetComponent<Rigidbody2D>().velocity = -transform.up;
        }
        
        m_currentBall.transform.SetParent(null);
        m_currentBall.Propulse(transform.up);
        
        m_currentBall = null;
    }

    private void StopBallCoroutines()
    {
        if(m_coroutineHold != null) StopCoroutine(m_coroutineHold);
        if(m_coroutineTrans != null) StopCoroutine(m_coroutineTrans);
        
        m_coroutineHold = null;
        m_coroutineTrans = null;
    }
    
    /// <summary>
    /// Verify if the Game object is in the layer Ball
    /// </summary>
    /// <param name="go"> The gameObject verify </param>
    /// <returns></returns>
    private bool VerifyIfBall(GameObject go)
    {
        //Si c'est un autre balle qui arrive
        if (m_currentBall != null && go == m_currentBall.gameObject) return false;

        //Si c'est un obj dans le layer balle
        return (m_layerBall.value & (1 << go.layer)) > 0;
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        //Debug.Log("verif si ball"+!VerifyIfBall(col.gameObject));
        if (!VerifyIfBall(col.gameObject)) return;

        m_currentBall = col.gameObject.GetComponent<BallController>().Able();
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (m_hasCatched) return;
        m_currentBall = null;
    }

    public void AddGhostBall()
    {
        m_ghostBall++;
    }
}