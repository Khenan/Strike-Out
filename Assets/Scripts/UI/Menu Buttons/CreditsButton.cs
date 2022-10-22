using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsButton : MenuButton
{
    public override void Interact()
    {
        SceneManager.Instance.GoToScene(5);
    }
}
