using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Quest2_VRC
{
    class TextBoxWriter : TextWriter
    {
        TextBox _textBox;
        public TextBoxWriter(TextBox textBox)
        {
            if (null == textBox)
                throw new ArgumentNullException(nameof(textBox));
            _textBox = textBox;
        }
        public override Encoding Encoding { get { return Encoding.Default; } }
        public override void Write(char value)
        {
            try
            {
                _textBox.AppendText(value.ToString());
            }
            catch (ObjectDisposedException) { }
        }
        public override void Write(string value)
        {
            try
            {
                _textBox.AppendText(value);
            }
            catch (ObjectDisposedException) { }
        }
        public override void Write(char[] buffer, int index, int count)
        {
            try
            {
                _textBox.AppendText(new string(buffer, index, count));
            }
            catch (ObjectDisposedException) { }
        }
        public override void Write(char[] buffer)
        {
            try
            {
                _textBox.AppendText(new string(buffer));
            }
            catch (ObjectDisposedException) { }
        }
    }
}