
public class Omega : Sponsor
{
    public override void Power()
    {
        GameManager.Instance.m_ballInGame.GetComponent<BallManager>().ActiveOmega(m_player.m_id);
    }

    public override void Active()
    {
        FeedBack();
    }
}
