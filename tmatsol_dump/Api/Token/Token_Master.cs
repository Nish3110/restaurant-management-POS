using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace tmatsol_dump.Api.Token
{
    public class Token_Master
    {
        //SqlConnection con = new SqlConnection("Data Source=thinksoftwares.dyndns.org,1434;Initial Catalog=TMATS00012020;User ID=tmats;Password=kajal3792kajal;");
        SqlConnection con = new SqlConnection("Data Source = 54.36.166.189; Initial Catalog = tmatsol_Report_DB_Entry; User ID = tmatsreport; Password=iLu4j22#;");

        public bool Token_Alredy_Deleted(string username, string tokenid, ref string Msg)
        {
            try
            {
                con.Open();
                string X_str = "select count(*) from user_tokens_deleted where login_id='" + username + "' and token_id='" + tokenid + "'";
                SqlCommand cmd = new SqlCommand(X_str, con);
                object i = cmd.ExecuteScalar();

                if (Convert.ToInt32(i) > 0)
                {
                    cmd.Dispose();
                    con.Close();
                    Msg = "Already Logout !!!";
                    return true;
                }

                cmd.Dispose();
                con.Close();
                return false;
            }
            catch (Exception e)
            {
                Msg = e.Message;
                return false;               
            }
        }

        public bool user_Token_InMst(string tokenID, ref string Msg)
        {
            try
            {
                con.Open();
                string X_str = "select Count(*) from user_tokens where token_id='"+tokenID+"'";
                SqlCommand cmd = new SqlCommand(X_str, con);
                object i = cmd.ExecuteScalar();

                //int max_token_allowed, Session_time_out;

                if (Convert.ToInt32(i)>0)
                {
                    return true;
                }
                else
                {
                    Msg = "No Token User Found !!!";
                    return false;
                }
            }
            catch (Exception e)
            {
                Msg = e.Message;
                return false;
            }
        }

        //public bool Session_time_out(string username,int token_id,int Session_time_out, ref string Msg)
        //{
        //    try
        //    {
        //        if (Session_time_out>0)
        //        {
        //            con.Open();
        //            string X_str = "select * from user_tokens where login_id='" + username + "' and token_id='" + token_id + "'";
        //            SqlCommand cmd = new SqlCommand(X_str, con);
        //            SqlDataReader i = cmd.ExecuteReader();

        //            //int max_token_allowed, Session_time_out;
        //            DateTime token_date=new DateTime();
        //            if (i.Read())
        //            {
        //                while (i.Read())
        //                {
        //                    token_date = Convert.ToDateTime(i["token_date"].ToString());
        //                }

        //                //Release resources  
        //                i.Close();
        //                cmd.Dispose();
        //                con.Close();

        //                TimeSpan ts = token_date - DateTime.Now;
        //                if (ts.TotalMinutes>Session_time_out)
        //                {
        //                    return true;
        //                }
        //                return false;
        //            }
        //            else
        //            {
        //                i.Close();
        //                cmd.Dispose();
        //                con.Close();

        //                Msg = "No Active Login Found !!!";
        //                return false;
        //            }
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Msg = e.Message;
        //        return false;
        //    }
        //}
        public bool Session_time_out(string username, string token_id, int Session_time_out, ref string Msg)
        {
            try
            {
                if (Session_time_out > 0)
                {
                    con.Open();
                    string X_str = "select * from user_tokens where login_id='" + username + "' and token_id='" + token_id + "'";
                    SqlCommand cmd = new SqlCommand(X_str, con);
                    SqlDataReader i = cmd.ExecuteReader();

                    //int max_token_allowed, Session_time_out;
                    DateTime token_date = new DateTime();
                    if (i.Read())
                    {
                        while (i.Read())
                        {
                            token_date = Convert.ToDateTime(i["token_date"].ToString());
                        }

                        //Release resources  
                        i.Close();
                        cmd.Dispose();
                        con.Close();
                        string X_msg = "";
                        TimeSpan ts = token_date - DateTime.Now;
                        if (ts.TotalMinutes > Session_time_out)
                        {
                            Token_Delete(username, token_id, ref X_msg);
                            if (X_msg.Trim() != "")
                            {
                                X_msg = Msg;
                            }
                            return true;
                        }
                        return false;
                    }
                    else
                    {
                        i.Close();
                        cmd.Dispose();
                        con.Close();

                        Msg = "No Active Login Found !!!";
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Msg = e.Message;
                return false;
            }
        }
        public bool max_Concate_Connections(string username, int max_token_allowed, ref string tokenID, ref string Msg)
        {
            try
            {
                Guid SessionID;
                string id;

                if (max_token_allowed > 0)
                {
                    con.Open();
                    string X_str = "select count(*) as [TotalCount] from user_tokens where login_id='" + username + "'";
                    //string X_str = "select * from user_tokens where login_id='" + username + "'";
                    SqlCommand cmd = new SqlCommand(X_str, con);
                    object i = cmd.ExecuteScalar();

                    if (i != null)
                    {
                        int c = Convert.ToInt16(i.ToString());

                        if (c >= max_token_allowed)
                        {
                            cmd.Dispose();
                            con.Close();
                            //delete TOP 1
                            //-----------OLD 21/12/2023------------------
                            //X_str = "delete TOP(1) from user_tokens where login_id='" + username + "'";

                            //-----------NEW 23/12/2023------------------
                            int Count = (c - max_token_allowed)+1; 
                          X_str = "DELETE FROM user_tokens WHERE token_id IN " +
                          "(SELECT TOP "+Count+" token_id FROM user_tokens WHERE login_id = '"+ username + "' ORDER BY token_date ASC)";
                            //-----------NEW 23/12/2023------------------

                            //insert
                            SessionID = System.Guid.NewGuid();
                            id = SessionID.ToString();
                            X_str += " insert into user_tokens([login_id],[token_id],[token_date]) values('" + username + "', '" + id + "', '" + DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss") + "')";
                            //insert
                            cmd.Dispose();
                            con.Open();
                            cmd = new SqlCommand(X_str, con);
                            cmd.ExecuteNonQuery();
                            con.Close();
                            tokenID = id;
                            return true;
                        }
                        //Release resources  
                        cmd.Dispose();
                        con.Close();
                        //insert 
                        SessionID = System.Guid.NewGuid();
                        id = SessionID.ToString();
                        X_str = "insert into user_tokens([login_id],[token_id],[token_date]) values('" + username + "', '" + id + "', '" + DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss") + "')";
                        cmd.Dispose();
                        con.Open();
                        cmd = new SqlCommand(X_str, con);
                        cmd.ExecuteNonQuery();
                        con.Close();
                        tokenID = id;
                        return false;                    
                    }
                    else
                    {
                        cmd.Dispose();
                        con.Close();

                        Msg = "No Active Login Found !!!";
                        return false;
                    }
                }
                else
                {
                    SessionID = System.Guid.NewGuid();
                    id = SessionID.ToString();
                    string X_str = " insert into user_tokens([login_id],[token_id],[token_date]) values('" + username + "', '" + id + "', '" + DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss") + "')";
                    //insert
                    con.Open();
                    SqlCommand cmd = new SqlCommand(X_str, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    tokenID = id;
                    return true;
                }
            }
            catch (Exception e)
            {
                con.Close();
                Msg = e.Message;
                return false;
            }
        }
        public void token_inserted(string username, ref string tokenID, ref string Msg)
        {
            try
            {
                ////delete
                //string X_str = "delete from user_tokens where login_id='" + username + "'";
                //con.Open();
                //SqlCommand cmd = new SqlCommand(X_str, con);
                //cmd.ExecuteNonQuery();
                //con.Close();

                ////insert
                //var SessionID = System.Guid.NewGuid();
                //var id = SessionID.ToString();
                //X_str = "insert into user_tokens([login_id],[token_id],[token_date])"+
                //               " values('" + username + "', '" + id + "', '" + DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss") + "')";
                //con.Open();
                //cmd = new SqlCommand(X_str, con);
                //cmd.ExecuteNonQuery();
                //con.Close();
                //tokenID = id;

                string sql = "select max_token_allowed from user_tokens_master where login_id='" + username + "'";
                con.Open();

                var dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                da.Fill(dt);

                con.Close();

                if (dt.Rows.Count==0)
                {
                    Msg = "No Token User Found !!!!";
                    return;
                }

                foreach (DataRow row in dt.Rows)
                {
                    var max_token_allowed=Convert.ToInt32(row["max_token_allowed"].ToString());
                    max_Concate_Connections(username, max_token_allowed, ref tokenID ,ref Msg);
                }
            }
            catch (Exception e)
            {
                Msg = e.Message;
            }
        }
        public void Token_Delete(string username, string tokenid, ref string Msg)
        {
            try
            {
                //string X_msg = "";
                //bool Ad = false;
                //Ad=Token_Alredy_Deleted(username,tokenid,ref X_msg);
                //if (Ad)
                //{
                //    Msg = X_msg;
                //    return;
                //}
                //else
                //{
                //    if(X_msg.Trim()!="")
                //    {
                //        Msg = X_msg;
                //        return;
                //    }
                //}

                //con.Open();
                //string X_str = "select * from user_tokens where login_id='" + username + "' and token_id='"+tokenid+"'";
                //SqlCommand cmd = new SqlCommand(X_str, con);
                //SqlDataReader i = cmd.ExecuteReader();
                //String x_str="insert into totke_delete() select pa,pa2,now from toke ehrere  "
                //while (i.Read())
                //{
                //    //Console.Write(reader["CustomerID"].ToString() + ", ");
                //    DateTime token_date = Convert.ToDateTime(i["token_date"].ToString());

                //    X_str = "insert into user_tokens_deleted values('"+i["login_id"].ToString() + "', '"+i["token_id"].ToString() + "', '"+ token_date.ToString("dd-MMM-yyyy HH:mm:ss") + "', '"+ DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss") + "')";
                //    cmd.Dispose();
                //    cmd = new SqlCommand(X_str, con);
                //    cmd.ExecuteNonQuery();
                //}

                ////Release resources  
                //i.Close();
                //cmd.Dispose();
                //con.Close();

                con.Open();
                string X_str = "insert into user_tokens_deleted([login_id],[token_id],[token_date],[token_deleted_at]) " +
                    "select [login_id],[token_id],[token_date],'" + DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss") +
                    "' from user_tokens " +
                    "where login_id='" + username + "' and token_id='" + tokenid + "'";

                //delete
                X_str += " delete from user_tokens where token_id='" + tokenid + "'";
                

                SqlCommand cmd = new SqlCommand(X_str, con);
                cmd.ExecuteNonQuery();

                //Release resources  

                cmd.Dispose();
                con.Close();
            }
            catch (Exception e)
            {
                Msg = e.Message;
            }
        }
        
    }
}