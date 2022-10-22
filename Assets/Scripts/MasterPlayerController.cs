using UnityEngine;

public class MasterPlayerController : MonoBehaviour
{
    [SerializeField, Tooltip("Player Object, for the gameplay scene")] private GameObject m_playerGameplay;
    
    [SerializeField, Tooltip("Player Object, for the sponsor scene")] private GameObject m_playerSponsor;

    public int m_id = 0;
    public PlayerManager m_playerManager;
    public SelecterController m_playerSelecter;

    private void Awake()
    {
        transform.position = Vector2.zero;
        InitBothPlayer();
        MasterInputManager.Instance.Add(this);
        
        m_id = MasterInputManager.Instance.m_id;
        gameObject.name = $"Player #{m_id}";
        
        //ActiveSponsorPlayer();
        DataManager.Instance.Add(this);
        SponsorManager.Instance.Join(this);
    }

    private void InitBothPlayer()
    {
        m_playerManager = m_playerGameplay.GetComponent<PlayerManager>();
        m_playerSelecter = m_playerSponsor.GetComponent<SelecterController>();
        m_playerSelecter.m_playerManager = m_playerManager;
        m_playerSelecter.m_masterPlayerController = this;
        m_playerManager.m_masterPlayerController = this;
    }

    public void ActiveGameplayPlayer()
    {
        m_playerSponsor.SetActive(false);
        m_playerGameplay.SetActive(true);
    }

    public void ActiveSponsorPlayer()
    {
        m_playerSponsor.SetActive(true);
        m_playerGameplay.SetActive(false);
    }
}
