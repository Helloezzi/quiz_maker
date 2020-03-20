using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker
{
    public class SaveFilaManager
    {
        private static SaveFilaManager instance;
        public static SaveFilaManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new SaveFilaManager();

                return instance;
            }
        }

        public bool IsNewFile { get; set; }

        public string FilePath { get; set; }

        public SaveFilaManager()
        {
            IsNewFile = true;
        }
    }
}
