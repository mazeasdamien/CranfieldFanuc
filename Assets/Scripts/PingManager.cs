using System.Collections;
using TMPro;
using UnityEngine;

public class PingManager : MonoBehaviour
{
    public string ip;
    public TMP_Text ping;
    public TMP_InputField _InputField;

    float elapsed = 0f;

    private void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= 1f)
        {
            elapsed = elapsed % 1f;
            StartCoroutine(StartPing(ip));
        }
    }

    IEnumerator StartPing(string ip)
    {
        WaitForSeconds f = new WaitForSeconds(1f);
        Ping p = new Ping(_InputField.text);
        while (p.isDone == false)
        {
            yield return f;
        }
        ping.text = $"PING ROBOT: {p.time}";
    }
}
