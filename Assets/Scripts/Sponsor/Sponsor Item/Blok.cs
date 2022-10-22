using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Blok : Sponsor
{
    private int m_tap = 0;
    private SpriteRenderer m_rnd;

    [SerializeField, Tooltip("Color Step")] private List<Color> m_stepColor = new List<Color>();
    
    private void OnEnable()
    {
        m_rnd = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    protected override void FeedBack()
    {
        m_spriteTransform.DOShakePosition(0.2f, 0.1f,10,30);
        
        SoundManager.Instance.PlayDestroySponsor();
        
        GameObject go = Instantiate(m_particuleBlock, transform.position, Quaternion.identity);
        Destroy(go,1f);
        Power();
    }

    public override void Power()
    {
        if (m_tap > 2) return;
        
        if (m_tap == 2)
        {
            StartCoroutine(Explosion());
            Destroy(gameObject,0.2f);
            return;
        }
        
        m_rnd.color = m_stepColor[m_tap];
        m_tap++;
    }

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(0.2f);
        Instantiate(m_particuleBlock, transform.position, Quaternion.identity);
    }

    public override void Active()
    {
        FeedBack();
    }
}
