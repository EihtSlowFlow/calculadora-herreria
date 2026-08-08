using CommunityToolkit.Mvvm.ComponentModel;

namespace CalculadoraHerreria.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private string _greeting = "¡Calculadora de Herrería Lista!";
}
