using System.Collections.Generic;

public class GameToSponsorButton : MenuButton
{
    public override void Interact()
    {
        DataManager.Instance.m_masterPlayerList.ForEach(p=>{DestroyImmediate(p.gameObject);});
        DataManager.Instance.m_masterPlayerList = new List<MasterPlayerController>();
        
        DestroyImmediate(MasterInputManager.Instance.gameObject);

        SceneManager.Instance.GoToScene(3);
    }
}