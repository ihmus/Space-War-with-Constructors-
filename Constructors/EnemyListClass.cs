public class EnemyListClass
{
    private List<Border> enemyShipBorders;

    public List<Border> EnemyShipBorders
    {
        get { return enemyShipBorders; }
        set { enemyShipBorders = value; }
    }

    public EnemyListClass()
    {
        enemyShipBorders = new List<Border>();
    }

    public void AddEnemyShip(Border border)
    {
        enemyShipBorders.Add(border);
    }
}
