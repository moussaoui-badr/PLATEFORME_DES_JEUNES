using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlateformeDesJeunesV7.Controllers;
using Repository.Data;

namespace Web.Controllers
{
    public class ExecuteSqlController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExecuteSqlController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ActionResult> Index(string text)
        {
            string sqlScript = text;
            try
            {
                _context.Database.ExecuteSqlRaw(sqlScript);
                ViewData["Success"] = "Script exécuté avec succès";
                return View();
            }
            catch(Exception ex)
            {
                ViewData["Error"] = ex.Message;
                return View();
            }
        }

        public ActionResult DeleteSecteur()
        {
            try
            {
                //Supprimer les doublants de la table Secteur qui ont Id supérieur
                
                string sqlScript = "DELETE FROM Secteurs WHERE Id NOT IN (SELECT MIN(Id) FROM Secteurs GROUP BY Nom)";

                _context.Database.ExecuteSqlRaw(sqlScript);


                return View();
            }
            catch
            {
                return View();
            }
        }
    }
}
