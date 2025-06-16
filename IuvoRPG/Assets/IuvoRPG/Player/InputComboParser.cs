using System;
using System.Collections.Generic;
using UnityEngine;

public class InputComboParser
{
    private readonly List<InputCombo> combos = new List<InputCombo>();

    public void AddCombo(InputCombo combo) => combos.Add(combo);

    public void Parse(InputBuffer buffer)
    {
        List<BufferedInput> inputs = buffer.GetBufferedInputs();

        foreach (var combo in combos)
        {
            int matchStartIndex = GetMatchStartIndex(combo, inputs);
            if (matchStartIndex >= 0)
            {
                combo.OnMatched?.Invoke();
                buffer.RemoveRange(matchStartIndex, combo.Sequence.Count);
                break;
            }
        }
    }

    private int GetMatchStartIndex(InputCombo combo, List<BufferedInput> inputs)
    {
        if (inputs.Count < combo.Sequence.Count)
            return -1;

        for (int start = inputs.Count - combo.Sequence.Count; start >= 0; start--)
        {
            bool matched = true;
            float startTime = inputs[start].Timestamp;
            float endTime = inputs[start + combo.Sequence.Count - 1].Timestamp;

            if (endTime - startTime > combo.MaxComboTime)
                continue;

            for (int i = 0; i < combo.Sequence.Count; i++)
            {
                if (inputs[start + i].ActionName != combo.Sequence[i])
                {
                    matched = false;
                    break;
                }
            }

            if (matched)
                return start;
        }

        return -1;
    }


}
