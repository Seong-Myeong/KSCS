﻿using KSCS.Class;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static KSCS.Class.KSCS_static;

namespace KSCS
{
    public static class Database
    {
        static MySqlConnection connection = null;
        public static MySqlConnection getDBConnection()
        {
            if (connection == null)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MySQL"].ConnectionString;
                connection = new MySqlConnection(connectionString);
                connection.Open();
            }
            return connection;
        }

        public static void ReadScheduleList()
        {
            string selectQuery = string.Format("SELECT * from Schedule JOIN Category ON Schedule.category_id=Category.id JOIN StudentCategory ON StudentCategory.student_id=Schedule.student_id and Schedule.category_id=Category.id and Category.id=StudentCategory.category_id WHERE Schedule.student_id={0} and  startDate BETWEEN DATE_FORMAT('{1}', '%Y-%m-%d') AND LAST_DAY('{1}') ORDER BY startDate ASC;", stdNum, new DateTime(year, month, 1).ToString("yyyy-MM-dd"));
            MySqlCommand cmd = new MySqlCommand(selectQuery, getDBConnection());
            MySqlDataReader table = cmd.ExecuteReader();
            monthScheduleList.Clear(); //한달 스케줄 초기화

            //하루 단위 리스트 생성
            for (int i = 0; i < DateTime.DaysInMonth(year, month); i++)
            {
                monthScheduleList.Add(new List<Schedule>());
            }

            while (table.Read())
            {
                Schedule schedule = new Schedule(
                    table["title"].ToString(),
                    table["content"].ToString(),
                    table["place"].ToString(),
                    table["type"].ToString(),
                    DateTime.Parse(table["startDate"].ToString()),
                    DateTime.Parse(table["endDate"].ToString()))
                {
                    id = int.Parse(table["id"].ToString()),
                };

                monthScheduleList[Convert.ToInt32(schedule.startDate.ToString("dd")) - 1].Add(schedule);
            }

            table.Close();
        }

        public static void ReadCategory()
        {
            categoryDict.Clear();
            string selectQuery = string.Format("SELECT * from Category JOIN StudentCategory ON Category.id=StudentCategory.category_id WHERE student_id='{0}';", stdNum);
            MySqlCommand cmd = new MySqlCommand(selectQuery, getDBConnection());
            MySqlDataReader table = cmd.ExecuteReader();
            while (table.Read())
            {
                categoryDict.Add(table["type"].ToString(), new string[2] { table["id"].ToString(), table["color"].ToString() });
            }
            table.Close();
        }

        public static void AddScheudle(Schedule schedule)
        {
            string insertQuery = string.Format("INSERT INTO Schedule(student_id,title,content,place,category_id,startDate,endDate) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}');",
                    stdNum,
                    schedule.title,
                    schedule.content,
                    schedule.place,
                    int.Parse(categoryDict[schedule.category][0]),
                    schedule.startDate.ToString("yyyy-MM-dd, HH:mm"),
                    schedule.endDate.ToString("yyyy-MM-dd, HH:mm"));
            MySqlCommand cmd = new MySqlCommand(insertQuery, connection);
            if (cmd.ExecuteNonQuery() != 1) MessageBox.Show("Failed to insert Data.");
            //추가 후, id 값 가져오기
            cmd.CommandText = "SELECT LAST_INSERT_ID() AS id";
            MySqlDataReader table = cmd.ExecuteReader();
            while (table.Read())
            {
                schedule.id = int.Parse(table["id"].ToString());
            }
            table.Close();
            //스케줄 리스트 추가
            monthScheduleList[UserDate.static_date - 1].Add(schedule);
            //스케줄 리스트 시작 시간 순 정렬
            monthScheduleList[UserDate.static_date - 1].OrderBy(sche => sche.startDate);
        }

        public static void UpdateSchedule(Schedule schedule,int index)
        {
            string updateQuery = string.Format("UPDATE Schedule SET title='{0}', content='{1}', place='{2}', category_id='{3}',startDate='{4}', endDate='{5}' WHERE id={6};",
                    schedule.title,
                    schedule.content,
                    schedule.place,
                    int.Parse(categoryDict[schedule.category][0]),
                    schedule.startDate.ToString("yyyy-MM-dd, HH:mm"),
                    schedule.endDate.ToString("yyyy-MM-dd, HH:mm"),
                    monthScheduleList[UserDate.static_date - 1][index].id
                    );
            MySqlCommand cmd = new MySqlCommand(updateQuery, getDBConnection());
            if (cmd.ExecuteNonQuery() != 1) MessageBox.Show("Failed to Update Data.");
            // 리스트 수정
            var selectedSchedule = monthScheduleList[UserDate.static_date - 1][index];
            selectedSchedule.title = schedule.title;
            selectedSchedule.content = schedule.content;
            selectedSchedule.place = schedule.place;
            selectedSchedule.category = schedule.category;
            selectedSchedule.startDate = schedule.startDate;
            selectedSchedule.endDate = schedule.endDate;
        }

        public static void DeleteSchedule(int index)
        {
            string deleteQuery = string.Format("DELETE FROM Schedule WHERE id='{0}';", monthScheduleList[UserDate.static_date - 1][index].id);
            MySqlCommand cmd = new MySqlCommand(deleteQuery, getDBConnection());
            if (cmd.ExecuteNonQuery() != 1) MessageBox.Show("Failed to Delete Data.");
            //리스트 삭제
            monthScheduleList[UserDate.static_date - 1].RemoveAt(index);
        }


    }
}
