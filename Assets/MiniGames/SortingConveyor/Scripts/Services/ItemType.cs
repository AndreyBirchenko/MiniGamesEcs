using System;

namespace MiniGames.SortingConveyor.Services
{
    [Flags]
    public enum ItemType
    {
        None = 1 << 1,
        Triangle = 1 << 2,
        Square = 1 << 3,
        Circle = 1 << 4,
    }
}