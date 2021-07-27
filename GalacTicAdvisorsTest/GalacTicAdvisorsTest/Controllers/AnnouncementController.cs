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
        private int PAGE_SIZE = 25;
        public AnnouncementController(IConfiguration _configuration, MySqlConnection _connection)
        {
            configuration = _configuration;
            connection = _connection;
        }

        /// <summary>
        /// The main GET endpoint that will get the Announcements from the Database
        /// </summary>
        /// <param name="page">The page number (1 .... infinity?)</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAnnouncementsForUser/{page}")]
        public async Task<IEnumerable<AnnouncementModel>> GetAnnouncementsForUser([FromRoute] int page)
        {
            await this.connection.OpenAsync();

            try
            {
                // PAGE NUMBER WILL BE ONE BASED
                int offset = (page - 1) * this.PAGE_SIZE;


                if (offset < 0)
                {
                    offset = 0;
                }

                using var command = new MySqlCommand($"SELECT * FROM announcements LIMIT {offset},{this.PAGE_SIZE}", connection);

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

        /// <summary>
        /// We assume that it's one at a time
        /// </summary>
        /// <returns>true or false if the operation was successful</returns>
        [HttpDelete]
        [Route("{uid}")]
        public async Task<bool> DeleteAnnouncement([FromRoute] int uid)
        {
            await this.connection.OpenAsync();

            try
            {
                using var command = new MySqlCommand($"DELETE FROM announcements WHERE uid = {uid}", connection);
                await command.ExecuteNonQueryAsync();
                await this.connection.CloseAsync();

                return true;
            }
            catch (Exception ex)
            {
                await this.connection.CloseAsync();
                throw ex;
            }
        }

        /// <summary>
        /// Create operation using POST
        /// </summary>
        /// <param name="announcement"></param>
        /// <returns>true/false</returns>
        [HttpPost]
        public async Task<bool> CreateAnnouncement([FromBody] AnnouncementModel announcement)
        {
            await this.connection.OpenAsync();

            try
            {
                using var command = new MySqlCommand($"INSERT INTO announcements (author,date,subject,body) VALUES (@author, @date, @subject, @body)", connection);
                command.Parameters.Add(new MySqlParameter("@author", announcement.Author));
                command.Parameters.Add(new MySqlParameter("@date", announcement.Date.ToString("yyyy-MM-dd")));
                command.Parameters.Add(new MySqlParameter("@subject", announcement.Subject));
                command.Parameters.Add(new MySqlParameter("@body", announcement.Body));

                await command.ExecuteNonQueryAsync();
                await this.connection.CloseAsync();

                return true;
            }
            catch (Exception ex)
            {
                await this.connection.CloseAsync();
                throw ex;
            }
        }

        [HttpPut]
        [Route("{uid}")]
        public async Task<bool> UpdateAnnouncement([FromRoute] int uid, [FromBody] AnnouncementModel announcement)
        {
            // WE COULD DO A SANITY CHECK TO SEE IF THE ITEM ACTUALLY EXISTS, BUT DUE TO TIME CONSTRAINTS WE'LL SKIP IT
            await this.connection.OpenAsync();

            try
            {
                using var command = new MySqlCommand($"UPDATE announcements SET author = @author, date = @date, subject = @subject, body =  @body WHERE uid = {uid}", connection);
                command.Parameters.Add(new MySqlParameter("@author", announcement.Author));
                command.Parameters.Add(new MySqlParameter("@date", announcement.Date.ToString("yyyy-MM-dd")));
                command.Parameters.Add(new MySqlParameter("@subject", announcement.Subject));
                command.Parameters.Add(new MySqlParameter("@body", announcement.Body));

                await command.ExecuteNonQueryAsync();
                await this.connection.CloseAsync();

                return true;
            }
            catch (Exception ex)
            {
                await this.connection.CloseAsync();
                throw ex;
            }
        }
    }
}
