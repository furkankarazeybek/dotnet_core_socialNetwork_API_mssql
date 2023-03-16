using Microsoft.Data.SqlClient;
using SocialNetworkAPI.Models;
using System.Collections.Generic;
using System.Data;

namespace SocialNetworkAPI.DAL
{
    public class Dal
    {
        public Response Registration(Registration registration, SqlConnection connection)
        {
            Response response = new Response();

            //SqlCommand cmd = new SqlCommand("INSERT INTO Registration(Name,Email,Password,PhoneNo,IsActive,IsApproved) VALUES('" + registration.Name +"','" + registration.Email + "','" + registration.Email + "','" + registration.Password + "','" + registration.PhoneNo + "',1,0) ",connection);

            try
            {
                SqlCommand cmd = new SqlCommand("sp_register", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", registration.Name);
                cmd.Parameters.AddWithValue("@Email", registration.Email);
                cmd.Parameters.AddWithValue("@Password", registration.Password);
                cmd.Parameters.AddWithValue("@PhoneNo", registration.PhoneNo);


                cmd.Parameters.Add("@ErrorMessage", System.Data.SqlDbType.Char, 200);
                cmd.Parameters["@ErrorMessage"].Direction = System.Data.ParameterDirection.Output;

                connection.Open();
                int i = cmd.ExecuteNonQuery();
                connection.Close();
                string message = (string)cmd.Parameters["@ErrorMessage"].Value;

                if (i > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = message;

                }
                else
                {
                    response.StatusCode = 404;
                    response.StatusMessage = message;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 404;
                response.StatusMessage = ex.Message;
            }

            return response;
        }

        public Response Login(Registration registration, SqlConnection connection)
        {

            Response response = new Response();


            try
            {
                SqlDataAdapter da = new SqlDataAdapter("sp_login", connection);
                da.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@Email", registration.Email);
                da.SelectCommand.Parameters.AddWithValue("@Password", registration.Password);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = "User is valid";
                }
                else
                {
                    response.StatusCode = 404;
                    response.StatusMessage = "User is invalid";
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 404;
                response.StatusMessage = ex.Message;
            }

            return response;

        }

        public Response UserApproval(Registration registration, SqlConnection connection)
        {
            Response response = new Response();

            SqlCommand cmd = new SqlCommand("UPDATE Registration SET IsApproved = 1 WHERE Id = '" + registration.Id + "' AND IsActive = 1 ", connection);
            connection.Open();
            int i = cmd.ExecuteNonQuery();
            if (i > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "User approved";
            }
            else
            {
                response.StatusCode = 400;
                response.StatusMessage = "User approval failed";
            }

            return response;
        }

        public Response AddNews(News news, SqlConnection connection)
        {


            Response response = new Response();

            try
            {
                SqlCommand cmd = new SqlCommand("sp_news", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Title", news.Title);
                cmd.Parameters.AddWithValue("@Content", news.Content);
                cmd.Parameters.AddWithValue("@Email", news.Email);


                connection.Open();
                int i = cmd.ExecuteNonQuery();
                connection.Close();

                if (i > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = "News added";

                }
                else
                {
                    response.StatusCode = 404;
                    response.StatusMessage = "News failed with adding";
                }

            }

            catch (Exception ex)
            {
                response.StatusCode = 404;
                response.StatusMessage = ex.Message;
            }




            return response;
        }

        public Response NewsList(SqlConnection connection)
        {
            Response response = new Response();


            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM News WHERE IsActive=1", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<News> lstNews = new List<News>();
            if (dt.Rows.Count > 0)
            {

                for(int i=0; i < dt.Rows.Count; i++)
                {
                    News news = new News();
                    news.Id = Convert.ToInt32(dt.Rows[i]["Id"]);
                    news.Title = Convert.ToString(dt.Rows[i]["Title"]);
                    news.Content = Convert.ToString(dt.Rows[i]["Content"]);
                    news.Email = Convert.ToString(dt.Rows[i]["Email"]);
                    news.IsActive = Convert.ToInt32(dt.Rows[i]["IsActive"]);
                    news.CreatedOn = Convert.ToDateTime(dt.Rows[i]["CreatedOn"]);
                    lstNews.Add(news);

                }
                if(lstNews.Count > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = "News data found";
                    response.listNews = lstNews;
                }
                else
                {
                    response.StatusCode = 404;
                    response.StatusMessage = "No news data found";
                    response.listNews = null;
                }

            }
            else
            {
                  response.StatusCode = 404;
                    response.StatusMessage = "No news data found";
                    response.listNews = null;
            }

            return response;
        }


        public Response AddArticle(Article article, SqlConnection connection)
        {


            Response response = new Response();

            try
            {
                SqlCommand cmd = new SqlCommand("sp_article", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Title", article.Title);
                cmd.Parameters.AddWithValue("@Content", article.Content);
                cmd.Parameters.AddWithValue("@Email", article.Email);
                cmd.Parameters.AddWithValue("@Image", article.Image);
                cmd.Parameters.AddWithValue("@Type", article.Type);


                connection.Open();
                int i = cmd.ExecuteNonQuery();
                connection.Close();

                if (i > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = "Article added";

                }
                else
                {
                    response.StatusCode = 404;
                    response.StatusMessage = "Article failed with adding";
                }

            }

            catch (Exception ex)
            {
                response.StatusCode = 404;
                response.StatusMessage = ex.Message;
            }




            return response;
        }

        public Response ArticleList(Article article, SqlConnection connection)
        {
            Response response = new Response();

          

            if(article.Type == "User") {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Article WHERE Email = '" + article.Email + "' AND IsActive=1", connection);
                DataTable dt = new DataTable();
                da.Fill(dt);
                List<Article> listArticle = new List<Article>();
                if (dt.Rows.Count > 0)
                {

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Article article1 = new Article();
                        article1.Id = Convert.ToInt32(dt.Rows[i]["Id"]);
                        article1.Title = Convert.ToString(dt.Rows[i]["Title"]);
                        article1.Content = Convert.ToString(dt.Rows[i]["Content"]);
                        article1.Email = Convert.ToString(dt.Rows[i]["Email"]);
                        article1.Image = Convert.ToString(dt.Rows[i]["Image"]);
                        article1.IsActive = Convert.ToInt32(dt.Rows[i]["IsActive"]);
                        article1.IsApproved = Convert.ToInt32(dt.Rows[i]["IsApproved"]);
                        listArticle.Add(article1);

                    }
                    if (listArticle.Count > 0)
                    {
                        response.StatusCode = 200;
                        response.StatusMessage = "Article data found";
                        response.listArticle = listArticle;
                    }
                    else
                    {
                        response.StatusCode = 404;
                        response.StatusMessage = "No Article data found";
                        response.listArticle = null;
                    }

                }
                else
                {
                    response.StatusCode = 404;
                    response.StatusMessage = "No Article data found";
                    response.listArticle = null;
                }

            }
            
            if(article.Type == "Page")
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Article WHERE  IsActive=1", connection);
                DataTable dt = new DataTable();
                da.Fill(dt);
                List<Article> listArticle = new List<Article>();
                if (dt.Rows.Count > 0)
                {

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Article article1 = new Article();
                        article1.Id = Convert.ToInt32(dt.Rows[i]["Id"]);
                        article1.Title = Convert.ToString(dt.Rows[i]["Title"]);
                        article1.Content = Convert.ToString(dt.Rows[i]["Content"]);
                        article1.Email = Convert.ToString(dt.Rows[i]["Email"]);
                        article1.Image = Convert.ToString(dt.Rows[i]["Image"]);
                        article1.IsActive = Convert.ToInt32(dt.Rows[i]["IsActive"]);
                        article1.IsApproved = Convert.ToInt32(dt.Rows[i]["IsApproved"]);
                        listArticle.Add(article1);

                    }
                    if (listArticle.Count > 0)
                    {
                        response.StatusCode = 200;
                        response.StatusMessage = "Article data found";
                        response.listArticle = listArticle;
                    }
                    else
                    {
                        response.StatusCode = 404;
                        response.StatusMessage = "No Article data found";
                        response.listArticle = null;
                    }

                }
                else
                {
                    response.StatusCode = 404;
                    response.StatusMessage = "No Article data found";
                    response.listArticle = null;
                }

            }
        





            return response;
        }

        public Response ArticleApproval(Article article, SqlConnection connection)
        {
            Response response = new Response();

            SqlCommand cmd = new SqlCommand("UPDATE Article SET IsApproved = 1 WHERE Id = '" + article.Id + "' AND IsActive = 1 ", connection);
            connection.Open();
            int i = cmd.ExecuteNonQuery();
            if (i > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "Staff deleted";
            }
            else
            {
                response.StatusCode = 400;
                response.StatusMessage = "Staff deleting failed";
            }

            return response;
        }


        public Response StaffRegistration(Staff staff, SqlConnection connection)
        {
            Response response = new Response();


            try
            {
                SqlCommand cmd = new SqlCommand("sp_staffregister", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", staff.Name);
                cmd.Parameters.AddWithValue("@Email", staff.Email);
                cmd.Parameters.AddWithValue("@Password", staff.Password);
                


                cmd.Parameters.Add("@ErrorMessage", System.Data.SqlDbType.Char, 200);
                cmd.Parameters["@ErrorMessage"].Direction = System.Data.ParameterDirection.Output;

                connection.Open();
                int i = cmd.ExecuteNonQuery();
                connection.Close();
                string message = (string)cmd.Parameters["@ErrorMessage"].Value;

                if (i > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = message;

                }
                else
                {
                    response.StatusCode = 404;
                    response.StatusMessage = message;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 404;
                response.StatusMessage = ex.Message;
            }

            return response;
        }

        public Response DeleteStaff(Staff staff, SqlConnection connection)
        {
            Response response = new Response();

            SqlCommand cmd = new SqlCommand("DELETE FROM Staff WHERE ID = '"+ staff.Id +"' AND IsActive=1  ", connection);
            connection.Open();
            int i = cmd.ExecuteNonQuery();
            if (i > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "Staff deleted";
            }
            else
            {
                response.StatusCode = 400;
                response.StatusMessage = "Staff deleting failed";
            }

            return response;
        }

        public Response AddEvent(Events events, SqlConnection connection)
        {


            Response response = new Response();

            try
            {
                SqlCommand cmd = new SqlCommand("sp_events", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Title", events.Title);
                cmd.Parameters.AddWithValue("@Content", events.Content);
                cmd.Parameters.AddWithValue("@Email", events.Email);
         


                connection.Open();
                int i = cmd.ExecuteNonQuery();
                connection.Close();

                if (i > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = "Event added";

                }
                else
                {
                    response.StatusCode = 404;
                    response.StatusMessage = "Event failed with adding";
                }

            }

            catch (Exception ex)
            {
                response.StatusCode = 404;
                response.StatusMessage = ex.Message;
            }




            return response;
        }

        public Response EventsList(SqlConnection connection)
        {
               Response response = new Response();


                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Events WHERE  IsActive=1", connection);
                DataTable dt = new DataTable();
                da.Fill(dt);
                List<Events> listEvents= new List<Events>();
                if (dt.Rows.Count > 0)
                {

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Events article1 = new Events();
                        article1.Id = Convert.ToInt32(dt.Rows[i]["Id"]);
                        article1.Title = Convert.ToString(dt.Rows[i]["Title"]);
                        article1.Content = Convert.ToString(dt.Rows[i]["Content"]);
                        article1.Email = Convert.ToString(dt.Rows[i]["Email"]);
                        article1.IsActive = Convert.ToInt32(dt.Rows[i]["IsActive"]);
                        article1.CreatedOn = Convert.ToDateTime(dt.Rows[i]["CreatedOn"]);

                    listEvents.Add(article1);

                    }
                    if (listEvents.Count > 0)
                    {
                        response.StatusCode = 200;
                        response.StatusMessage = "Article data found";
                        response.listEvents = listEvents;
                    ;
                }
                    else
                    {
                        response.StatusCode = 404;
                        response.StatusMessage = "No Article data found";
                        response.listEvents = null;
                    }

                }
                else
                {
                    response.StatusCode = 404;
                    response.StatusMessage = "No Article data found";
                    response.listEvents = null;
                }

            






            return response;
        }

    }
}

