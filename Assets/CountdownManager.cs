using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountdownManager : MonoBehaviour
{
    public GameObject three;
    public GameObject two;
    public GameObject one;
    public GameObject start;

    private void Start()
    {
        three.SetActive(false);
        two.SetActive(false);
        one.SetActive(false);
        start.SetActive(false);
    }

    public void StartCountdown()
    {
        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        three.SetActive(true);
        yield return new WaitForSeconds(1);
        three.SetActive(false);
        two.SetActive(true);
        yield return new WaitForSeconds(1);
        two.SetActive(false);
        one.SetActive(true);
        yield return new WaitForSeconds(1);
        one.SetActive(false);
        start.SetActive(true);
        yield return new WaitForSeconds(1);
        start.SetActive(false);
    }
}
