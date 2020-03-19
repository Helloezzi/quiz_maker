using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ProblemEditor
{
    [Serializable]
    public class BaseItem : INotifyPropertyChanged
    {
        public string Id { get; set; }

        public ElementType Type { get; set; }

        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        private int order;
        public int Order
        {
            get
            {
                return order;
            }
            set
            {
                order = value;
                OnPropertyChanged("Order");
            }
        }

        public bool IsExpand { get; set; }

        public string Description;

        private ObservableCollection<BaseItem> children = new ObservableCollection<BaseItem>();
        public ObservableCollection<BaseItem> Children
        {
            get { return children; }
            set
            {
                children = value;
                OnPropertyChanged("Children");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    [Serializable]
    public class Chapter : BaseItem
    {
        public string SubTitle { get; set; }                
    }

    [Serializable]
    public class Quiz : BaseItem
    {
        public string Contents { get; set; }

        public string ParentID { get; set; }
    }

    [Serializable]
    public class SubjectiveType : Quiz
    {
        public string Answer;
    }

    [Serializable]
    public class MultipleChoiceType : Quiz
    {
        public int Answer;

        public List<Choice> ListChoice;
    }

    [Serializable]
    public class Choice : BaseItem
    {
        public int Index;

        public string Contents;
    }

    [Serializable]
    public class Resource : BaseItem
    {
    }
    
    [Serializable]
    public class ImageResource : Resource
    {
        public string ImagePath;
    }
}
