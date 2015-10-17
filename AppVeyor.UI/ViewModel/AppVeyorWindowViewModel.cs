using AppVeyor.Api;
using AppVeyor.Common;
using AppVeyor.Common.Entities;
using AppVeyor.Common.Exceptions;
using AppVeyor.Common.Extensions;
using AppVeyor.UI.Aspects;
using AppVeyor.UI.Common;
using AppVeyor.UI.Controls.ClosableTab;
using AppVeyor.UI.Model;
using AppVeyor.UI.Options;
using AppVeyor.UI.Services;
using AppVeyor.UI.ViewModel.Base;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Constants = AppVeyor.UI.Common.Constants;

namespace AppVeyor.UI.ViewModel
{
    public class AppVeyorWindowViewModel : ViewModelBase
    {
        #region Private Fields

        private const string APPVEYOR_URL = "https://ci.appveyor.com/";
        private const string GIT_URL = "https://github.com/";
        private const string PROJECTS_KEY = "projects";
        private static readonly AppVeyorWindowViewModel _instance = new AppVeyorWindowViewModel();
        private static readonly TimeSpan _timeToWait = TimeSpan.FromSeconds(10);
        private readonly Dictionary<string, ObservableCollection<Project>> _cacheProjectsSearch = new Dictionary<string, ObservableCollection<Project>>();
        private readonly AsyncManualResetEvent _isPausedResetEvent = new AsyncManualResetEvent();
        private CancellationTokenSource _cancellationTokenSource;
        private RelayCommand<object> _clearMessagesCommand;
        private ObservableCollection<Message> _errors = new ObservableCollection<Message>();
        private Visibility _isLoading;
        private bool _isTabEnvironmentsSelected;
        private bool _isTabEnvironmentsVisible;
        private bool _isTabProjectsSelected;
        private bool _isTabUsersSelected;
        private bool _isTabUsersVisible;
        private string _message;
        private RelayCommand<Project> _onBuildCancel;
        private RelayCommand<Project> _onBuildStart;
        private RelayCommand<Project> _onDeploymentsCommand;
        private RelayCommand<Project> _onSettingsCommand;
        private RelayCommand<Project> _onViewHistoryCommand;
        private AppVeyorOptions _options;
        private bool _pausePolling;
        private RelayCommand<Project> _projectBranchCommitIdClickedCommand;
        private RelayCommand<Project> _projectBuildMessageClickedCommand;
        private RelayCommand<Project> _projectBuildVersionClickedCommand;
        private RelayCommand<Project> _projectCommittedByClickedCommand;
        private RelayCommand<Project> _projectNameClickedCommand;
        private ObservableCollection<Project> _projects;
        private Repository _repository;
        private bool _searchResultDisplayed;
        private TabItem _selectedTab;
        private bool _showMessage;
        private RelayCommand<Message> _copyMessagesCommand;

        #endregion Private Fields

        #region Public Constructors

        static AppVeyorWindowViewModel()
        {
        }

        #endregion Public Constructors

        #region Private Constructors

        private AppVeyorWindowViewModel()
        {
        }

        #endregion Private Constructors

        #region Public Properties

        public static AppVeyorWindowViewModel Instance
        {
            get
            {
                return _instance;
            }
        }

        public CancellationTokenSource CancellationTokenSource
        {
            get { return _cancellationTokenSource; }
            set { _cancellationTokenSource = value; }
        }

        public ICommand ClearMessagesCommand
        {
            get
            {
                if (_clearMessagesCommand == null)
                {
                    _clearMessagesCommand = new RelayCommand<object>(OnClearMessagesClicked);
                }
                return _clearMessagesCommand;
            }
        }

        public ICommand CopyMessagesCommand
        {
            get
            {
                if (_copyMessagesCommand == null)
                {
                    _copyMessagesCommand = new RelayCommand<Message>(OnCopyMessageClicked);
                }
                return _copyMessagesCommand;
            }
        }

        public ObservableCollection<Message> Errors
        {
            get { return _errors; }
            set
            {
                _errors = value;
                OnPropertyChanged();
            }
        }

        public Visibility IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        public bool IsTabEnvironmentsSelected
        {
            get { return _isTabEnvironmentsSelected; }
            set
            {
                _isTabEnvironmentsSelected = value;
                OnPropertyChanged();
            }
        }

