namespace Lab2Try2Famutdinov.Models
{
    public class User
    {
        public int Id { get; set; }                // Уникальный идентификатор пользователя
        public string Username { get; set; }       // Логин пользователя
        public string Email { get; set; }          // Адрес электронной почты
        public string Role { get; set; }           // Роль пользователя: "Admin" или "Customer"

        // Поля для клиента (только для Role == "Customer")
        public string Name { get; set; }           // Имя клиента
        public string Phone { get; set; }          // Телефон клиента
        public string Address { get; set; }        // Адрес клиента

        /// <summary>
        /// Проверяет, является ли пользователь администратором.
        /// </summary>
        public bool IsAdmin()
        {
            return Role?.Equals("Admin", StringComparison.OrdinalIgnoreCase) ?? false;
        }

        /// <summary>
        /// Проверяет, является ли пользователь клиентом.
        /// </summary>
        public bool IsCustomer()
        {
            return Role?.Equals("Customer", StringComparison.OrdinalIgnoreCase) ?? false;
        }

        /// <summary>
        /// Обновляет основные данные пользователя.
        /// </summary>
        public void UpdateUserInfo(string username, string email, string password)
        {
            if (!string.IsNullOrWhiteSpace(username))
                Username = username;
            if (!string.IsNullOrWhiteSpace(email))
                Email = email;
        }

        /// <summary>
        /// Обновляет данные клиента (только для роли "Customer").
        /// </summary>
        public void UpdateCustomerInfo(string name, string phone, string address)
        {
            if (!IsCustomer())
                throw new InvalidOperationException("Нельзя обновить данные клиента для пользователя, не являющегося клиентом.");

            if (!string.IsNullOrWhiteSpace(name))
                Name = name;
            if (!string.IsNullOrWhiteSpace(phone))
                Phone = phone;
            if (!string.IsNullOrWhiteSpace(address))
                Address = address;
        }

        /// <summary>
        /// Проверяет, заполнены ли обязательные данные для клиента.
        /// </summary>
        public bool IsCustomerDataValid()
        {
            if (!IsCustomer())
                return false;

            return !string.IsNullOrWhiteSpace(Name) &&
                   !string.IsNullOrWhiteSpace(Phone) &&
                   !string.IsNullOrWhiteSpace(Address);
        }
    }
}
