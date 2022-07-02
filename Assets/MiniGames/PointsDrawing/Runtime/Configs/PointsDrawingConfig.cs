using Client.Cofigs;

using MiniGames.PointsDrawing.Views;

using UnityEngine;

namespace MiniGames.PointsDrawing.Configs
{
    [CreateAssetMenu(menuName = "MiniGames/PointsDrawingConfig", order = 0)]
    public class PointsDrawingConfig : BaseMiniGameConfig
    {
        [field: SerializeField] public Iteration[] Iterations { get; private set; }
        [field: SerializeField] public DotView DotView { get; private set; }
        [field: SerializeField] public ItemView[] ItemViews { get; private set; }
        [field: SerializeField] public LineView LineView { get; private set; }
    }
}