        public bool IsTabEnvironmentsVisible
        {
            get { return _isTabEnvironmentsVisible; }
            set
            {
                _isTabEnvironmentsVisible = value;
                OnPropertyChanged();
            }
        }

        public bool IsTabProjectsSelected
        {
            get { return _isTabProjectsSelected; }
            set
            {
                _isTabProjectsSelected = value;
                OnPropertyChanged();
            }
        }

        public bool IsTabUsersSelected
        {
            get { return _isTabUsersSelected; }
            set
            {
                _isTabUsersSelected = value;
                OnPropertyChanged();
            }
        }

        public bool IsTabUsersVisible
        {
            get { return _isTabUsersVisible; }
            set
            {
                _isTabUsersVisible = value;
                OnPropertyChanged();
            }
        }

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        public ICommand OnCancelBuildCommand
        {
            get
            {
                if (_onBuildCancel == null)
                {
                    _onBuildCancel = new RelayCommand<Project>(OnCancelBuildClicked, CanCancelBuild);
                }
                return _onBuildCancel;
            }
        }

        public ICommand OnDeploymentsCommand
        {
            get
            {
                if (_onDeploymentsCommand == null)
                {
                    _onDeploymentsCommand = new RelayCommand<Project>(OnDeploymentsClicked, project => true);
                }
                return _onDeploymentsCommand;
            }
        }

        public ICommand OnSettingsCommand
        {
            get
            {
                if (_onSettingsCommand == null)
                {
                    _onSettingsCommand = new RelayCommand<Project>(OnSettingsClicked, project => true);
                }
                return _onSettingsCommand;
            }
        }

        public ICommand OnStartBuildCommand
        {
            get
            {
                if (_onBuildStart == null)
                {
                    _onBuildStart = new RelayCommand<Project>(OnStartBuildClicked, CanStartBuild);
                }
                return _onBuildStart;
            }
        }

        public ICommand OnViewHistoryCommand
        {
            get
            {
                if (_onViewHistoryCommand == null)
                {
                    _onViewHistoryCommand = new RelayCommand<Project>(OnViewHistoryClicked, project => true);
                }
                return _onViewHistoryCommand;
            }
        }

        public bool PausePolling
        {
            get { return _pausePolling; }
            set
            {
                _pausePolling = value;
                if (value)
                {
                    _isPausedResetEvent.Reset();
                }
                else
                {
                    _isPausedResetEvent.Set();
                }
            }
        }

        public ICommand ProjectBuildMessageClickedCommand
        {
            get
            {
                if (_projectBuildMessageClickedCommand == null)
                {
                    _projectBuildMessageClickedCommand = new RelayCommand<Project>(OnProjectBuildMessageClicked);
                }
                return _projectBuildMessageClickedCommand;
            }
        }

        public ICommand ProjectBuildVersionClickedCommand
        {
            get
            {
                if (_projectBuildVersionClickedCommand == null)
                {
                    _projectBuildVersionClickedCommand = new RelayCommand<Project>(OnProjectBuildVersionClicked);
                }
                return _projectBuildVersionClickedCommand;
            }
        }

        public ICommand ProjectCommitIdClickedCommand
        {
            get
            {
                if (_projectBranchCommitIdClickedCommand == null)
                {
                    _projectBranchCommitIdClickedCommand = new RelayCommand<Project>(ProjectCommitIdClicked, CanExecuteProjectCommitIdClickedCommand);
                }
                return _projectBranchCommitIdClickedCommand;
            }
        }

        public ICommand ProjectCommittedByClickedCommand
        {
            get
            {
                if (_projectCommittedByClickedCommand == null)
                {
                    _projectCommittedByClickedCommand = new RelayCommand<Project>(ProjectCommittedByClicked, CanExecuteProjectCommittedByClicked);
                }
                return _projectCommittedByClickedCommand;
            }
        }

        public ICommand ProjectNameClickedCommand
        {
            get
            {
                if (_projectNameClickedCommand == null)
                {
                    _projectNameClickedCommand = new RelayCommand<Project>(OnProjectNameClicked);
                }
                return _projectNameClickedCommand;
            }
        }

        public ObservableCollection<Project> Projects
        {
            get { return _projects; }
            set
            {
                _projects = value;
                OnPropertyChanged();
            }
        }

