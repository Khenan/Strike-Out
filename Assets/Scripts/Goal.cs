using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Goal : MonoBehaviour
{
    
    //--------------------------Color----------------------------//
    [Header("Color")]
    [SerializeField, Tooltip("3 couleurs pour le changement d'état")] private List<Color> m_listColor = new List<Color>();
    
    //--------------------------Component----------------------------//
    [Header("Component")]
    [SerializeField, Tooltip("Sprite Renderer")] private SpriteRenderer m_spreiteRenderer;
    
    //--------------------------Spawn Ball----------------------------//
    [Header("Spwan Ball")]
    [SerializeField, Tooltip("l'endroit ou la balle spawn quand elle arrive dans ce goal")] private Transform m_spawnEnnemie;
    
    //--------------------------FeedBack----------------------------//
    [SerializeField, Tooltip("L'expolison du goal quand on marque")] public GameObject m_explosion;
    
    //--------------------------Public Hide----------------------------//
    [HideInInspector] public PlayerManager m_playerManager;
    
    //--------------------------Private----------------------------//
    [HideInInspector]public int m_index;

    private void Awake()
    {
        UpdateSprite();
    }

    public void UpdateSprite()
    {
        m_spreiteRenderer.color = m_listColor[m_index];
        m_index++;
    }

    private void AddScore()
    {
        SoundManager.Instance.PlayGoalExplosion();
        GameObject go = ExplosionGoal();
        Destroy(go,1f);
        UpdateSprite();
        GameManager.Instance.RespawnBall(m_spawnEnnemie);
        
    }

    public GameObject ExplosionGoal()
    {
        return Instantiate(m_explosion, transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (m_index >= m_listColor.Count)
        {
            GameManager.Instance.Win(m_playerManager.m_masterPlayerController);
            return;
        }
        col.transform.localScale = Vector2.zero;
        AddScore();
    }
}
