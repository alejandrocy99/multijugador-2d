using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class GameManager : MonoBehaviour
{
    public void Start()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("Frog", new Vector3(-4f, -2.6f, 0f), Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate("Virtual", new Vector3(-3f, -2.6f, 0f), Quaternion.identity);
        }
    }
}
