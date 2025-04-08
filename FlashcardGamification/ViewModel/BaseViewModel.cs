﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace FlashcardGamification.ViewModel
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))] 
        bool isBusy;

        [ObservableProperty]
        string title;

        public bool IsNotBusy => !isBusy;

    }
}

