using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiatePrefab : CharacterAction
{
    public GameObject prefab;
    public Transform spawnPos;

    protected override void Perform(int context = 0)
    {
        Instantiate(prefab, spawnPos);

        End();
    }
}
