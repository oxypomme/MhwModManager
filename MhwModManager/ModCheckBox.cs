﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MhwModManager
{
    public class ModCheckBox : CheckBox
    {
        public static readonly DependencyProperty ModNameProperty = DependencyProperty.Register("ModName", typeof(string), typeof(ModCheckBox));
        public static readonly DependencyProperty CategoryProperty = DependencyProperty.Register("Category", typeof(string), typeof(ModCheckBox));
        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register("IsActive", typeof(Boolean), typeof(ModCheckBox));

        public string ModName
        {
            get { return (string)GetValue(ModNameProperty); }
            set { SetValue(ModNameProperty, value); }
        }

        public string Category
        {
            get { return (string)GetValue(CategoryProperty); }
            set { SetValue(CategoryProperty, value); }
        }

        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        static ModCheckBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ModCheckBox), new FrameworkPropertyMetadata(typeof(ModCheckBox)));
        }
    }
}