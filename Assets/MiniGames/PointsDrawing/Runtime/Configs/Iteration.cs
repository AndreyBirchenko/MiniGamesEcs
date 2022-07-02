using System;

using UnityEngine;

namespace MiniGames.PointsDrawing.Configs
{
    [Serializable]
    public class Iteration
    {
        [field: SerializeField] public int StartingNumber { get; private set; }
    }
}