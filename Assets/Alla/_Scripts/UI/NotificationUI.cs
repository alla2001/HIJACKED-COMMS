using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using TMPro;
[ExecuteAlways]
public class NotificationUI: MonoBehaviour
{

    public TextMeshPro textMeshPro;
    public float time = 1;
    private void Start()
    {
        StartCoroutine(WaitFade());
    }
    IEnumerator WaitFade()
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
    public void Update()
    {
        transform.forward = -Camera.main.transform.position + transform.position;
    }

    public void SetText(string text, Color color)
    {
        textMeshPro.text = text;
       textMeshPro.color = color;
    }
}
