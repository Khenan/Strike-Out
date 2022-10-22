using DG.Tweening;
using UnityEngine;

public class Bogy : Sponsor
{
    public override void Power()
    {
        m_player.m_playerManager.m_playerController.m_speedMovement -= 0.06f;
        m_player.m_playerManager.transform.localScale += Vector3.one * 0.15f;
    }
    
    public override void Active()
    {
        FeedBack();
    }
}
