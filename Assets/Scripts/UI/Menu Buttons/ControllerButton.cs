public class ControllerButton : MenuButton
{
    public override void Interact()
    {
        SceneManager.Instance.GoToScene(6);
    }
}
