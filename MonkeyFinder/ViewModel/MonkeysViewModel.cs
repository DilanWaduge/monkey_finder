using MonkeyFinder.Services;

namespace MonkeyFinder.ViewModel;

public partial class MonkeysViewModel : BaseViewModel
{
    private readonly IGeolocation _geolocation;
    private readonly MonkeyService _monkeyService;

    // public Command GetMonkeysCommand { get; }
    public MonkeysViewModel(
        IGeolocation geolocation,
        MonkeyService monkeyService)
    {
        Title = "Monkey Finder";
        _monkeyService = monkeyService;
        _geolocation = geolocation;
        // GetMonkeysCommand = new Command(async () => await GetMonkeysAsync());
        // GetMonkeysCommand.Execute(null);
    }

    public ObservableCollection<Monkey> Monkeys { get; set; } = [];

    [RelayCommand]
    public async Task GetClosestMonkeyAsync()
    {
        if (IsBusy || Monkeys.Count == 0)
        {
            return;
        }

        try
        {
            Location location = await _geolocation.GetLastKnownLocationAsync();
            if (location == null)
            {
                location = await _geolocation.GetLocationAsync(new GeolocationRequest { DesiredAccuracy = GeolocationAccuracy.Medium, Timeout = TimeSpan.FromSeconds(30) });
            }

            Monkey first = Monkeys.OrderBy(m => location.CalculateDistance(new Location(m.Latitude, m.Longitude), DistanceUnits.Miles))
                .FirstOrDefault();

            await Shell.Current.DisplayAlert("", first?.Name + " " + first?.Location, "OK");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
    }

    [RelayCommand]
    public async Task GoToDetailsAsync(Monkey monkey)
    {
        if (monkey is null)
        {
            return;
        }

        await Shell.Current.GoToAsync(
            $"{nameof(DetailsPage)}",
            true,
            new Dictionary<string, object> { { "Monkey", monkey } });
    }

    [RelayCommand]
    public async Task GetMonkeysAsync()
    {
        if (IsBusy)
        {
            return;
        }

        try
        {
            IsBusy = true;
            IReadOnlyList<Monkey> monkeys = await _monkeyService.GetMonkeysAsync();

            if (Monkeys.Count != 0)
            {
                Monkeys.Clear();
            }

            foreach (Monkey monkey in monkeys)
            {
                Monkeys.Add(monkey);
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert(
                "Error!",
                "Unable to get monkeys due to error",
                "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}