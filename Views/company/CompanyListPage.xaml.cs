using DGSWManager.Data;
using DGSWManager.Models;
using System.Collections.ObjectModel;
namespace DGSWManager.Views.company;


public partial class CompanyListPage : ContentPage
{
    CorprationDatabase database;
	public class Content
	{
		public string ContentName { get; set; }
	}
    public ObservableCollection<CorprationInfoModel> Items { get; set; } = new();
    ObservableCollection<Content> contents = new ObservableCollection<Content>();
    public ObservableCollection<Content> Contents { get { return contents; } }
    protected override async void OnNavigatedTo(NavigatedToEventArgs e)
    {
        var items = await database.GetItemsAsync();
        MainThread.BeginInvokeOnMainThread(() =>
        {
            contents.Clear();
            foreach (var item in items)
                contents.Add(new Content() { ContentName = item.CorprationName });
        });
    }
    public CompanyListPage(CorprationDatabase corprationDatabase)
	{
        InitializeComponent(); 
        database = corprationDatabase;
        collectionView.ItemsSource = contents;
    }
}