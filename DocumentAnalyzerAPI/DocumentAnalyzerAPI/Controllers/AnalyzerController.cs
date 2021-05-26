using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DocumentAnalyzerAPI.Models;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http.Features;
using DataHandlerMongoDB.Factory;
using DataHandlerMongoDB.Repository;
using DataHandlerMongoDB.Model;
using FileMongo = DataHandlerMongoDB.Model.File;
using DataHandlerSQL.Factory;
using DataHandlerSQL.Repository;
using DataHandlerSQL.Model;
using DocumentASnalyzerAPI.Models;
using System.Security.Claims;

namespace DocumentAnalyzerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize]
    public class AnalyzerController : Controller
    {
        private static IMongoRepository<FileMongo> mongo_repository;
        private static IUnitOfWorkFactory uow_factory;

        public AnalyzerController(IMongoRepositoryFactory factory, IUnitOfWorkFactory unit_factory)
        {
            mongo_repository = factory.Create<FileMongo>();
            uow_factory = unit_factory;
        }

        [HttpPost, Route("/documents/notify")]
        public IActionResult Notify(List<NotificationData> requests)
        {
            BrokerHandler.NotifyAnalyzers(requests, "10001");
            return Ok();
        }

        /*[HttpPost, Route("/documents/notify/")]
        public IActionResult NotifyDocument(NotificationData data)
        {
            try
            {
                ClaimsPrincipal claims = HttpContext.User;
                string owner = claims.FindFirst("email").Value;

                //NLPService.NLPController.Instance.AddDocument(data.Url, owner);

                IUnitOfWork unit_of_work = uow_factory.Create();

                List<Match> processingResults = EmployeeFinder.FindEmployeeReferences(data, int.Parse(owner), mongo_repository, unit_of_work);

                EmployeeFinder.AddUserReferences(processingResults, unit_of_work);

                return Ok();
            } 
            catch
            {
                return BadRequest();
            }
        }*/

        [HttpGet, Route("/object")]
        public IActionResult GetAzure()
        {
            return Ok("https://soafiles.blob.core.windows.net/files?sp=racwdl&st=2021-04-30T21:44:27Z&se=2021-06-16T05:44:27Z&sv=2020-02-10&sr=c&sig=kpTVVN2JZryg0FrW4fxWmFURSo0yUbkVtZZrS4e2dws%3D");
        }

        [HttpGet, Route("/documents")]
        public IActionResult GetUserDocuments()
        {
            try
            {
                /*ClaimsPrincipal claims = HttpContext.User;
                string owner = claims.FindFirst(ClaimTypes.NameIdentifier).Value;*/

                IUnitOfWork unit_of_work = uow_factory.Create();

                //List<UserDocument> userFiles = EmployeeFinder.FindEmployeeDocuments(int.Parse(owner), mongo_repository);
                List<UserDocument> userFiles = EmployeeFinder.FindEmployeeDocuments("10001", mongo_repository);
                List<UserDocument> result = EmployeeFinder.GetDocumentReferences(userFiles, unit_of_work);

                /* Returns JSON [{"Title": String,"Status": Boolean, "Url": String,"UserDocumentReferences":[{"Name":String,"Qty":Integer},{"Name":String,"Qty":Integer}, ...]}]
                 */
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet, Route("/documents/users/count")]
        public IActionResult GetUserGlobalCount()
        {
            IUnitOfWork unit_of_work = uow_factory.Create();

            List<EmployeeCount>  counts = EmployeeFinder.GetEmployeeGlobalCounter(unit_of_work, mongo_repository);
            return Ok(counts);
        }
    }
}
