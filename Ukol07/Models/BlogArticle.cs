using Ukol07.Areas.Identity.Data;

namespace Ukol07.Models; 

public class BlogArticle {
    public int Id { get; set; }
    
    public string Title { get; set; }
    
    public string Content { get; set; }

    public DateTime DateTime { get; set; }
    
    public virtual User Author { get; set; }

    public virtual ICollection<Comment> Comments { get; set; }
}