        public TabItem SelectedTab
        {
            get { return _selectedTab; }
            set
            {
                _selectedTab = value;
                OnPropertyChanged();
            }
        }

        public IServiceProvider ServiceProvider { get; set; }

        public bool ShowMessage
        {
            get { return _showMessage; }
            set
            {
                _showMessage = value;
                OnPropertyChanged();
            }
        }

        #endregion Public Properties

        #region Public Methods

        public void AddAlert(Exception exception, string message = "Oops! An error occurred.")
        {
            IsLoading = Visibility.Collapsed;
            message += Environment.NewLine + exception;

            var errorMsg = new Message
            {
                TimeStamp = DateTime.Now,
                Text = message
            };
            _errors.Add(errorMsg);

            Errors = _errors.OrderByDescending(x => x.TimeStamp).ToObservableCollection();
        }

        public void ClearSearch()
        {
            Telemetry.Instance.TrackEvent("Search results cleared");
            if (_cacheProjectsSearch.ContainsKey(PROJECTS_KEY))
            {
                Projects = _cacheProjectsSearch[PROJECTS_KEY].ToObservableCollection();
                return;
            }
            _searchResultDisplayed = false;
            //TODO: GET FROM SERVER
            Initialize();
        }

        public void ContextChanged(string newContext)
        {
            switch (newContext)
            {
                case Constants.ENVIRONMENTS:
                    IsTabEnvironmentsVisible = true;
                    IsTabEnvironmentsSelected = true;
                    break;

                case Constants.PROJECTS:
                    IsTabProjectsSelected = true;
                    break;

                case Constants.USERS:
                    IsTabUsersVisible = true;
                    IsTabUsersSelected = true;
                    break;
            }
        }

        public void StopPollingForProjects()
        {
            //PausePolling = true;
            _isPausedResetEvent.Reset();
        }

        public void StartPollingForProjects()
        {
            //PausePolling = false;
            _isPausedResetEvent.Set();

            Poll();
        }

        [HandleException]
        public void Initialize()
        {
            IsLoading = Visibility.Hidden;
            IsTabProjectsSelected = true;

            var eventManager = ServiceProvider.GetService<SEventManager, IEventManager>();
            _options = eventManager.AppVeyorOptions;
            _options.OptionsChanged += OnOptionsChanged;

            if (string.IsNullOrWhiteSpace(_options.ApiToken))
            {
                Telemetry.Instance.TrackEvent("API Token empty error");
                var message =
                    string.Format(
                        "Please input API token from your AppVeyor account in Tools | Options | AppVeyor.{0}Token can be found on API token page under your AppVeyor account.",
                        Environment.NewLine);
                ShowMessageOnUI(message);
                StopPollingForProjects();
                Projects = null;
            }
            else
            {
                _repository = new Repository(_options.ApiToken);
                IsLoading = Visibility.Visible;
                Projects = new ObservableCollection<Project>();
                IsTabEnvironmentsVisible = false;
                IsTabUsersVisible = false;
                ShowMessageOnUI("Loading...Please wait!");
                //await GetDataFromServerAsync();
                var metrics = new Dictionary<string, double>
                    {
                        { "ProjectsCount", Projects.Count }
                    };
                var properties = new Dictionary<string, string>();
                Telemetry.Instance.TrackEvent("GetProjectsFromServer", properties, metrics);
                StartPollingForProjects();
                IsLoading = Visibility.Collapsed;

            }
        }

        private void HideMessageFromUI()
        {
            ShowMessage = false;
        }

        [HandleException]
        public void Search(string searchQuery)
        {
            Telemetry.Instance.TrackEvent("Search");
            var searchString = searchQuery.ToLower();

            IEnumerable<Project> searchResults;
            if (!_cacheProjectsSearch.ContainsKey(PROJECTS_KEY))
            {
                //put the original projects list from server in cache
                _cacheProjectsSearch.Add(PROJECTS_KEY, Projects.ToObservableCollection());
                searchResults = Projects.Where(project => Search(searchString, project));
                Telemetry.Instance.TrackEvent("Search results returned from server");
            }
            else
            {
                //original cache exists in cache, so use that for search
                var projects = _cacheProjectsSearch[PROJECTS_KEY];
                searchResults = projects.Where(project => Search(searchString, project));
                Telemetry.Instance.TrackEvent("Search results returned from cache");
            }

            Projects = searchResults.ToObservableCollection();
            _searchResultDisplayed = true;
        }

