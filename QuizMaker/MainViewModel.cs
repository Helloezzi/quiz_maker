using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace QuizMaker
{
    public class MainViewModel : BaseViewModel
    {
        public static MainViewModel Instance;

        public ObservableCollection<TreeNode> treeviewItemCollection;
        public ObservableCollection<TreeNode> TreeviewItemCollection
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
        public ICommand CreateQuizCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SelectCommand { get; }
        public ICommand SwitchViewCommand { get; }
        public ICommand NewFileCommand { get; }
        public ICommand QuizContentChangeCommand { get; }
        public ICommand AnswerSelectedCommand { get; }

        private string currentContent;
        public string CurrentContent
        {
            get { return currentContent; }
            set
            {
                currentContent = value;                
                if (CurrentSelectItem is Quiz quiz)
                {
                    quiz.Content = value;
                }
                OnPropertyChanged("CurrentContent");
            }
        }

        private QuizType currentQuizType;
        public QuizType CurrentQuizType
        {
            get
            {
                return currentQuizType;
            }
            set
            {
                currentQuizType = value;
                if (CurrentSelectItem is Quiz quiz)
                {
                    quiz.QuizType = value;
                }
                OnPropertyChanged("CurrentQuizType");
            }
        }

        private TreeNode CurrentSelectItem { get; set; }

        private string selectTitle;
        public string SelectTitle
        {
            get
            {
                return selectTitle;
            }
            set
            {
                selectTitle = value;
                OnPropertyChanged("SelectTitle");
            }
        }

        private List<Answer> listCurrentAnswer;
        public List<Answer> ListCurrentAnswer
        {
            get
            {
                return listCurrentAnswer;
            }
            set
            {
                listCurrentAnswer = value;
                OnPropertyChanged("ListCurrentAnswer");
            }
        }

        public MainViewModel()
        {
            Instance = this;            

            TreeviewItemCollection = new ObservableCollection<TreeNode>();
            ListCurrentAnswer = new List<Answer>();

            LoadFileCommand = new RelayCommand<object>(p => LoadFile());
            SaveFileCommand = new RelayCommand<object>(p => SaveFile());
            CreateChapterCommand = new RelayCommand<object>(p => AddChapter());
            CreateQuizCommand = new RelayCommand<object>(p => AddChild());
            DeleteCommand = new RelayCommand<object>(p => DeleteNode((TreeNode)p));
            SelectCommand = new RelayCommand<object>(p => SelectNode((TreeNode)p));
            SwitchViewCommand = new RelayCommand<object>(p => SetSwitchView());
            NewFileCommand = new RelayCommand<object>(p => NewFile());
            QuizContentChangeCommand = new RelayCommand<object>(p => QuizContentChange((string)p));
            AnswerSelectedCommand = new RelayCommand<object>(p => AnswerSelect((object)p));
        }
        
        private void AnswerSelect(object sender)
        {
            Console.WriteLine("111");
        }

        private void QuizContentChange(string content)
        {
            if (CurrentSelectItem is Quiz quiz)
            {
                quiz.Content = content;                
            }
        }

        private void NewFile()
        {
            //Window parent = Window.GetWindow(cr) as Window;
            //MessageBoxResult result = MessageBox.Show(parent, cr.Message, cr.Caption, cr.MsgBoxButton, cr.MsgBoxImage);
            Window owner = Application.Current.MainWindow;
            if (MessageBox.Show(owner, "새파일 생성", "정말 지울거임?", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                SwitchView = (int)ElementType.None;
                SelectTitle = "";
                CurrentSelectItem = null;
                CurrentContent = "";
                TreeviewItemCollection.Clear();

                SaveFilaManager.Instance.FilePath = "";
                SaveFilaManager.Instance.IsNewFile = true;
            }
        }

        private void SetSwitchView()
        {
            Console.WriteLine("change view");
        }

        public void AddChild()
        {
            AddChild(CurrentSelectItem);
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
                child.Name = "Quiz";
                child.Type = ElementType.Quiz;
                child.QuizType = QuizType.MultipleChoice_x4;
                child.ParentID = parent.Id;

                for (int i = 0; i < 4; i++)
                {
                    Answer asw = new Answer();
                    asw.ParentID = child.Id;
                    asw.Id = Guid.NewGuid().ToString();
                    asw.IsCorrect = false;
                    asw.Order = i + 1;
                    asw.Type = ElementType.Answer;
                    child.ListAnswer.Add(asw);
                }
                chapter.Children.Add(child);
            }
        }

        private void SelectNode(TreeNode item)
        {
            CurrentSelectItem = item;

            if (item is Chapter chapter)
            {
                // click chapter
                SwitchView = (int)ElementType.Chapter;
                SelectTitle = $"{CurrentSelectItem.Name}({CurrentSelectItem.Order})";
            }
            else if (item is Quiz quiz)
            {
                // click quiz
                SwitchView = (int)ElementType.Quiz;
                BaseItem parent = FindItem(quiz.ParentID);
                SelectTitle = $"{parent.Name}({parent.Order})";
                SelectTitle += " - ";
                SelectTitle += $"{CurrentSelectItem.Name}({CurrentSelectItem.Order})";
                CurrentQuizType = quiz.QuizType;
                CurrentContent = quiz.Content;
                ListCurrentAnswer = quiz.ListAnswer;
            }
            else
            {
                SwitchView = (int)ElementType.None;
            }
        }

        private void DeleteNode(TreeNode item)
        {
            foreach(TreeNode parent in TreeviewItemCollection)
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
                ObservableCollection<TreeNode> collection = JasonManager.Import(file);
                TreeviewItemCollection = collection;

                SaveFilaManager.Instance.FilePath = file;
                SaveFilaManager.Instance.IsNewFile = false;
            }
        }

        private void SaveFile()
        {
            if (SaveFilaManager.Instance.IsNewFile == false)
            {
                //Console.WriteLine(CurrentContent);                

                JasonManager.Export(SaveFilaManager.Instance.FilePath, treeviewItemCollection);
                return;
            }

            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            //dlg.FileName = "json file";
            dlg.DefaultExt = ".json";
            dlg.Filter = "json documents (.json)|*.json";

            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                JasonManager.Export(dlg.FileName, TreeviewItemCollection);

                SaveFilaManager.Instance.FilePath = dlg.FileName;
                SaveFilaManager.Instance.IsNewFile = false;
            }               
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
            foreach(TreeNode item in treeviewItemCollection)
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
            foreach(TreeNode item in TreeviewItemCollection)
            {
                if (item is Chapter chapter)
                {
                    chapter.Order = count++;
                }

                if (item is Quiz quiz)
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
