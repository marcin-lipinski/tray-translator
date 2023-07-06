using OibraryExample;
using System.Windows;

namespace TrayTranslator;
public partial class ChildForm : Window
{
    private readonly IExampleOfDI exampleOfDI;

    public ChildForm(IExampleOfDI exampleOfDI)
    {
        InitializeComponent();
        this.exampleOfDI = exampleOfDI;
        exampleOfDInfo.Text = exampleOfDI.GetString();
    }
}
