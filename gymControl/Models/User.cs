using System;
using System.Collections.Generic;

namespace gymControl.Models;

public partial class User
{
    public int Id { get; set; }

    public int PartnerId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public DateOnly? PayDay { get; set; }

    public int? MonthsPaid { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Updated { get; set; }

    public bool? Active { get; set; }

    public virtual Partner Partner { get; set; } = null!;

    public DateOnly? caducateDate => PayDay?.AddMonths(MonthsPaid ?? 0);
}
