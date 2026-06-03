using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using GymApp.Models;
using GymApp.App_Start;
using System.Data;

namespace GymApp.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly string _conn = DbConfig.ConnectionString;


        public void Add(Member member)
        {
            using (var con = new SqlConnection(_conn))
            using (var cmd = new SqlCommand("sp_AddMember", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Fname", member.Fname);
                cmd.Parameters.AddWithValue("@Sname", member.Sname);
                cmd.Parameters.AddWithValue("@Email", member.Email);
                cmd.Parameters.AddWithValue("@Phone", member.Phone);
                cmd.Parameters.AddWithValue("@DOB", member.DOB);
                cmd.Parameters.AddWithValue("@DateRegistered", member.DateRegistered);
                con.Open();
                cmd.ExecuteNonQuery();

            }
        }

        public Member GetByName(string fname, string sname)
        {
            using (var con = new SqlConnection(_conn))
            using (var cmd = new SqlCommand("sp_GetMemberByName", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Fname", fname);
                cmd.Parameters.AddWithValue("@Sname", sname);
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read()) return null;
                    return MapMember(reader);

                }
            }
        }

        public Member GetById(int memId)
        {
            using (var con = new SqlConnection(_conn))
            using (var cmd = new SqlCommand("sp_GetMemberById", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MemID", memId);
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read()) return null;
                    return MapMember(reader);
                }
            }
        }

        public void Update(Member member, string originalFname, string originalSname)
        {
            using (var con = new SqlConnection(_conn))
            using (var cmd = new SqlCommand("sp_UpdateMember", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OriginalFname", originalFname);
                cmd.Parameters.AddWithValue("@OriginalSname", originalSname);
                cmd.Parameters.AddWithValue("@Fname", member.Fname);
                cmd.Parameters.AddWithValue("@Sname", member.Sname);
                cmd.Parameters.AddWithValue("@Email", member.Email);
                cmd.Parameters.AddWithValue("@Phone", member.Phone);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        //Handles both renew and close member accounts by passing the status as a parameter
        public void SetStatus(int memId, string status)
        {
            using (var con = new SqlConnection(_conn))
            using (var cmd = new SqlCommand("sp_SetMemberStatus", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MemID", memId);
                cmd.Parameters.AddWithValue("@Status", status);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void AddFunds(string fname, string sname, double amount)
        {
            using (var con = new SqlConnection(_conn))
            using (var cmd = new SqlCommand("sp_AddFunds", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Fname", fname);
                cmd.Parameters.AddWithValue("@Sname", sname);
                cmd.Parameters.AddWithValue("@Amount", amount);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<Member> GetByStatus(string status)
        {
            var members = new List<Member>();
            using (var con = new SqlConnection(_conn))
            using (var cmd = new SqlCommand("sp_GetMembersByStatus", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Status", status);
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        members.Add(MapMember(reader));
                    }
                }
            }
            return members;
        }

        //Helper method to map the data from the database to a Member object so that code is not repeated in multiple methods
        private Member MapMember(SqlDataReader reader) => new Member
        {
            MemID = (int)reader["MemID"],
            Fname = reader["Fname"].ToString(),
            Sname = reader["Sname"].ToString(),
            Email = reader["Email"].ToString(),
            Phone = reader["Phone"].ToString(),
            DOB = (DateTime)reader["DOB"],
            Status = reader["Status"].ToString(),
            Wallet = (int)reader["Wallet"],
            DateRegistered = (DateTime)reader["DateRegistered"]

        };
    }
}