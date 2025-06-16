using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class Stat : SemiBehavior
{
    [Header("Stat")]
    [SerializeField] protected string _statName;
    [SerializeField] protected int _levelValue;
    [SerializeField] protected float statLevelRate = 0.2f;

    public Stat()
    {
        _statName = string.Empty;
        _levelValue = 0;

    }

    public IEnumerator LerpStatValue(int currentStatValue, int newStatValue, int minStatValue, int maxStatValue, float statLerpSpeed)
    {
        float time = 0.0f;

        while (time < statLerpSpeed)
        {
            currentStatValue = Mathf.Clamp((int)Mathf.Lerp(currentStatValue, newStatValue, statLerpSpeed * Time.deltaTime), minStatValue, maxStatValue);

            time += Time.deltaTime;
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
    }

    #region Getters & Setters

    public int GetLevel() => _levelValue;
    public string GetName() => _statName;

    protected void SetLevel(int newLevel) => _levelValue = newLevel;
    protected void SetName(string newName) => _statName = newName;

    #endregion
}