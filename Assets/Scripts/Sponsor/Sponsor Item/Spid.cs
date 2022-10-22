public class Spid : Sponsor
{
    public override void Power()
    {
        m_player.m_playerManager.m_playerController.m_speedMovement += 0.1f;
    }

    public override void Active()
    {
        FeedBack();
    }
}
