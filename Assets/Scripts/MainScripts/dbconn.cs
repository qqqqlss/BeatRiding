using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

using System.Data;     //C#의 데이터 테이블 때문에 사용
using MySql.Data;     //MYSQL함수들을 불러오기 위해서 사용
using MySql.Data.MySqlClient;    //클라이언트 기능을사용하기 위해서 사용

public class dbconn 
{
    MySqlConnection sqlconn = null;
    private string sqlDBip = "122.32.165.55";
    private string sqlDBname = "coex";
    private string sqlDBid = "team";
    private string sqlDBpw = "abcd1234";


    private void sqlConnect()
    {
        //접속 확인하기
        try
        {

            //DB정보 입력
            string sqlDatabase = "Server=" + sqlDBip + ";Database=" + sqlDBname + ";UserId=" + sqlDBid + ";Password=" + sqlDBpw + "";
            sqlconn = new MySqlConnection(sqlDatabase);
            sqlconn.Open();
            Debug.Log("SQL의 접속 상태 : " + sqlconn.State); //접속이 되면 OPEN이라고 나타남
        }
        catch (Exception msg)
        {
            Debug.Log(msg); //기타다른오류가 나타나면 오류에 대한 내용이 나타남
        }
    }

    private void sqldisConnect()
    {
        sqlconn.Close();
        Debug.Log("SQL의 접속 상태 : " + sqlconn.State); //접속이 끊기면 Close가 나타남 
    }

    public void sqlcmdall(string allcmd) //함수를 불러올때 명령어에 대한 String을 인자로 받아옴
    {
        sqlConnect(); //접속

        MySqlCommand dbcmd = new MySqlCommand(allcmd, sqlconn); //명령어를 커맨드에 입력
        dbcmd.ExecuteNonQuery(); //명령어를 SQL에 보냄

        sqldisConnect(); //접속해제
    }

    public DataTable selsql(string sqlcmd)  //리턴 형식을 DataTable로 선언함
    {
        DataTable dt = new DataTable(); //데이터 테이블을 선언함

        sqlConnect();
        MySqlDataAdapter adapter = new MySqlDataAdapter(sqlcmd, sqlconn);
        adapter.Fill(dt); //데이터 테이블에  채워넣기를함
        sqldisConnect();

        return dt; //데이터 테이블을 리턴함
    }
}
/*
# 클래스 불러오는 방법 (다른 c#코드에서 해당 코드를 적어야한다)
# dbconn mysqlDB = new dbconn();
# mysqlDB.sqlcmdall("update set score=100000,username='짱구' where level="EASY" AND title="Like that"); ==> 예시 업데이트!
# 해당 쿼리문을 적는 조건으로 cs파일엔 level과 title을 문자열로 비교해주고 일치할때 조건에 맞는 쿼리 입력하는게 낫지 않을까 생각! 
# mysqlDB.sqlcmdall("SQL구문을 적으면 됩니다. (UPDATE, INSERT 구문 사용)");
# switch문? if문? 메모리 할당 고려해서 가장 효율적인 코드로 작성
# DataTable dt = mysqlDB.selsql("SQL구문을 적으면 됩니다. (SELECT구문 사용)'") => 사용할일 있으면...
# 월요일 시러ㅓㅓㅓㅓㅓㅓㅓ
*/
