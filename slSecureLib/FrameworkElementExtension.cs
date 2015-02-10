using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace slSecureLib
{
    public static class FrameworkElementExtension
    {
        public static void SetValidation(this FrameworkElement frameworkElement, string message)
        {
            CustomValidation customValidation = new CustomValidation(message);

            Binding binding = new Binding("ValidationError")
            {
                Mode = System.Windows.Data.BindingMode.TwoWay,
                NotifyOnValidationError = true,
                ValidatesOnExceptions = true,
                Source = customValidation
            };
            frameworkElement.SetBinding(Control.TagProperty, binding);
        }

        public static void RaiseValidationError(this FrameworkElement frameworkElement)
        {
            BindingExpression bindingExpression = frameworkElement.GetBindingExpression(Control.TagProperty);

            if (bindingExpression != null)
            {
                ((CustomValidation)bindingExpression.DataItem).ShowErrorMessage = true;
                bindingExpression.UpdateSource();
            }
        }

        public static void ClearValidationError(this FrameworkElement frameworkElement)
        {
            BindingExpression bindingExpression = frameworkElement.GetBindingExpression(Control.TagProperty);

            if (bindingExpression != null)
            {
                ((CustomValidation)bindingExpression.DataItem).ShowErrorMessage = false;
                bindingExpression.UpdateSource();
            }
        }

        public static bool IsPlainTextValid(this string inputText)
        {
            bool isTextValid = true;

            foreach (char character in inputText)
            {
                if (char.IsWhiteSpace(character) != true)
                {
                    if (char.IsLetterOrDigit(character) != true)
                    {
                        if (CharUnicodeInfo.GetUnicodeCategory(character) != UnicodeCategory.NonSpacingMark)
                        {
                            isTextValid = false;
                            break;
                        }
                    }
                }
            }
            return isTextValid;
        }

        public static bool IsNumberValid(this string inputNumber)
        {
            bool isNumberValid = true;

            uint number = 0;

            if (UInt32.TryParse(inputNumber, out number) != true)
            {
                isNumberValid = false;
            }

            return isNumberValid;
        }

        public static bool IsEmailValid(this string inputEmail)
        {
            bool isEmailValid = true;

            string emailExpression = @"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$";

            Regex regex = new Regex(emailExpression);

            if (regex.IsMatch(inputEmail) != true)
            {
                isEmailValid = false;
            }

            return isEmailValid;
        }

    }
}
