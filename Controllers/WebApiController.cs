using Microsoft.AspNetCore.Mvc;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System;
using api.Data;
using Microsoft.Data.SqlClient;
using System.Net.Http.Headers;
using System.Collections.Generic;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebApiController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public WebApiController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IConfigurationRoot Configuration { get; set; }
        [HttpPost("PostData")]
        [AllowAnonymous]
        public async Task<IActionResult> PostData([FromBody]string[] objData)
        {
            #region  Post Data
            using(var _dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    string pageSize = Request.Headers["PageSize"];
                    List<Names> list=new List<Names>();
                    foreach(string item in objData)
                    {
                        Names obj=new Names();
                        obj.Name=item;
                        obj.PageSize=Convert.ToInt32(pageSize);
                        list.Add(obj);
                        _dbContext.Names.Add(obj);
                        _dbContext.SaveChanges();
                    }
                    _dbContextTransaction.Commit();
                    return Ok(list);
                }
                catch (Exception ex)
                {
                    if(_dbContextTransaction!=null)
                        _dbContextTransaction.Rollback();
                    if (ex.InnerException != null)
                        return BadRequest(ex.InnerException.Message);
                    else
                        return BadRequest(ex.Message);
                }
            }
            #endregion
        }
        [HttpGet("GetData")]
        [AllowAnonymous]
        public async Task<IActionResult> GetData()
        {
            #region  Get Data
            try
            {
                var data=_dbContext.Names;
                int n=0;
                string nameWrap="";
                foreach(Names item in data)
                {   
                    n=item.PageSize;
                    nameWrap+=item.Name+" ";
                }
                try{
                    nameWrap=nameWrap.Substring(n)+"..";
                }catch(Exception exp){}
                return Ok(new {data=nameWrap});
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                        return BadRequest(ex.InnerException.Message);
                else
                    return BadRequest(ex.Message);
            }
            #endregion
        }

        
    }
    
}