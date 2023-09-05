using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using YellowCarrot.Data;
using YellowCarrot.Model;
using YellowCarrot.Repos;

namespace YellowCarrot
{
    /// <summary>
    /// Interaction logic for ChangeIngridientsWindow.xaml
    /// </summary>
    public partial class ChangeIngridientsWindow : Window
    {
        private Ingridient Spaghetti;
        private Recipe recipe;

        public ChangeIngridientsWindow(Ingridient ingridient, Recipe theRecipe)
        {
            InitializeComponent();

            this.Spaghetti = ingridient;
            this.recipe = theRecipe;
        }

        private void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            using (YellowCarrotContext context = new YellowCarrotContext())
            {

                if (txbName.Text.Length == 0)
                {

                    MessageBox.Show("Please write the ingredient to save");
                }
                else
                {

                    Spaghetti.Name = txbName.Text;
                    Spaghetti.Quantity = txbInfo.Text;


                    new IngridentRepo(context).UpdateIngridient(Spaghetti);
                    context.SaveChanges();


                    DetailsWindow detailsWindow = new(recipe);
                    detailsWindow.Show();

                    Close();

                }
            }
        }
    }
}
