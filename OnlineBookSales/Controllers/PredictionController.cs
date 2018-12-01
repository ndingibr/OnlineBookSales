using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineBookSales.Core;
using OnlineBookSales.Core.Entities;
using OnlineBookSales.Infrastructure;

namespace OnlineBookSales.API
{
    //[Authorize(Policy = "Roles")]
    [Route("api/[controller]")]
    public class PredictionController : Controller
    {
        private IRepository<ModelAttributes> _modelAttributesRepo;
        private IRepository<ModelAccuracy> _modelAccuracyRepo;
        private IPythonService _pythonService;
        
        public PredictionController(IRepository<ModelAttributes> modelAttributesRepo, IRepository<ModelAccuracy> modelAccuracyRepo, IPythonService pythonService)
        {
            _modelAttributesRepo = modelAttributesRepo;
            _modelAccuracyRepo = modelAccuracyRepo;
            _pythonService = pythonService;
        }

        //[Authorize(Policy = "Roles")]
        [HttpGet]
        public List<int> Get()
        {
          
            var disc = new Dictionary<string, string>()
            {
                {"PdDistrict", "BAYEVIEW" },
                {"day", "1"},
                {"Category", "ARSON" },
                {"year", "2004" },
                {"month", "4" },
                {"DayOfWeek", "THURSDAY" },
                {"shift", "3.0" }
            };

            return _pythonService.GetDailyPredictionAsync(disc);
        }

        [HttpGet("{getattributes}")]
        public IActionResult GetAttributes()
        {
            var attributeModels = new List<AttributesModel>() {
                new AttributesModel() { Attribute= "PdDistrict",  Checked = false },
                new AttributesModel() { Attribute = "Day", Checked = false },
                new AttributesModel() { Attribute = "Category", Checked = false },
                new AttributesModel() { Attribute = "Year", Checked = false },
                new AttributesModel() { Attribute = "Month", Checked = false },
                new AttributesModel() { Attribute = "DayOfWeek", Checked = false },
                new AttributesModel() { Attribute = "Shift", Checked = false }
                };

            return Ok(attributeModels);
        }

        [HttpGet("{getmodelaccuracies}")]
        public IActionResult GetModelAccuracies()
        {
            return null;
        }

        [HttpPost("{submitattributes}")]
        public IActionResult SubmitAttributes([FromBody]List<AttributesModel> attributesModels)
        {
            foreach (AttributesModel item in attributesModels)
            {
                var model = new ModelAttributes
                {
                    Attribute = item.Attribute,
                    Checked = item.Checked,
                };
                _modelAttributesRepo.Insert(model);
                _modelAttributesRepo.GetAll();

            }
            return Ok();
        }

        //[HttpPost("{predictionParameters}")]
        //public List<int> GetDailyPrediction([FromBody]PredictionParameters predictionParameters)
        //{
        //    var dictionary = new Dictionary<string, string>()
        //     {
        //         { "PdDistrict", predictionParameters.PdDistrict },
        //         { "Day", predictionParameters.Day },
        //         { "Category", predictionParameters.Category },
        //         { "Year", predictionParameters.Year },
        //         { "Month", predictionParameters.Month },
        //         { "DayOfWeek", predictionParameters.DayOfWeek },
        //         { "Shift", predictionParameters.Shift },
        //     };
        //    return pythonOnlineBookSales.APIHelper.GetDailyPredictionAsync(dictionary);
        //}
    }
}
