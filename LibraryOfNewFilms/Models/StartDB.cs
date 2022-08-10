using DAL.Context;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryOfNewFilms.Models
{
	public static class StartDB
	{
		static string connectionString = ConfigurationManager.ConnectionStrings["LabraryOfNewFilms"].ConnectionString;
		public static void Start()
		{
			try
			{
				string query = @"create procedure AddRelationshipGenreFilm 
									@filmId int, @genreId int
									as
									begin
										insert into GenreFilms
										values (@filmId, @genreId)
									end;";
				SendCommand(query);
				query = @"create procedure AddRelationshipActorFilm 
							@filmId int, @actorId int
							as
							begin
								insert into FilmActors
								values (@actorId, @filmId)
							end;";
				SendCommand(query);
				query = @"create procedure AddRelationshipProducerFilm 
							@filmId int, @producerId int
							as
							begin
								insert into ProducerFilms
								values (@filmId, @producerId)
							end;";
				SendCommand(query);
				query = @"create procedure AddRelationshipUserFilm 
							@filmId int, @userId int
							as
							begin
								insert into UserFilms
								values (@filmId, @userId)
							end;";
				SendCommand(query);
				query = @"create procedure RemoveRelationshipUserFilm
							@filmId int, @userId int
							as
							begin
								delete from UserFilms
								where Film_Id = @filmId and User_UserId = @userId;
							end;";
				SendCommand(query);
			}
			catch (Exception ex)
			{

			}

		}
		public static void SendCommand(string query)
		{
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				SqlCommand cmd = new SqlCommand(query, connection);
				cmd.ExecuteNonQuery();
				connection.Close();
			}
		}
	}
}
