using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Shop.Pages.Admin
{
    public class IndexModel : PageModel
    {
        public List<MessageInfo> listMessages= new List<MessageInfo>();
        public int page = 1;
        public int totalPages = 0;
        private readonly int pageSize = 10;
        public void OnGet()
        {
            page = 1;
            string requestPage = Request.Query["page"];
            if(requestPage!=null)
            {
                try
                {
                    page=int.Parse(requestPage);
                }
                catch (Exception ex)
                {
                    page = 1;

                }
            }
            try
            {
                string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Shop;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sqlCount = "SELECT COUNT(*) FROM messages";
                    using(SqlCommand sqlCommand = new SqlCommand(sqlCount,connection)) 
                    {
                        decimal count=(int)sqlCommand.ExecuteScalar();
                        totalPages = (int)Math.Ceiling(count / pageSize);
                        
                    }
                    string sql = "SELECT * FROM messages order by ID ASC"+
                        " OFFSET @skip ROWS FETCH NEXT @pageSize ROWS ONLY";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@skip", (page - 1) * pageSize);
                        command.Parameters.AddWithValue("@pageSize", pageSize);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                MessageInfo messageInfo = new MessageInfo();
                                messageInfo.Id = reader.GetInt32(0);
                                messageInfo.FirstName = reader.GetString(1);
                                messageInfo.LastName = reader.GetString(1);
                                messageInfo.Email = reader.GetString(1);
                                messageInfo.Phone = reader.GetString(1);
                                messageInfo.Subject = reader.GetString(1);
                                messageInfo.CreatedAt = reader.GetDateTime(7).ToString("MM/dd//yyyy");
                                listMessages.Add(messageInfo);
                            }
                        }
                    }
                 
                }
            }

            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }
        public class MessageInfo
        {
            public int Id { get; set; }
            public string FirstName { get; set; } = "";
            public string LastName { get; set; } = "";
            public string Email { get; set; } = "";
            public string Phone { get; set; } = "";
            public string Subject { get; set; } = "";
            public string Message { get; set; } = "";

            public string CreatedAt { get; set; } = "";




        }
    }
}
