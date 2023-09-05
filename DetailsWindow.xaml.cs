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
    /// Interaction logic for DetailsWindow.xaml
    /// </summary>
    public partial class DetailsWindow : Window
    {
        private object reciperecipe;
        private Recipe recipe = new Recipe();
        private int theID = new();

        public Recipe RecipeId { get; }

        public DetailsWindow(Recipe recipeId)
        {
            InitializeComponent();
            showIngridient(recipeId.RecipeId);
            RecipeId = recipeId;
            this.theID = recipeId.RecipeId;
            btnRemoveIngridient.IsEnabled = false;
            btnChangeIngridient.IsEnabled = false;

            btnSave.IsEnabled = false;
            btnRecipeTags.IsEnabled = false;
        }


        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            // Stäng ner sidan och hå tillbaka till Main Window
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            //stänger sidan
            Close();
        }


        private void lvlIngridiens_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
       
            if (lvlIngridiens.SelectedIndex > -1)
            {

            }
   
            else if (lvlIngridiens.SelectedIndex == -1)
            {
            
                btnRemoveIngridient.IsEnabled = false;
                btnChangeIngridient.IsEnabled = false;

            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            using (YellowCarrotContext context = new YellowCarrotContext())
            {
             
                if (txbIngridientName.Text.Length == 0)
                {
                    MessageBox.Show("Ingridient must contain a name!");
                }
                else
                {

               
                    new IngridentRepo(context).AddIngridient(new Ingridient()
                    {
                        Name = txbIngridientName.Text,
                        Quantity = txbIngridientQuantity.Text,
                        RecipeID = RecipeId.RecipeId,
                    });


                    context.SaveChanges();
                    
                    MessageBox.Show("Ingridient was now added to the list!");
              

                    UpdateUIList();
                }
            }
        }

        private void UpdateUIList()
        {
            // Ta bort allt
            lvlIngridiens.Items.Clear();
            txbIngridientName.Clear();
            txbIngridientQuantity.Clear();
           
            showIngridient(theID);
        }


        private void btnChangeIngridient_Click(object sender, RoutedEventArgs e)
        {
            if (lvlIngridiens.SelectedItem == null)
            {
                MessageBox.Show("You forgot to choose what ingridient from list!");
            }
            else
            {
                // Tilldelar ingrediensen en tagg 
                ListViewItem SelectedItem = lvlIngridiens.SelectedItem as ListViewItem;
                Ingridient ingridient = SelectedItem.Tag as Ingridient;


                ChangeIngridientsWindow changeIngridientWindow = new(ingridient,recipe);
                changeIngridientWindow.Show();
                Close();
            }


        }

        // Lås upp knapp och visa ingrediens
        private void btnUnlock_Click(object sender, RoutedEventArgs e)
        {
            btnRemoveIngridient.IsEnabled = true;
            btnChangeIngridient.IsEnabled = true;

            btnSave.IsEnabled = true;
            btnRecipeTags.IsEnabled = true;
            btnUnlock.IsEnabled = false;
        }

        private void showIngridient(int recipeId)
        {

            using (YellowCarrotContext context = new YellowCarrotContext())
            {
             
                var I = new IngridentRepo(context).GetIngridient(recipeId);

                
                foreach (Ingridient currentRecipe in I)
                {
                    ListViewItem item = new();
                    item.Tag = currentRecipe;
                    item.Content = $"{currentRecipe.Name}  {currentRecipe.Quantity}";
                    lvlIngridiens.Items.Add(item);

                }
            }

        }

        private void btnRemoveIngridient_Click(object sender, RoutedEventArgs e)
        {
          
            ListViewItem selectedItem = lvlIngridiens.SelectedItem as ListViewItem;
          
            Ingridient ingridient = selectedItem.Tag as Ingridient;

            
            using (YellowCarrotContext context = new YellowCarrotContext())
            {
                new IngridentRepo(context).RemoveIngridient(ingridient);
                context.SaveChanges();

            }

            UpdateUIList();
        }

        private void btnInfo_Click(object sender, RoutedEventArgs e)
        {
       
            MessageBox.Show
       ("Here you can add Ingrident/quantity, change or delete a recipe\r \r");
        }

        private void btnRecipeTags_Click(object sender, RoutedEventArgs e)
        {
            
            ChangeTagsWindow changeTagsWindow = new(RecipeId);
            changeTagsWindow.Show();

        
            Close();
        }
    }
}
