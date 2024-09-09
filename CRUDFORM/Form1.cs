using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace CRUDFORM
{
    public partial class Form1 : Form
    {
        private string connectionString = "Server=localhost;Database=data;User ID=root;Password=12345;";

        public Form1()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();

                    string query = "SELECT * FROM data";
                    using (MySqlDataAdapter da = new MySqlDataAdapter(query, con))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dataGridView1.DataSource = dt;

                        dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
                        dataGridView1.RowTemplate.Height = 40;
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Database error occurred. Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred. Error: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();

                    
                    string query = "UPDATE data SET name = @name, age = @age WHERE id = @id";
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@id", int.Parse(textBox1.Text));
                        cmd.Parameters.AddWithValue("@name", textBox3.Text);
                        cmd.Parameters.AddWithValue("@age", int.Parse(textBox2.Text));

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Record updated successfully!");
                LoadData(); 
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Please enter valid data. Error: " + ex.Message);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Database error occurred. Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred. Error: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();

                    
                    string query = "INSERT INTO data (id, name, age) VALUES (@id, @name, @age)";
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@id", int.Parse(textBox1.Text));
                        cmd.Parameters.AddWithValue("@name", textBox3.Text);
                        cmd.Parameters.AddWithValue("@age", int.Parse(textBox2.Text));

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Record inserted successfully!");
                LoadData(); 
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Please enter valid data. Error: " + ex.Message);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Database error occurred. Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred. Error: " + ex.Message);
            }
        }
        private void button4_Click (object sender, EventArgs e)
        {
            string searchText = textBox1.Text.Trim();

            if (string.IsNullOrEmpty(searchText))
            {
                MessageBox.Show("Please enter a search query.");
                return;
            }

            
            DataTable result = SearchRecords(searchText);
            dataGridView1.DataSource = result;

            
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridView1.RowTemplate.Height = 40; 
        }

        private DataTable SearchRecords(string searchText)
        {
            DataTable dt = new DataTable();

            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();

                    string query = "SELECT * FROM data WHERE id = @searchText OR name = @searchText";
                    using (MySqlDataAdapter da = new MySqlDataAdapter(query, con))
                    {
                        da.SelectCommand.Parameters.AddWithValue("@searchText", searchText);
                        da.Fill(dt);
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Database error occurred. Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred. Error: " + ex.Message);
            }

            return dt;
        }



        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(textBox1.Text);

                if (!RecordExists(id))
                {
                    MessageBox.Show("ID not found.");
                    return;
                }

                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();

                    
                    string query = "DELETE FROM data WHERE id = @id";
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@id", id);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Record deleted successfully!");
                LoadData(); 
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Please enter a valid ID. Error: " + ex.Message);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Database error occurred. Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred. Error: " + ex.Message);
            }
        }

        private bool RecordExists(int id)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                string query = "SELECT COUNT(*) FROM data WHERE id = @id";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

    }
}
