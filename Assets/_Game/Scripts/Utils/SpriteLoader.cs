using UnityEngine;

namespace _Game.Scripts.Utils
{
    public static class SpriteLoader
    {
        public static Sprite LoadOlderSprite(int level)
        {
            int lvl = level + 1;
            return Resources.Load<Sprite>("Sprites/Character/p" + lvl);
        }

        public static Sprite LoadDaySprite(int level)
        {
            return Resources.Load<Sprite>("Sprites/Time/t" + level);
        }
    }
}