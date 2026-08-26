using System.Net.Http;
using System.Windows;
using Core;

namespace Overlay
{
    public partial class MainWindow : Window
    {
        private static readonly string[] Urls =
        {
            "https://raw.githubusercontent.com/WFCD/warframe-items/master/data/json/Warframes.json",
            "https://raw.githubusercontent.com/WFCD/warframe-items/master/data/json/Primary.json",
            "https://raw.githubusercontent.com/WFCD/warframe-items/master/data/json/Secondary.json",
            "https://raw.githubusercontent.com/WFCD/warframe-items/master/data/json/Melee.json",
            "https://raw.githubusercontent.com/WFCD/warframe-items/master/data/json/Archwing.json",
            "https://raw.githubusercontent.com/WFCD/warframe-items/master/data/json/Arch-Gun.json",
            "https://raw.githubusercontent.com/WFCD/warframe-items/master/data/json/Arch-Melee.json",
            "https://raw.githubusercontent.com/WFCD/warframe-items/master/data/json/Sentinels.json",
            "https://raw.githubusercontent.com/WFCD/warframe-items/master/data/json/SentinelWeapons.json",
            "https://raw.githubusercontent.com/WFCD/warframe-items/master/data/json/Pets.json",
            "https://raw.githubusercontent.com/WFCD/warframe-items/master/data/json/Gear.json",
        };

        private const string MissionRewardsUrl =
            "https://raw.githubusercontent.com/WFCD/warframe-drop-data/main/data/missionRewards.json";
        private Dictionary<string, Recipe> _recipes = new();
        private Dictionary<string, DropLocation> _locations = new();

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            StatusText.Text = "Loading Warframe data...";
            FarmButton.IsEnabled = false;

            await LoadData();

            StatusText.Text = $"Ready — {_recipes.Count} items, {_locations.Count} locations.";
            FarmButton.IsEnabled = true;

            var mockInventory = new Dictionary<string, int>
            {
                ["Nekros Prime Chassis Blueprint"] = 1,
                ["Orokin Cell"] = 2
            };

            var plan = FarmPlanBuilder.Build(
                "Nekros Prime", 1, mockInventory, _recipes, _locations);

            var overlay = new FarmOverlay();
            overlay.SetPlan(plan);
            overlay.Show();
        }

        private void FarmButton_Click(object sender, RoutedEventArgs e)
        {
            RunSearch();
        }

        private void RunSearch()
        {
            string target = TargetBox.Text;
            var plan = Planner.Plan(target, 1, new Dictionary<string, int>(), _recipes);

            ResultsList.Items.Clear();
            foreach (var item in plan)
            {
                string line;
                if (_locations.TryGetValue(item.Key, out var loc))
                {
                    string chance = loc.Chance.HasValue ? $" ({loc.Chance}%)" : "";
                    line = $"{item.Value}x {item.Key} — best at {loc.Location}{chance}";
                }
                else
                {
                    line = $"{item.Value}x {item.Key} — no drop location";
                }
                ResultsList.Items.Add(line);
            }
        }

        private async Task LoadData()
        {
            using var http = new HttpClient();

            foreach (string url in Urls)
            {
                try
                {
                    string json = await http.GetStringAsync(url);
                    foreach (var entry in WarframeDataAdapter.Parse(json))
                        _recipes[entry.Key] = entry.Value;
                }
                catch
                {
                    // Skip a file that fails to load.
                }
            }

            var components = new Dictionary<string, DropLocation>();
            foreach (var recipe in _recipes.Values)
                foreach (var ingredient in recipe.Ingredients)
                    if (ingredient.BestLocation != null)
                        components[ingredient.Name] = ingredient.BestLocation;

            var missionRewards = new Dictionary<string, DropLocation>();
            try
            {
                string json = await http.GetStringAsync(MissionRewardsUrl);
                missionRewards = MissionRewardsAdapter.Parse(json);
            }
            catch
            {
                // Optional source — fall back to component locations only.
            }

            _locations = DropLocationMerger.Merge(
                CuratedResourceLocations.All,
                DropLocationMerger.Merge(components, missionRewards));
        }

        private void TargetBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            string query = TargetBox.Text;

            if (query.Length == 0)
            {
                SuggestPopup.IsOpen = false;
                return;
            }

            var matches = ItemSearch.Match(query, _recipes.Keys);

            SuggestList.Items.Clear();
            foreach (var name in matches.Take(10))
                SuggestList.Items.Add(name);

            SuggestPopup.IsOpen = SuggestList.Items.Count > 0;
        }

        private void SuggestList_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (SuggestList.SelectedItem is string name)
            {
                TargetBox.Text = name;
                SuggestPopup.IsOpen = false;
            }
        }

        private void TargetBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            // Only act when the suggestion popup is open.
            if (!SuggestPopup.IsOpen)
                return;

            switch (e.Key)
            {
                case System.Windows.Input.Key.Down:
                    MoveSelection(+1);
                    e.Handled = true;
                    break;

                case System.Windows.Input.Key.Up:
                    MoveSelection(-1);
                    e.Handled = true;
                    break;

                case System.Windows.Input.Key.Enter:
                    if (SuggestList.SelectedItem is string picked)
                    {
                        TargetBox.Text = picked;
                        TargetBox.CaretIndex = picked.Length; // cursor to 
                    }
                    SuggestPopup.IsOpen = false;
                    RunSearch();
                    e.Handled = true;
                    break;

                case System.Windows.Input.Key.Escape:
                    SuggestPopup.IsOpen = false;
                    e.Handled = true;
                    break;
            }
        }

        private void MoveSelection(int direction)
        {
            int count = SuggestList.Items.Count;
            if (count == 0)
                return;

            int index = SuggestList.SelectedIndex + direction;

            // Wrap around top/bottom.
            if (index < 0) index = count - 1;
            if (index >= count) index = 0;

            SuggestList.SelectedIndex = index;
            SuggestList.ScrollIntoView(SuggestList.SelectedItem);
        }
    }
}