using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BlogApi.Models;

public partial class Blogger
{
    public int Id { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public string? Email { get; set; }
    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
