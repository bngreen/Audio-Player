﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DSP;

namespace AudioPlayer
{
	/// <summary>
	/// Interaction logic for EqualizerWindow.xaml
	/// </summary>
	public partial class EqualizerWindow : Window
	{
        public AudioPlayerViewModel PlayerModel { get; private set; }
		public EqualizerWindow(AudioPlayerViewModel model)
		{
            PlayerModel = model;
			this.InitializeComponent();
		}
	}
}