        public void TabClosed(ClosableTab closableTab)
        {
            switch (closableTab.Title)
            {
                case Constants.ENVIRONMENTS:
                    IsTabEnvironmentsVisible = false;
                    IsTabEnvironmentsSelected = false;
                    break;

                case Constants.USERS:
                    IsTabUsersVisible = false;
                    IsTabUsersSelected = false;
                    break;
            }
        }

        #endregion Public Methods

        #region Private Methods

        [HandleException]
        private static bool Search(string searchString, Project project)
        {
            var hasBuild = project.Builds.Any();
            return project.Name.ToLower().Contains(searchString);
            //|| project.RepositoryBranch.ToLower().Contains(searchString)
            //|| project.RepositoryName.ToLower().Contains(searchString)
            //|| hasBuild && project.Builds.First().Message.ToLower().Contains(searchString)
            //|| hasBuild && project.Builds.First().Version.Contains(searchString)
            // || hasBuild && project.Builds.First().AuthorName.ToLower().Contains(searchString)
            //|| hasBuild && project.Builds.First().AuthorUsername.ToLower().Contains(searchString)
            //|| hasBuild && project.Builds.First().CommitterName.ToLower().Contains(searchString)
            //|| hasBuild && project.Builds.First().CommitterUsername.ToLower().Contains(searchString)
            //|| hasBuild && project.Builds.First().CommitId.ToLower().Contains(searchString);
        }

        private bool CanCancelBuild(Project project)
        {
            return project.IsInProgress();
        }

        private bool CanExecuteProjectCommitIdClickedCommand(Project project)
        {
            return project.RepositoryType.EqualsIgnoreCase("github");
        }

        private bool CanExecuteProjectCommittedByClicked(Project project)
        {
            return project.RepositoryType.EqualsIgnoreCase("github");
        }

        private bool CanStartBuild(Project project)
        {
            if (project.IsInProgress())
                return false;
            return true;
        }

        private async Task GetDataFromServerAsync()
        {
            var response = await _repository.GetProjects();
            if (response.HasError)
            {
                var properties = new Dictionary<string, string>();
                properties["Method Name"] = "GetProjectsFromServer";
                Telemetry.Instance.TrackException(response.Exception, properties);
                if (response.Exception.GetType() == typeof(AppVeyorUnauthorizedException))
                {
                    // (Errors.IsEmpty() || (!Errors.IsEmpty() && Errors.Any(x => (DateTime.Now - x.TimeStamp).Minutes > 1)))
                    {
                        AddAlert(response.Exception, response.Exception.Message); 
                    }
                    //if unauthorized pause till correct token is set
                    StopPollingForProjects();
                    ShowMessageOnUI("Error occurred...Please check Errors tab for more details.");
                }
                else
                {
                    AddAlert(response.Exception, response.Exception.Message);
                }
            }
            else
            {
                var projectsFromServer = response.Result.ToObservableCollection();
                var comparer = new ProjectComparer();
                if (!_searchResultDisplayed && !projectsFromServer.ListEqual(Projects, comparer))
                {
                    //rebind only if any of the project properties are changed
                    await Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new Action(delegate()
                    {
                        Projects = projectsFromServer;
                        IsLoading = Visibility.Collapsed;
                        HideMessageFromUI();

                    }));
                }
            }
            // Users = await model.GetUsers();
            // Environments = await model.GetEnvironments();
        }

        private void ShowMessageOnUI(string message)
        {
            ShowMessage = true;
            Message = message;
        }

