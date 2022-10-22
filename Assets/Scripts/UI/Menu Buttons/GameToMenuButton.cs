using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameToMenuButton : MenuButton
{
    public override void Interact()
    {
        DataManager.Instance.m_masterPlayerList.ForEach(p=>{DestroyImmediate(p.gameObject);});
        DataManager.Instance.m_masterPlayerList = new List<MasterPlayerController>();
        
        DestroyImmediate(MasterInputManager.Instance.gameObject);

        SceneManager.Instance.GoToScene(2);
    }
}
