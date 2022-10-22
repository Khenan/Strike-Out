using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SponsorMenu : MonoBehaviour
{
    [SerializeField, Tooltip("Line 1")] private List<SponsorButton> m_line1List = new List<SponsorButton>();
    [SerializeField, Tooltip("Line 1")] private List<SponsorButton> m_line2List = new List<SponsorButton>();
    
    public static List<List<SponsorButton>> m_sponsorButtonList = new List<List<SponsorButton>>();

    private void OnEnable()
    {
        m_sponsorButtonList.Add(m_line1List);
        m_sponsorButtonList.Add(m_line2List);
    }
    private void OnDisable()
    {
        m_sponsorButtonList = new List<List<SponsorButton>>();
    }
}
