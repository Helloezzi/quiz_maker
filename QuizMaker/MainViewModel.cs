using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProblemEditor
{
    public class MainViewModel : BaseViewModel
    {
        public static MainViewModel Instance;

        public ObservableCollection<BaseItem> treeviewItemCollection;
        public ObservableCollection<BaseItem> TreeviewItemCollection
        {
            get
            {
                return treeviewItemCollection;
            }
            set
            {
                treeviewItemCollection = value;
                OnPropertyChanged("TreeviewItemCollection");
            }
        }

        private int switchView;
        public int SwitchView
        {
            get
            {
                return switchView;
            }
            set
            {
                switchView = value;
                OnPropertyChanged("SwitchView");
            }
        }

        public ICommand LoadFileCommand { get; }
        public ICommand SaveFileCommand { get; }
        public ICommand CreateChapterCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SelectCommand { get; }
        public ICommand SwitchViewCommand { get; }

        private BaseItem CurrentSelectItem { get; set; }

        private string testName;
        public string TestName
        {
            get
            {
                return testName;
            }
            set
            {
                testName = value;
                OnPropertyChanged("TestName");
            }
        }

        public MainViewModel()
        {
            Instance = this;

            TreeviewItemCollection = new ObservableCollection<BaseItem>();

            LoadFileCommand = new RelayCommand<object>(p => LoadFile());
            SaveFileCommand = new RelayCommand<object>(p => SaveFile());
            CreateChapterCommand = new RelayCommand<object>(p => AddChapter());
            DeleteCommand = new RelayCommand<object>(p => DeleteNode((BaseItem)p));
            SelectCommand = new RelayCommand<object>(p => SelectNode((BaseItem)p));
            SwitchViewCommand = new RelayCommand<object>(p => SetSwitchView());
        }

        private void SetSwitchView()
        {
            Console.WriteLine("111");
        }

        public void AddNode(BaseItem parent = null)
        {
            if (parent == null)
            {
                // chapter
                AddChapter();
            }
            else
            {
                AddChild(parent);
            }
        }

        private void AddChapter()
        {
            Chapter chapter = new Chapter();
            chapter.Id = Guid.NewGuid().ToString();
            chapter.Order = GetChapterCount() + 1;
            chapter.Type = ElementType.Chapter;
            chapter.Name = "Chapter";
            chapter.IsExpand = true;
            TreeviewItemCollection.Add(chapter);
        }

        private void AddChild(BaseItem parent)
        {
            if (parent is Chapter chapter)
            {
                Quiz child = new Quiz();
                child.Id = Guid.NewGuid().ToString();
                child.Order = chapter.Children.Count + 1;
                child.Name = "Problem";
                child.Type = ElementType.Problem;
                child.ParentID = parent.Id;
                chapter.Children.Add(child);
            }
        }

        private void SelectNode(BaseItem item)
        {
            CurrentSelectItem = item;

            if (item is Chapter chapter)
            {
                SwitchView = 0;
                TestName = $"{CurrentSelectItem.Name}({CurrentSelectItem.Order})";
            }
            else
            {
                SwitchView = 1;

                Quiz problem = (Quiz)item;
                BaseItem parent = FindItem(problem.ParentID);

                TestName = $"{parent.Name}({parent.Order})";
                TestName += " - ";
                TestName += $"{CurrentSelectItem.Name}({CurrentSelectItem.Order})";
            }
        }

        private void DeleteNode(BaseItem item)
        {
            foreach(BaseItem parent in TreeviewItemCollection)
            {
                if (parent == item)
                {
                    TreeviewItemCollection.Remove(item);
                    return;
                }
                else
                {
                    foreach(BaseItem child in parent.Children)
                    {
                        if (child == item)
                        {
                            parent.Children.Remove(item);
                            return;
                        }
                    }
                }
            }
            Sort();
        }

        private void LoadFile()
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();

            openFileDialog.FileName = "json file";
            openFileDialog.DefaultExt = ".json";
            openFileDialog.Filter = "json documents (.json)|*.json";

            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result == true)
            {
                string file = openFileDialog.FileName;
                Console.WriteLine(file);
                ObservableCollection<Chapter> collection = JasonManager.Import(file);
                foreach(Chapter chapter in collection)
                {
                    TreeviewItemCollection.Add(chapter);
                }
            }
        }

        private void SaveFile()
        {
            JasonManager.Export(TreeviewItemCollection);
        }
        
        private int GetChapterCount()
        {
            int count = 0;
            foreach(BaseItem item in TreeviewItemCollection)
            {
                if (item is Chapter chapter)
                {
                    count++;
                }
            }
            return count;
        }

        private BaseItem FindItem(string id)
        {
            foreach(BaseItem item in treeviewItemCollection)
            {
                if (item.Id == id)
                {
                    return item;
                }
                else
                {
                    foreach(BaseItem child in item.Children)
                    {
                        if (child.Id == id)
                        {
                            return child;
                        }
                    }
                }
            }
            return null;
        }

        private void Sort()
        {
            int count = 1;
            foreach(BaseItem item in TreeviewItemCollection)
            {
                if (item is Chapter chapter)
                {
                    chapter.Order = count++;
                }

                if (item is Quiz problem)
                {
                    int childCount = 1;
                    foreach(BaseItem child in item.Children)
                    {
                        child.Order = childCount++;
                    }
                }
            }
        }
    }
}
