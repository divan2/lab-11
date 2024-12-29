
using System.Text;
using System.Text.Json;


public class Category
{
    public int categoryID { get; set; }
    public string? categoryName { get; set; }
    public string? description { get; set; }
    public byte[]? Picture { get; set; }
}

class Program
{
    private static readonly HttpClient _client;
    private static readonly string _apiUrl = "https://localhost:7254/api/Categories";

    static Program()
    {
        var handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
        {
            return true;
        };
        _client = new HttpClient(handler);
    }

    static async Task Main(string[] args)
    {
        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

        while (true)
        {
            Console.WriteLine("\nВыберите действие:");
            Console.WriteLine("1. Получить все категории");
            Console.WriteLine("2. Получить категорию по ID");
            Console.WriteLine("3. Создать категорию");
            Console.WriteLine("4. Обновить категорию");
            Console.WriteLine("5. Удалить категорию");
            Console.WriteLine("6. Выйти");

            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    await GetAllCategories();
                    break;
                case "2":
                    await GetCategoryById();
                    break;
                case "3":
                    await CreateCategory();
                    break;
                case "4":
                    await UpdateCategory();
                    break;
                case "5":
                    await DeleteCategory();
                    break;
                case "6":
                    return;
                default:
                    Console.WriteLine("Неверный выбор.");
                    break;
            }
        }
    }
    static async Task GetAllCategories()
    {
        HttpResponseMessage response = await _client.GetAsync(_apiUrl);

        if (response.IsSuccessStatusCode)
        {
            string responseBody = await response.Content.ReadAsStringAsync();
            try
            {
                Category[]? categories = JsonSerializer.Deserialize<Category[]>(responseBody);


                if (categories != null)
                {
                    foreach (var category in categories)
                    {
                        Console.WriteLine($"{categories}    ID: {category.categoryID}, Name: {category.categoryName}, description: {category.description}");
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка десериализации: {ex.Message}");
                Console.WriteLine($"Тело ответа: {responseBody}");
            }
        }
        else
        {
            Console.WriteLine($"Ошибка при получении категорий. Статус код: {response.StatusCode}");
        }
    }
    static async Task GetCategoryById()
    {
        Console.Write("Введите ID категории: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            HttpResponseMessage response = await _client.GetAsync($"{_apiUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                Category? category = JsonSerializer.Deserialize<Category>(responseBody);

                if (category != null)
                    Console.WriteLine($"ID: {category.categoryID}, Name: {category.categoryName}, description: {category.description}");
            }
            else
                Console.WriteLine($"Ошибка при получении категории. Статус код: {response.StatusCode}");
        }
        else
            Console.WriteLine("Неверный ID.");
    }
    static async Task CreateCategory()
    {
        Console.Write("Введите имя категории: ");
        string? name = Console.ReadLine();
        Console.Write("Введите описание категории: ");
        string? description = Console.ReadLine();


        Category newCategory = new Category() { categoryName = name, description = description };
        string json = JsonSerializer.Serialize(newCategory);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await _client.PostAsync(_apiUrl, content);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Категория успешно создана.");
        }
        else
            Console.WriteLine($"Ошибка при создании категории. Статус код: {response.StatusCode}");
    }
    static async Task UpdateCategory()
    {
        Console.Write("Введите ID категории для обновления: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            Console.Write("Введите новое имя категории: ");
            string? name = Console.ReadLine();
            Console.Write("Введите новое описание категории: ");
            string? description = Console.ReadLine();

            Category updateCategory = new Category { categoryID = id, categoryName = name, description = description };
            string json = JsonSerializer.Serialize(updateCategory);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PutAsync($"{_apiUrl}/{id}", content);

            if (response.IsSuccessStatusCode)
                Console.WriteLine("Категория успешно обновлена.");
            else
                Console.WriteLine($"Ошибка при обновлении категории. Статус код: {response.StatusCode}");
        }
        else
            Console.WriteLine("Неверный ID.");
    }
    static async Task DeleteCategory()
    {
        Console.Write("Введите ID категории для удаления: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            HttpResponseMessage response = await _client.DeleteAsync($"{_apiUrl}/{id}");
            if (response.IsSuccessStatusCode)
                Console.WriteLine("Категория успешно удалена.");
            else
                Console.WriteLine($"Ошибка при удалении категории. Статус код: {response.StatusCode}");
        }
        else
            Console.WriteLine("Неверный ID.");
    }
}
