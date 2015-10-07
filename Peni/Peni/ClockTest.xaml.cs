﻿<?xml version="1.0" encoding="utf-8" ?>
	<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
	xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
	xmlns:local="clr-namespace:XamlSamples;assembly=XamlSamples"
	x:Class="Peni.ClockTest"
	Title="Clock Page">

	<Label Text="{Binding DateTime,
	StringFormat='{0:T}'}"
	FontSize="Large"
	HorizontalOptions="Center"
	VerticalOptions="Center">
	<Label.BindingContext>
	<local:ClockViewModel />
	</Label.BindingContext>
	</Label>
	</ContentPage>
