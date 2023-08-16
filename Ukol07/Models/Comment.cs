using Ukol07.Areas.Identity.Data;

namespace Ukol07.Models; 

public class Comment {
    public int Id { get; set; }
    
    public string Content { get; set; }
    
    public virtual User Author { get; set; }

    public DateTime DateTime { get; set; }

    public virtual BlogArticle RelevantArticle { get; set; }
}