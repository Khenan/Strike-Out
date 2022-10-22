using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;
using Component = System.ComponentModel.Component;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    //--------------------------Player----------------------------//
    [Header("Player")]
    [SerializeField, Tooltip("Position des différents joueurs au spawn")]
    private List<Transform> m_listTransform;

    [SerializeField, Tooltip("Texte Ready pout lancer le jeu")]
    public List<ReadyInGame> m_listReady;

    //--------------------------Sponsors----------------------------//
    [Header("Sponsors")] 
    [SerializeField, Tooltip("Sponsors du player 1")] private List<GameObject> m_sponsorsPlayer1;
    [SerializeField, Tooltip("Sponsors du player 2")] private List<GameObject> m_sponsorsPlayer2;
    
    private List<List<GameObject>> m_sponsors = new List<List<GameObject>>();
    
    //--------------------------Goals----------------------------//
    [Header("Goal")] 
    [SerializeField, Tooltip("Les deux Buts")]
    private List<Goal> m_goalList;

    [SerializeField, Tooltip("Particule d'explosion")]
    private VisualEffectAsset m_particleGoal;

    //--------------------------Ball----------------------------//
    [Header("Ball")] 
    [SerializeField, Tooltip("Position de spawn de la balle")] private List<Transform> m_spawnBall;
    
    [SerializeField, Tooltip("La prefab des ghost balls")] public GameObject m_prefabGhostBall;

    [SerializeField, Tooltip("Prefab de la ball")]
    private GameObject m_ballPrefab;
    
    //--------------------------Win Objects----------------------------//
    [Header("Win Objects")] 
    [SerializeField, Tooltip("Les obj qui vont disparaitre dans le menu win")] private List<Transform> m_disapearObjects = new List<Transform>();
    [SerializeField, Tooltip("Spawn des 4 particule")] private List<Transform> m_spawnParticule = new List<Transform>();
    
    [SerializeField, Tooltip("Text win")] private List<TextMeshPro> m_textWin;
    
    [SerializeField, Tooltip("Le menu lorque un des joueur gagne")] private Transform m_winMenu;
    [SerializeField, Tooltip("particule lancé lorsqu'on gagne")] private GameObject m_winParticule;

    //--------------------------Private----------------------------//
    private int m_indexSpawn;

    [HideInInspector]public GameObject m_ballInGame;

    private void Awake()
    {
        DataManager.Instance.m_masterPlayerList.ForEach(p => { p.m_playerSelecter.m_state = SelecterController.States.GAMEPLAY;});
        m_sponsors.Add(m_sponsorsPlayer1);
        m_sponsors.Add(m_sponsorsPlayer2);
    }

    public void OnPlayerJoin(MasterPlayerController player)
    {
        m_listReady[player.m_id].CancelButton();

        SetUpSponsors(player);
        
        SetUpGame(player);
    }

    private void SetUpSponsors(MasterPlayerController player)
    {
        SelecterController selecter = player.m_playerSelecter;
        GameObject first = DataManager.Instance.GetSponsor(selecter.m_firstSponsor);
        GameObject second = DataManager.Instance.GetSponsor(selecter.m_secondSponsor);

        SetUpSponsor(player, 0, first);
        SetUpSponsor(player, 1, second);
    }

    private void SetUpSponsor(MasterPlayerController player, int pos, GameObject go)
    {
        GameObject sponsor = Instantiate(go, m_sponsors[player.m_id][pos].transform);

        sponsor.GetComponent<Sponsor>().Init(player);
    }

    private void SetUpGame(MasterPlayerController player)
    {
        InitPlayerWhenSpawning(player);
        
        StartCoroutine(WaitForPlayerSpawn(player.m_playerManager.gameObject));

        m_indexSpawn++;
        if (m_indexSpawn > 1) StartCoroutine(SpawnBall());
    }

    public void StartReadyButton(int id)
    {
        m_listReady[id].StartButton();
    }

    private void InitPlayerWhenSpawning(MasterPlayerController player)
    {
        player.m_playerManager.transform.position = m_listTransform[player.m_id].position;
        player.m_playerManager.transform.rotation = m_listTransform[player.m_id].rotation;
        
        player.m_playerManager.transform.localScale = Vector2.zero;

        player.m_playerManager.m_goal = m_goalList[player.m_id];
        player.m_playerManager.InitGoalScript();
    }

    public void RespawnBall(Transform transform)
    {
        m_ballInGame.transform.position = transform.position;
        m_ballInGame.GetComponent<BallController>().ResetBall();
        //Instantiate(m_particleGoal, m_ballInGame.transform.position, Quaternion.identity);
        StartCoroutine(SpawnGameObjectToSize(m_ballInGame));
    }

    IEnumerator SpawnBall()
    {
        yield return new WaitForSeconds(2.1f);
        SoundManager.Instance.PlayReadyBall();
        m_ballInGame = Instantiate(m_ballPrefab, m_spawnBall[Random.Range(0, 2)].position, Quaternion.identity);
        StartCoroutine(SpawnGameObjectToSize(m_ballInGame));
    }

    IEnumerator WaitForPlayerSpawn(GameObject go)
    {
        yield return new WaitForSeconds(1.1f);
        StartCoroutine(SpawnGameObjectToSize(go));
    }

    /// <summary>
    /// Spawn a GameObject with a lerp between scale 0 and 1
    /// </summary>
    /// <param name="go"> the gameObject</param>
    /// <returns></returns>
    IEnumerator SpawnGameObjectToSize(GameObject go)
    {
        float time = 0;

        go.transform.localScale = Vector2.zero;
        Vector2 initScale = go.transform.localScale;

        while (time < 0.5f)
        {
            time += Time.deltaTime;
            go.transform.localScale = Vector2.Lerp(initScale, Vector2.one * 0.5f, time / 0.5f);
            yield return null;
        }

        go.transform.localScale = Vector2.one * 0.5f;
    }

    protected override string GetSingletonName()
    {
        return "GameManager";
    }

    public void Win(MasterPlayerController player)
    {
        player.m_playerManager.m_goal.ExplosionGoal();
        //Switch Controller
        SceneManager.Instance.AblePlayerInput(true);
        DataManager.Instance.m_masterPlayerList.ForEach(p => {
        {
            p.m_playerManager.transform.localScale = Vector2.zero;
            p.m_playerManager.enabled = false;
        }});
        
        //Dispay Menu Win
        m_winMenu.gameObject.SetActive(true);
        m_textWin.ForEach(p => { p.text = $"Player {Mathf.Abs(player.m_id - 1) + 1} WIN";});
        m_disapearObjects.ForEach(p=>{p.gameObject.SetActive(false);});

        SoundManager.Instance.PlayWin();
        StartCoroutine(SpawnParticuleWin());

        DataManager.Instance.m_masterPlayerList.ForEach(p =>
        {
            p.m_playerManager.ResetPlayerVariables();
            p.m_playerManager.transform.localScale = Vector2.zero;
            p.m_playerManager.m_shadowTransform.localScale = Vector2.zero;
        });
        
        Destroy(m_ballInGame);
    }

    IEnumerator SpawnParticuleWin()
    {
        yield return new WaitForSeconds(1);
        Instantiate(m_winParticule, m_spawnParticule[0]);
        yield return new WaitForSeconds(0.3f);
        Instantiate(m_winParticule, m_spawnParticule[1]);
        yield return new WaitForSeconds(0.3f);
        Instantiate(m_winParticule, m_spawnParticule[2]);
    }

    public void RestartGame(RestartButton button)
    {
        //Faire remonter le bouton
        button.Released();
        
        //Hide Menu Win
        m_winMenu.gameObject.SetActive(false);
        m_disapearObjects.ForEach(p=>{p.gameObject.SetActive(true);});
        
        DataManager.Instance.m_masterPlayerList.ForEach(p =>
        {
            p.m_playerManager.m_goal.m_index = 0;
            p.m_playerManager.m_goal.UpdateSprite();
        });
        
        //Switch Controller
        SceneManager.Instance.AblePlayerInput(false);
        DataManager.Instance.m_masterPlayerList.ForEach(p => { p.m_playerManager.enabled = true;});
        
        //Restart Game
        m_indexSpawn = 0;
        DataManager.Instance.m_masterPlayerList.ForEach(p =>
        {
            Debug.Log(p.m_id);
            SetUpSponsors(p);
            SetUpGame(p);
        });
    }
}