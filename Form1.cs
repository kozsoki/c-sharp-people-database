﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;


namespace Greencode.DatabaseConnection.KorosiZsombor
{
    
    public partial class Form1 : Form
    {
        public DataGridView dataGridView1 = new DataGridView();

        private MySqlConnection connect;

        private PersonDTO personDTO;

        private Person person;

        public Form1() 
        {     
            InitializeComponent();

            string ConnectString = "Server=localhost;Database=people;Uid=root;Pwd=12345678;";

            MySqlConnection connect = new MySqlConnection(ConnectString);
            
            personDTO = new PersonDTO(connect);

            Init();

            load_data();
        }

        void load_data()
        {
            List<Person> people = personDTO.FindAll();
                    
            foreach (Person person in people)
            {
                object[] row = new object[] { person.Id.ToString(), person.Firstname, person.Lastname };
                
                dataGridView1.Rows.Add(row);
            }
        }
             
        public void Init()
        {
            dataGridView1.ColumnCount = 3;

            dataGridView1.Columns[0].Name = "ID";

            dataGridView1.Columns[1].Name = "First Name";

            dataGridView1.Columns[2].Name = "Last Name";

            DataGridViewButtonColumn deletebuttonColumn = new DataGridViewButtonColumn();

            deletebuttonColumn.HeaderText = "Delete";

            deletebuttonColumn.Name = "Delete";

            deletebuttonColumn.Text = "Delete";

            deletebuttonColumn.UseColumnTextForButtonValue = true;

            dataGridView1.Columns.Add(deletebuttonColumn);

            dataGridView1.CellClick += new DataGridViewCellEventHandler(dataGridView1_CellDeleteClick);

            DataGridViewButtonColumn editbuttonColumn = new DataGridViewButtonColumn();

            editbuttonColumn.HeaderText = "Edit";

            editbuttonColumn.Name = "Edit";

            editbuttonColumn.Text = "Edit";

            editbuttonColumn.UseColumnTextForButtonValue = true;

            dataGridView1.Columns.Add(editbuttonColumn);

            dataGridView1.CellClick += new DataGridViewCellEventHandler(dataGridView1_CellEditClick);
        }
        public void RefreshTable()
        {
            dataGridView1.Rows.Clear();

            load_data();
        }
       
      
        private void dataGridView1_CellDeleteClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (e.ColumnIndex == dataGridView1.Columns["Delete"].Index && e.RowIndex >= 0 )
            {
                personDTO.Delete(Int32.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString()));

                MessageBox.Show("ID= "+dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString()+" deleted!");

                RefreshTable();
            }
        }
        
        private void dataGridView1_CellEditClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Edit")
            {
                Form2 form2 = new Form2(this);

                person = personDTO.FindOne(Int32.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString()));

                ((TextBox)form2.Controls["textBox1"]).Text = person.Id.ToString();

                ((TextBox)form2.Controls["first_name_txt"]).Text = person.Firstname;

                ((TextBox)form2.Controls["last_name_txt"]).Text = person.Lastname;
                
                form2.Show();
            }
        }

        private void btn_addpperson_Click_1(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(this);

            form2.SetId(0);

            form2.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

