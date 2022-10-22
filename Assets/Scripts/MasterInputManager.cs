using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterInputManager : Singleton<MasterInputManager>
{
    private List<MasterPlayerController> m_masterPlayerControllers = new List<MasterPlayerController>();
    private MasterPlayerController m_lastPlayer = null;
    public int m_id = -1;

    private void Awake()
    {
        UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(gameObject,UnityEngine.SceneManagement.SceneManager.GetSceneByBuildIndex(0));
    }
    
    public void InitPlayerAfterJoin()
    {
        m_id++;
    }

    public void Add(MasterPlayerController masterPlayerController)
    {
        UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(masterPlayerController.gameObject,UnityEngine.SceneManagement.SceneManager.GetSceneByBuildIndex(0));
    }

    protected override string GetSingletonName()
    {
        return "MasterInputManager";
    }
}
