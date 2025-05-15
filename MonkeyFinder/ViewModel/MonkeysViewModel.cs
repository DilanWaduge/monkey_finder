using MonkeyFinder.Services;

namespace MonkeyFinder.ViewModel;

public partial class MonkeysViewModel : BaseViewModel
{
    private readonly MonkeyService _monkeyService;

    // public Command GetMonkeysCommand { get; }
    public MonkeysViewModel(MonkeyService monkeyService)
    {
        Title = "Monkey Finder";
        _monkeyService = monkeyService;
        // GetMonkeysCommand = new Command(async () => await GetMonkeysAsync());
        // GetMonkeysCommand.Execute(null);
    }

    public ObservableCollection<Monkey> Monkeys { get; set; } = [];

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
            Debug.WriteLine(ex);

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