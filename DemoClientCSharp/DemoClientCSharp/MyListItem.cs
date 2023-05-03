using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoClientCSharp
{
    class MyListItem
    {
        public string Text;
        public string Value;

        public MyListItem(string text, string value)
        {
            this.Text = text;
            this.Value = value;
        }


        public override string ToString()
        {
            return Text;
        }
    }
}
