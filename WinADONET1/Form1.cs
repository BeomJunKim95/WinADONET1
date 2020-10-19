using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WinADONET1
{
	public partial class Form1 : Form
	{
		string strConn = "Server=127.0.0.1,3306;Uid=root;pwd=1234;Database = employees";

		public Form1()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			try
			{
				//MySQL전용 프로바이더 사용
				/*using*/ /*(*/
				MySqlConnection conn = new MySqlConnection("Server=127.0.0.1,3306;Uid=root;pwd=1234;Database = employees");/*)*/// 레퍼런스 타입이니까 new해줘야함
				// 이렇게 생성자에도 커넥션 구문을 넣어서 해도 된다 (제일 자주사용)
				//{
					//conn.ConnectionString = "Server=127.0.0.1,3306;Uid=root;pwd=1234;Database = employees";

					conn.Open();// 커넥션이 들어가면 오픈해줘야함
					MessageBox.Show("DB연결성공!");
					conn.Close(); //close까먹을수도 있으니 이것도 using으로 묶을수있음	

				//}
			}
			catch(Exception err)
			{
				MessageBox.Show(err.Message);
			}
			// 이런식으로도 가능
			//try
			//{
			//	string strConn = "Server=127.0.0.1,3306;Uid=root;pwd=1234;Database = employees";
		    //							// 3306포트번호는 생략가능 => 기본포트번호를 쓸거면 생략해도 된다
			//	MySqlConnection conn = new MySqlConnection(strConn);
			//	conn.Open();
			//	MessageBox.Show("DB연결성공!");
			//	conn.Close();
			//}
			//catch (Exception err)
			//{
			//	MessageBox.Show(err.Message);
			//}
		}

		private void button2_Click(object sender, EventArgs e) //사원조회 버튼클릭
		{
			try
			{
				MySqlConnection conn = new MySqlConnection(strConn); // 매번 커넥션스트링을 쓸수 없으니 전역으로빼줘서 계속 쓴다
				conn.Open();
				
				MySqlCommand cmd = new MySqlCommand();
				cmd.Connection = conn;
				cmd.CommandText = @"select emp_no, concat(first_name, ' ', last_name) emp_name
									from employees
									where emp_no < 10004";
				MySqlDataReader reader = cmd.ExecuteReader();
				reader.Read();//다음줄로 커서를 이동시켜줌
				label1.Text = reader["emp_no"].ToString(); //오브젝트타입이라 ToString해줘야함
				textBox1.Text = reader["emp_name"].ToString();

				reader.Read();
				label2.Text = reader[0].ToString(); //이렇게 인덱스를 주어도 된다
				textBox2.Text = reader[1].ToString();// 하지만 그냥 컬럼명을 쓰는게 더 좋음

				reader.Read();
				label3.Text = reader.GetString("emp_no"); //이렇게 형변환해서 주는 방법도 있다
				textBox3.Text = reader.GetString("emp_name");

				conn.Close(); // 이렇게 할거 다하고 닫아주는게 연결지향방법이다


			}
			catch(Exception err)
			{
				MessageBox.Show(err.Message);
			}
		}

		private void button3_Click(object sender, EventArgs e)
		{
			try
			{
				MySqlConnection conn = new MySqlConnection(strConn); // 매번 커넥션스트링을 쓸수 없으니 전역으로빼줘서 계속 쓴다
				//conn.Open();

				MySqlCommand cmd = new MySqlCommand();
				cmd.Connection = conn;
				cmd.CommandText = @"select emp_no, concat(first_name, ' ', last_name) emp_name
									from employees
									where emp_no < 10004";
				MySqlDataReader reader = cmd.ExecuteReader();

				int i = 0;
				conn.Open(); // Execute문 직전에 써줘도 됨
				//if(reader.Read()) //if문을 쓰는 경우는 select의 결과 건수가 1건일때
				//{ 
				//} // 이걸 주석하지않으면 먼저 한줄읽고 밑에서 한줄더읽기 때문에 2,3번째의 데이터만 출력
				while (reader.Read())//while문을 쓰는 경우는 select의 결과 건수가 여러건일때
				{
					if (i == 0)
					{
						label1.Text = reader["emp_no"].ToString(); 
						textBox1.Text = reader["emp_name"].ToString();
					}
					if(i==1)
					{
						label2.Text = reader["emp_no"].ToString(); 
						textBox2.Text = reader["emp_name"].ToString();
					}
					if(i==2)
					{
						label3.Text = reader["emp_no"].ToString();
						textBox3.Text = reader["emp_name"].ToString();
					}
					i++;
				}
				conn.Close(); // 이렇게 할거 다하고 닫아주는게 연결지향방법이다


			}
			catch (Exception err)
			{
				MessageBox.Show(err.Message);
			}
		}

		private void button4_Click(object sender, EventArgs e)
		{
			MySqlConnection conn = new MySqlConnection(strConn);
			//conn.Open();

			string sql = @"select emp_no, concat(first_name, ' ', last_name) emp_name
						   from employees
						   where emp_no < 10004;";
			MySqlDataAdapter da = new MySqlDataAdapter(sql, conn);// 생성자의 3번쨰로 쓰는게 제일 일반적
			DataSet ds = new DataSet(); // 빈 데이터셋 만들어주고

			da.Fill(ds);    //ds.Tables.Count == 1
			conn.Open(); //fill하기 직전에만 오픈하면 된다
			conn.Close();

			label6.Text = ds.Tables[0].Rows[0]["emp_no"].ToString();
			textBox6.Text = ds.Tables[0].Rows[0]["emp_name"].ToString();

			label5.Text = ds.Tables[0].Rows[1]["emp_no"].ToString();
			textBox5.Text = ds.Tables[0].Rows[1]["emp_name"].ToString();

			label4.Text = ds.Tables[0].Rows[2]["emp_no"].ToString();
			textBox4.Text = ds.Tables[0].Rows[2]["emp_name"].ToString();

		}
	}
}