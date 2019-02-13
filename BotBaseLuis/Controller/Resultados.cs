using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BotBaseLuis.Controller
{
    public class Resultados
    {
        public string GetResultados(string equipoLocal, string equipoVisita)
        {
            SqlConnection conn = new SqlConnection(@"Data Source=localhost\SQLEXPRESS;Initial Catalog=BotFutbol;Integrated Security=True");
            conn.Open();

            SqlCommand command = new SqlCommand("SELECT * " +
                                                "FROM Resultados " +
                                                "INNER JOIN Equipo as EquipoLocal " +
                                                "ON EquipoLocal.ID = Resultados.ID_Local " +
                                                "INNER JOIN Equipo as EquipoVisita " +
                                                "ON EquipoVisita.ID = Resultados.ID_Visita " +
                                                "WHERE EquipoLocal.Nombre = '"+equipoLocal+"' " +
                                                "AND EquipoVisita.Nombre = '"+ equipoVisita+"';", conn);
            //command.Parameters.AddWithValue("@equipoLocal", equipoLocal);
            //command.Parameters.AddWithValue("@equipoVisita", equipoVisita);
            // int result = command.ExecuteNonQuery();
            string resultado = "";
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {

                    resultado = "Estos wns salieron: " + reader["Goles_Local"].ToString() + "-" + reader["Goles_Visita"].ToString() + " en " + reader["Campeonato"].ToString();
                }
            }

            if (resultado == "")
            {
                resultado = "No se encontraron resultados";
            }
            conn.Close();
            return resultado;
        }
        public string GetResultados(string equipoLocal, string equipoVisita, int anio)
        {
            SqlConnection conn = new SqlConnection(@"Data Source=localhost\SQLEXPRESS;Initial Catalog=BotFutbol;Integrated Security=True");
            conn.Open();

            SqlCommand command = new SqlCommand("SELECT * " +
                                                "FROM Resultados " +
                                                "INNER JOIN Equipo as EquipoLocal " +
                                                "ON EquipoLocal.ID = Resultados.ID_Local " +
                                                "INNER JOIN Equipo as EquipoVisita " +
                                                "ON EquipoVisita.ID = Resultados.ID_Visita " +
                                                "WHERE EquipoLocal.Nombre = '" + equipoLocal + "' " +
                                                "AND YEAR(FECHA) = "+ anio + " " +
                                                "AND EquipoVisita.Nombre = '" + equipoVisita + "';", conn);
            //command.Parameters.AddWithValue("@equipoLocal", equipoLocal);
            //command.Parameters.AddWithValue("@equipoVisita", equipoVisita);
            // int result = command.ExecuteNonQuery();
            string resultado = "";
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {

                    resultado = "Estos wns salieron: " + reader["Goles_Local"].ToString() + "-" + reader["Goles_Visita"].ToString() + " en el " + reader["Campeonato"].ToString();
                }
            }

            if (resultado == "")
            {
                resultado = "No se encontraron resultados";
            }
            conn.Close();
            return resultado;
        }
    }
}
