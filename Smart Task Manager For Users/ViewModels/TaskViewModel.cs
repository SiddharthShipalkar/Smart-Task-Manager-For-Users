using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Windows.Input;
using Smart_Task_Manager_For_Users.Models;
using System.Windows;

namespace Smart_Task_Manager_For_Users.ViewModels
{
    public class TaskViewModel:INotifyPropertyChanged
    {
        public ObservableCollection<TaskModel> Tasks { get; set; }
        private ObservableCollection<TaskModel> _filteredTasks;

        #region Properties 
        public ObservableCollection<TaskModel> FilterdTasks
        {
            get { return _filteredTasks; }
            set { _filteredTasks = value; OnPropertyChanged(nameof(FilterdTasks)); }
        }
        private string _selectedFilter;
        public string SelectedFilter
        {
            get { return _selectedFilter; }
            set { _selectedFilter = value; 
                OnPropertyChanged(nameof(SelectedFilter));
                ApplyFilter(_selectedFilter);
            }
        }
        private TaskModel _selectedTask;
        public TaskModel SelectedTask
        {
            get => _selectedTask;
            set
            {
                _selectedTask = value;
                OnPropertyChanged(nameof(SelectedTask));
            }
        }
        private string _newTaskName;
        public string NewTaskName
        {
            get{ return _newTaskName; } 
            set { _newTaskName = value; OnPropertyChanged(nameof(NewTaskName)); }
        }

        private string _newTaskDescription;
        public string NewTaskDescription
        {
            get { return _newTaskDescription; }
            set { _newTaskDescription = value; OnPropertyChanged(nameof(NewTaskDescription)); }
        }

        private string _taskCatogery;
        public string TaskCatogery
        {
            get { return _taskCatogery; }
            set { _taskCatogery = value;OnPropertyChanged(nameof(TaskCatogery)); }
        }
        private DateTime _newDueDate = DateTime.Now;
        public DateTime NewDueDate
        {
            get => _newDueDate;
            set { _newDueDate = value; OnPropertyChanged(nameof(NewDueDate)); }
        }
        private string _newPriority;

        public string NewPriority
        {
            get { return _newPriority; }
            set { _newPriority = value; OnPropertyChanged(nameof(NewPriority)); }
        }

        private bool _isCompltedBool;

        public bool IsCompltedBool
        {
            get { return _isCompltedBool; }
            set { _isCompltedBool = value; OnPropertyChanged(nameof(IsCompltedBool)); }
        }
        private bool _isEditDialogOpen;
        public bool IsEditDialogOpen
        {
            get { return _isEditDialogOpen; }
            set { _isEditDialogOpen = value; OnPropertyChanged(nameof(IsEditDialogOpen)); }
        }

        private string _newIsCompleted;
        public string NewIsCompleted
        {
            get => _newIsCompleted; 
            set {
                if (value == "True")
                    IsCompltedBool = true;
                else if (value == "False")
                    IsCompltedBool = false;
                _newIsCompleted = value; 
                OnPropertyChanged(nameof(NewIsCompleted)); 
            }
        }
        #endregion

        #region Icommands 
        public ICommand AddTaskCommand { get; }
        public ICommand EditTaskCommand { get; }
        public ICommand OpenEditDialogCommand { get; }
        public ICommand CloseEditDialogCommand { get; }
        public ICommand SaveTaskCommand { get; }
        public ICommand DeleteTaskCommand { get; }
        #endregion




        public TaskViewModel()
        {
            Tasks = new ObservableCollection<TaskModel>();
            Tasks.CollectionChanged += Tasks_CollectionChanged;
            FilterdTasks = new ObservableCollection<TaskModel>(Tasks);
            AddTaskCommand =new RelayCommand(AddTask);
            EditTaskCommand = new RelayCommand(EditTask);
            OpenEditDialogCommand = new RelayCommand(OpenEditDialog);
            CloseEditDialogCommand = new RelayCommand(CloseEditDialog);
            SaveTaskCommand = new RelayCommand(SaveTask);
            DeleteTaskCommand = new RelayCommand(DeleteTask);

            Tasks.Add(new TaskModel
            {
                Id = Guid.NewGuid(),
                TaskName = "Complete Documentation",
                Category = "Work",
                TaskDescription = "Finish writing the user guide for the project.",
                DueDate = DateTime.Now.AddDays(2),
                TaskPriority = "Low",
                IsCompleted = false,
            });
            Tasks.Add(new TaskModel {
                Id = Guid.NewGuid(),
                TaskName = "Grocery Shopping",
                TaskDescription = "Buy vegetables and other essentials.",
                Category = "Personal",
                TaskPriority = "Low",
                DueDate = DateTime.Now.AddDays(1),
                IsCompleted = false
            });
            CheckDueDates();

        }
        private void CheckDueDates()
        {
            var upcomingTasks = Tasks.Where(task => task.DueDate <= DateTime.Now.AddDays(2) && !task.IsCompleted).ToList();

            foreach (var task in upcomingTasks)
            {
                // Show a notification or reminder (can be done using a pop-up or a message)
                MessageBox.Show($"Reminder: The task '{task.TaskName}' is due soon.");
            }
        }
        private void DeleteTask(object obj)
        {
            if(obj is TaskModel selectedTask)
                Tasks.Remove(selectedTask);
        }

        private void SaveTask(object obj)
        {
            IsEditDialogOpen = false;
        }

        private void CloseEditDialog(object obj)
        {
            IsEditDialogOpen = false;
        }

        private void EditTask(object obj)
        {
            OpenEditDialog(obj);
        }

        private void OpenEditDialog(object obj)
        {
            if(obj is TaskModel task && task!=null )
            {
                SelectedTask = task;
                IsEditDialogOpen = true;
            }
                
        }

        private void Tasks_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ApplyFilter(SelectedFilter);
        }

         
        public void ApplyFilter(string SelectedFilter)
        {
            if (SelectedFilter == "Completed")
            {
                FilterdTasks = new ObservableCollection<TaskModel>(Tasks.Where(e => e.IsCompleted == true));
            }
            else if (SelectedFilter== "Incomplete")
            {
                FilterdTasks = new ObservableCollection<TaskModel>(Tasks.Where(e => e.IsCompleted == false));

            }
            else
            {
                FilterdTasks = new ObservableCollection<TaskModel>(Tasks);

            }
        }
        public void AddTask(object parameter)
        {
            Tasks.Add(new TaskModel
            {
                Id = Guid.NewGuid(),
                TaskName = NewTaskName,
                Category = TaskCatogery,
                TaskDescription = NewTaskDescription,
                DueDate = NewDueDate,
                IsCompleted = IsCompltedBool,
            });
            NewTaskName = string.Empty;
            TaskCatogery = string.Empty;
            NewTaskDescription = string.Empty;
            NewDueDate = DateTime.Now;
            NewIsCompleted = "False";


        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
      
    }
}
