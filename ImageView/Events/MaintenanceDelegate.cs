namespace ImageViewer.Events;

/// <summary>
/// </summary>
/// <param name="parameters">The parameters.</param>
/// <returns></returns>
public delegate bool MaintenanceDelegate(Func<WorkParameters, bool> parameters);