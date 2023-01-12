using ATMApp.Domain.Entities;
using ATMApp.Domain.Enums;
using ATMApp.Domain.Interfaces;
using ATMApp.UI;
using ConsoleTables;
using System;

namespace ATMApp
{
    public class ATMApp : IUserLogin, IUserAccountActions, ITransaction
    {
        private List<UserAccount> userAccountList;
        private UserAccount selectedAccount;
        private List<Transaction> _listOfTransactions;
        private const decimal minimumKeptAmount = 500;
        private readonly AppScreen screen;

        public ATMApp()
        {
            screen = new AppScreen();
        }
        public void CheckUserCardNumAndPassword()
        {
            bool isCorrectLogin = false;
            while(isCorrectLogin == false) 
            {
                UserAccount inputAccount = AppScreen.UserLoginForm();
                AppScreen.LoginProgress();
                foreach(UserAccount account in userAccountList)
                {
                    selectedAccount = account;
                    if(inputAccount.CardNumber.Equals(selectedAccount.CardNumber))
                    {
                        selectedAccount.TotalLogin++;
                        if(inputAccount.CardPin.Equals(selectedAccount.CardPin))
                        {
                            selectedAccount = account;
                            if(selectedAccount.IsLocked || selectedAccount.TotalLogin > 3)
                            {
                                AppScreen.PrintLockScreen();
                            }
                            else
                            {
                                selectedAccount.TotalLogin = 0;
                                isCorrectLogin = true;
                                break;
                            }
                        }
                    }
                }
                if (isCorrectLogin == false)
                {
                    Utility.PrintMessage("\nInvalid Card Number or PIN.", false);
                    selectedAccount.IsLocked = selectedAccount.TotalLogin == 3;
                    if (selectedAccount.IsLocked)
                    {
                        AppScreen.PrintLockScreen();
                    }
                }
                Console.Clear();
            }
        }
        private void ProcessMenuoption()
        {
            switch(Validator.Convert<int>("an option: "))
            {
                case (int)AppMenu.CheckBalance:
                    CheckBalance();
                    break;
                case (int)AppMenu.PlaceDeposit:
                    PlaceDeposit();
                    break;
                case (int)AppMenu.MakeWithdrawal:
                    MakeWithDrawal();
                    break;
                case (int)AppMenu.InternalTransfer:
                    var internalTransfer = screen.InternalTransferForm();
                    ProcessInternalTransfer(internalTransfer);
                    break;
                case (int)AppMenu.ViewTransaction:
                    ViewTransaction();
                    break;
                case (int)AppMenu.Logout:
                    AppScreen.LogoutProgress();
                    Utility.PrintMessage("You hav successfully logged out. Please collect your ATM card.", true);
                    Run();
                    break;
                default:
                    Utility.PrintMessage("Invalid option", false);
                    break;
            }
        }

        public void Run()
        {
            AppScreen.Welcome();
            CheckUserCardNumAndPassword();
            AppScreen.WelcomeCustomer(selectedAccount.FullName);
            while (true)
            {
                AppScreen.DisplayAppMenu();
                ProcessMenuoption();
            }
        }

        public void InitializeData()
        {
            userAccountList = new List<UserAccount>
            {

                new UserAccount{Id=1, FullName = "User1", AccountNumber=123456,CardNumber =321321, CardPin=123123,AccountBalance=50000.00m,IsLocked=false},
                new UserAccount{Id=2, FullName = "User2", AccountNumber=456789,CardNumber =654654, CardPin=456456,AccountBalance=4000.00m,IsLocked=false},
                new UserAccount{Id=3, FullName = "user3", AccountNumber=123555,CardNumber =987987, CardPin=789789,AccountBalance=2000.00m,IsLocked=false},
            };
            _listOfTransactions = new List<Transaction>();
        }

        public void CheckBalance()
        {
            Utility.PrintMessage($"Your Account Balance is {Utility.FormatAmount(selectedAccount.AccountBalance)}");
        }

        public void PlaceDeposit()
        {
            Console.WriteLine("\nOnly multiples of 500 and 1000 are allowed.\n");
            var transaction_amt = Validator.Convert<int>($"amount {AppScreen.Cur}");

            Console.WriteLine("\nChecking and Counting Bank Notes.");
            Utility.PrintDotAnimation();
            Console.WriteLine("");
            if(transaction_amt <= 0)
            {
                Utility.PrintMessage("Amount needs to be greater than zero.",false); 
                return;
            }
            if(transaction_amt % 500 != 0)
            {
                Utility.PrintMessage("Enter amount in multiples of 500 or 1000. Try Again.", false); 
                return;
            }
            if(PreviewBankNotesCount(transaction_amt) == false)
            {
                Utility.PrintMessage("You have cancelled you action.", false);
                return;
            }
            InsertTransaction(selectedAccount.Id, TransactionType.Deposit, transaction_amt, "");

            selectedAccount.AccountBalance += transaction_amt;

            Utility.PrintMessage($"Your deposit of {Utility.FormatAmount(transaction_amt)} was succesful.", true);
        }

