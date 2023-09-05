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
    /// Interaction logic for ChangeTagsWindow.xaml
    /// </summary>
    public partial class ChangeTagsWindow : Window

    {
        private Recipe currentRecipe = new();
        public ChangeTagsWindow(Recipe Recipe)
        {
            InitializeComponent();

             this.currentRecipe = Recipe;
        

            ShowAllTags();
            ShowCurrentTag();
        }

        private void ShowCurrentTag()
        {
            
            using (YellowCarrotContext context = new YellowCarrotContext())
            {
                
                var CurrentTag = context.Tags.Where(x => x.TagNameId == currentRecipe.TagsID).ToList();

                foreach (var alltags in CurrentTag)
                {
                    ListViewItem item = new ListViewItem();
                    item.Tag = alltags;
                    item.Content = alltags.TagNameId;
                    lvlCurrentTags.Items.Add(item);
                }

            }
        }
        private void btnChangeTags_Click(object sender, RoutedEventArgs e)
        {
            using (YellowCarrotContext context = new YellowCarrotContext())
            {
         
                ComboBoxItem selectedItem = cbxAllTags.SelectedItem as ComboBoxItem;
                Tags theTag = selectedItem.Tag as Tags;

                
                Tags tagdb = context.Tags.FirstOrDefault(t => t.TagNameId == theTag.TagNameId);

                
                currentRecipe.tags = tagdb;
      
                context.Recipes.Update(currentRecipe);
               
                context.SaveChanges();

                DetailsWindow detailsWindow = new(currentRecipe);
                detailsWindow.Show();
                Close();

            }
        }

        // Closes window and goes back to DetailsWindow
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
          
            DetailsWindow detailsWindow = new(currentRecipe);
            detailsWindow.Show();
           
            Close();
        }

        private void ShowAllTags()
        {
            using (YellowCarrotContext context = new YellowCarrotContext())
            {
              
                var AllTags = new TagRepo(context).GetTags();

              
                foreach (var tag in AllTags)
                {
                    ComboBoxItem item = new();
                    item.Tag = tag;
                    item.Content = tag.TagNameId;
                    cbxAllTags.Items.Add(item);
                }
            }
        }
    }
}
