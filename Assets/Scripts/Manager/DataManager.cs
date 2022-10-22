using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public List<MasterPlayerController> m_masterPlayerList = new List<MasterPlayerController>();
    public List<GameObject> m_listAllSponsor = new List<GameObject>();
    
    public void Add(MasterPlayerController player)
    {
        m_masterPlayerList.Add(player);
    }

    public GameObject GetSponsor(int id)
    {
        return m_listAllSponsor[id];
    }
    
    protected override string GetSingletonName()
    {
        return "DataManager";
    }
}
