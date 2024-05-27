using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public partial class AppManager : Control
{

    public static Budget CurrentBudget;

    [Export]
    public PackedScene TransactionList;

    [Export]
    public PackedScene AddTransactionMenu;

    VBoxContainer container;
    float totalIncome = 0;
    float totalExpense = 0;
    Dictionary<string, float> Category;

    public override void _Ready()
    {
        CurrentBudget = new Budget() {
            Name = "Current Budget",
            Expenses = new List<Transaction>(),
            Income = new List<Transaction>()
        };

        container = GetNode<VBoxContainer>("TransactionList/ScrollContainer/TransactionList");
    }

    public override void _Process(double delta)
    {
        // GD.Print(CurrentBudget.Name);
    }

    public void OnAddTransactionButtonDown()
    {
        AddTransactionWindow transactionMenu = AddTransactionMenu.Instantiate<AddTransactionWindow>();
        AddChild(transactionMenu);
        transactionMenu.AddTransaction += AddTransactionToTransactions;
    }

    private void AddTransactionToTransactions(string name, string date, float amount, int type, bool income)
    {
        name = name.Replace("POS Withdrawal - ", "");
        Transaction transaction = new Transaction(){
                Name = name,
                Date = DateTime.Parse(date).Date,
                Amount = amount,
                Type = (TransactionType) type
        };

        if(income)
        {
            CurrentBudget.Income.Add(transaction);
            totalIncome += transaction.Amount;
            GetNode<RichTextLabel>("Total/Income/IncomeAmount").Text = "[center][b]" + totalIncome.ToString();
        }
        else 
        {
            CurrentBudget.Expenses.Add(transaction);
            totalExpense += transaction.Amount;
            GetNode<RichTextLabel>("Total/Expense/ExpenseAmount").Text = "[center][b]" + totalExpense.ToString();
        }

        AddTransactionToList(transaction);

        // Update PieChart
        GetNode<TextureProgressBar>("Total/TextureProgressBar").MaxValue = totalIncome;
        GetNode<TextureProgressBar>("Total/TextureProgressBar").Value = totalExpense;

    }

    private void AddTransactionToList(Transaction transaction)
    {
            Panel row = TransactionList.Instantiate<Panel>();
            row.GetNode<RichTextLabel>("TransactionRow/Date").Text = transaction.Date.ToString("d");
            row.GetNode<RichTextLabel>("TransactionRow/Name").Text = transaction.Name;
            row.GetNode<RichTextLabel>("TransactionRow/Amount").Text = transaction.Amount.ToString();
            row.GetNode<OptionButton>("TransactionRow/Type").Selected = (int)transaction.Type;
            container.AddChild(row);
    }

    private void LoadCSV(string path)
    {
        List<string[]> parsedDate = new List<string[]>();

        using(StreamReader reader = new StreamReader(path))
        {
            while(!reader.EndOfStream) 
            {
                string line = reader.ReadLine();
                string[] fields = line.Split(',');
                parsedDate.Add(fields);
            }
        }

        foreach(var tableRow in parsedDate)
        {
            if(tableRow[0].Equals("Account Number")){
                continue;
            }

            AddTransactionToTransactions(tableRow[3], tableRow[1], 
                    tableRow[4] != "" ? float.Parse(tableRow[4]) : float.Parse(tableRow[5]), 
                    5, tableRow[4] == "");
        }
    }

    private void OnFileDialogFileSelected(string path)
    {
        LoadCSV(path);
        GetNode<FileDialog>("FileDialog").Visible = false;
    }

    private void OnButtonButtonDown()
    {
        GetNode<FileDialog>("FileDialog").Visible = true;
    }
}
