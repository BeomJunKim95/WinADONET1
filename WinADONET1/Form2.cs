using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace WinADONET1
{
	public partial class Form2 : Form
	{
		//App.config 활용
		string strConn = ConfigurationManager.ConnectionStrings["myDB"].ConnectionString; // 이러면 문자열로 리턴을해준다

		string email = ConfigurationManager.AppSettings["email"];

		public Form2()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			MySqlConnection conn = new MySqlConnection(strConn);

			string sql = @"select emp_no, concat(first_name, ' ', last_name) emp_name
from employees
where emp_no < 10004;";
			MySqlDataAdapter da = new MySqlDataAdapter(sql, conn);
			DataSet ds = new DataSet();

			conn.Open();
			da.Fill(ds, "dtEmp");

			sql = "select dept_no, dept_name from departments;";
			da.SelectCommand.CommandText = sql;
			da.Fill(ds, "dtDept");

			conn.Close();

			dataGridView1.DataSource = ds.Tables[0];
			dataGridView2.DataSource = ds.Tables["dtDept"];
		}

		private void button2_Click(object sender, EventArgs e)
		{
			MySqlConnection conn = new MySqlConnection(strConn);

			string sql = @"select emp_no, concat(first_name, ' ', last_name) emp_name
							from employees
							where emp_no = @emp_no" ; // 어디까지 검색하는지 입력
			//MySqlDataAdapter da = new MySqlDataAdapter(sql, conn);
			MySqlDataAdapter da = new MySqlDataAdapter();

			MySqlCommand cmd = new MySqlCommand();
			cmd.CommandText = sql;
			cmd.Connection = conn;
			cmd.Parameters.Add(new MySqlParameter("@emp_no", MySqlDbType.Int32));
			cmd.Parameters[0].Value = textBox1.Text;

			da.SelectCommand = cmd;

			DataSet ds = new DataSet();

			conn.Open();
			da.Fill(ds, "dtEmp");

			sql = "select dept_no, dept_name from departments;";
			da.SelectCommand.CommandText = sql;
			da.Fill(ds, "dtDept");

			conn.Close();

			dataGridView1.DataSource = ds.Tables[0];
			dataGridView2.DataSource = ds.Tables["dtDept"];

			comboBox1.DisplayMember = "dept_name";
			comboBox1.ValueMember = "dept_no";
			comboBox1.DataSource = ds.Tables["dtDept"];
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			MessageBox.Show(comboBox1.SelectedValue.ToString());
		}
	}
}
