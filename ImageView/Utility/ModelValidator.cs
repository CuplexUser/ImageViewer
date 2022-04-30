using System.ComponentModel.DataAnnotations;

namespace ImageViewer.Utility
{
    public sealed class ModelValidator
    {
        private readonly object _model;

        public List<ValidationResult> ValidationResults { get; }

        public ModelValidator(object model)
        {
            _model = model;
            ValidationResults = new List<ValidationResult>();
        }

        public bool ValidateModel()
        {
            var validationContext = new ValidationContext(_model, null, null);
            return Validator.TryValidateObject(_model, validationContext, ValidationResults, true);
        }
    }
}