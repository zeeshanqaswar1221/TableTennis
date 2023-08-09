using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayPing : NetworkBehaviour
{
    TextMeshProUGUI text;

    public override void Spawned()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public override void FixedUpdateNetwork()
    {
        if (Runner.IsForward && Runner.Simulation.Tick % 100 == 0)
            text.text = $"Ping: {1000 * Runner.GetPlayerRtt(Runner.LocalPlayer):N0}ms     Type {Runner.CurrentConnectionType}";
    }
}
