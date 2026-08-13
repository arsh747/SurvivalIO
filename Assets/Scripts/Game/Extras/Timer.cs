using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField] private Image UiFill;
    [SerializeField] private TextMeshProUGUI UiText;

    public int Dur;
    private int remDur;
    public UnityEvent OnTimerEnd;
    public GameObject portal;
    

    private void Start()
    {
        Being(Dur);
        portal.SetActive(false);    
    }

    private void Being(int sec)
    {
        remDur = sec;
        StartCoroutine(UpdateTimer());
    }

    public IEnumerator UpdateTimer()
    {
        while(remDur >= 0)
        {
            UiText.text = $"{remDur/60:00} : {remDur % 60:00}";
            UiFill.fillAmount = Mathf.InverseLerp(0,Dur,remDur);
            remDur--;
            yield return new WaitForSeconds(1f);
        }
        OnEnd();
    }

    private void OnEnd()
    {
        OnTimerEnd.Invoke();
        print("End");

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
    
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }

        portal.SetActive(true);
    }
}
