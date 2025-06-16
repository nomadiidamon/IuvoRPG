using System;
using System.Collections.Generic;

[Serializable]
public class InputCombo
{
    public string Name;
    public List<string> Sequence;
    public float MaxComboTime = 1.0f;
    public FlexibleEvent OnMatched = new FlexibleEvent();

    public InputCombo(string name, List<string> sequence, float maxComboTime, Action onMatched)
    {
        Name = name;
        Sequence = sequence;
        MaxComboTime = maxComboTime;
        OnMatched.AddListener(onMatched);
    }
}