        public void MakeWithDrawal()
        {
            var transaction_amt = 0;
            int selectedAmount = AppScreen.SelectAmount();
            if (selectedAmount == -1)
            {
                MakeWithDrawal();
                return;
            }
            else if (selectedAmount != 0)
            {
                transaction_amt = selectedAmount;
            }
            else
            {
                transaction_amt = Validator.Convert<int>($"amount {AppScreen.Cur}");
            }
            if (transaction_amt <= 0)
            {
                Utility.PrintMessage("Amount needs to be greater than zero. Try agin", false);
                return;
            }
            if (transaction_amt % 500 != 0)
            {
                Utility.PrintMessage("You can only withdraw amount in multiples of 500 or 1000 Rupees. Try again.", false);
                return;
            }

            if (transaction_amt > selectedAccount.AccountBalance)
            {
                Utility.PrintMessage($"Withdrawal failed. Your balance is too low to withdraw" +
                    $"{Utility.FormatAmount(transaction_amt)}", false);
                return;
            }
            if ((selectedAccount.AccountBalance - transaction_amt) < minimumKeptAmount)
            {
                Utility.PrintMessage($"Withdrawal failed. Your account needs to have " +
                    $"minimum {Utility.FormatAmount(minimumKeptAmount)}", false);
                return;
            }
           
            InsertTransaction(selectedAccount.Id, TransactionType.Withdrawal, -transaction_amt, "");
           
            selectedAccount.AccountBalance -= transaction_amt;
            
            Utility.PrintMessage($"You have successfully withdrawn " +
                $"{Utility.FormatAmount(transaction_amt)}.", true);

        }

        private bool PreviewBankNotesCount(int amount)
        {
            int thousandNotesCount = amount / 1000;
            int fiveHundredNotesCount = (amount%1000) / 500;
            Console.WriteLine("\nSummary");
            Console.WriteLine("------");
            Console.WriteLine($"{AppScreen.Cur}1000 x {thousandNotesCount} = {1000 * thousandNotesCount}");
            Console.WriteLine($"{AppScreen.Cur}500 x {fiveHundredNotesCount} = {500 * fiveHundredNotesCount}");
            Console.WriteLine($"Total Amount: {Utility.FormatAmount(amount)}\n\n");

            int opt = Validator.Convert<int>("1 to confirm");
            return opt.Equals(1);

        }

        public void InsertTransaction(long _UserBankAccountId, TransactionType _tranType, decimal _tranAmount, string _desc)
        {
            var transaction = new Transaction()
            {
                TransactionId = Utility.GetTransactionId(),
                UserBankAccountId = _UserBankAccountId,
                TransactionDate = DateTime.Now,
                TransactionType = _tranType,
                TransactionAmount = _tranAmount,
                Description = _desc
            };
            _listOfTransactions.Add(transaction);
        }

        public void ViewTransaction()
        {
            var filteredTransactionList = _listOfTransactions.Where(t => t.UserBankAccountId == selectedAccount.Id).ToList();
            //check if there's a transaction
            if (filteredTransactionList.Count <= 0)
            {
                Utility.PrintMessage("You have no transaction yet.", true);
            }
            else
            {
                var table = new ConsoleTable("Id", "Transaction Date", "Type", "Descriptions", "Amount " + AppScreen.Cur);
                foreach (var tran in filteredTransactionList)
                {
                    table.AddRow(tran.TransactionId, tran.TransactionDate, tran.TransactionType, tran.Description, tran.TransactionAmount);
                }
                table.Options.EnableCount = false;
                table.Write();
                Utility.PrintMessage($"You have {filteredTransactionList.Count} transaction(s)", true);
            }
        }

        private void ProcessInternalTransfer(InternalTransfer internalTransfer)
        {
            if (internalTransfer.TransferAmount <= 0)
            {
                Utility.PrintMessage("Amount needs to be more than zero. Try again.", false);
                return;
            }
            if (internalTransfer.TransferAmount > selectedAccount.AccountBalance)
            {
                Utility.PrintMessage($"Transfer failed. You do not hav enough balance" +
                    $" to transfer {Utility.FormatAmount(internalTransfer.TransferAmount)}", false);
                return;
            }
             
            if ((selectedAccount.AccountBalance - internalTransfer.TransferAmount) < minimumKeptAmount)
            {
                Utility.PrintMessage($"Transfer failed. Your account needs to have minimum" +
                    $" {Utility.FormatAmount(minimumKeptAmount)}", false);
                return;
            }

        
            var selectedBankAccountReceiver = (from userAcc in userAccountList
                                               where userAcc.AccountNumber == internalTransfer.RecipientBankAccountNumber
                                               select userAcc).FirstOrDefault();
            if (selectedBankAccountReceiver == null)
            {
                Utility.PrintMessage("Transfer failed. Receiver bank account number is invalid.", false);
                return;
            }
            //check receiver's name
            if (selectedBankAccountReceiver.FullName != internalTransfer.RecipientBankAccountName)
            {
                Utility.PrintMessage("Transfer Failed. Recipient's bank account name does not match.", false);
                return;
            }

            //add transaction to transactions record- sender
            InsertTransaction(selectedAccount.Id, TransactionType.Transfer, -internalTransfer.TransferAmount, "Transfered " +
                $"to {selectedBankAccountReceiver.AccountNumber} ({selectedBankAccountReceiver.FullName})");
            //update sender's account balance
            selectedAccount.AccountBalance -= internalTransfer.TransferAmount;

            //add transaction record-reciever
            InsertTransaction(selectedBankAccountReceiver.Id, TransactionType.Transfer, internalTransfer.TransferAmount, "Transfered from " +
                $"{selectedAccount.AccountNumber}({selectedAccount.FullName})");
            //update reciever's account balance
            selectedBankAccountReceiver.AccountBalance += internalTransfer.TransferAmount;
            //print success message
            Utility.PrintMessage($"You have successfully transfered" +
                $" {Utility.FormatAmount(internalTransfer.TransferAmount)} to " +
                $"{internalTransfer.RecipientBankAccountName}", true);

        }
    }
}