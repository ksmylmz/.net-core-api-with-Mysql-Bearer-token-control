using System;
using System.Data;
using System.Collections.Generic;
using Dresses.Models;
using Dresses.Repo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Dresses.Controllers
{
    [ApiController]
    [Route("/api/dress")]
    [Authorize]
    public class DressController : ControllerBase
    {
        DataRepo repo = new DataRepo();
        
        [HttpGet]
        public ActionResult Get()
        {
            List<DressModel> dresses = repo.GetDresses();
            return  Ok(dresses);

        }

        
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            DressModel dress = repo.GetDress(id.ToString());
            if (dress != null)
            {
               return Ok(dress);


            }
            else
            {
                return NotFound(new ResponseModel { Error="Dress Not Found"});
                
            }
        }
        [HttpPost]
        public ActionResult Post(DressModel dress)
        {
            /*if (!ModelState.IsValid) 
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);*/
            try
            {
                repo.InsertDress(dress);
                return StatusCode(201,new ResponseModel { Message="Dress append"});
            }
            catch (Exception ex)
            {
                return StatusCode(500,new ResponseModel { Error=ex.Message});
            }

        }
        [HttpPut]
        public ActionResult Put(DressModel dress)
        {
            /*if (!ModelState.IsValid) 
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);*/
           try
            {
                DressModel chkDress = repo.GetDress(dress.DresID.ToString());
                if (chkDress == null)
                {
                    return NotFound(new ResponseModel { Error = "Dress Not Found" });
                }
                else
                {
                    repo.updateDress(dress);
                    return Ok(new ResponseModel { Message = "Data is Updated" });
                }

            }
            catch (Exception ex)
            {

                return StatusCode(500,new ResponseModel { Error = ex.Message });
            }

        }
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            try
            {
                DressModel chkDress = repo.GetDress(id);
                if (chkDress == null)
                {
                    return NotFound(new ResponseModel { Message = "Data Not Found" });
                 }
                else
                {
                    repo.deleteDress(id);
                    return Ok(new ResponseModel { Message = "Data is Removed" });
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500,new ResponseModel { Error = ex.Message });
            }

        }
    }
}
