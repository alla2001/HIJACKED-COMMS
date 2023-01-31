using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleHilighter : SingletonMonoBehaviour<CircleHilighter>
{
    public Material circleMaterial;
    private void Start()
    {
       GameManager.startPlaying += () => { ClearHilight(); };
    }
    public void HilightCircle(Vector3 pos, float radius)
    {
        circleMaterial.SetVector("_Center",pos);

        circleMaterial.SetFloat("_Radius",radius);


    }
    public void ClearHilight()
    {
        circleMaterial.SetFloat("_Radius", 0);
    }
}
