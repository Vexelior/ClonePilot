using ClonePilot.Core;

namespace ClonePilot.MVVM.ViewModels
{
    /// <summary>
    /// The view model for the home window
    /// </summary>
    class HomeViewModel : ObservableObject
    {
        /// <summary>
        /// The text that is displayed in the link box
        /// </summary>
        public string? LinkBoxText;

        /// <summary>
        /// The text that is displayed in the path box
        /// </summary>
        public string? PathBoxText;

        /// <summary>
        /// The text that is displayed in the output box
        /// </summary>
        public string? OutputText { get; set; }

        /// <summary>
        /// The tracked text that is displayed in the link box
        /// </summary>
        public string? TrackedLinkBoxText
        {
            get { return LinkBoxText ?? string.Empty; }
            set
            {
                LinkBoxText = value;
                OnPropertyChanged(nameof(TrackedLinkBoxText));
            }
        }

        /// <summary>
        /// The tracked text that is displayed in the path box
        /// </summary>
        public string? TrackedPathBoxText
        {
            get { return PathBoxText ?? string.Empty; }
            set
            {
                PathBoxText = value;
                OnPropertyChanged(nameof(TrackedPathBoxText));
            }
        }

        /// <summary>
        /// Property for the Master branch radio button
        /// </summary>
        public bool IsMasterBranchChecked { get; set; }

        /// <summary>
        /// Property for the Custom branch radio button
        /// </summary>
        public bool IsCustomBranchChecked { get; set; }

        /// <summary>
        /// The tracked property for the Master branch radio button
        /// </summary>
        public bool IsMasterBranch
        {
            get { return IsMasterBranchChecked; }
            set
            {
                IsMasterBranchChecked = value;
                OnPropertyChanged(nameof(IsMasterBranch));
            }
        }

        /// <summary>
        /// The tracked property for the Custom branch radio button
        /// </summary>
        public bool IsCustomBranch
        {
            get { return IsCustomBranchChecked; }
            set
            {
                IsCustomBranchChecked = value;
                OnPropertyChanged(nameof(IsCustomBranch));
            }
        }

        /// <summary>
        /// The command to trigger the clone button
        /// </summary>
        public RelayCommand? EnterKeyCommand { get; set; }

        /// <summary>
        /// The constructor for the home view model that sets the default values on the initial load
        /// </summary>
        public HomeViewModel()
        {
            IsMasterBranchChecked = true;
            PathBoxText = System.IO.Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads") ? 
                          Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads" : 
                          string.Empty;
        }
    }
}
