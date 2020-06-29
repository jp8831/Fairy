using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Fairy : MonoBehaviour, ISelectable
{
    public void OnDeselect ()
    {
        NavMeshHit navMeshHit;

        if (NavMesh.SamplePosition (transform.position, out navMeshHit, 5.0f, NavMesh.AllAreas))
        {
            transform.position = navMeshHit.position;
        }
    }

    public void OnSelect ()
    {
    }
}
