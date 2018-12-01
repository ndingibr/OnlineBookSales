using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineBookSales.Core;
using OnlineBookSales.Core.Entities;
using OnlineBookSales.Infrastructure;

namespace OnlineBookSales.API
{

    [Route("api/[controller]")]
    public class DataController : Controller
    {
        private IRepository<Data> _dataRepo;
        private IRepository<Train> _trainRepo;
        private IRepository<PatrolTrain> _patrolTrain;
        private IPythonService _pythonService;

        public DataController(IRepository<Data> dataRepo, IRepository<Train> trainRepo,
            IRepository<PatrolTrain> patrolTrain, IPythonService pythonService)
        {
            _dataRepo = dataRepo;
            _trainRepo = trainRepo;
            _patrolTrain = patrolTrain;
            _pythonService = pythonService;
        }
             
        [HttpGet]
        public List<Data> Get(DateTime? startDate, DateTime? endDate)
        {

            var query = _dataRepo.GetAll();
            if (startDate != null)
                query = query.Where(x => x.Date >= startDate);

            if (endDate != null)
                query = query.Where(x => x.Date <= endDate);


            if (startDate != null && endDate !=null)
            {
                _trainRepo.RemoveRange(_trainRepo.GetAll());
                var train = (from data in query
                             select new Train
                             {
                                 Date = data.Date,
                                 Category = data.Category,
                                 Description = data.Description,
                                 DayofWeek = data.DayofWeek,
                                 District = data.District,
                                 Resolution = data.Resolution,
                                 Address = data.Address,
                                 Longitude = data.Longitude,
                                 Latitude = data.Latitude
                             }).ToList();

                _trainRepo.AddRange(train);
            }

            BuildPatrol();

            var returnValues = query.Take(100).ToList();
            return returnValues;
        }

        private void BuildPatrol()
        {
            _patrolTrain.RemoveRange(_patrolTrain.GetAll());
            var pythonData = _pythonService.InsertPatrolData();
        }
             
    }
}
