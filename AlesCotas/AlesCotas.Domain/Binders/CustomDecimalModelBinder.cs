using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace AlesCotas.Presentation.Binders
{
    

    public class CustomDecimalModelBinder : IModelBinder
    {
     
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProviderResult == ValueProviderResult.None)
            {
                return;
            }

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

            var valueAsString = valueProviderResult.FirstValue;

            if (string.IsNullOrEmpty(valueAsString))
            {
                return;
            }

            decimal value;
            var culture = CultureInfo.GetCultureInfo("en-US");

            if (decimal.TryParse(valueAsString, NumberStyles.Float | NumberStyles.AllowThousands, culture, out value))
            {
                bindingContext.Result = ModelBindingResult.Success(value);
            }
            else
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, $"The value '{valueAsString}' is not valid for {bindingContext.ModelMetadata.DisplayName}.");
            }

            await Task.CompletedTask;
        }
    }
}
