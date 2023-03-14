using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DGSWManager.Models;
using DGSWManager.RestAPI;
using System.Collections.ObjectModel;

namespace DGSWManager.ViewModels;
public partial class CorpInfoViewModel : ObservableObject
{
    public CorpInfoViewModel(IUserService userService)
    {
        _service = userService;
        CorpInfoModels ??= new ObservableCollection<CorpInfoModel>();
    }
    [ObservableProperty]
    private int _counter;

    [ObservableProperty]
    private string _message;

    private readonly IUserService _service;

    [RelayCommand]
    private void IncrementCounter() => Counter++;

    [RelayCommand]
    private async void CallAPI()
    {
        Message = await _service.Execute();


        var bizs = System.Text.Json.JsonSerializer.Deserialize<List<CorpInfoModel>>(Message);
        foreach (var biz in bizs)
        {
            CorpInfoModels.Add(new CorpInfoModel
            {
                bizNo = biz.bizNo,
                taxType = biz.taxType,
            });
        }
    }
    public ObservableCollection<CorpInfoModel> CorpInfoModels
    {
        get;
        set;
    }
}
public partial class CorpInfoViewModel : ObservableObject
{
    [ObservableProperty]
    public int bizNo;
    [ObservableProperty]
    public string taxType;
}



