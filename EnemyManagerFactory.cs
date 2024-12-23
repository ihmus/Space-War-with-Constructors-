using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SpaceWar
{
    public static class EnemyManagerFactory
    {
        private static AbsoluteLayout absoluteLayout;
        private static List<string> enemyShipList;
        private static double speed;
        private static PositionManager positionManager;
        private static EnemyListClass enemyListClass;
        public static void Initialize(AbsoluteLayout layout, List<string> ships, double initialSpeed,PositionManager pm,EnemyListClass e)
        {
            absoluteLayout = layout ?? throw new ArgumentNullException(nameof(layout));
            enemyShipList = ships ?? throw new ArgumentNullException(nameof(ships));
            speed = initialSpeed;
            positionManager = pm;
            enemyListClass = e;
        }

        public static EnemyManager CreateEnemyManager()
        {
            if (absoluteLayout == null || enemyShipList == null)
                throw new InvalidOperationException("Factory is not initialized.");

            return new EnemyManager(absoluteLayout, enemyShipList, speed,positionManager,enemyListClass);
        }
    }
}
