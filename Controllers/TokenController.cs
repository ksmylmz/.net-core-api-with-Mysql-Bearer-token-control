using Dresses.Models;
using Dresses.Repo;
using System;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Dresses.Controllers
{
    [ApiController]
    [Route("/api/token")]
    [AllowAnonymous]
    public class TokenController :ControllerBase
    {
        DataRepo repo = new DataRepo();

        public ActionResult Post(LoginModel login)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest("User Credentials are  not valid");
            }
            else
            {
                LoginModel userInfo = repo.CheckLogin(login);
                if(userInfo == null)
                {
                    return NotFound(new ResponseModel { Error = "User Not Found" });
                }
                else
                {
                    if (DateTime.Now>userInfo.ExpireTime)
                    {
                        userInfo.Token = generateNewToken(userInfo);
                        repo.updateNewToken(userInfo);
                    }
                    return Ok(userInfo.Token);
                }

            }

        }

        private bool chkIsExpired(string expireTime)
        {
            DateTime time = DateTime.Parse(expireTime);
            return (DateTime.Now > time) ? true : false;
        }

        private string generateNewToken(LoginModel userInfo)
        {
            byte[] data = System.Text.ASCIIEncoding.ASCII.GetBytes(userInfo.Username+"|"+userInfo.Password+"|"+DateTime.Now.ToString());
            return System.Convert.ToBase64String(data);
        }
    }
}
