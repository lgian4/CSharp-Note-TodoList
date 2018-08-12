using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
namespace Note___TodoList
{
    /// <summary>
    /// The Task Class
    /// contains all method for Crud Task Table
    /// </summary>
    class Task
    {

        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _title = value;
                }
                else
                {
                    _title = "NaN";
                }
            }
        }

        private string _details;
        public string Details
        {
            get { return _details; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _details = value;
                }
                else
                {
                    _details = "NaN";
                }
            }
        }

        private string _priority;
        public string Priority
        {
            get { return _priority; }
            set
            {
                if (value == "none" || value == "urgent")
                {
                    _priority = value;
                }
                else
                {
                    _priority = "none";
                }

            }
        }

        private string _status;
        public string Status
        {
            get { return _status; }
            set
            {
                if (value == "incomplete" || value == "complete" || value == "delete")
                {
                    _status = value;
                }
                else
                {
                    _status = "incomplete";
                }

            }
        }

        private string _categoryName;
        public string CategoryName
        {
            get { return _categoryName; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    value = "Untitled";
                }
                _categoryName = value;
            }
        }

       


        public Task() { }
        public Task(int id, string title, string details, string priority, string status, string categoryName)
        {
            Id = id;
            Title = title;
            Details = details;
            Priority = priority;
            Status = status;
            CategoryName = categoryName;
           
        }
        /// <summary>
        /// Get List of Task With Complete Status
        /// </summary>
        /// <returns>List of Task with status = Complete </returns>
        public List<Task> GetCompleteTask()
        {
            List<Task> taskList = new List<Task>();
            SqlHelper sqlHelper = new SqlHelper();
            List<MySqlParameter> sqlParameters = new List<MySqlParameter>();
            var queryResult = sqlHelper.executeStoreProcedure<DataSet>(sqlParameters, "GetCompleteTask");

            Task task;
            foreach (DataRow dataRow in queryResult.Tables[0].Rows)
            {
                task = new Task(
                    Convert.ToInt32(dataRow[0].ToString()),
                    dataRow[1].ToString(),
                    dataRow[2].ToString(),
                    dataRow[3].ToString(),
                    dataRow[4].ToString(),
                    dataRow[5].ToString()
                   );
                taskList.Add(task);
            }

            return taskList;
        }

        /// <summary>
        /// Get List of Task With Incomplete Status
        /// </summary>
        /// <returns>List of Task with status = Incomplete </returns>
        public List<Task> GetIncompleteTask()
        {
            List<Task> taskList = new List<Task>();
            SqlHelper sqlHelper = new SqlHelper();
            List<MySqlParameter> sqlParameters = new List<MySqlParameter>();
            var queryResult = sqlHelper.executeStoreProcedure<DataSet>(sqlParameters, "GetIncompleteTask");

            Task task;
            foreach (DataRow dataRow in queryResult.Tables[0].Rows)
            {
                task = new Task(
                    Convert.ToInt32(dataRow[0].ToString()),
                    dataRow[1].ToString(),
                    dataRow[2].ToString(),
                    dataRow[3].ToString(),
                    dataRow[4].ToString(),
                    dataRow[5].ToString()
                  );
                taskList.Add(task);
            }
            return taskList;
        }

        /// <summary>
        /// Get List of Task With Delete Status
        /// </summary>
        /// <returns>List of Task with status = Delete </returns>
        public List<Task> GetTrashTask()
        {
            List<Task> taskList = new List<Task>();
            SqlHelper sqlHelper = new SqlHelper();
            List<MySqlParameter> sqlParameters = new List<MySqlParameter>();
            var queryResult = sqlHelper.executeStoreProcedure<DataSet>(sqlParameters, "GetTrashTask");

            Task task;
            foreach (DataRow dataRow in queryResult.Tables[0].Rows)
            {
                task = new Task(
                    Convert.ToInt32(dataRow[0].ToString()),
                    dataRow[1].ToString(),
                    dataRow[2].ToString(),
                    dataRow[3].ToString(),
                    dataRow[4].ToString(),
                    dataRow[5].ToString()
                  );
                taskList.Add(task);
            }
            return taskList;
        }

        /// <summary>
        /// Get List Category that use by task
        /// </summary>
        /// <returns> List Category that use by task</returns>
        public List<string> GetCategoryName()
        {
            List<string> categoryNameList = new List<string>();
            SqlHelper sqlHelper = new SqlHelper();
            List<MySqlParameter> sqlParameters = new List<MySqlParameter>();
            var queryResult = sqlHelper.executeStoreProcedure<DataSet>(sqlParameters, "GetAllCategoryNameDistinct");

            foreach (DataRow dataRow in queryResult.Tables[0].Rows)
            {
                CategoryName = dataRow[0].ToString();
                categoryNameList.Add(CategoryName);
            }
            return categoryNameList;
        }

        /// <summary>
        /// Get List of Task By Category
        /// </summary>
        /// <param name="categoryName">Name of a Category</param>
        /// <returns>Lis of Task with the same category</returns>
        public List<Task> GetTaskByCategory(string categoryName)
        {
            CategoryName = categoryName;
            List<Task> taskList = new List<Task>();
            SqlHelper sqlHelper = new SqlHelper();
            List<MySqlParameter> sqlParameters = new List<MySqlParameter>();
            sqlParameters.Add(new MySqlParameter("_category", CategoryName));
            var queryResult = sqlHelper.executeStoreProcedure<DataSet>(sqlParameters, "GetTaskByCategory");
            Task task;
            foreach (DataRow dataRow in queryResult.Tables[0].Rows)
            {
                task = new Task(
                    Convert.ToInt32(dataRow[0].ToString()),
                    dataRow[1].ToString(),
                    dataRow[2].ToString(),
                    dataRow[3].ToString(),
                    dataRow[4].ToString(),
                    dataRow[5].ToString()
                  );
                taskList.Add(task);
            }
            return taskList;
        }

        ///// <summary>
        ///// Get one task by ID
        ///// </summary>
        ///// <param name="id">Id of the task</param>
        ///// <returns>Task with the same Id</returns>
        //public Task GetOneTask(int id)
        //{
        //    Id = id;
        //    SqlHelper sqlHelper = new SqlHelper();
        //    List<MySqlParameter> sqlParameters = new List<MySqlParameter>();
        //    sqlParameters.Add(new MySqlParameter("_category", CategoryName));
        //    var queryResult = sqlHelper.executeStoreProcedure<DataSet>(sqlParameters, "GetOneTask");
        //    DataRow dataRow = queryResult.Tables[0].Rows[0];
        //    Task task;
        //    task = new Task(
        //        Convert.ToInt32(dataRow[0].ToString()),
        //        dataRow[1].ToString(),
        //        dataRow[2].ToString(),
        //        dataRow[3].ToString(),
        //        dataRow[4].ToString(),
        //        dataRow[5].ToString(),
        //        DateTime.Parse(dataRow[6].ToString()));
        //    return task;
        //}

        /// <summary>
        /// Insert a Task 
        /// </summary>
        /// <param name="title">Title of the task</param>
        /// <param name="details">Details of the task</param>
        /// <param name="priority">Priority of the task</param>
        /// <param name="categoryName">Categoryof the task</param>
        public void AddTask(string title, string details, string priority, string categoryName)
        {
            Title = title;
            Details = details;
            Priority = priority;
            CategoryName = categoryName;
            SqlHelper sqlHelper = new SqlHelper();
            List<MySqlParameter> sqlParameters = new List<MySqlParameter>();
            sqlParameters.Add(new MySqlParameter("_title", Title));
            sqlParameters.Add(new MySqlParameter("_details", Details));
            sqlParameters.Add(new MySqlParameter("_priority", Priority));
            sqlParameters.Add(new MySqlParameter("_category", CategoryName));
            sqlHelper.executeNonQuery(sqlParameters, "AddTask");
        }

        /// <summary>
        /// Delete Task By Id
        /// </summary>
        /// <param name="id">Id of the task</param>
        public void DeleteTask(int id)
        {
            Id = id;
           SqlHelper sqlHelper = new SqlHelper();
            List<MySqlParameter> sqlParameters = new List<MySqlParameter>();
            sqlParameters.Add(new MySqlParameter("_id", Id));
            sqlHelper.executeNonQuery(sqlParameters, "DeleteTask");
        }

        /// <summary>
        /// Update A task
        /// </summary>
        /// <param name="id">Id of the Task</param>
        /// <param name="title">Changed Title</param>
        /// <param name="details">Changed Details</param>
        /// <param name="priority">Changed Priority</param>
        /// <param name="status">Changed Status</param>
        /// <param name="categoryName">Changed Category</param>
        public void UpdateTask(int id, string title, string details, string priority, string status, string categoryName)
        {
            Id = id;
            Title = title;
            Details = details;
            Priority = priority;
            Status = status;
            CategoryName = categoryName;
            SqlHelper sqlHelper = new SqlHelper();
            List<MySqlParameter> sqlParameters = new List<MySqlParameter>();
            sqlParameters.Add(new MySqlParameter("_id", Id));
            sqlParameters.Add(new MySqlParameter("_title", Title));
            sqlParameters.Add(new MySqlParameter("_details", Details));
            sqlParameters.Add(new MySqlParameter("_priority", Priority));
            sqlParameters.Add(new MySqlParameter("_status", Status));
            sqlParameters.Add(new MySqlParameter("_category", CategoryName));
            sqlHelper.executeNonQuery(sqlParameters, "UpdateTask");
        }

        /// <summary>
        /// Update Task Priority by Id
        /// </summary>
        /// <param name="id">Id task</param>
        /// <param name="priority">Changed Priority</param>
        public void UpdatePriorityTask(int id, string priority)
        {
            Id = id;
            Priority = priority;
            SqlHelper sqlHelper = new SqlHelper();
            List<MySqlParameter> sqlParameters = new List<MySqlParameter>();
            sqlParameters.Add(new MySqlParameter("_id", Id));
            sqlParameters.Add(new MySqlParameter("_priority", Priority));
            sqlHelper.executeNonQuery(sqlParameters, "UpdatePriorityTask");
        }

        /// <summary>
        /// Update Task Status by Id
        /// </summary>
        /// <param name="id">Id task</param>
        /// <param name="status">Changed Status</param>
        public void UpdateStatusTask(int id, string status)
        {
            Id = id;
            Status = status;
            SqlHelper sqlHelper = new SqlHelper();
            List<MySqlParameter> sqlParameters = new List<MySqlParameter>();
            sqlParameters.Add(new MySqlParameter("_id", Id));
            sqlParameters.Add(new MySqlParameter("_status", status));
            sqlHelper.executeNonQuery(sqlParameters, "UpdateStatusTask");
        }
    }
}
