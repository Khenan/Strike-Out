using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MenuButton
{
    public override void Interact() { Application.Quit(); }
}