        [HandleException]
        private async void OnCancelBuildClicked(Project project)
        {
            if (TaskDialog.OSSupportsTaskDialogs && _options.ConfirmStop)
            {
                StopPollingForProjects();
                using (TaskDialog dialog = new TaskDialog())
                {
                    dialog.WindowTitle = Constants.EXTENSION_NAME;
                    dialog.Width = 250;
                    dialog.AllowDialogCancellation = true;
                    dialog.IsVerificationChecked = !_options.ConfirmStop;
                    dialog.VerificationText = "Do not show this dialog again";

                    dialog.MainInstruction = string.Format("Stop build for {0}?", project.Name);
                    dialog.MainIcon = TaskDialogIcon.Information;
                    dialog.Content = string.Format("Clicking Stop will trigger build for {0}", project.Name);
                    TaskDialogButton stopBuildButton = new TaskDialogButton("Stop");
                    dialog.Buttons.Add(stopBuildButton);
                    var cancelButton = new TaskDialogButton("Cancel");
                    dialog.Buttons.Add(cancelButton);
                    cancelButton.Default = true;
                    var taskDialogButton = dialog.ShowDialog();

                    if (taskDialogButton == cancelButton)
                    {
                        Telemetry.Instance.TrackEvent("Cancel Build confirmation dialog cancelled");
                        StartPollingForProjects();
                        return;
                    }
                    if (taskDialogButton == stopBuildButton)
                    {
                        var properties = new Dictionary<string, string>
                        {
                            {"Prompt before build stop", (!dialog.IsVerificationChecked).ToString()}
                        };
                        Telemetry.Instance.TrackEvent("Cancel Build triggered after confirmation", properties);
                        IsLoading = Visibility.Visible;
                        _options.ConfirmStop = !dialog.IsVerificationChecked;
                        _options.SaveSettingsToStorage();
                        var response = await _repository.CancelBuild(project);
                        ValidateResponseAndRefresh(response);
                    }
                }
            }
            else
            {
                Telemetry.Instance.TrackEvent("Cancel Build without confirmation");
                IsLoading = Visibility.Visible;
                var response = await _repository.CancelBuild(project);
                ValidateResponseAndRefresh(response);
            }
        }

        private void OnClearMessagesClicked(object obj)
        {
            Errors.Clear();
            Errors = Errors.ToObservableCollection();
            Telemetry.Instance.TrackEvent("Errors cleared");
        }

        private void OnDeploymentsClicked(Project project)
        {
            var url = string.Format("{0}project/{1}/{2}/deployments", APPVEYOR_URL, project.AccountName, project.Name);
            OpenUrl(url);
            Telemetry.Instance.TrackEvent("Depolyments link clicked");
        }

        private void OnOptionsChanged(object sender, OptionsChangedEventArgs args)
        {
            var properties = new Dictionary<string, string>
            {
                {"Prompt before build start", (!args.AppVeyorOptions.ConfirmStart).ToString()},
                {"Prompt before build stop", (!args.AppVeyorOptions.ConfirmStart).ToString()}
            };
            Telemetry.Instance.TrackEvent("Options changed", properties);

            Initialize();
        }

        private void OnProjectBuildMessageClicked(Project project)
        {
            var url = string.Format("{0}project/{1}/{2}/build/{3}", APPVEYOR_URL,
                project.AccountName, project.Name, project.ToBuildVerionString());
            OpenUrl(url);
            Telemetry.Instance.TrackEvent("Build message link clicked");
        }

        private void OnProjectBuildVersionClicked(Project project)
        {
            var url = string.Format("{0}project/{1}/{2}/build/{3}", APPVEYOR_URL,
               project.AccountName, project.Name, project.ToBuildVerionString());
            OpenUrl(url);
            Telemetry.Instance.TrackEvent("Build version link clicked");

        }

        private void OnProjectNameClicked(Project project)
        {
            var url = string.Format("{0}project/{1}/{2}", APPVEYOR_URL, project.AccountName, project.Name);
            OpenUrl(url);
            Telemetry.Instance.TrackEvent("Project name link clicked");
        }

        private void OnSettingsClicked(Project project)
        {
            var url = string.Format("{0}project/{1}/{2}/settings", APPVEYOR_URL, project.AccountName, project.Name);
            OpenUrl(url);

            Telemetry.Instance.TrackEvent("Project settings link clicked");
        }

