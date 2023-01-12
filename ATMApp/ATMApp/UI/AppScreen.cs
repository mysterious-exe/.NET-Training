using ATMApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace ATMApp.UI
{
    public class AppScreen
    {
        internal const string Cur = "Rs.  ";
        internal static void Welcome()
        {
            Console.Clear();
            Console.Title = "ATM Appplication";
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n\n--------------------Welcome to my ATM Application--------------------\n");
            Console.WriteLine("Please insert your ATM card");
            Console.WriteLine("");
            Utility.PressEnterToContinue();
        }
        internal static UserAccount UserLoginForm()
        {
            UserAccount tempUserAccount = new UserAccount();
            tempUserAccount.CardNumber = Validator.Convert<long>("Your Card Number: ");
            tempUserAccount.CardPin = Convert.ToInt32(Utility.GetSecretInput("Enter Your Card PIN: "));
            return tempUserAccount;
        }

        internal static void LoginProgress()
        {
            Console.WriteLine("\nChecking Card Number and PIN ...");
            Utility.PrintDotAnimation();
        }
        internal static void PrintLockScreen()
        {
            Console.Clear();
            Utility.PrintMessage("Your account is LOCKED. Please go to the nearest branch to unlock your account. Thank You.", true);
            Utility.PressEnterToContinue();
            Environment.Exit(1);
        }
        internal static void WelcomeCustomer(string fullName)
        {
            Console.WriteLine($"Welcome back, {fullName}");
            Utility.PressEnterToContinue();
        }

        internal static void DisplayAppMenu()
        {
            Console.Clear();
            Console.WriteLine("-------My ATM App Menu-------");
            Console.WriteLine(":                           :");
            Console.WriteLine("1.Account Balance           :");
            Console.WriteLine("2.Cash Deposit              :");
            Console.WriteLine("3.Withdrawal                :");
            Console.WriteLine("4.Transfer                  :");
            Console.WriteLine("5.Transactions              :");
            Console.WriteLine("6.Logout                    :");

        }

        internal static void LogoutProgress()
        {
            Console.WriteLine("Thank you or using the ATM Application.");
            Utility.PrintDotAnimation();
            Console.Clear();
        }


        internal static int SelectAmount()
        {
            Console.WriteLine("");
            Console.WriteLine(":1.{0}500      5.{0}10,000", Cur);
            Console.WriteLine(":2.{0}1000     6.{0}15,000", Cur);
            Console.WriteLine(":3.{0}2000     7.{0}20,000", Cur);
            Console.WriteLine(":4.{0}5000     8.{0}40,000", Cur);
            Console.WriteLine(":0.Other");
            Console.WriteLine("");

            int selectedAmount = Validator.Convert<int>("option:");
            switch (selectedAmount)
            {
                case 1:
                    return 500;
                    break;
                case 2:
                    return 1000;
                    break;
                case 3:
                    return 2000;
                    break;
                case 4:
                    return 5000;
                    break;
                case 5:
                    return 10000;
                    break;
                case 6:
                    return 15000;
                    break;
                case 7:
                    return 20000;
                    break;
                case 8:
                    return 40000;
                    break;
                case 0:
                    return 0;
                    break;
                default:
                    Utility.PrintMessage("Invalid input. Try again.", false);
                    return -1;
                    break;
            }
        }
        internal InternalTransfer InternalTransferForm()
        {
            var internalTransfer = new InternalTransfer();
            internalTransfer.RecipientBankAccountNumber = Validator.Convert<long>("recipient's account number:");
            internalTransfer.TransferAmount = Validator.Convert<decimal>($"amount {Cur}");
            internalTransfer.RecipientBankAccountName = Utility.GetUserInput("recipient's name:");
            return internalTransfer;
        }
    }
}
