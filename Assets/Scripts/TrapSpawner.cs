using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpawner : MonoBehaviour
{
    #region Singleton class : TrapSpawner

    public static TrapSpawner trapSpawner;

    void Awake()
    {
        if (trapSpawner == null)
        {
            trapSpawner = this;
        }
    }
    #endregion

    public GameObject Trap_pref;

    public ArrayList traps = new ArrayList();
    public List<Vector3> trapPos = new List<Vector3>();

    public Vector3[] pos;
    public void SpawnTrapsOnMap(GameObject map)
    {
        //находим объекты у которых есть нужный нам тег "Trap". 
        for (int i = 0; i < map.transform.childCount; i++)
        {
            if (map.transform.GetChild(i).tag == "Trap")
            {
                trapPos.Add(map.transform.GetChild(i).transform.position);
            }
        }

        for (int i = 0; i < trapPos.Count; i++)
        {
            int value = Random.Range(0, 2);
            if (value == 1)
            {
                Instantiate(Trap_pref, trapPos[i], Quaternion.identity);
            }
        }
        trapPos.Clear();
    }
}