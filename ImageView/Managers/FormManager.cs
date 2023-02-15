namespace ImageViewer.Managers;

/// <summary>
///     FormManager
/// </summary>
/// <seealso cref="ImageViewer.Managers.ManagerBase" />
public class FormManager : ManagerBase
{
    /// <summary>
    ///     The scope
    /// </summary>
    private readonly ILifetimeScope _scope;


    /// <summary>
    ///     Initializes a new instance of the <see cref="FormManager" /> class.
    /// </summary>
    /// <param name="scope">The scope.</param>
    public FormManager(ILifetimeScope scope)
    {
        _scope = scope;
        FormInstances = new Dictionary<string, object>();
    }

    /// <summary>
    ///     Gets the form instances.
    /// </summary>
    /// <value>
    ///     The form instances.
    /// </value>
    private Dictionary<string, object> FormInstances { get; }

    /// <summary>
    ///     Gets the form instance.
    /// </summary>
    /// <param name="formType">Type of the form.</param>
    /// <returns></returns>
    public Form GetFormInstance(Type formType)
    {
        if (!formType.IsAssignableTo<Form>())
        {
            return null;
        }

        string formName = formType.Name;

        if (FormInstances.ContainsKey(formName))
        {
            if (FormInstances[formName] is not Form instance || instance.IsDisposed)
            {
                FormInstances.Remove(formName);
            }
            else
            {
                return FormInstances[formName] as Form;
            }
        }

        object formInstance = _scope.Resolve(formType);
        ((Form)formInstance).Closed += FormManager_Closed;
        FormInstances.Add(formName, formInstance);

        return formInstance as Form;
    }

    /// <summary>
    ///     Handles the Closed event of the FormManager control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
    private void FormManager_Closed(object sender, EventArgs e)
    {
        var frm = (Form)sender;
        if (frm != null && FormInstances.ContainsKey(frm.Name))
        {
            FormInstances.Remove(frm.Name);
        }
    }

    public async Task<FormImageView> GetFormImageViewAsync(int id)
    {
        var form = await Task<FormImageView>.Factory.StartNew(() => CreateFormInstance(id));
        return form;
    }

    private FormImageView CreateFormInstance(int id)
    {
        var form = _scope.Resolve<FormImageView>(new NamedParameter("id", id));
        return form;
    }

    /// <summary>
    ///     Shows the form.
    /// </summary>
    /// <param name="formType">Type of the form.</param>
    public bool ShowForm(Type formType)
    {
        try
        {
            var form = GetFormInstance(formType);
            form.Show();
            form.Focus();
            return true;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "failed to create an instance of the following form {Name}", formType.Name);
        }

        return false;
    }
}