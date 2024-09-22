namespace WEB_253505_AZAROV.Domain.Models;
public class ResponseData<T>
{
    public T? Data { get; set; }
    public bool Successfull { get; set; } = true;
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Получить объект успешного ответа
    /// </summary>
    /// <param name="data">передаваемые данные</param>
    /// <returns></returns>
    public static ResponseData<T> Success(T data)
    {
        return new ResponseData<T> { Data = data };
    }
    /// <summary>
    /// Получение объекта ответа с ошибкой
    /// </summary>
    /// <param name="message">Сообщение об ошибке</param>
    /// <param name="data">Передаваемые данные</param>
    /// <returns></returns>
    public static ResponseData<T> Error(string message, T? data=default)
    {
        return new ResponseData<T> { ErrorMessage = message,
        Successfull = false, Data = data };
    }
}