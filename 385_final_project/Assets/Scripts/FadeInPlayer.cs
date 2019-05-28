using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInPlayer : MonoBehaviour
{
    private Material mat;

    void Start()
    {
        Color color = GetComponent<Renderer>().material.color;
        GetComponent<Renderer>().material.color = new Color(color.r, color.g, color.b, 0);
        StartCoroutine(FadeIn());

    }

    private IEnumerator FadeIn()
    {
        float counter = 0;
        Color color = GetComponent<Renderer>().material.color;

        while(counter < 6f)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, counter / 6f);
            GetComponent<Renderer>().material.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }
    }
}
