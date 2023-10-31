using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Service.IService;

namespace Web.Controllers
{
#nullable disable
    public class DemandCandidateController : Controller
    {
        private readonly ICandidatPublicService _candidatPublicService;
        private readonly IEmailService _emailService;

        public DemandCandidateController(ICandidatPublicService candidatPublicService, IEmailService emailService)
        {
            _candidatPublicService = candidatPublicService;
            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateClient model)
        {
            try
            {
                Response responseDocument = new();
                Response responseDiplome = new();
                Response response = await _candidatPublicService.AddCandidat(model.Candidat);
                if (model.DocumentModel != null)
                {
                    responseDocument = await _candidatPublicService.AddDocument(model.DocumentModel, response.ID);
                }
                if (model.DiplomeModel != null)
                {
                    responseDiplome = await _candidatPublicService.AddDiplome(model.DiplomeModel, response.ID);
                }
                /*if (!responseDocument.Success || !responseDiplome.Success)
                {
                    ViewData["MessageDiplome"] = responseDiplome.Message;
                    Console.WriteLine(responseDiplome.Message2);
                    ViewData["MessageDocument"] = responseDocument.Message;
                    Console.WriteLine(responseDocument.Message2);
                    ViewData["Message"] = response.Message;
                    Console.WriteLine(response.Message2);
                    return View(model);
                }*/
                //await _emailService.SendEmailAsync("badredine_moussaoui@hotmail.com", "Demande d\'inscription", "https://www.plateformedesjeunes-ainsebaa.com/GestionCandidatPublic/Details/" + response.ID, null);
                if (!response.Success)
                {
                    return View(model);
                }
                return RedirectToAction("Login", "Authentication", new { Success = "1", CandidatId = response.ID });
            }
            catch (Exception)
            {
                return View(model);
            }
        }
    }
}
