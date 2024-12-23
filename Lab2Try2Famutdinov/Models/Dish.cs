namespace Lab2Try2Famutdinov.Models
{
    public class Dish
    {
        public int Id { get; set; } // Уникальный идентификатор блюда
        public string Name { get; set; } // Название блюда
        public float Price { get; set; } // Цена блюда
        public bool IsAvailable { get; set; } // Доступность блюда

        // Метод для обновления доступности
        public void UpdateAvailability(bool availability)
        {
            IsAvailable = availability;
        }

    }
}
