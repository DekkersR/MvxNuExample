using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace MvxNuExample.UWP
{
    public class NavMenuItem
    {
        public string Label { get; set; }
        public Symbol Symbol { get; set; }
        public char SymbolAsChar => (char)Symbol;
        public ICommand Command { get; set; }
        public object Parameters { get; set; }
    }
}
