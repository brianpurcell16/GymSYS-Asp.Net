using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using GymApp.Models;
using System.Data;
using GymApp.App_Start;

namespace GymApp.Repositories
{
    public class ClassRepository : IClassRepository
    {
        private readonly string _conn = DbConfig.ConnectionString;
        public void Add(GymClass gymClass)
        {
            using (var con = new SqlConnection(_conn))
            using (var cmd = new SqlCommand("sp_AddClass", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Title", gymClass.Title);
                cmd.Parameters.AddWithValue("@Price", gymClass.Price);
                cmd.Parameters.AddWithValue("@CTime", gymClass.CTime);
                cmd.Parameters.AddWithValue("@CDate", gymClass.CDate);
                cmd.Parameters.AddWithValue("@Capacity", gymClass.Capacity);
                cmd.Parameters.AddWithValue("@AvailableSpaces", gymClass.Capacity);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public GymClass GetById(int classId)
        {
            using (var con = new SqlConnection(_conn))
            using (var cmd = new SqlCommand("sp_GetClassById", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ClassID", classId);
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read()) return null;
                    return MapClass(reader);
                }
            }
        }

        public List<GymClass> GetActiveClasses()
        {
            var classes = new List<GymClass>();
            using (var con = new SqlConnection(_conn))
            using (var cmd = new SqlCommand("sp_GetActiveClasses", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        classes.Add(MapClass(reader));
                    }
                }
            }
            return classes;
        }

        public void Cancel(int classId)
        {
            using (var con = new SqlConnection(_conn))
            using (var cmd = new SqlCommand("sp_CancelClass", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ClassID", classId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public bool DecreaseSpaces(int classId)
        {
            using (var con = new SqlConnection(_conn))
            using (var cmd = new SqlCommand("sp_DecreaseSpaces", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ClassID", classId);
                con.Open();
                int rowsAffected = Convert.ToInt32(cmd.ExecuteScalar());
                return rowsAffected > 0; 
            }
        }

        public void IncreaseSpaces(int classId)
        {
            using (var con = new SqlConnection(_conn))
            using (var cmd = new SqlCommand("sp_IncreaseSpaces", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ClassID", classId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private GymClass MapClass(SqlDataReader reader) => new GymClass
        {
            ClassID = (int)reader["ClassID"],
            Title = reader["Title"].ToString(),
            Price = (int)reader["Price"],
            Status = reader["Status"].ToString(),
            CTime = reader["CTime"].ToString(),
            CDate = (DateTime)reader["CDate"],
            Capacity = (int)reader["Capacity"],
            AvailableSpaces = (int)reader["AvailableSpaces"]
        };
    }
}