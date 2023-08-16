using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ukol07.Areas.Identity.Data;
using Ukol07.Models;

namespace Ukol07.Controllers; 

public class ArticleController : Controller {
    private readonly UserManager<User> _userManager;

    public ArticleController(UserManager<User> userManager) {
        _userManager = userManager;
    }

    // Tato akce bude zobrazovat jednotlivé články i s komentáři
    public IActionResult Index(int id) {
        try {
            if (TempData["alert"] != null) {
                ViewBag.alert = TempData["alert"];
            }
            using (AppDbContext ctx = new AppDbContext()) {
                // Dále pokud je přihlášený uživatel, tak předáme jeho přezdívku kvůli komentářům
                string logedInUserId = _userManager.GetUserId(HttpContext.User);
                if (logedInUserId != null) {
                    User? loggedUser = ctx.Users.Find(logedInUserId);
                    ViewBag.loggedUserNickname = loggedUser.BlogNickname;
                }
                
                var resultArticle = ctx.BlogArticles
                    .Include(a => a.Author)
                    .Include(a => a.Comments)
                    .FirstOrDefault(a => a.Id == id);
                if (resultArticle != null) {
                    return View(resultArticle);
                }
                else {
                    return View(null);
                }
            }
        }
        catch (Exception e) {
            Console.WriteLine(e);
            throw;
        }
    }
    
    // Zobrazení výčtu všech článku
    public IActionResult AllArticles() {
        try {
            List<BlogArticle> articles = new List<BlogArticle>();
            using (AppDbContext dbContext = new AppDbContext()) {
                foreach (var article in dbContext.BlogArticles.Include(a => a.Author)) {
                    articles.Add(article);
                }
                return View(articles);
            }
        }
        catch (Exception e) {
            Console.WriteLine(e);
            throw;
        }
    }
    
    // Zobrazení článků patřícíh pouze příhlášenému uživateli
    public IActionResult UserArticles() {
        string logedInUserId = _userManager.GetUserId(HttpContext.User);
        if (logedInUserId != null) {
            List<BlogArticle> articles = new List<BlogArticle>();
            try {
                using (AppDbContext dbContext = new AppDbContext()) {
                    foreach (BlogArticle article in dbContext.BlogArticles.Include(a => a.Author)) {
                        if (logedInUserId == article.Author.Id) {
                            articles.Add(article);
                        }
                    }
                    return View(articles);
                }
            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
        }
        else {
            return View(null);
        }
        
    }
    
    // Smazání článku
    public IActionResult DeleteArticle(int id) {
        try {
            using (AppDbContext ctx = new AppDbContext()) {
                BlogArticle? article = ctx.BlogArticles
                    .Include(a => a.Comments)
                    .FirstOrDefault(a => a.Id == id);
                if (article != null) {
                    
                    // Musíme smazat i všechny commenty u článku
                    foreach (var comm in article.Comments) {
                        ctx.Remove(comm);
                    }
                    ctx.Remove(article);
                    ctx.SaveChanges();
                }
            }
        }
        catch (Exception e) {
            Console.WriteLine(e);
            throw;
        }
        return RedirectToAction("UserArticles", "Article");
    }
    
    // Zobrazení formuláře článku
    public IActionResult ArticleForm(int id = -1) {
        if (id != -1) {
            try {
                using (AppDbContext ctx = new AppDbContext()) {
                    BlogArticle? article = ctx.BlogArticles
                        .Include(a => a.Author)
                        .FirstOrDefault(a => a.Id == id);
                    if (article != null) {
                        return View(article);
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
        }
        return View(null);
    }
    
    // Uložení článku
    public IActionResult SaveArticle(string title, string content) {
        try {
            using (AppDbContext ctx = new AppDbContext()) {
                string logedInUserId = _userManager.GetUserId(HttpContext.User);
                User? logedInUser = ctx.Users.Find(logedInUserId);
                if (logedInUser != null) {
                    BlogArticle newArticle = new BlogArticle() {
                        Title = title ?? "",
                        Content = content ?? "",
                        Author = logedInUser,
                        DateTime = DateTime.Now,
                        Comments = new List<Comment>()
                    };
                    ctx.BlogArticles.Add(newArticle);
                    ctx.SaveChanges();
                }
                return RedirectToAction("UserArticles", "Article");
            }
        }
        catch (Exception e) {
            Console.WriteLine(e);
            throw;
        }
    }
    
    // Editace článku
    public IActionResult EditArticle(int id, string title, string content) {
        try {
            using (AppDbContext ctx = new AppDbContext()) {
                BlogArticle? article = ctx.BlogArticles
                    .Include(a => a.Author)
                    .FirstOrDefault(a => a.Id == id);
                if (article != null) {
                    article.Title = title ?? "";
                    article.Content = content ?? "";
                }
                ctx.SaveChanges();
                return RedirectToAction("UserArticles", "Article");
            }
        }
        catch (Exception e) {
            Console.WriteLine(e);
            throw;
        }
    }
    
    // Přidání komentáře k danému článku
    public IActionResult AddCommentToArticle(int articleId, string content) {
        try {
            using (AppDbContext ctx = new AppDbContext()) {
                string logedInUserId = _userManager.GetUserId(HttpContext.User);
                User? logedInUser = ctx.Users.Find(logedInUserId);
                
                // Pokud není přihlášený, žádný uživatel -> změna se neprovede 
                if (logedInUser == null) {
                    TempData["alert"] = "For manipulating with comments please login to your account.";
                    return RedirectToAction("Index", "Article", new { id = articleId });
                }
                else {
                    BlogArticle? currentArticle = ctx.BlogArticles
                        .Include(a => a.Comments)
                        .FirstOrDefault(a => a.Id == articleId);
                    if (currentArticle != null) {
                        Comment comm = new Comment() {
                            Author = logedInUser,
                            Content = content,
                            RelevantArticle = currentArticle,
                            DateTime = DateTime.Now
                        };
                        currentArticle.Comments.Add(comm);
                        ctx.Update(currentArticle);
                        ctx.SaveChanges();
                        return RedirectToAction("Index", "Article", new { id = articleId });
                    }
                }
            }
        }
        catch (Exception e) {
            Console.WriteLine(e);
            throw;
        }
        return RedirectToAction("Index", "Article", new { id = articleId });
    }

    // Smazání komentáře -> editace komentáře mi příjde zbytečná (instagram to taky nemá)
    public IActionResult DeleteCommentFromArticle(int commentId, int articleId) {
        try {
            using (AppDbContext ctx = new AppDbContext()) {
                Comment? commToDelete = ctx.Comments
                    .Include(c => c.RelevantArticle)
                    .FirstOrDefault(c => c.Id == commentId);
                BlogArticle? relavantArticle = ctx.BlogArticles
                    .Include(a => a.Comments)
                    .FirstOrDefault(a => a.Id == articleId);
                if (commToDelete != null && relavantArticle != null) {
                    relavantArticle.Comments.Remove(commToDelete);
                    ctx.Update(relavantArticle);
                    ctx.Remove(commToDelete);
                    ctx.SaveChanges();
                }

                return RedirectToAction("Index", "Article", new { id = articleId });
            }
        }
        catch (Exception e) {
            Console.WriteLine(e);
            throw;
        }
    }
}