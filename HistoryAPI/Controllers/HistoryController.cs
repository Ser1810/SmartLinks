using DB;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace HistoryAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HistoryController : ControllerBase
    {
        [HttpPost("add")]
        public IActionResult Add([FromBody] Record record)
        {
            try
            {
                string insertSql = "INSERT INTO \"History\" (\"Url\", \"RedirectURL\", \"DateTime\", \"Headers\") VALUES (@value1, @value2, @value3, @value4)";

                var parameters = new NpgsqlParameter[]
                {
                    new NpgsqlParameter("@value1", record.URL),
                    new NpgsqlParameter("@value2", record.RedirectURL),
                    new NpgsqlParameter("@value3", record.DateTime),
                    new NpgsqlParameter("@value4", record.Headers)
                };

                SqlExecutor.ExecuteInsert(insertSql, parameters);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }

    public class Record
    {
        public string URL { get; set; }

        public string RedirectURL { get; set; }

        public DateTime? DateTime { get; set; }

        public string Headers { get; set; }
    }
}
