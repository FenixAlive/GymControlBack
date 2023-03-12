using System;
using System.Collections.Generic;

namespace gymControl.Models;

public partial class Partner
{
    
    public int? Id { get; set; }

    public string Username { get; set; } = null!;

    public string? Passwd { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? Updated { get; set; }

    public bool? Active { get; set; }

    public virtual ICollection<User> Users { get; } = new List<User>();
}

public class PartnerQuery
{
    public int? Id { get; set; } = null;
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public bool? Active { get; set; } = null;

}
