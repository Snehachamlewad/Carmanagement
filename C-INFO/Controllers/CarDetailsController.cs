using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using C_INFO.Models;

namespace C_INFO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarDetailsController : ControllerBase
    {
        CarContext db;
        public CarDetailsController(CarContext _db)
        {
            db = _db;
        }
        [HttpGet]
        public IEnumerable<CAR1> GetCars()
        {
            return db.CAR1S;
        }
        [HttpPost]
        public string Post([FromBody] CAR1 Car)
        {
            /*if (food.IsActive == 1)
            {
                db.SaveChanges();
            }
            else
                return "not available";*/
            db.CAR1S.Add(Car);
            db.SaveChanges();
            return "success";
        }

        [HttpPut]
        public string Put([FromBody] CAR1 Car)
        {
            var tblsampleObj = db.CAR1S.Where(x => x.Id == Car.Id);
            if (tblsampleObj != null)
            {
                db.CAR1S.Update(Car);
                db.SaveChanges();
                return "Success";
            }

            return "Fail";
        }
    }
}
