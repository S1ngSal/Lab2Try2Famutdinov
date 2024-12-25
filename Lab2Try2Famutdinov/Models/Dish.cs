using System.Text.Json.Serialization;

namespace Lab2Try2Famutdinov.Models
{
    public class Dish
    {
        public int Id { get; set; } // Уникальный идентификатор блюда
        public string Name { get; set; } // Название блюда
        public float Price { get; set; } // Цена блюда
        public bool IsAvailable { get; set; } // Доступность блюда

        // Навигационная коллекция для связи "многие ко многим"
        
        [JsonIgnore]
        public List<Order> Orders { get; set; } // Список заказов, в которые входит данное блюдо

        // Метод для обновления доступности
        public void UpdateAvailability(bool availability)
        {
            IsAvailable = availability;
        }

        public Dish()
        {
            Orders = new List<Order>(); // Инициализация коллекции заказов
        }
    }
}
