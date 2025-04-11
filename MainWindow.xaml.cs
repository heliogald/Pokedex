using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Pokedex
{
    public partial class MainWindow : Window
    {
        private int currentPokemonId = 1;
        private readonly HttpClient httpClient = new HttpClient();

        public MainWindow()
        {
            InitializeComponent();
            _ = LoadPokemon(currentPokemonId); // carregamento inicial
        }

        private async Task LoadPokemon(object idOrName)
        {
            try
            {
                string url = $"https://pokeapi.co/api/v2/pokemon/{idOrName}";
                string json = await httpClient.GetStringAsync(url);
                var pokemon = JsonConvert.DeserializeObject<PokemonData>(json);

                currentPokemonId = pokemon.id;
                PokemonName.Text = $"{pokemon.name.ToUpper()} (#{pokemon.id})";
                PokemonDescription.Text = $"Altura: {pokemon.height} | Peso: {pokemon.weight}";
                PokemonImage.Source = new BitmapImage(new Uri(pokemon.sprites.front_default));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar Pokémon: {ex.Message}");
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                _ = LoadPokemon(SearchBox.Text.ToLower());
            }
        }

        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SearchButton_Click(sender, e);
            }
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPokemonId > 1)
            {
                _ = LoadPokemon(currentPokemonId - 1);
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            _ = LoadPokemon(currentPokemonId + 1);
        }
    }
}
