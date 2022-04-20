using System.Collections;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class PingManager : MonoBehaviour
{
    public TMP_Text ping;
    float elapsed = 0.5f;

    private void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= 1f)
        {
            elapsed = elapsed % 1f;
            StartCoroutine(StartPing());
        }
    }

    IEnumerator StartPing()
    {
        WaitForSeconds f = new WaitForSeconds(1f);
        Ping p = new Ping("LAP002408.cns.cranfield.ac.uk");
        while (p.isDone == false)
        {
            yield return f;
        }
        ping.text = $"PING ROBOT: {p.time}";
    }
}
