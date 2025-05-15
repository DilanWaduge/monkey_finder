namespace MonkeyFinder.ViewModel;

[QueryProperty("Monkey", "Monkey")]
public partial class MonkeyDetailsViewModel : BaseViewModel
{
    private readonly IMap _map;

    [ObservableProperty] private Monkey monkey;


    public MonkeyDetailsViewModel(IMap map)
    {
        _map = map;
    }

    [RelayCommand]
    private async Task OpenMap()
    {
        try
        {
            await _map.OpenAsync(
                Monkey.Latitude,
                Monkey.Longitude,
                new MapLaunchOptions { Name = Monkey.Name, NavigationMode = NavigationMode.None });
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error, no Maps app!", ex.Message, "OK");
        }
    }
}