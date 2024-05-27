using System;
using System.Collections.Generic;

public class Budget {
    public string Name;
    public DateTime DateStart;
    public DateTime DateEnd;
    public List<Transaction> Income;
    public List<Transaction> Expenses;
}