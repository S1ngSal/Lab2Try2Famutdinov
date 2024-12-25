namespace Lab2Try2Famutdinov.Models
{
    public class Order
    {
        public int Id { get; set; } // Идентификатор заказа
        public int UserId { get; set; } // Идентификатор пользователя
        public List<OrderItem> OrderItems { get; set; } // Список позиций (блюд) в заказе
        public float TotalPrice { get; set; } // Общая стоимость
        public DateTime OrderDate { get; set; } // Дата заказа

        public Order()
        {
            OrderItems = new List<OrderItem>(); // Инициализация коллекции позиций заказа
        }

    }
}
