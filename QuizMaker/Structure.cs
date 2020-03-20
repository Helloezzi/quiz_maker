using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker
{
    public enum QuizType
    {
        Subjective,        
        MultipleChoice_x4,
        MultipleChoice_x2,
    }

    public enum ElementType
    {
        None,
        Chapter,
        Quiz,
        Answer,
    }
}
