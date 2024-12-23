namespace Lab2Try2Famutdinov.Models
{
    public class Order
    {
        public int Id { get; set; } // Уникальный идентификатор заказа
        public int UserId { get; set; } // Идентификатор клиента, сделавшего заказ
        public List<Dish> Dishes { get; set; } // Список блюд в заказе
        public float TotalPrice { get; set; } // Общая стоимость заказа
        public DateTime OrderDate { get; set; } // Дата и время заказа

        public Order()
        {
            Dishes = new List<Dish>();
            OrderDate = DateTime.Now; // Устанавливаем текущую дату и время
        }

        public void CalculateTotalPrice()
        {
            TotalPrice = Dishes.Sum(d => d.Price); // Суммируем цены всех блюд
        }
    }
}
