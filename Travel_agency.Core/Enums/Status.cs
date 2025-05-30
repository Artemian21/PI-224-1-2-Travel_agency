using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel_agency.Core.Enums
{
    public enum Status
    {
        Pending,    // Очікує підтвердження
        Confirmed,  // Підтверджено
        Cancelled,  // Скасовано користувачем
        Rejected,   // Відхилено оператором
        Paid,       // Оплачено
        Completed   // Завершено
    }
}
