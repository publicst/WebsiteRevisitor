using System.Windows;

namespace WebsiteRevisitor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MainWindow window = new MainWindow();

            string jsonPath = "Data/websites.json"; // make sure to include it as resource
            var viewModel = new MainWindowViewModel(jsonPath);

            window.DataContext = viewModel;
            window.Show();
        }
    }
}