        [HandleException]
        private async void OnStartBuildClicked(Project project)
        {
            if (TaskDialog.OSSupportsTaskDialogs && _options.ConfirmStart)
            {
                StopPollingForProjects();
                using (TaskDialog dialog = new TaskDialog())
                {
                    dialog.WindowTitle = Constants.EXTENSION_NAME;
                    dialog.Width = 250;

                    dialog.AllowDialogCancellation = true;
                    dialog.IsVerificationChecked = !_options.ConfirmStart;
                    dialog.VerificationText = "Do not show this dialog again";
                    dialog.MainInstruction = string.Format("Start build for {0}?", project.Name);
                    dialog.MainIcon = TaskDialogIcon.Information;
                    dialog.Content = string.Format("Clicking Start will trigger build for {0}", project.Name);
                    TaskDialogButton startBuildButton = new TaskDialogButton("Start");
                    dialog.Buttons.Add(startBuildButton);
                    var cancelButton = new TaskDialogButton("Cancel ");
                    dialog.Buttons.Add(cancelButton);
                    cancelButton.Default = true;
                    var taskDialogButton = dialog.ShowDialog();

                    if (taskDialogButton == cancelButton)
                    {
                        Telemetry.Instance.TrackEvent("Start Build confirmation dialog cancelled");
                        StartPollingForProjects();
                        return;
                    }
                    if (taskDialogButton == startBuildButton)
                    {
                        var properties = new Dictionary<string, string>
                        {
                            {"Prompt before build start", (!dialog.IsVerificationChecked).ToString()}
                        };
                        Telemetry.Instance.TrackEvent("Start Build triggered after confirmation", properties);

                        _options.ConfirmStart = !dialog.IsVerificationChecked;
                        _options.SaveSettingsToStorage();

                        IsLoading = Visibility.Visible;
                        var response = await _repository.StartBuild(project);
                        ValidateResponseAndRefresh(response);
                    }
                }
            }
            else
            {
                Telemetry.Instance.TrackEvent("Start Build without confirmation");
                IsLoading = Visibility.Visible;
                var response = await _repository.StartBuild(project);
                ValidateResponseAndRefresh(response);
            }
        }

        private void OnViewHistoryClicked(Project project)
        {
            //https://ci.appveyor.com/project/onlyutkarsh/dropdownlinkbutton/history
            var url = string.Format("{0}project/{1}/{2}/history", APPVEYOR_URL, project.AccountName, project.Name);
            OpenUrl(url);
            Telemetry.Instance.TrackEvent("Project history link clicked");
        }

        [HandleException(false)]
        private void OpenUrl(string url)
        {
            //IVsWindowFrame ppFrame;
            //if (_browserService == null)
            //{
            //    _browserService = ServiceProvider.GetService(typeof(IVsWebBrowsingService)) as IVsWebBrowsingService;
            //}
            //if (_browserService != null && _options.OpenLinksInVisualStudio)
            //{
            //    _browserService.Navigate(url, (uint)__VSWBNAVIGATEFLAGS.VSNWB_ForceNew, out ppFrame);
            //}
            //else
            //{
            //Unable to find the browser service, open it with default browser
            Process.Start(url);
            //}
        }

        [HandleException]
        private async Task Poll()
        {
            // Start polling, don't stop polling
            while (true)
            {
                //Debug.WriteLine("***POLLING***");
                //await token.WaitWhilePausedAsync();
                await _isPausedResetEvent.WaitAsync();

                // Get data
                await GetDataFromServerAsync();
                //Debug.WriteLine("***BOUND TO UI***");
                await Task.Delay(_timeToWait);
            }
        }

        private void ProjectCommitIdClicked(Project project)
        {
            var url = string.Format("{0}{1}/commit/{2}", GIT_URL, project.RepositoryName, project.ToCommitIdString());
            OpenUrl(url);
            Telemetry.Instance.TrackEvent("Project CommitId clicked");
        }

        private void ProjectCommittedByClicked(Project project)
        {
            var gitUserName = project.RepositoryName.Split('/').ToList().FirstOrDefault();
            var url = GIT_URL;
            if (!string.IsNullOrWhiteSpace(gitUserName))
            {
                url = string.Format("{0}{1}", GIT_URL, gitUserName);
            }
            OpenUrl(url);
            Telemetry.Instance.TrackEvent("Project Committed by clicked");
        }

        private void ValidateResponseAndRefresh<T>(AppVeyorServiceResponse<T> response)
        {
            if (!response.HasError)
            {
                Initialize();
                return;
            }
            AddAlert(response.Exception, response.Exception.Message);
        }

        #endregion Private Methods

        [HandleException(false)]
        private void OnCopyMessageClicked(Message message)
        {
            Telemetry.Instance.TrackEvent("Copy to clipboard");
            Clipboard.SetDataObject(string.Format("TimeStamp:{0}{1}Message:{2}", message.TimeStamp, Environment.NewLine,
                   message.Text));
        }
    }
}