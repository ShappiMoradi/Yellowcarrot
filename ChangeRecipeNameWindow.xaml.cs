using System;
using System.Collections.Generic;
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
    /// Interaction logic for ChangeRecipeNameWindow.xaml
    /// </summary>
    public partial class ChangeRecipeNameWindow : Window
    {
        private Recipe Recipe;
        public ChangeRecipeNameWindow(Recipe CurrentRecipe)
        {
            InitializeComponent();
            this.Recipe = CurrentRecipe;
        }

        private void btnSaveName_Click(object sender, RoutedEventArgs e)
        {
            using (YellowCarrotContext context = new YellowCarrotContext())
            {
               
                if (txbChangeRecipeName.Text.Length < 3)
                {
                    MessageBox.Show("The Recipe must contain a minimum of 3 letters");
                }
                else
                {
                    Recipe.RecipeName = txbChangeRecipeName.Text;
                    new RecipeRepo(context).UpdateRecipe(Recipe);
               
                    context.SaveChanges();

                   MessageBox.Show("New Recipe name has been changed!");
            
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                 
                    Close();

                }


            }
        }
    }
}
