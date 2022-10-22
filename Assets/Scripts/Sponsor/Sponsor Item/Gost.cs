
public class Gost : Sponsor
{
    public override void Power()
    {
        m_player.m_playerManager.m_playerInteraction.AddGhostBall();
    }

    public override void Active()
    {
        FeedBack();
    }
}