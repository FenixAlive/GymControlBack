using System;
using System.Collections.Generic;

namespace gymControl.Models;

public partial class User
{
    public int? Id { get; set; }

    public int PartnerId { get; set; }

    public string? userPass { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public DateOnly? PayDay { get; set; }

    public int? MonthsPaid { get; set; }

    public DateOnly? CaducateDate { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? Updated { get; set; }

    public bool? Active { get; set; }


    public DateOnly? caducateDateCalc => PayDay?.AddMonths(MonthsPaid ?? 0);
}

public partial class UserQuery
{
    public int? Id { get; set; } = null;
    public string? FirstName { get; set; } = null;
    public string? LastName { get; set; } = null;
    public string? Email { get; set; } = null;
    public string? Phone { get; set; } = null;
    public int? MonthsPaid { get; set; } = null;
    public bool? Active { get; set; } = null;

}