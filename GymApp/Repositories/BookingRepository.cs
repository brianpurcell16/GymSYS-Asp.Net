using GymApp.App_Start;
using GymApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace GymApp.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly string _conn = DbConfig.ConnectionString;

        public void CreateBooking(int memId, int classId, decimal price)
        {
            using (var connection = new SqlConnection(_conn))
            using (var command = new SqlCommand("sp_AddBooking", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@MemId", memId);
                command.Parameters.AddWithValue("@ClassId", classId);
                command.Parameters.AddWithValue("@Price", price);
                command.Parameters.AddWithValue("@DateBooked", DateTime.Now);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public Booking GetById(int bookingId)
        {
            using (var connection = new SqlConnection(_conn))
            using (var command = new SqlCommand("sp_GetBookingById", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@BookingId", bookingId);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (!reader.Read()) return null;
                    return MapBooking(reader);
                }
            }
        }

        public List<Booking> GetActiveBookings()
        {
            var bookings = new List<Booking>();
            using (var connection = new SqlConnection(_conn))
            using (var command = new SqlCommand("sp_GetActiveBookings", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        bookings.Add(MapBooking(reader));
                    }
                }
            }
            return bookings;
        }

        public void CancelBooking(int bookingId)
        {
            using (var connection = new SqlConnection(_conn))
            using (var command = new SqlCommand("sp_CancelBooking", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@BookingId", bookingId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public bool DeductWallet(string fname, string sname, int classId)
        {
            using (var connection = new SqlConnection(_conn))
            using (var command = new SqlCommand("sp_DeductWallet", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FName", fname);
                command.Parameters.AddWithValue("@LName", sname);
                command.Parameters.AddWithValue("@ClassId", classId);

                //this is used as the stored procedure will set the output parameter to 1 if the deduction was successful, otherwise it remains 0
                var successParam = new SqlParameter("@Success", SqlDbType.Bit) 
                { 
                    Direction = ParameterDirection.Output 
                };
                command.Parameters.Add(successParam);
                connection.Open();
                command.ExecuteNonQuery();
                return (int)successParam.Value == 1;
            }
        }

        public List<(string Month, decimal Total)> GetRevenueByYear(int year)
        {
            var revenue = new List<(string Month, decimal Total)>();
            using (var connection = new SqlConnection(_conn))
            using (var command = new SqlCommand("sp_GetRevenueByYear", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Year", year);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        revenue.Add((reader["Month"].ToString(), (decimal)reader["Total"]));
                    }
                }
            }
            return revenue;
        }

        private Booking MapBooking(SqlDataReader reader) => new Booking
        {
            BookingId = (int)reader["BookingId"],
            MemId = (int)reader["MemId"],
            ClassId = (int)reader["ClassId"],
            Price = (decimal)reader["Price"],
            DateBooked = (DateTime)reader["DateBooked"],
            Status = reader["Status"].ToString()
        };

    }
}