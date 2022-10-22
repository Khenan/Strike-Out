using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Sponsor : MonoBehaviour
{
    public MasterPlayerController m_player;
    [FormerlySerializedAs("m_sprite")] public Transform m_spriteTransform;
    [SerializeField, Tooltip("Le particule system correspondant au sponsor")] public GameObject m_particuleBlock;
    private bool m_isActive;

    public void Init(MasterPlayerController player)
    {
        m_player = player;
    }

    protected virtual void FeedBack()
    {
        if (m_isActive) return;
        m_isActive = true;

        StartCoroutine(ChasePlayer());
    }

    IEnumerator ChasePlayer()
    {
        float time = 0;
        Vector3 initPosition = m_spriteTransform.position;
        Vector3 initScale = m_spriteTransform.localScale;
        
        while (Vector3.Distance(m_player.m_playerManager.transform.position,m_spriteTransform.position) > 0.1f)
        {
            m_spriteTransform.position = Vector3.Lerp(initPosition, m_player.m_playerManager.transform.position, time*2);
            m_spriteTransform.localScale = Vector3.Lerp(initScale, Vector3.zero, time);
            time += Time.deltaTime;
            yield return null;
        }
        
        Destroy(m_spriteTransform.gameObject);
        
        m_player.m_playerManager.transform.DOShakeScale(0.2f, 0.2f, 2, 2);
        Power();
        
        SoundManager.Instance.PlayGetSponsor();
        SoundManager.Instance.PlayDestroySponsor();
        
        // Feed backs
        GameObject go = Instantiate(m_particuleBlock, transform.position, Quaternion.identity);
        Destroy(go,1f);
        Destroy(gameObject);
    }
    
    public abstract void Power();
    public abstract void Active();
}