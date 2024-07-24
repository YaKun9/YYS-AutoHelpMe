using AutoHelpMe.ViewModels.Windows;
using SageTools.Extension;
using Wpf.Ui;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace AutoHelpMe.Views.Windows
{
    public partial class MainWindow : INavigationWindow
    {
        private readonly string _originTitle;
        public MainWindowViewModel ViewModel { get; }

        public MainWindow(MainWindowViewModel viewModel, IPageService pageService, INavigationService navigationService)
        {
            ViewModel = viewModel;
            DataContext = this;
            _originTitle = viewModel.ApplicationTitle;
            SystemThemeWatcher.Watch(this);

            InitializeComponent();
            SetPageService(pageService);
            RootNavigation.SelectionChanged += RootNavigationOnSelectionChanged;
            navigationService.SetNavigationControl(RootNavigation);
        }

        private void RootNavigationOnSelectionChanged(NavigationView sender, RoutedEventArgs args)
        {
            var pageContent = RootNavigation?.SelectedItem?.Content.ToString();
            if (pageContent.IsNullOrWhiteSpace() || pageContent == "首页")
            {
                pageContent = string.Empty;
            }
            else
            {
                pageContent = $" - {pageContent}";
            }
            ViewModel.ApplicationTitle = _originTitle + pageContent;
        }

        #region INavigationWindow methods

        public INavigationView GetNavigation() => RootNavigation;

        public bool Navigate(Type pageType) => RootNavigation.Navigate(pageType);

        public void SetPageService(IPageService pageService) => RootNavigation.SetPageService(pageService);

        public void ShowWindow() => Show();

        public void CloseWindow() => Close();

        #endregion INavigationWindow methods

        /// <summary>
        /// Raises the closed event.
        /// </summary>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Make sure that closing this window will begin the process of closing the application.
            Application.Current.Shutdown();
        }

        INavigationView INavigationWindow.GetNavigation()
        {
            throw new NotImplementedException();
        }

        public void SetServiceProvider(IServiceProvider serviceProvider)
        {
            throw new NotImplementedException();
        }
    }
}