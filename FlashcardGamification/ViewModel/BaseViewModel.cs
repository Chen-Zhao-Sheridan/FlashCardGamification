using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace FlashcardGamification.ViewModel
{
    /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/BaseViewModel"/>
    public partial class BaseViewModel : ObservableObject
    {
        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/BaseViewModel_IsBusy"/>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))] 
        bool isBusy;

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/BaseViewModel_Title"/>
        [ObservableProperty]
        string title;

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/BaseViewModel_IsNotBusy"/>
        public bool IsNotBusy => !IsBusy;

    }
}

