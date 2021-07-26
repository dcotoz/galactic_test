using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GalacTicAdvisorsTest.Models;
using Microsoft.Extensions.Configuration;
using System.Threading;
using MySqlConnector;
namespace GalacTicAdvisorsTest.Controllers
{
    /// <summary>
    /// In this controller we will define the CRUD operations for the announcement class. Please take a look at the Readme file for assumptions and comments
    /// </summary>
    [ApiController]
    [Route("/api/Announcements")]
    public class AnnouncementController : ControllerBase
    {
        /// <summary>
        /// We inject the configuration and MySQL connection
        /// </summary>
        /// <param name="_configuration"></param>
        /// 
        private IConfiguration configuration { get; set; }
        private MySqlConnection connection { get; set; }
        public AnnouncementController(IConfiguration _configuration, MySqlConnection _connection)
        {
            configuration = _configuration;
            connection = _connection;
        }

        /// <summary>
        /// The main GET endpoint that will get the Announcements from the Database
        /// </summary>
        /// <param name="page">The page number</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAnnouncementsForUser/{page}")]
        public async Task<IEnumerable<AnnouncementModel>>  GetAnnouncementsForUser([FromRoute]int page)
        {
            await this.connection.OpenAsync();

            try
            {
                using var command = new MySqlCommand("SELECT * FROM announcements", connection);
                
                using var reader = await command.ExecuteReaderAsync();
                List<AnnouncementModel> announcements = new List<AnnouncementModel>();

                while (await reader.ReadAsync())
                {
                    AnnouncementModel model = new AnnouncementModel();
                    model.UID = (int)reader.GetValue(0);
                    model.Author = reader.GetValue(1).ToString();
                    model.Date = DateTime.Parse(reader.GetValue(2).ToString());
                    model.Subject = reader.GetValue(3).ToString();
                    model.Body = reader.GetValue(4).ToString();
                    announcements.Add(model);
                }

                await this.connection.CloseAsync();
               
                return announcements;
            }
            catch (Exception ex)
            {
                await this.connection.CloseAsync();
                throw ex;
            }
        }
    }
}
