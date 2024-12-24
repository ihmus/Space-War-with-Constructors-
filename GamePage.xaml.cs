using Microsoft.Maui.Layouts;
using System.Diagnostics;

namespace SpaceWar;

public partial class GamePage : ContentPage
{
    private int starnumbers = 135;

    private int myhealth = 100;
    private string MyShipPath = "spaceship.png", BulletPath = "bullet.png";
    private int score = 0;
    private double speed = Math.Sqrt(0.0001);
    private StarMoving stars;
    private MyShipClass myShipClass;
    MyShipBorders myshipborder=new MyShipBorders();
    PositionManager positionManager = new PositionManager();
    EnemyListClass enemyListClass = new EnemyListClass();
    private List<String> enemeyshiplist = new List<String>
        {
            "enemyship.png",
            "enemyship2.png",
        };
    public GamePage()
    {
        InitializeComponent();
        GeneralConstructors generalconstructors = new GeneralConstructors(myhealth, score, HealthLabel, ScoreLabel);
        EnemyManagerFactory.Initialize(absoluteLayout, enemeyshiplist, speed, positionManager, enemyListClass, generalconstructors);
        stars = new StarMoving(absoluteLayout, starnumbers, generalconstructors); // 100 yýldýz ekleme
        BulletManagerFactory.Initialize(absoluteLayout, BulletPath, enemyListClass, generalconstructors, stars);
        myShipClass = new MyShipClass(absoluteLayout, MyShipPath, stars, positionManager,enemyListClass, myshipborder, generalconstructors); // Example usage
        myShipClass.MyShip(0.5, 0.5);
        this.Disappearing += OnDisappearing;
    }

    private void OnDisappearing(object sender, EventArgs e)
    {
        stars.Stop();
    }
}
