using Microsoft.Maui.Controls;
using System;

namespace SpaceWar
{
    public static class BulletManagerFactory
    {
        private static AbsoluteLayout absoluteLayout;
        private static string bulletPath;
        private static EnemyListClass enemyListClass;

        public static void Initialize(AbsoluteLayout layout, string path, EnemyListClass enemies)
        {
            absoluteLayout = layout ?? throw new ArgumentNullException(nameof(layout));
            bulletPath = path ?? throw new ArgumentNullException(nameof(path));
            enemyListClass = enemies ?? throw new ArgumentNullException(nameof(enemies));
        }

        public static BulletManager CreateBulletManager()
        {
            if (absoluteLayout == null || bulletPath == null)
                throw new InvalidOperationException("Factory is not initialized.");

            return new BulletManager(absoluteLayout, bulletPath, enemyListClass);
        }
    }
}
