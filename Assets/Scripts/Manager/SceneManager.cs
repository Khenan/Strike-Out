using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneManager : Singleton<SceneManager>
{
    private string m_nameCurrentScene = null;
    [HideInInspector]public int m_idCurrentScene = 0;

    // Master Input Manager
    [SerializeField, Tooltip("Master Input Manager")]
    private GameObject m_masterInputManager;
    
    // Animator de la Transition
    [SerializeField, Tooltip("BlackTransition in the SceneManager's Canvas")]
    private Animator m_blackTransitionAnimator = null;

    private int m_openAnimation = Animator.StringToHash("Open");
    private int m_closeAnimation = Animator.StringToHash("Close");

    private int m_popAnimation = Animator.StringToHash("Pop");
    private int m_depopAnimation = Animator.StringToHash("Depop");

    private bool m_onChange = false;

    private bool m_canPlay = true;

    private PlayerInput m_playerInput;
    public bool CanPlay => m_canPlay;

    private void Awake()
    {
        m_playerInput = GetComponent<PlayerInput>();
        m_nameCurrentScene = UnityEngine.SceneManagement.SceneManager.GetSceneByBuildIndex(1).name;
        m_idCurrentScene = 1;
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
    }

    /// <summary>
    /// Change game scene to an other scene from a build index (LoadSceneMode.Additive).
    /// </summary>
    /// <param name="buildIndex">Build index as shown in the Build Settings window.</param>
    public void GoToScene(int buildIndex)
    {
        if (m_onChange) return;
        m_onChange = true;
        if (m_nameCurrentScene != null)
        {
            Debug.Log(m_nameCurrentScene);
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(m_nameCurrentScene);
        }

        StartCoroutine(UnloadAsync(buildIndex));
    }

    private void StartTransition()
    {
        m_blackTransitionAnimator.SetTrigger(m_closeAnimation);
    }

    private void EndTransition()
    {
        m_blackTransitionAnimator.SetTrigger(m_openAnimation);

        m_onChange = false;
        StartCoroutine(PlayAccess());
    }

    IEnumerator PlayAccess()
    {
        yield return new WaitForSeconds(GetTimeCurrentAnim() * 1.5f);
        m_canPlay = true;

        //Activer le masterInputManager dans la scene sponsor
        if (m_idCurrentScene == 3)
        {
            GameObject go = Instantiate(m_masterInputManager);
            UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(go,UnityEngine.SceneManagement.SceneManager.GetSceneByBuildIndex(0));
        }
    }

    IEnumerator UnloadAsync(int buildIndex)
    {
        // On fait la transition
        StartTransition();

        // On décharge la scène
        if (m_idCurrentScene > 0)
        {
            yield return new WaitForSeconds(GetTimeCurrentAnim());

            // Le joueur ne peut plus appuyer sur les inputs
            m_canPlay = false;

            UnityEngine.SceneManagement.SceneManager.SetActiveScene(UnityEngine.SceneManagement.SceneManager
                .GetSceneByBuildIndex(0));

            AsyncOperation asynOp = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(m_idCurrentScene);
            // On attend que le déchargement soit fait
            while (!asynOp.isDone)
            {
                yield return null;
            }
        }

        m_nameCurrentScene = UnityEngine.SceneManagement.SceneManager.GetSceneByBuildIndex(buildIndex).name;
        m_idCurrentScene = buildIndex;

        // Desactiver le player input si scene sponsor
        if (buildIndex == 3)
        {
            AblePlayerInput(false);
        }

        // On charge la scène
        StartCoroutine(LoadAsync(buildIndex));
    }

    IEnumerator LoadAsync(int buildIndex)
    {
        // On charge la scène
        AsyncOperation asynOp =
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);

        // On attend que le chargement soit fait
        while (!asynOp.isDone)
        {
            yield return null;
        }
        UnityEngine.SceneManagement.SceneManager.SetActiveScene(
            UnityEngine.SceneManagement.SceneManager.GetSceneByBuildIndex(buildIndex));
        // On fait la transition
        EndTransition();
    }

    private float GetTimeCurrentAnim()
    {
        float seconds = 1f;
        foreach (var animatorClipInfo in m_blackTransitionAnimator.GetCurrentAnimatorClipInfo(0))
        {
            seconds = animatorClipInfo.clip.length;
        }

        return seconds;
    }

    protected override string GetSingletonName()
    {
        return "SceneManager";
    }

    public void AblePlayerInput(bool value)
    {
        m_playerInput.enabled = value;
    }

    public PlayerInput GetPlayerInput()
    {
        return m_playerInput;
    }
}