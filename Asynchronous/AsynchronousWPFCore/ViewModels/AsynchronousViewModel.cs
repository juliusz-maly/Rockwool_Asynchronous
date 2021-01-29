using AsynchronousWPFCore.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace AsynchronousWPFCore.ViewModels
{
    public class AsynchronousViewModel : ViewModelBase
    {
        private const int WaitingTime = 3000;
        private Action Example3AsCancelAction;
        
        private bool _isProgressBarRunning = false;
        public bool IsProgressBarRunning
        {
            get => _isProgressBarRunning;
            set
            {
                _isProgressBarRunning = value;
                OnPropertyChanged();
            }
        }

        private string _labelContent = "Press the button";
        public string LabelContent
        {
            get => _labelContent;
            set
            {
                _labelContent = value;
                OnPropertyChanged();
            }
        }

        private ICollectionView _items = new ListCollectionView(new List<int>());
        public ICollectionView Items
        {
            get => _items;
            set
            {
                _items = value;
                OnPropertyChanged();
            }
        }

        

        public ICommand Example1Command { get; set; }
        public ICommand Example2Command { get; set; }
        public ICommand Example1AsCommand { get; set; }
        public ICommand Example2AsCommand { get; set; }
        public ICommand Example3AsCommand { get; set; }
        public ICommand Example3AsCancelCommand { get; set; }
        public ICommand Example4AsCommand { get; set; }
        public ICommand Example5AsCommand { get; set; }
        public ICommand ClearLabelCommand { get; set; }

        public AsynchronousViewModel()
        {
            Example1Command = new RelayCommand(Example_NonAsync);
            Example2Command = new RelayCommand(Example_NonAsync_ThrowingException);
            Example1AsCommand = new RelayCommand(Example_Async);
            Example2AsCommand = new RelayCommand(Example_ThrowingException);
            Example3AsCommand = new RelayCommand(Example_Cancellation);
            Example4AsCommand = new RelayCommand(Example_Deadlock);
            Example5AsCommand = new RelayCommand(Example_ConfigureAwait);

            Example3AsCancelCommand = new RelayCommand(() =>
            {
                Example3AsCancelAction?.Invoke();
            });
            ClearLabelCommand = new RelayCommand(() => LabelContent = "Press the button");
        }


        /////////////////////// Basic synchronous and asynchronous operations ///////////////////////

        private void Example_NonAsync()
        {
            IsProgressBarRunning = true;

            LabelContent = WorkingHttpGetOperation();

            IsProgressBarRunning = false;
        }

        private async void Example_Async()
        {
            IsProgressBarRunning = true;

            LabelContent = await WorkingHttpGetOperationAsync();

            IsProgressBarRunning = false;
        }

        private string WorkingHttpGetOperation()
        {
            Task.Delay(WaitingTime).Wait();
            IOOperation.GetResponse("http://www.onet.pl");
            return "Operation completed";
        }

        private async Task<string> WorkingHttpGetOperationAsync()
        {
            await Task.Delay(WaitingTime);
            await IOOperation.GetResponseAsync("http://www.onet.pl");
            return "Operation completed";
        }


        /////////////////////// Faulty synchronous and asynchronous operations ///////////////////////

        private void Example_NonAsync_ThrowingException()
        {
            IsProgressBarRunning = true;

            try
            {
                FailingHttpGetOperation();
                LabelContent = "Operation completed";
            }
            catch (Exception e)
            {
                LabelContent = "Error: " + e.Message;
            }

            IsProgressBarRunning = false;
        }

        private void FailingHttpGetOperation()
        {
            try
            {
                IOOperation.GetResponse("http://www.not-existing-link.abcd");
            }
            catch
            {
                throw;
            }
        }


        private void Example_ThrowingException()
        {
            IsProgressBarRunning = true;

            try
            {
                FailingHttpGetOperationAsync();
                LabelContent = "Operation completed";
            }
            catch (Exception e)
            {
                LabelContent = "Error: " + e.Message;
            }

            IsProgressBarRunning = false;
        }

        private async void FailingHttpGetOperationAsync() // async void alert!
        {
            try
            {
                await IOOperation.GetResponseAsync("http://www.not-existing-link.abcd");
            }
            catch
            {
                throw; // this is causing application shutdown even when calling method is in try/catch
            }
        }


        /////////////////////// Cancellation of asynchronous operations ///////////////////////

        private async void Example_Cancellation()
        {
            var cancelTokenSrc = new CancellationTokenSource();
            var task = RunOperationAsync(cancelTokenSrc.Token).ContinueWith(delegate { }, TaskScheduler.Current);
            //var task = Task.Run(() => RunOperationAsync(cancelTokenSrc.Token)); // run async task in another thread
            //var task = Task.Factory.StartNew(() => RunOperationAsync(cancelTokenSrc.Token)); //Task<Task>

            Example3AsCancelAction = () => {
                cancelTokenSrc.Cancel();
            };

            try
            {
                LabelContent = "Task started";
                await task;
                LabelContent = "Task completed";
            }
            catch (OperationCanceledException)
            {
                LabelContent = "Task cancelled";
            }
            catch (Exception e) // not AggregateException as in task.Wait()
            {
                LabelContent = "Error: " + e.Message;
            }
            finally
            {
                IsProgressBarRunning = false;
            }
        }

        private async Task RunOperationAsync(CancellationToken ct)
        {
            IsProgressBarRunning = true;
            for (int i = 0; i < 10; i++)
            {
                //if (!ct.IsCancellationRequested) // when we don't want to throw an exception

                await Task.Delay(WaitingTime, ct);
                //ct.ThrowIfCancellationRequested(); // not necessary if cancellatonToken was passed into awaited task as above            
                
                // testing behavior on real exception throwing
                //throw new Exception("test exception");
            }
        }


        /////////////////////// Deadlock in asynchronous operations ///////////////////////

        private void Example_Deadlock()
        {
            LabelContent = "Task started";
            IsProgressBarRunning = true;

            RunOperationWithDeadlockAsync().Wait();

            #region hidden commented out code
            //no deadlock -thread changed!
            //Task.Run(() =>
            //{
            //    RunOperationWithDeadlockAsync().Wait();
            //}).Wait();
            #endregion
            
            IsProgressBarRunning = false;
            LabelContent = "Task completed";
        }

        private async Task RunOperationWithDeadlockAsync()
        {
            await Task.Delay(WaitingTime);
        }


        /////////////////////// ConfigureAwait(false) in asynchronous operations ///////////////////////

        private async void Example_ConfigureAwait()
        {
            var sc1 = SynchronizationContext.Current;

            IsProgressBarRunning = true;
            await Method1Async();
            IsProgressBarRunning = false;

            var sc2 = SynchronizationContext.Current;
        }

        private async Task Method1Async()
        {
            var sc1 = SynchronizationContext.Current;
            Items.Refresh(); // operation on UI - needs UI context

            await Method2Async().ConfigureAwait(false);

            var sc2 = SynchronizationContext.Current;
            Items.Refresh(); // operation on UI - needs UI context

            #region hidden commented out code
            sc1.Post(x =>
            {
                Items.Refresh();
                MessageBox.Show("Items refreshed from UI thread!");
            }, null);
            #endregion
        }

        private async Task Method2Async()
        {
            var sc1 = SynchronizationContext.Current;

            await Task.Delay(WaitingTime);

            var sc2 = SynchronizationContext.Current;
            Items.Refresh(); // operation on UI - needs UI context
        }
    }
}
