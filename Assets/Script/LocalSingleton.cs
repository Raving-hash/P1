using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalSingleton : MonoBehaviour
{
    public NetworkUser localUser;
    public GameObject playerPrefab;
    public PlayerRepo localRepo = new PlayerRepo();
    public uint localFrameID = 0;
    public void InitAllPlayer(Hashtable history)
    {
        foreach (DictionaryEntry user_entry in history)
        {
            uint netID = (uint)user_entry.Key;
            Hashtable user_ht = (Hashtable)user_entry.Value;
            foreach (DictionaryEntry player_entry in user_ht)
            {
                string deviceID = (string)player_entry.Key;
                List<FrameOperation> li = (List<FrameOperation>)player_entry.Value;
                var pd = localRepo.RegisterPlayer(playerPrefab, netID, deviceID);
                pd.operationArray = li;
                localRepo.localOperationBuffer.AddRange(li);
            }
        }
        localRepo.localOperationBuffer.Sort((x, y) => { return (int)x.frameID - (int)y.frameID; });
    }
}
