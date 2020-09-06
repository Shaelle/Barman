using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miss : MonoBehaviour
{

    [SerializeField] LevelManager levelManager;

    private void OnTriggerExit(Collider other)
    {
        levelManager.Miss();
    }
}
