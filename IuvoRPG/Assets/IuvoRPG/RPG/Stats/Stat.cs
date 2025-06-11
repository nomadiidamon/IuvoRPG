using IuvoUnity._BaseClasses._RPG;
using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class Stat : MonoBehaviour
{
    [SerializeField] private string _statName;
    [SerializeField] private int _levelValue;
    [SerializeField] private float statLevelRate = 0.2f;


    public IEnumerator LerpStatValue(int currentStatValue, int newStatValue, int minStatValue, int maxStatValue, float statLerpSpeed)
    {
        currentStatValue = Mathf.Clamp((int)Mathf.Lerp(currentStatValue, newStatValue, statLerpSpeed * Time.deltaTime), minStatValue, maxStatValue);

        CoroutineManager.Instance.waitForSecondsDictionary.TryGetValue(statLerpSpeed, out var result);
        if (result != null)
        {
            yield return result;

        }
        else
        {
            var speed = new WaitForSeconds(statLerpSpeed);
            CoroutineManager.Instance.waitForSecondsDictionary.Add(statLerpSpeed, speed);
            yield return speed;
        }
    }

    #region Getters & Setters

    public int GetLevel() => _levelValue;
    public string GetName() => _statName;

    protected void SetLevel(int newLevel) => _levelValue = newLevel;
    protected void SetName(string newName) => _statName = newName;

    #endregion
}