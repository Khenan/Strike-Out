using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButton : MenuButton
{
    public override void Interact()
    {
        SceneManager.Instance.GoToScene(3);
    }
}
