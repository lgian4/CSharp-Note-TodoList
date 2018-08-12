using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data;
namespace Note___TodoList
{
    /// <summary>
    /// The Category class
    /// </summary>
    class Category
    {
        //private int _id;
        //public int Id
        //{
        //    get { return _id; }
        //    set { _id = value; }
        //}

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public Category() { }
        //public Category(int id, string name)
        //{
        //    Id = id;
        //    Name = name;
        //}

        //public List<Category> GetAllCategory()
        //{
        //    List<Category> categoryList = new List<Category>();
        //    SqlHelper sqlHelper = new SqlHelper();
        //    List<MySqlParameter> sqlParameters = new List<MySqlParameter>();
        //    var queryResult = sqlHelper.executeStoreProcedure<DataSet>(sqlParameters, "GetAllCategory");

        //    Category category;
        //    foreach (DataRow dataRow in queryResult.Tables[0].Rows)
        //    {
        //        category = new Category(Convert.ToInt32(dataRow[0].ToString()), dataRow[1].ToString());
        //        categoryList.Add(category);
        //    }
        //    return categoryList;
        //}

        public List<string> GetListCategoryName()
        {
            List<string> categoryNameList = new List<string>();
            SqlHelper sqlHelper = new SqlHelper();
            List<MySqlParameter> sqlParameters = new List<MySqlParameter>();
            var queryResult = sqlHelper.executeStoreProcedure<DataSet>(sqlParameters, "GetAllCategoryName");

            foreach (DataRow dataRow in queryResult.Tables[0].Rows)
            {
                Name = dataRow[0].ToString();
                categoryNameList.Add(Name);
            }
            return categoryNameList;
        }
        //never use
        //public string GetNameCategory(int id)
        //{
        //    Id = id;
        //    List<Category> categoryList = new List<Category>();
        //    SqlHelper sqlHelper = new SqlHelper();
        //    List<MySqlParameter> sqlParameters = new List<MySqlParameter>();
        //    sqlParameters.Add(new MySqlParameter("_categoryId", Id));
        //    var queryResult = sqlHelper.executeStoreProcedure<DataSet>(sqlParameters, "GetNameCategory");
        //    DataRow dataRow = queryResult.Tables[0].Rows[0];
        //    Name = dataRow[0].ToString();
            
        //    return Name;
        //}

        //public void AddCategory(string name)
        //{
        //    Name = name;
        //    List<Category> categoryList = new List<Category>();
        //    SqlHelper sqlHelper = new SqlHelper();
        //    List<MySqlParameter> sqlParameters = new List<MySqlParameter>();
        //    sqlParameters.Add(new MySqlParameter("_name", Name));
        //    sqlHelper.executeNonQuery(sqlParameters, "AddCategory");
        //}

        //public void DeleteCategory(int id)
        //{
        //    Id = id;
        //    List<Category> categoryList = new List<Category>();
        //    SqlHelper sqlHelper = new SqlHelper();
        //    List<MySqlParameter> sqlParameters = new List<MySqlParameter>();
        //    sqlParameters.Add(new MySqlParameter("_categoryId", Id));
        //    sqlHelper.executeNonQuery(sqlParameters, "DeleteCategory");
        //}

        //public void UpdateCategory(int id, string name)
        //{
        //    Id = id;
        //    Name = name;
        //    SqlHelper sqlHelper = new SqlHelper();
        //    List<MySqlParameter> sqlParameters = new List<MySqlParameter>();
        //    sqlParameters.Add(new MySqlParameter("_id", Id));
        //    sqlParameters.Add(new MySqlParameter("_name", Name));
        //    sqlHelper.executeNonQuery(sqlParameters, "DeleteCategory");
        //}

    }
}
