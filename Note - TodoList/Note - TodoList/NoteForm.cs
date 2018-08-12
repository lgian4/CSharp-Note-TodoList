using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;
using MetroFramework;
namespace Note___TodoList
{
    public partial class NoteForm : MetroForm
    {
        /// <summary>
        /// List of all the task 
        /// </summary>
        List<Task> allTask = new List<Task>();

        /// <summary>
        /// Type of Function or Mode that currently ongoing : read, create, update,  nodate
        /// </summary>
        string Func = "";

        /// <summary>
        /// Selected Id for All the tab :
        /// CurrentId[0] = for Home / Incomplete Tab
        /// CurrentId[1] = for Complete Tab
        /// CurrentId[2] = for Trash Tab
        /// </summary>
        int[] CurrentIndex = new int[3];

        /// <summary>
        /// List of The color use for panel to identified it's status and priority :
        /// PanelColor[0] = Incomplete Urgent, 
        /// PanelColor[1] = Incomplete None, 
        /// PanelColor[2] = Complete Urgent, 
        /// PanelColor[3] = Complete None, 
        /// PanelColor[4] = Delete Urgent, 
        /// PanelColor[5] = Delete None, 
        /// </summary>
        Color[] PanelColor = new Color[6] { Color.IndianRed, Color.DeepSkyBlue, Color.DimGray, Color.Silver, Color.Gray, Color.DarkGray, };

