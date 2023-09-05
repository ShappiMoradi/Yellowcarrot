using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using YellowCarrot.Data;
using YellowCarrot.Model;
using YellowCarrot.Repos;

namespace YellowCarrot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public object reciperecipe { get; private set; }

        public MainWindow()
        {
            InitializeComponent();

            btnChangeName.IsEnabled = false;
            btnSelect.IsEnabled = false;
            btnDelete.IsEnabled = false;

            UpdateUIList();
            ShowAllTags();


        }

        private void lvlRecipeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (YellowCarrotContext context = new YellowCarrotContext())
            {
                if (lvlRecipeList.SelectedIndex > -1)
                {
                    btnSelect.IsEnabled = true;
                    btnDelete.IsEnabled = true;
                    btnChangeName.IsEnabled = true;

                }
                // Om inget blir valt
                else if (lvlRecipeList.SelectedIndex == -1)
                {
                    btnSelect.IsEnabled = false;
                    btnDelete.IsEnabled = false;
                    btnChangeName.IsEnabled = false;
                }
            }




        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Recipe recipe = new Recipe();
           



            using (YellowCarrotContext context = new YellowCarrotContext())
            {
                // Popup ruta som dyker up om användaren har angett mindre än 3 bokstäver

                if (txbRecipeName.Text.Length == 0)
                {
                    MessageBox.Show("Please enter the recipe name ");
                }
                else if (txbRecipeName.Text.Length < 3)
                {
                    MessageBox.Show("The recipe must contain atleast 3 letters! ");
                }
                else if (cbxTagSelection.SelectedIndex < 0)
                {
                    MessageBox.Show("Enter tag first before saving");
                }
                else
                {
                        
                        recipe.RecipeName = txbRecipeName.Text;
                        new RecipeRepo(context).AddRecipe(recipe);
                        ComboBoxItem selectedItem = cbxTagSelection.SelectedItem as ComboBoxItem;
                        Tags SelectedTag = selectedItem.Tag as Tags;

                        Tags TagDb = context.Tags.FirstOrDefault(t => t.TagNameId == SelectedTag.TagNameId);




                        // Anger TagID och lagrar infon till databasen
                        recipe.tags = TagDb;
                       
                        
                        context.SaveChanges();


                        MessageBox.Show($"{recipe.RecipeName} is now added to the list");
                        txbRecipeName.Clear();

                        UpdateUIList();
                    

                }



                


            }
        }

        // Väljer recept som ska justeras och öppnar ett nytt fönster
        private void btnChangeName_Click(object sender, RoutedEventArgs e)
        {
            ListViewItem selectedItem = lvlRecipeList.SelectedItem as ListViewItem;
            Recipe recipe = selectedItem.Tag as Recipe;
            ChangeRecipeNameWindow changeRecipeNameWindow = new(recipe);
            changeRecipeNameWindow.Show();
            Close();
        }


        // Uppdatera UI
        private void UpdateUIList()
        {
            lvlRecipeList.Items.Clear();

            using (YellowCarrotContext context = new YellowCarrotContext())
            {
                var AllRecipes = new RecipeRepo(context).GetAllRecipes();

                foreach (var allRecipe in AllRecipes)
                {
                    ListViewItem item = new ListViewItem();

                    item.Tag = allRecipe;
                    item.Content = allRecipe.RecipeName;
                    lvlRecipeList.Items.Add(item);
                    lvlRecipeList.SelectedItem = btnSelect;
                }


            }

        }
        //Visa tagg
        private void ShowAllTags()
        {
            using (YellowCarrotContext context = new YellowCarrotContext())
            {
                var allTags = new TagRepo(context).GetTags();

                foreach (var tag in allTags)
                {
                    ComboBoxItem item = new();
                    item.Tag = tag;
                    item.Content = tag.TagNameId;
                    cbxTagSelection.Items.Add(item);
                }
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
           
            ListViewItem selectedItem = lvlRecipeList.SelectedItem as ListViewItem;
     
            Recipe recipe = selectedItem.Tag as Recipe;

            using (YellowCarrotContext context = new YellowCarrotContext())
            {
                
                if (MessageBox.Show("Are you sure that you want to delete this recipe?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {

                }
                else
                {
                  
                    new RecipeRepo(context).RemoveRecipe(recipe);

           
                    context.SaveChanges();
                    UpdateUIList();  
                    
                    
                }
            }
           
        }
        // info om appen
        private void btnInfo_Click(object sender, RoutedEventArgs e)
        {
        
            MessageBox.Show("  Welcome to info\r\r " +
                "When added a recipe you can see it in the list!\r " +
                "If you want to add an ingridient or tag you can do so by select the recipe/tag from the list and then click on the details button\r\r\r If you wish to delete an recipe or see the details you have to select a Recipe from the list!");
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            using (YellowCarrotContext context = new YellowCarrotContext())
            {

                ListViewItem? item = lvlRecipeList.SelectedItem as ListViewItem;

                Recipe reciperecipe = item.Tag as Recipe;


                btnSelect.IsEnabled = true;

                DetailsWindow detailsWindow = new(reciperecipe);

                detailsWindow.Show();
            }
            Close();
        }
    }
}

