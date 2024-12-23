using Microsoft.Maui.Layouts;
using System.Diagnostics;

namespace SpaceWar;

public partial class GamePage : ContentPage
{
    private int starnumbers = 135;

    private Random random = new Random();
    private int myhealth = 100;
    double _xOffset, _yOffset;
    double _startX, _startY;
    private Random _random = new Random();
    private const int CreateInterval = 200, MovingTime = 1000, callingtime = 3000; // Milisaniye cinsinden oluþturma aralýðý
    private double _currentX, _currentY;
    private double MyShipX = 0.5, MyShipY = 0.5;
    private string MyShipPath = "spaceship.png", BulletPath = "bullet.png";
    private Border MyShipBorder, EnemyShipBorder;
    private MyShipBorders myShipBorders;
    private string EnemyShip = "enemyship.png";
    private List<Border> enemyShipBorders = new List<Border>();
    private int score = 0;
    private double speed = Math.Sqrt(0.0001);
    private bool isrunnig = false;
    private int runcounter = 0;
    private StarMoving stars;
    private MyShipClass myShipClass;
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
        EnemyManagerFactory.Initialize(absoluteLayout, enemeyshiplist, speed, positionManager, enemyListClass);
        BulletManagerFactory.Initialize(absoluteLayout, BulletPath, enemyListClass);
        stars = new StarMoving(absoluteLayout, starnumbers); // 100 yýldýz ekleme
        myShipClass = new MyShipClass(absoluteLayout, MyShipPath, stars, positionManager,enemyListClass); // Example usage
        myShipClass.MyShip(0.5, 0.5);
        //fired();
        //enemyfired();

        // Sayfa kapanýrken çalýþacak olan Disappearing olayýna abone ol
        this.Disappearing += OnDisappearing;
    }

    private void OnDisappearing(object sender, EventArgs e)
    {
        // Ýþlemleri sonlandýrma kodunu buraya ekleyin
        // Örneðin, animasyonlarý durdurabilir veya veri akýþlarýný sonlandýrabilirsiniz.
        stars.Stop();
        // Diðer temizleme iþlemleri
    }
}

    /*
    private async void enemyfired()
    {
        double layoutWidth = absoluteLayout.Width; double layoutHeight = absoluteLayout.Height;
        if (!enemyShipBorders.Any()) // Liste boþsa {
            Console.WriteLine("Enemy ship borders list is empty.");// Döngüyü sonlandýr
        while (isrunnig)
        {
            double i = 0.7;
            foreach (var enemyShipBorder in enemyShipBorders.ToList())  // ToList() kullanarak güvenli iterasyon saðlanýr
            {
                double Y = AbsoluteLayout.GetLayoutBounds(enemyShipBorder).Y;
                double X = AbsoluteLayout.GetLayoutBounds(enemyShipBorder).Location.X;
                double right = X + (0.00003 * layoutWidth);
                double left = X - (0.00003 * layoutWidth);
                Debug.WriteLine($"X: {X}, Right: {right}, Left: {left}"); // Konumlarý kontrol etmek için
                if (score <= 3)
                {
                    var bulletManager = BulletManagerFactory.CreateBulletManager(); // Use the bulletManager to create bullets
                    bulletManager.CreateNewBullet(X, Y + 0.08, false, MovingTime);
                }
                else if (score <= 8)
                {
                    var bulletManager = BulletManagerFactory.CreateBulletManager(); // Use the bulletManager to create bullets

                    bulletManager.CreateNewBullet(right, Y + 0.08, false, MovingTime);
                    bulletManager.CreateNewBullet(left, Y + 0.08, false, MovingTime);
                }
                else
                {
                    var bulletManager = BulletManagerFactory.CreateBulletManager(); // Use the bulletManager to create bullets

                    bulletManager.CreateNewBullet(X, Y + 0.08, false, MovingTime);
                    bulletManager.CreateNewBullet(right, Y + 0.08, false, MovingTime);
                    bulletManager.CreateNewBullet(left, Y + 0.08, false, MovingTime);

                }
                //Debug.WriteLine($"{Y}{X}oluþturuldu");
                i += 0.01;
            }
            await Task.Delay(CreateInterval); // Yeni Image oluþturma aralýðý
                                              // Example of an async task (e.g., logging)
        }
    }

}*/