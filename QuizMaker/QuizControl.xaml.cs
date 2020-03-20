using System;
using System.Windows.Controls;

namespace QuizMaker
{
    /// <summary>
    /// QuizControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class QuizControl : UserControl
    {
        public QuizControl()
        {
            InitializeComponent();

            this.DataContext = MainViewModel.Instance;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            MainViewModel.Instance.QuizContentChangeCommand?.Execute(textBox.Text);
        }
    }
}
