using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleManager : MonoBehaviour
{
    public void Fire()
    {
        foreach (Muzzle mzl in GetComponentsInChildren<Muzzle>())
        {
            mzl.gameObject.SetActive(true);
        }
    }
}
