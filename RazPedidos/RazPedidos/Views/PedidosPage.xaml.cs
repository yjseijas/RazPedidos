using Syncfusion.SfDataGrid.XForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DevExpress.Mobile.DataGrid;
using DevExpress.Mobile.DataGrid.Theme;

namespace RazPedidos.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PedidosPage : ContentPage
	{

        public PedidosPage ()
		{
			InitializeComponent();

            

            ThemeManager.ThemeName = Themes.Light;

            ThemeManager.Theme.HeaderCustomizer.BackgroundColor = Color.FromRgb(187, 228, 208);
            ThemeFontAttributes myFont = new ThemeFontAttributes("Blue",
                                        ThemeFontAttributes.FontSizeFromNamedSize(NamedSize.Medium),
                                        FontAttributes.Bold, Color.Black);
            ThemeManager.Theme.HeaderCustomizer.Font = myFont;

            ThemeManager.Theme.CellCustomizer.SelectionColor = Color.FromRgb(186, 220, 225);
            ThemeFontAttributes myFont1 = new ThemeFontAttributes("Yellow",
                                        ThemeFontAttributes.FontSizeFromNamedSize(NamedSize.Medium),
                                        FontAttributes.None, Color.Black);
            ThemeManager.Theme.CellCustomizer.Font = myFont1;

            
            //grid.SortMode = GridSortMode.Multiple;

            //grid.Columns["num"].SortOrder = DevExpress.Data.ColumnSortOrder.Descending;
            //grid.Columns["num"].SortIndex = 0;

            //grid.Columns["codcli"].SortOrder = DevExpress.Data.ColumnSortOrder.Descending;
            //grid.Columns["codcli"].SortIndex = 1;


        }
    }
}