using Godot;
using System;

public partial class AddTransactionWindow : Panel
{

	[Signal]
	public delegate void AddTransactionEventHandler(string name, string date, float amount, int type, bool income);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnCancelButtonDown()
	{
		QueueFree(); // Removes this object from memory and the scene
	}

	public void OnAddButtonDown()
	{
		EmitSignal(SignalName.AddTransaction, 
			GetNode<LineEdit>("VBoxContainer/Name/LineEdit").Text,
			GetNode<LineEdit>("VBoxContainer/Date/LineEdit").Text,
			float.Parse(GetNode<LineEdit>("VBoxContainer/Amount/LineEdit").Text),
			GetNode<OptionButton>("VBoxContainer/Type/Type").Selected,
			GetNode<CheckButton>("VBoxContainer/Income/CheckButton").ButtonPressed
		);
		QueueFree();
	}
}
