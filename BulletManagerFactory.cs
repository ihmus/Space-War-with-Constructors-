using Microsoft.Maui.Controls;
using System;

namespace SpaceWar
{
    public static class BulletManagerFactory
    {
        private static AbsoluteLayout absoluteLayout;
        private static string bulletPath;
        private static EnemyListClass enemyListClass;
        private static GeneralConstructors generalConstructors;
        private static StarMoving star;
        public static void Initialize(AbsoluteLayout layout, string path, EnemyListClass enemies, GeneralConstructors generalConstructorsParam,StarMoving st)
        {
            absoluteLayout = layout ?? throw new ArgumentNullException(nameof(layout));
            bulletPath = path ?? throw new ArgumentNullException(nameof(path));
            enemyListClass = enemies ?? throw new ArgumentNullException(nameof(enemies));
            generalConstructors = generalConstructorsParam ?? throw new ArgumentNullException(nameof(generalConstructorsParam));
            star = st ?? throw new ArgumentNullException(nameof(st));
        }

        public static BulletManager CreateBulletManager()
        {
            if (absoluteLayout == null || bulletPath == null)
                throw new InvalidOperationException("Factory is not initialized.");

            return new BulletManager(absoluteLayout, bulletPath, enemyListClass,generalConstructors,star);
        }
    }
}
