using System.Collections.Generic;

using MiniGames.PointsDrawing.Views;

namespace MiniGames.PointsDrawing.Services
{
    public class IterationRepository
    {
        public List<DotView> DotViews = new List<DotView>(10);
        public List<ItemView> ItemViews = new List<ItemView>(5);
        public ItemView CurrentItemView;

        public DotView GetStartPoint()
        {
            var dotView = DotViews[0];
            
            dotView.IsCorrectForStart = true;

            return dotView;
        }
    }
}