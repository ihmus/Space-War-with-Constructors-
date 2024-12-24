public class MyShipBorders
{
    private Border myShipBorders;

    // Property adını MyShipBorder olarak değiştirdik
    public Border MyShipBorder
    {
        get { return myShipBorders; }
        set { myShipBorders = value; }
    }

    // Parametreli constructor
    public MyShipBorders(Border border)
    {
        MyShipBorder = border;  // Property'yi parametre ile başlatıyoruz
    }

    // Parametresiz constructor
    public MyShipBorders()
    {
        // Varsayılan bir Border nesnesi oluşturuluyor
        MyShipBorder = new Border();
    }
}
