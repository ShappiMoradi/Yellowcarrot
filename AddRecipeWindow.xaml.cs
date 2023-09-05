using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using YellowCarrot.Data;
using YellowCarrot.Models;
using YellowCarrot.Services;

namespace YellowCarrot;

/// <summary>
/// Interaction logic for AddRecipeWindow.xaml
/// </summary>
public partial class AddRecipeWindow : Window
{
    private RecipeRepository _recipeRepository = new(new());
    private Recipe _recipe = new();
    public AddRecipeWindow()
    {
        InitializeComponent();



    }
    //stänger addrecipewindow och öppnar mainwindow
    private void btnClose_Click(object sender, RoutedEventArgs e)
    {
        MainWindow mainWindow = new();
        mainWindow.Show();
        Close();
    }

    // lägg till ingridiens och mängd i listview
    private void btnAddIn_Click(object sender, RoutedEventArgs e)
    {
        string ingredient = txtIngredient.Text;
        string quantity = txtQuantity.Text;
        
        _recipe.Ingredients.Add(new Ingredient
        {
            Name = ingredient,
            Quantity = quantity,
        });

        if(string.IsNullOrEmpty(txtIngredient.Text) || string.IsNullOrEmpty(txtQuantity.Text))
        {
            MessageBox.Show("Fill in both the ingredient and its quantity before you add it to your recipe ", "Warning");
        }
        else
        {
            lvRecipeList.Items.Add($"🍴 {quantity} {ingredient}");
        }

        ClearIngridientAndQuantity();

    }

    //rensar text när man lagt till
    private void ClearIngridientAndQuantity()
    {
        txtIngredient.Clear();
        txtQuantity.Clear();
    }
    //spara alla ingridienser till ett recept
    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
        _recipe.RecipeName = txtRcpName.Text;
        _recipeRepository.CreateRecipe(_recipe);

        MainWindow mainWindow = new();
        mainWindow.Show();
        Close();

    }
    //lägg til ingridienser i receptet
    private void btnAddTag_Click(object sender, RoutedEventArgs e)
    {
        string tag = txtTag.Text;
        _recipe.Tags.Add(new Tags
        {
            Name = tag,
        });

        if (string.IsNullOrEmpty(txtTag.Text))
        {
            MessageBox.Show("Fill in tag before you add it to your recipe ", "Warning");
        }
        else
        {
            lvAddTag.Items.Add(tag);


        }

        ClearTaglv();
    }
    // rensar txttag när man lagt till
    private void ClearTaglv()
    {
        txtTag.Clear();
    }


}
