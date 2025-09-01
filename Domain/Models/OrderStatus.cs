namespace Domain.Models
{
    public enum OrderStatus
    {
        Created = 1,//Создание
        Pending,//Создан не оплачен
        Success,//Оплачен
        Failed,//Оплата не прошла
        Reject,//Отменен
        Error//Доп статус
    }
}
