using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace QuizMaker
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

        public string Description;        

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    [Serializable]
    public class TreeNode : BaseItem
    {
        public bool IsExpand { get; set; }

        private ObservableCollection<BaseItem> children = new ObservableCollection<BaseItem>();
        [JsonIgnore]
        public ObservableCollection<BaseItem> Children
        {
            get { return children; }
            set
            {
                children = value;
                OnPropertyChanged("Children");
            }
        }
    }

    [Serializable]
    public class Chapter : TreeNode
    {
        public string SubTitle { get; set; }                
    }

    [Serializable]
    public class Quiz : TreeNode
    {
        public Quiz()
        {
            ListAnswer = new List<Answer>();
        }

        public string Content { get; set; }

        public string ParentID { get; set; }

        public QuizType QuizType { get; set; }

        [JsonIgnore]
        public List<Answer> ListAnswer { get; set; }
    }

    [Serializable]
    public class Answer : BaseItem
    {
        public string ParentID;

        private string content;
        public string Content
        {
            get
            {
                return content;
            }
            set
            {
                content = value;
                OnPropertyChanged("Content");
            }
        }

        private bool isCorrect;
        public bool IsCorrect
        {
            get
            {
                return isCorrect;
            }
            set
            {
                isCorrect = value;
                OnPropertyChanged("IsCorrect");
            }
        }       

        public Uri ImageUri;
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
