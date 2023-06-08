using DGSWManager.Data;
using DGSWManager.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace DGSWManager.Views.company;


public partial class CompanyListPage : ContentPage
{
    CorprationDatabase database;
    public ObservableCollection<CorprationInfoModel> Items { get; set; } = new();
    public CompanyListPage(CorprationDatabase corprationDatabase)
    {
        InitializeComponent();
        database = corprationDatabase;
        BindingContext = this;
    }
    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        var items = await database.GetItemsAsync();
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Items.Clear();
            foreach(var item in items)
            {
                Items.Add(item);
            }        
        });
    }
    async void OnItemAdded(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(CompanyRegister), true, new Dictionary<string, object>
        {
            ["Item"] = new CorprationInfoModel()
        });
    }

    private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not CorprationInfoModel item)
            return;

        await Shell.Current.GoToAsync(nameof(CompanyRegister), true, new Dictionary<string, object>
        {
            ["Item"] = item
        });
    }
}