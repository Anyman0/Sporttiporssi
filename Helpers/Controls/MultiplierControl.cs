using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sporttiporssi.Helpers.Controls
{
    public class MultiplierControl : StackLayout
    {
        private Label _valueLabel;
        private Button _incrementButton;
        private Button _decrementButton;
        private double _value;

        public bool IsInteger { get; set; } = true;
        public bool IsBigInteger { get; set; } = false;
        public double Value
        {
            get => _value;
            set
            {
                _value = value;
                if(IsBigInteger)
                {
                    _valueLabel.Text = ((long)value).ToString();
                }
                else
                {
                    _valueLabel.Text = IsInteger ? ((int)_value).ToString() : _value.ToString("F1");
                }               
            }
        }

        public MultiplierControl()
        {
            Orientation = StackOrientation.Horizontal;
            _decrementButton = new Button { Text = "-" };
            _decrementButton.Clicked += OnDecrementClicked;
            _valueLabel = new Label { VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.CenterAndExpand };
            _incrementButton = new Button { Text = "+" };
            _incrementButton.Clicked += OnIncrementClicked;

            Children.Add(_decrementButton);
            Children.Add(_valueLabel);
            Children.Add(_incrementButton);

            Value = 1; // Default initial value
        }

        private void OnIncrementClicked(object sender, EventArgs e)
        {
            if (IsInteger)
            {
                Value += 1;
            }
            else if(IsBigInteger)
            {
                Value += 100000;
            }
            else
            {
                Value += 0.1;
            }
        }

        private void OnDecrementClicked(object sender, EventArgs e)
        {
            if (IsInteger)
            {               
                Value -= 1;
            }
            else if(IsBigInteger)
            {
                if(Value > 2000000) // Ensuring that we stick to minimum team value
                Value -= 100000;
            }
            else
            {              
                Value -= 0.1;
            }
        }
    }
}