        /// <summary>
        ///  Initialize Component
        /// </summary>
        public NoteForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        /// <summary>
        /// Load all of the Task
        /// </summary>
        private void LoadData()
        {
            //clear everythings
            allTask.Clear();
            TaskIncompletePanel.Controls.Clear();
            TaskCompletePanel.Controls.Clear();
            TaskDeletePanel.Controls.Clear();

            //set complete tab properties
            btnComplete1.Enabled = true;
            btnComplete2.Enabled = true;
            btnComplete1.Visible = true;
            btnComplete2.Visible = true;
            txtTitleComplete.ReadOnly = true;
            txtDetailsComplete.ReadOnly = true;
            txtCategoryComplete.ReadOnly = true;
            togglePriorityComplete.Enabled = false;
            btnComplete1.Text = "Delete";
            metroToolTip1.SetToolTip(btnComplete1, "Delete selected Note");
            btnComplete2.Text = "Active";
            metroToolTip1.SetToolTip(btnComplete1, "Active selected Note");
            txtTitleComplete.Text = "";
            txtDetailsComplete.Text = "";
            txtCategoryComplete.Text = "";
            togglePriorityComplete.Checked = false;


            // set trash tab Properties
            btnDelete1.Visible = true;
            btnDelete2.Visible = true;
            btnDelete1.Enabled = true;
            btnDelete2.Enabled = true;
            txtTitleDelete.Enabled = true;
            txtDetailsDelete.Enabled = true;
            txtCategoryDelete.Enabled = true;
            txtTitleDelete.ReadOnly = true;
            txtDetailsDelete.ReadOnly = true;
            txtCategoryDelete.ReadOnly = true;
            togglePriorityDelete.Enabled = false;
            btnDelete1.Text = "Delete Permanently";
            metroToolTip1.SetToolTip(btnDelete1, "Delete Permanently selected Note");
            btnDelete2.Text = "Active";
            metroToolTip1.SetToolTip(btnIncomplete1, "Active selected note");
            txtTitleDelete.Text = "";
            txtDetailsDelete.Text = "";
            txtCategoryDelete.Text = "";
            togglePriorityDelete.Checked = false;

            try
            {
                LoadCategoryAutoComplete();
                Task task = new Task();
                //load category combobox
                LoadCategory();
                // load Incomplete Tab
                var taskIncompleteList = task.GetIncompleteTask();
                if (taskIncompleteList.Count == 0)
                {
                    StateChange("nodata");
                }
                else
                {
                    StateChange("read");
                    LoadPanel(taskIncompleteList, TaskIncompletePanel);
                    ReadPanel(0);
                }
                //load complete tab
                var taskCompleteList = task.GetCompleteTask();
                if (taskCompleteList.Count > 0)
                {
                    var index = allTask.Count;
                    LoadPanel(taskCompleteList, TaskCompletePanel);
                    ReadPanel(index);
                }
                else
                {
                    btnComplete1.Enabled = false;
                    btnComplete2.Enabled = false;
                }
                //load Trash tab
                var taskTrashList = task.GetTrashTask();
                if (taskTrashList.Count > 0)
                {
                    var index = allTask.Count;
                    LoadPanel(taskTrashList, TaskDeletePanel);
                    ReadPanel(index);
                }
                else
                {
                    btnDelete1.Enabled = false;
                    btnDelete2.Enabled = false;
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException e)
            {
                ErrorLog.Write(e);
            }
            catch (IndexOutOfRangeException e)
            {
                ErrorLog.Write(e);
                var dialog = MetroMessageBox.Show(this, e.Message, "Error !", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (dialog == DialogResult.Retry)
                {
                    LoadData();
                }
                else
                {
                    this.Close();

                }
            }
        }

        /// <summary>
        /// Read Selected Task to its panel
        /// </summary>
        /// <param name="index">Index in allTask to read</param>
        private void ReadPanel(int index)
        {
            try
            {
                //if from incomplete tab
                if (allTask[index].Status == "incomplete" && Func == "read")
                {
                    CurrentIndex[0] = index;
                    txtTitleIncomplete.Text = allTask[index].Title;
                    txtDetailsIncomplete.Text = allTask[index].Details;
                    txtCategoryIncomplete.Text = allTask[index].CategoryName;

                    togglePriorityIncomplete.Enabled = false;
                    togglePriorityIncomplete.Checked = false;
                    if (allTask[index].Priority == "urgent")
                    {
                        togglePriorityIncomplete.Checked = true;
                    }
                    togglePriorityIncomplete.Enabled = true;
                }
                // if from complete tab
                else if (allTask[index].Status == "complete")
                {
                    CurrentIndex[1] = index;
                    txtTitleComplete.Text = allTask[index].Title;
                    txtDetailsComplete.Text = allTask[index].Details;
                    txtCategoryComplete.Text = allTask[index].CategoryName;
                    togglePriorityComplete.Checked = false;
                    if (allTask[index].Priority == "urgent")
                    {
                        togglePriorityComplete.Checked = true;
                    }
                }
                // if from trash tab
                else if (allTask[index].Status == "delete")
                {
                    CurrentIndex[2] = index;
                    txtTitleDelete.Text = allTask[index].Title;
                    txtDetailsDelete.Text = allTask[index].Details;
                    txtCategoryDelete.Text = allTask[index].CategoryName;
                    togglePriorityDelete.Checked = false;
                    if (allTask[index].Priority == "urgent")
                    {
                        togglePriorityDelete.Checked = true;
                    }
                }
            }
            catch (ArgumentOutOfRangeException e)
            {
                ErrorLog.Write(e);
                var dialog = MetroMessageBox.Show(this, e.Message, "Error !", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error, 150);
                if (dialog == DialogResult.Retry)
                {
                    LoadData();
                }
                else
                {
                    this.Close();
                }
            }

        }

        /// <summary>
        /// Save Change priority in Home / Incomplete Tab
        /// </summary>
        /// <param name="urgent">status of toggle priority</param>
        private void SavePriority(bool urgent)
        {
            // if read mode and user change the toggle
            if (Func == "read" && togglePriorityIncomplete.Enabled == true)
            {
                //get id from selected panel
                var id = allTask[CurrentIndex[0]].Id; ;
                var priority = "none";
                if (urgent == true)
                {
                    priority = "urgent";
                }
                try
                {
                    Task task = new Task();
                    task.UpdatePriorityTask(id, priority);
                    LoadData();

                }
                catch (MySql.Data.MySqlClient.MySqlException e)
                {
                    ErrorLog.Write(e);
                }
                catch (IndexOutOfRangeException e)
                {
                    ErrorLog.Write(e);
                    var dialog = MetroMessageBox.Show(this, e.Message, "Error !", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error, 150);
                    if (dialog == DialogResult.Retry)
                    {
                        LoadData();
                    }
                    else
                    {
                        this.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Complete Selected
        /// </summary>
        /// <remarks>Only work in Read Mode</remarks>
        private void CompleteTask()
        {

            if (Func == "read")
            {
                var dialogResult = MetroMessageBox.Show(this, "Do you want to complete this", "Are you sure ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, 150);
                if (dialogResult == DialogResult.Yes)
                {
                    try
                    {
                        Task task = new Task();
                        task.UpdateStatusTask(allTask[CurrentIndex[0]].Id, "complete");
                        LoadData();
                    }
                    catch (MySql.Data.MySqlClient.MySqlException e)
                    {
                        ErrorLog.Write(e);
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        ErrorLog.Write(e);
                        var dialog = MetroMessageBox.Show(this, e.Message, "Error !", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error, 150);
                        if (dialog == DialogResult.Retry)
                        {
                            LoadData();
                        }
                        else
                        {
                            this.Close();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Save a Task in Insert Mode
        /// </summary>
        private void InsertSave()
        {
            if (Func == "create" && !string.IsNullOrEmpty(txtTitleIncomplete.Text.Trim()))
            {
                var title = txtTitleIncomplete.Text.Trim();
                var details = txtDetailsIncomplete.Text.Trim();
                var category = txtCategoryIncomplete.Text.Trim();
                var priority = "none";
                title = title.Substring(0, 1).ToUpperInvariant() + title.Substring(1);
                if (!string.IsNullOrEmpty(details))
                {
                    details = details.Substring(0, 1).ToUpperInvariant() + details.Substring(1).ToLowerInvariant();
                }
                if (!string.IsNullOrEmpty(category))
                {
                    category = category.Substring(0, 1).ToUpperInvariant() + category.Substring(1).ToLowerInvariant();
                }
                if (togglePriorityIncomplete.Checked == true)
                {
                    priority = "urgent";
                }
                try
                {
                    Task task = new Task();
                    task.AddTask(title, details, priority, category);
                    LoadData();
                }
                catch (MySql.Data.MySqlClient.MySqlException e)
                {
                    ErrorLog.Write(e);
                }
            }
            else if (Func == "create" && string.IsNullOrEmpty(txtTitleIncomplete.Text.Trim()))
            {
                txtTitleIncomplete.Focus();
            }
        }

        /// <summary>
        /// Save a task in Update Mode
        /// </summary>
        private void UpdateSave()
        {
            if (Func == "update" && !string.IsNullOrEmpty(txtTitleIncomplete.Text.Trim()))
            {
                var title = txtTitleIncomplete.Text.Trim();
                var details = txtDetailsIncomplete.Text.Trim();
                var category = txtCategoryIncomplete.Text.Trim();
                var priority = "none";
                title = title.Substring(0, 1).ToUpperInvariant() + title.Substring(1);
                if (!string.IsNullOrEmpty(details))
                {
                    details = details.Substring(0, 1).ToUpperInvariant() + details.Substring(1).ToLowerInvariant();
                }
                if (!string.IsNullOrEmpty(category))
                {
                    category = category.Substring(0, 1).ToUpperInvariant() + category.Substring(1).ToLowerInvariant();
                }
                if (togglePriorityIncomplete.Checked == true)
                {
                    priority = "urgent";
                }
                try
                {
                    Task task = new Task();
                    task.UpdateTask(allTask[CurrentIndex[0]].Id, title, details, priority, "incomplete", category);
                    LoadData();
                }
                catch (MySql.Data.MySqlClient.MySqlException e)
                {
                    ErrorLog.Write(e);
                }
                catch (IndexOutOfRangeException e)
                {
                    ErrorLog.Write(e);
                    var dialog = MetroMessageBox.Show(this, e.Message, "Error !", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error, 150);
                    if (dialog == DialogResult.Retry)
                    {
                        LoadData();
                    }
                    else
                    {
                        this.Close();
                    }
                }
            }

            // Update Validation
            else if (Func == "update" && string.IsNullOrEmpty(txtTitleIncomplete.Text.Trim()))
            {
                txtTitleIncomplete.Focus();
            }
        }

        /// <summary>
        /// Active Task from Complete Tab or Trash Tab to Home Tab
        /// </summary>
        /// <param name="tabName">Tab Name</param>
        private void ActiveTask(string tabName)
        {
            var index = CurrentIndex[2];
            if (tabName == "complete")
            {
                index = CurrentIndex[1];
            }
            var dialogResult = MetroMessageBox.Show(this, "Do you want to Active this", "Are you sure ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, 150);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    Task task = new Task();
                    task.UpdateStatusTask(allTask[index].Id, "incomplete");
                    LoadData();
                }
                catch (MySql.Data.MySqlClient.MySqlException e)
                {
                    ErrorLog.Write(e);
                }
            }
        }

        /// <summary>
        /// Delete Task
        /// </summary>
        private void DeleteTask()
        {
            var dialogResult = MetroMessageBox.Show(this, "Do you want to Delete this", "Are you sure ?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, 150);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    Task task = new Task();
                    task.UpdateStatusTask(allTask[CurrentIndex[1]].Id, "delete");
                    LoadData();
                }
                catch (MySql.Data.MySqlClient.MySqlException e)
                {
                    ErrorLog.Write(e);
                }
            }
        }

        /// <summary>
        /// Change to Read Mode from Update or Create Mode
        /// </summary>
        private void CencelTask()
        {
            StateChange("read");
            ReadPanel(CurrentIndex[0]);
        }

        /// <summary>
        /// Delete a Task Permanently 
        /// </summary>
        private void DeleteTaskPermently()
        {
            var dialogResult = MetroMessageBox.Show(this, "Do you want to Permanently Delete this", "Are you sure ?", MessageBoxButtons.YesNo, MessageBoxIcon.Error, 150);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    Task task = new Task();
                    task.DeleteTask(allTask[CurrentIndex[2]].Id);
                    LoadData();
                }
                catch (MySql.Data.MySqlClient.MySqlException e)
                {
                    ErrorLog.Write(e);
                }
                catch (IndexOutOfRangeException e)
                {
                    ErrorLog.Write(e);
                    var dialog = MetroMessageBox.Show(this, e.Message, "Error !", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                    if (dialog == DialogResult.Retry)
                    {
                        LoadData();
                    }
                    else
                    {
                        this.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Change Function or Mode
        /// </summary>
        /// <param name="func">Name of Mode : nodata, create, update, read</param>
        private void StateChange(string func)
        {
            Func = func;
            // Nodata Mode
            if (Func == "nodata")
            {
                List<Task> taskList = new List<Task>();
                Task task = task = new Task(
                    0,
                    "No Data",
                    "No Data Curently available",
                    "none",
                    "incomplete",
                    "untitled");
                taskList.Add(task); ;
                LoadPanel(taskList, TaskIncompletePanel);
                StateChange("create");
            }
            //Create Mode
            else if (Func == "create")
            {
                btnAddTask.Visible = false;
                btnIncomplete1.Visible = true;
                btnIncomplete2.Visible = true;
                txtTitleIncomplete.Enabled = true;
                txtDetailsIncomplete.Enabled = true;
                txtCategoryIncomplete.Enabled = true;
                txtTitleIncomplete.ReadOnly = false;
                txtDetailsIncomplete.ReadOnly = false;
                txtCategoryIncomplete.ReadOnly = false;
                togglePriorityIncomplete.Enabled = false;
                btnIncomplete1.Text = "Insert";
                metroToolTip1.SetToolTip(btnIncomplete1, "Save Note");
                btnIncomplete2.Text = "Cancel";
                metroToolTip1.SetToolTip(btnIncomplete2, "Cancel");
                txtTitleIncomplete.Text = "";
                txtDetailsIncomplete.Text = "";
                txtCategoryIncomplete.Text = "";
                togglePriorityIncomplete.Checked = false;
                togglePriorityIncomplete.Enabled = true;
                txtTitleIncomplete.Focus();
            }
            //Read Mode
            else if (Func == "read")
            {
                btnAddTask.Visible = true;
                btnIncomplete1.Visible = true;
                btnIncomplete2.Visible = true;
                txtTitleIncomplete.Enabled = true;
                txtDetailsIncomplete.Enabled = true;
                txtCategoryIncomplete.Enabled = true;
                txtTitleIncomplete.ReadOnly = true;
                txtDetailsIncomplete.ReadOnly = true;
                txtCategoryIncomplete.ReadOnly = true;
                togglePriorityIncomplete.Enabled = false;
                btnIncomplete1.Text = "Update";
                metroToolTip1.SetToolTip(btnIncomplete1, "Update selected Note");
                btnIncomplete2.Text = "Complete";
                metroToolTip1.SetToolTip(btnIncomplete2, "Complete or finish selected note");
                txtTitleIncomplete.Text = "";
                txtDetailsIncomplete.Text = "";
                txtCategoryIncomplete.Text = "";
                togglePriorityIncomplete.Enabled = true;
            }
            //update mode
            else if (Func == "update")
            {
                btnAddTask.Visible = false;
                btnIncomplete1.Visible = true;
                btnIncomplete2.Visible = true;
                txtTitleIncomplete.Enabled = true;
                txtDetailsIncomplete.Enabled = true;
                txtCategoryIncomplete.Enabled = true;
                txtTitleIncomplete.ReadOnly = false;
                txtDetailsIncomplete.ReadOnly = false;
                txtCategoryIncomplete.ReadOnly = false;
                togglePriorityIncomplete.Enabled = true;
                btnIncomplete1.Text = "Save";
                metroToolTip1.SetToolTip(btnIncomplete1, "Save Note");
                btnIncomplete2.Text = "Cancel";
                metroToolTip1.SetToolTip(btnIncomplete2, "Cancel");
            }

        }

        /// <summary>
        /// Load Category Name to CboCategory
        /// </summary>
        private void LoadCategory()
        {
            cboCategory.Items.Clear();
            cboCategory.Items.Add("All");
            Task task = new Task();
            List<string> categoryNameList = task.GetCategoryName();
            foreach (string categoryName in categoryNameList)
            {
                cboCategory.Items.Add(categoryName);
            }
            cboCategory.Enabled = false;
            cboCategory.SelectedIndex = 0;
            cboCategory.Enabled = true;
        }

        /// <summary>
        /// Load list of task to its panel
        /// </summary>
        /// <param name="taskList">List of task</param>
        /// <param name="panel">Panel to load</param>
        private void LoadPanel(List<Task> taskList, MetroFramework.Controls.MetroPanel panel)
        {
            panel.Controls.Clear();

            for (int i = 0; i < taskList.Count; i++)
            {
                allTask.Add(taskList[i]);
                var title = taskList[i].Title;
                var details = taskList[i].Details;
                var priority = taskList[i].Priority;
                var status = taskList[i].Status;
                var categoryName = taskList[i].CategoryName;
                try
                {
                    CreateTaskCard(i, title, details, priority, status, categoryName, panel);
                }
                catch (MySql.Data.MySqlClient.MySqlException e)
                {
                    ErrorLog.Write(e);
                }
                catch (ArgumentOutOfRangeException e)
                {
                    ErrorLog.Write(e);
                }
                catch (IndexOutOfRangeException e)
                {
                    ErrorLog.Write(e);
                    var dialog = MetroMessageBox.Show(this, e.Message, "Error !", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                    if (dialog == DialogResult.Retry)
                    {
                        LoadData();
                    }
                    else
                    {
                        this.Close();
                    }
                }

            }
        }

        /// <summary>
        ///Load AutoComplete for txtCategory in Home Tab 
        /// </summary>
        private void LoadCategoryAutoComplete()
        {
            try
            {
                Category category = new Category();
                var categoryNames = category.GetListCategoryName();
                txtCategoryIncomplete.AutoCompleteCustomSource.AddRange(categoryNames.ToArray());
            }
            catch (MySql.Data.MySqlClient.MySqlException e)
            {
                ErrorLog.Write(e);
            }
        }

        /// <summary>
        /// Create a Task Card on the panel 
        /// </summary>
        /// <param name="i">Task Card Number in the panel to calculate position</param>
        /// <param name="title">Title of the task</param>
        /// <param name="details">Details of the task</param>
        /// <param name="priority">Priority of the task</param>
        /// <param name="status">Status of task</param>
        /// <param name="categoryName">Category of the task</param>
        /// <param name="panel">Panel to load</param>
        private void CreateTaskCard(int i, string title, string details, string priority, string status, string categoryName, MetroFramework.Controls.MetroPanel panel)
        {
            try
            {
                var currentIndex = allTask.Count() - 1;
                Color color;
                //choose panel category
                if (priority == "urgent" && status == "incomplete")
                {
                    color = PanelColor[0];
                }
                else if (priority == "none" && status == "incomplete")
                {
                    color = PanelColor[1];
                }
                else if (priority == "urgent" && status == "complete")
                {
                    color = PanelColor[2];
                }
                else if (priority == "none" && status == "complete")
                {
                    color = PanelColor[3];
                }
                else if (priority == "urgent" && status == "delete")
                {
                    color = PanelColor[4];
                }
                else if (priority == "none" && status == "delete")
                {
                    color = PanelColor[5];
                }
                else { color = Color.DeepSkyBlue; }

                //Create the controls
                MetroFramework.Controls.MetroLabel taskTitle = new MetroFramework.Controls.MetroLabel();
                MetroFramework.Controls.MetroLabel taskDetails = new MetroFramework.Controls.MetroLabel();
                MetroFramework.Controls.MetroLabel taskCategory = new MetroFramework.Controls.MetroLabel();
                MetroFramework.Controls.MetroPanel taskPanel = new MetroFramework.Controls.MetroPanel();

                //load task title
                taskTitle.Text = title;
                taskTitle.AutoSize = true;
                taskTitle.BackColor = System.Drawing.Color.Transparent;
                taskTitle.FontSize = MetroFramework.MetroLabelSize.Tall;
                taskTitle.FontWeight = MetroFramework.MetroLabelWeight.Regular;
                taskTitle.Location = new System.Drawing.Point(3, 10);
                taskTitle.Size = new System.Drawing.Size(106, 25);
                taskTitle.TabIndex = 2;
                taskTitle.UseCustomBackColor = true;
                taskTitle.UseCustomForeColor = true;
                taskTitle.Click += (sender, e) => ReadPanel(currentIndex);

                //load task details
                taskDetails.Text = details;
                taskDetails.BackColor = System.Drawing.Color.Transparent;
                taskDetails.Location = new System.Drawing.Point(3, 35);
                taskDetails.Size = new System.Drawing.Size(419, 19);
                taskDetails.TabIndex = 3;
                taskDetails.ForeColor = System.Drawing.Color.White;
                taskDetails.UseCustomForeColor = true;
                taskDetails.UseCustomBackColor = true;
                taskDetails.Click += (sender, e) => ReadPanel(currentIndex);

                //load task category
                taskCategory.Text = categoryName;
                taskCategory.AutoSize = true;
                taskCategory.BackColor = System.Drawing.Color.Transparent;
                taskCategory.Location = new System.Drawing.Point(339, 10);
                taskCategory.Size = new System.Drawing.Size(54, 19);
                taskCategory.TabIndex = 4;
                taskCategory.ForeColor = System.Drawing.Color.White;
                taskCategory.UseCustomForeColor = true;
                taskCategory.Click += (sender, e) => ReadPanel(currentIndex);
                taskCategory.UseCustomBackColor = true;

                //load task panel
                taskPanel.BackColor = color;
                taskPanel.Controls.Add(taskCategory);
                taskPanel.Controls.Add(taskTitle);
                taskPanel.Controls.Add(taskDetails);
                taskPanel.HorizontalScrollbarBarColor = true;
                taskPanel.HorizontalScrollbarHighlightOnWheel = false;
                taskPanel.HorizontalScrollbarSize = 10;
                taskPanel.Location = new System.Drawing.Point(11, i * 70);
                taskPanel.Size = new System.Drawing.Size(425, 64);
                taskPanel.TabIndex = 3;
                taskPanel.ForeColor = System.Drawing.Color.White;
                taskPanel.UseCustomForeColor = true;
                taskPanel.UseCustomBackColor = true;
                taskPanel.UseCustomForeColor = true;
                taskPanel.VerticalScrollbarBarColor = true;
                taskPanel.VerticalScrollbarHighlightOnWheel = false;
                taskPanel.VerticalScrollbarSize = 10;
                taskPanel.Click += (sender, e) => ReadPanel(currentIndex);
                panel.Controls.Add(taskPanel);
                panel.AutoScroll = true;
            }
            catch (ArgumentOutOfRangeException e)
            {
                ErrorLog.Write(e);
            }
        }

        /// <summary>
        /// Load Task by Category
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboCategory.Enabled == true)
            {
                try
                {
                    List<Task> taskIncompleteList;
                    if (cboCategory.SelectedIndex > 0)
                    {
                        var selectedCategory = cboCategory.SelectedItem.ToString();
                        Task task = new Task();
                        taskIncompleteList = task.GetTaskByCategory(selectedCategory);
                    }
                    else
                    {
                        Task task = new Task();
                        taskIncompleteList = task.GetIncompleteTask();
                    }
                    if (taskIncompleteList.Count == 0)
                    {
                        StateChange("nodata");
                    }
                    else
                    {
                        StateChange("read");
                        var index = allTask.Count();
                        LoadPanel(taskIncompleteList, TaskIncompletePanel);
                        ReadPanel(index);
                    }
                }
                catch (MySql.Data.MySqlClient.MySqlException a)
                {
                    ErrorLog.Write(a);
                }
                catch (IndexOutOfRangeException v)
                {
                    ErrorLog.Write(v);
                    var dialog = MetroMessageBox.Show(this, v.Message, "Error !", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                    if (dialog == DialogResult.Retry)
                    {
                        LoadData();
                    }
                    else
                    {
                        this.Close();
                    }
                }
            }
        }

        private void togglePriorityIncomplete_CheckedChanged(object sender, EventArgs e)
        {
            SavePriority(togglePriorityIncomplete.Checked);
        }

        private void btnDelete2_Click(object sender, EventArgs e)
        {
            ActiveTask("trash");
        }

        private void btnIncomplete2_Click(object sender, EventArgs e)
        {
            if (Func == "read")
            {
                CompleteTask();
            }
            else
            {
                CencelTask();
            }


        }

        private void btnAddTask_Click(object sender, EventArgs e)
        {
            StateChange("create");
        }

        private void btnIncomplete1_Click(object sender, EventArgs e)
        {
            if (Func == "read")
            {
                StateChange("update");
            }
            else
            {
                InsertSave();
                UpdateSave();
            }
        }

        private void btnComplete1_Click(object sender, EventArgs e)
        {
            DeleteTask();
        }

        private void btnComplete2_Click(object sender, EventArgs e)
        {
            ActiveTask("complete");
        }

        private void btnDelete1_Click(object sender, EventArgs e)
        {
            DeleteTaskPermently();
        }

        private void txtTitleIncomplete_Click(object sender, EventArgs e)
        {

        }
    }
}

