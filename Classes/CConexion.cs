using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ConexionSqlserver.Classes
{
    class CConexion
    {
        SqlConnection conex = new SqlConnection();
        SqlCommand sql;
        private static String server="localhost";
        private static String bd="prueba";
        private static String port= "62211";
        private static String user="sa";
        private static String password="123";

        string cadenaConexion = "Data Source=" + server + "," + port + ";" + "user id=" + user + ";" + "password=" + password + ";" + "Initial Catalog=" + bd + ";" + "Persist Security Info=true";


        public SqlConnection establecerConexion()
        {

            try
            {
                conex.ConnectionString = cadenaConexion;
                conex.Open();
                MessageBox.Show("Se ha establecido conexión con la base de datos");

                //getPersonasStr();

                MessageBox.Show(getPersonasList().First.Value.getNombre());

                deletePersona(1);

                conex.Close();
            }
            catch (SqlException e)
            {
                MessageBox.Show("No se pudo establecer conexión por el siguiente error: "+e.ToString());
            }
            return conex;
        }

        public String getPersonasStr()
        {
            string result = "";
            sql = new SqlCommand("SELECT * FROM persona;", conex);
            SqlDataReader datareader = sql.ExecuteReader();

            while (datareader.Read())
            {
                result += datareader.GetValue(0) + "-" + datareader.GetValue(1) + "-" + datareader.GetValue(2)+"\n";
            }

            MessageBox.Show(result);

            return result;
        }

        public Classes.Persona[] getPersonas()
        {
            
            Classes.Persona[] personas = new Classes.Persona [15];

            sql = new SqlCommand("SELECT * FROM persona;", conex);
            SqlDataReader datareader = sql.ExecuteReader();

            int i = 0;
            while (datareader.Read())
            {
                personas[i] = new Persona(
                    Convert.ToInt32(datareader.GetValue(0)), 
                    datareader.GetValue(1).ToString(), 
                    Convert.ToInt32(datareader.GetValue(2))
                );
                i++;
            }


            return personas;
        }

        public LinkedList<Persona> getPersonasList()
        {
            LinkedList<Persona> personas = new LinkedList<Persona>();

            sql = new SqlCommand("SELECT * FROM persona;", conex);
            SqlDataReader datareader = sql.ExecuteReader();
            while (datareader.Read())
            {
                personas.AddLast(
                    new Persona(
                        Convert.ToInt32(datareader.GetValue(0)),
                        datareader.GetValue(1).ToString(),
                        Convert.ToInt32(datareader.GetValue(2))
                        )
                    );

       
            }
            datareader.Close();
            sql.Dispose();
            return personas;
        }


        public String setPersona(String nombre, int edad)
        {
          

            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.InsertCommand = new SqlCommand("INSERT INTO persona (nombre, edad) VALUES ('"+nombre+"',"+edad+");", conex);

            String rowAfected = adapter.InsertCommand.ExecuteNonQuery().ToString();

            return rowAfected;
        }

        public void deletePersona(int id)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();

            adapter.DeleteCommand = new SqlCommand("DELETE FROM persoa WHERE id = " + id + ";", conex);
            try
            {
                adapter.DeleteCommand.ExecuteNonQuery();
                MessageBox.Show("Se ha eliminado el registro");
            }catch(SqlException e)
            {
                MessageBox.Show("ha ocurrido un error en la eliminación -- " + e);
            }
            
        }


    }
}
