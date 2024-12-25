public class OrderItem
{
    public int Id { get; set; }
    public int DishId { get; set; }
    public int Quantity { get; set; }

    // Вспомогательные свойства для хранения данных о блюде
    public string DishName { get; set; }
    public float DishPrice { get; set; }
}
