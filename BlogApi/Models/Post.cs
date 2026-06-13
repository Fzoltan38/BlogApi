using System;
using System.Collections.Generic;

namespace BlogApi.Models;

public partial class Post
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Content { get; set; }

    public DateTime? CreatedTime { get; set; }

    public int? BlogId { get; set; }

    public virtual Blogger? Blog { get; set; }
}
