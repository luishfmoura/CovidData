using CovidData.Models.Returns;
using CovidData.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace CovidData.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CovidCasesController : ControllerBase
    {
        private readonly CovidCasesService _covidCasesService;

        public CovidCasesController(CovidCasesService covidCasesService)
        {
            _covidCasesService = covidCasesService;
        }

        [HttpGet]
        [Route("")]
        [SwaggerOperation(Summary = "Gets Default Message", Description = "Gets a default message.")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success!")]
        [SwaggerResponse((int)HttpStatusCode.NoContent, "No Data found!")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, "Internal Server Error")]
        public Response Get()
        {
            Response response = _covidCasesService.DefaultMessage();

            if (response.Status)
            {
                if (response.Data != null)
                {
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.StatusCode = HttpStatusCode.NoContent;

                }

            }
            else
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }

        [HttpGet]
        [Route("/cases/{date}/count")]
        [SwaggerOperation(Summary = "Gets Covid Data", Description = "Gets cumulative Covid Data in a specific date, unified by country and separated by variant")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success!")]
        [SwaggerResponse((int)HttpStatusCode.NoContent, "No Data found!")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, "Internal Server Error")]
        public Response Count(string date)
        {

            Response response = _covidCasesService.GetCount(date);

            if (response.Status)
            {
                if (response.Data != null)
                {
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.StatusCode = HttpStatusCode.NoContent;
                }

            }
            else
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }


        [HttpGet]
        [Route("/cases/{date}/cumulative")]
        [SwaggerOperation(Summary = "Gets Covid Data", Description = "Gets cumulative Covid Data in a specific date, unified by country and separated by variant")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success!")]
        [SwaggerResponse((int)HttpStatusCode.NoContent, "No Data found!")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, "Internal Server Error")]
        public Response Cumulative(string date)
        {
            Response response = _covidCasesService.GetCumulative(date);

            if (response.Status)
            {
                if (response.Data != null)
                {
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.StatusCode = HttpStatusCode.NoContent;
                }

            }
            else
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }

        [HttpGet]
        [Route("/dates")]
        [SwaggerOperation(Summary = "Gets dates", Description = "Gets all dates in the DataBase")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success!")]
        [SwaggerResponse((int)HttpStatusCode.NoContent, "No Data found!")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, "Internal Server Error")]
        public Response Dates()
        {
            Response response = _covidCasesService.GetDates();

            if (response.Status)
            {
                if (response.Data != null)
                {
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.StatusCode = HttpStatusCode.NoContent;
                }

            }
            else
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Upload CSV file", Description = "Uploads all Data in a CSV file to the Database")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success!")]
        [SwaggerResponse((int)HttpStatusCode.NoContent, "No Data found!")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, "Internal Server Error")]
        [Route("/upload")]
        public Response Upload(IFormFile file, bool persistData)
        {
            Response response = _covidCasesService.UploadData(file, persistData);

            if (response.Status)
            {
                response.StatusCode = HttpStatusCode.OK;
            }
            else
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Delete Data", Description = "Deletes all Data in the Database")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success!")]
        [SwaggerResponse((int)HttpStatusCode.NoContent, "No Data found!")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, "Internal Server Error")]
        [Route("/delete")]
        public Response Delete()
        {
            Response response = _covidCasesService.DeleteData();

            if (response.Status)
            {
                response.StatusCode = HttpStatusCode.OK;
            }
            else
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }
    }
}
