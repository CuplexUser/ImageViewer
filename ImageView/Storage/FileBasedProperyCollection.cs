using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Serilog;

namespace ImageViewer.Storage
{
    /// <summary>
    /// Thread Safe read and write to a file based property collection
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public abstract class FileBasedPropertyCollection : IDisposable
    {
        //In Minutes
        /// <summary>
        /// The save to file delay
        /// </summary>
        private const int SaveToFileDelay = 1;
        /// <summary>
        /// The automatic reset event
        /// </summary>
        private readonly AutoResetEvent _autoResetEvent;
        /// <summary>
        /// The background worker
        /// </summary>
        private readonly BackgroundWorker _backgroundWorker;
        /// <summary>
        /// The file operation in progress
        /// </summary>
        private bool _fileOperationInProgress;
        
        /// <summary>
        /// The last database save
        /// </summary>
        private DateTime _lastDatabaseSave;

        private readonly Timer _updateTimer;

        
        private bool _isDirty;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileBasedPropertyCollection"/> class.
        /// </summary>
        protected FileBasedPropertyCollection()
        {
            //var property = 1;
            _lastDatabaseSave = DateTime.Now;
            _backgroundWorker = new BackgroundWorker();
            _autoResetEvent = new AutoResetEvent(true);
            _backgroundWorker.DoWork += BackgroundWorker_DoWork;
            _updateTimer = new Timer(TimerCallback, null, -1, SaveToFileDelay * 60 * 1000);
            _backgroundWorker.WorkerSupportsCancellation = true;
        }

        private void TimerCallback(object state)
        {
            Thread.CurrentThread.Join();
            if (!_backgroundWorker.IsBusy)
            {
                _backgroundWorker.RunWorkerAsync();
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether [file operation in progress].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [file operation in progress]; otherwise, <c>false</c>.
        /// </value>
        private bool FileOperationInProgress
        {
            get => _fileOperationInProgress;
            set
            {
                _fileOperationInProgress = value;
                if (_fileOperationInProgress)
                    _autoResetEvent.Reset();
                else
                    _autoResetEvent.Set();
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is dirty.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is dirty; otherwise, <c>false</c>.
        /// </value>
        protected virtual bool IsDirty
        {
            get => _isDirty;
            private set
            {
                _isDirty = value;
                if (_isDirty)
                {
                    _updateTimer.Change(0, SaveToFileDelay * 60 * 1000);
                }

            } 
        }

        /// <inheritdoc />
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (!_backgroundWorker.IsBusy) _backgroundWorker.CancelAsync();

            _backgroundWorker.Dispose();
            _autoResetEvent.Dispose();
        }

        /// <summary>
        /// Occurs when [load settings completed].
        /// </summary>
        public virtual event EventHandler LoadSettingsCompleted;
        /// <summary>
        /// Occurs when [save settings completed].
        /// </summary>
        public virtual event EventHandler SaveSettingsCompleted;

        /// <summary>
        /// Saves the local database.
        /// </summary>
        public bool SecureSaveDatabaseToFile()
        {
            bool result = false;
            if (FileOperationInProgress)
            {
                return false;
            }
            try
            {
                FileOperationInProgress = true;
                _lastDatabaseSave = DateTime.Now;
                result = SaveToFileDatabase();
                if (result)
                {
                    IsDirty = false;
                    SaveSettingsCompleted?.Invoke(this, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "FileBasedProperyCollection.Save exception: {Message}", ex.Message);
            }
            finally
            {
                FileOperationInProgress = false;
            }
            return result;
        }
        public bool SecureLoadDatabase()
        {
            bool result = false;
            if (FileOperationInProgress)
            {
                return false;
            }

            try
            {
                FileOperationInProgress = true;
                result = LoadFromFileDatabase();
                if (result)
                {
                    IsDirty = false;
                    LoadSettingsCompleted?.Invoke(this, EventArgs.Empty);
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "Exception in FileBasedProperyCollection: {Message}", e.Message);

            }
            finally
            {
                FileOperationInProgress = false;
            }

            return result;
        }

        public virtual bool InterlockedSave()
        {
            if (!IsDirty)
            {
                return true;
            }

            using (var scope = new SingleThreadedScope())
            {
                bool result = scope.PerformWork(SaveToFileDatabase);
                IsDirty = false;
                return result;
            }
        }

        /// <summary>
        /// Loads the settings from database. Implemented by subclass
        /// </summary>
        /// <returns></returns>
        protected abstract bool LoadFromFileDatabase();
        /// <summary>
        /// Saves the settings from database. Implemented by subclass
        /// </summary>
        /// <returns></returns>
        protected abstract bool SaveToFileDatabase();

        /// <summary>
        /// Loads the local database.
        /// </summary>


        /// <summary>
        /// Models the updated.
        /// </summary>
        public virtual void ModelUpdated()
        {
            IsDirty = true;
            if (!_backgroundWorker.IsBusy) _backgroundWorker.RunWorkerAsync();
        }

        /// <summary>
        /// Handles the DoWork event of the BackgroundWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DoWorkEventArgs"/> instance containing the event data.</param>
        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (IsDirty  && _lastDatabaseSave < DateTime.Now.AddMinutes(SaveToFileDelay))
            {
                if (_fileOperationInProgress)
                {
                    return;
                }
                
                try
                {
                    FileOperationInProgress = true;
                    bool result = SaveToFileDatabase();
                
                    if (result)
                    {
                        IsDirty = false;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex,"FileBasedProperyCollection Exception: {Message}",ex.Message);
                }
                finally
                {
                    FileOperationInProgress = false;    
                }
              
                
            }
        }
    }

    public class SingleThreadedScope : IDisposable
    {
        private readonly object _lockObj = new object();

        public bool PerformWork(Func<bool> workAction)
        {
            lock (_lockObj)
            {
                return workAction.Invoke();

            }
        }



        public void Dispose()
        {
        }
    }
}