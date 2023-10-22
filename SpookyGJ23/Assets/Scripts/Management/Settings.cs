using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    [SerializeField] int targetFramerate = 60;

    void Awake()
    {
        Application.targetFrameRate = targetFramerate;            
    }
}