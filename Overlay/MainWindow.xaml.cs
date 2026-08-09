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

            StatusText.Text = $"Ready — {_recipes.Count} items loaded.";
            FarmButton.IsEnabled = true;

            var overlay = new FarmOverlay();
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
                string line = _locations.TryGetValue(item.Key, out var loc)
                    ? $"{item.Value}x {item.Key} — best at {loc.Location} ({loc.Chance}%)"
                    : $"{item.Value}x {item.Key} — no drop location";
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

            foreach (var recipe in _recipes.Values)
                foreach (var ingredient in recipe.Ingredients)
                    if (ingredient.BestLocation != null)
                        _locations[ingredient.Name] = ingredient.BestLocation;
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