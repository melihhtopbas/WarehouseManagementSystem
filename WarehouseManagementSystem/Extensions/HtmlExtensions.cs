using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace WarehouseManagementSystem.Extensions
{
    public static class UrlExtensions
    {
        public static string ContentAbsolute(this UrlHelper urlHelper, string contentPath, bool toAbsolute = false)
        {
            var path = urlHelper.Content(contentPath);
            var url = new Uri(HttpContext.Current.Request.Url, path);

            return toAbsolute ? url.AbsoluteUri : path;
        }
    }
    public static class HtmlExtensions
    {

        public static MvcHtmlString LabelDataEntryFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
                                            Expression<System.Func<TModel, TProperty>> expression,
                                             object htmlAttributesExtra = null)
        {
            IDictionary<string, object> labelAttributes = System.Web.WebPages.Html.HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributesExtra);


            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            labelAttributes.Add(metadata.IsRequired
                ? new KeyValuePair<string, object>("class", "control-label required")
                : new KeyValuePair<string, object>("class", "control-label"));

            return htmlHelper.LabelFor(expression, labelAttributes);
        }

        public static MvcHtmlString TextBoxBlockFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
                                            Expression<System.Func<TModel, TProperty>> expression,
                                             object htmlAttributesTextBox = null)
        {
            IDictionary<string, object> textBoxAttributes = System.Web.WebPages.Html.HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributesTextBox);
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            if (textBoxAttributes.ContainsKey("class"))
            {
                textBoxAttributes["class"] = textBoxAttributes["class"] + " form-control";
            }
            else
            {
                textBoxAttributes.Add(new KeyValuePair<string, object>("class", "form-control"));
            }
            if (!textBoxAttributes.ContainsKey("autocomplete"))
            {
                textBoxAttributes.Add(new KeyValuePair<string, object>("autocomplete", "off"));
            }

            var label = metadata.IsRequired
                ? htmlHelper.LabelFor(expression, new { @class = "control-label required" })
                : htmlHelper.LabelFor(expression, new { @class = "control-label" });
            var textBox = htmlHelper.TextBoxFor(expression, textBoxAttributes);
            var valMessage = htmlHelper.ValidationMessageFor(expression, String.Empty, new { @class = "text-danger" });
            return MvcHtmlString.Create($"{label}<div class=\"input-group input-group-sm\">{textBox}</div>{valMessage}");
        }

        public static MvcHtmlString TextBoxBlockLabelAsAddonFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
                                            Expression<System.Func<TModel, TProperty>> expression,
                                             object htmlAttributesTextBox = null)
        {
            IDictionary<string, object> textBoxAttributes = System.Web.WebPages.Html.HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributesTextBox);
            if (textBoxAttributes.ContainsKey("class"))
            {
                textBoxAttributes["class"] = textBoxAttributes["class"] + " form-control";
            }
            else
            {
                textBoxAttributes.Add(new KeyValuePair<string, object>("class", "form-control"));
            }
            if (!textBoxAttributes.ContainsKey("autocomplete"))
            {
                textBoxAttributes.Add(new KeyValuePair<string, object>("autocomplete", "off"));
            }
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

            //, style = "font-weight:bold;"
            var displayName = htmlHelper.DisplayNameFor(expression);
            var labelAddon = metadata.IsRequired
                ? $"<span class=\"input-group-addon as-label required\"><strong>{displayName}</strong> </span>"
                : $"<span class=\"input-group-addon as-label\">{displayName} </span>";
            var textBox = htmlHelper.TextBoxFor(expression, textBoxAttributes);
            var valMessage = htmlHelper.ValidationMessageFor(expression, String.Empty, new { @class = "text-danger" });
            return MvcHtmlString.Create($"<div class=\"input-group input-group-sm\">{labelAddon}{textBox}</div>{valMessage}");
        }
        public static MvcHtmlString TextBoxBlockLabelAsAddonForAsteriks<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
                                            Expression<System.Func<TModel, TProperty>> expression,
                                             object htmlAttributesTextBox = null)
        {
            IDictionary<string, object> textBoxAttributes = System.Web.WebPages.Html.HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributesTextBox);
            if (textBoxAttributes.ContainsKey("class"))
            {
                textBoxAttributes["class"] = textBoxAttributes["class"] + " form-control";
            }
            else
            {
                textBoxAttributes.Add(new KeyValuePair<string, object>("class", "form-control"));
            }
            if (!textBoxAttributes.ContainsKey("autocomplete"))
            {
                textBoxAttributes.Add(new KeyValuePair<string, object>("autocomplete", "off"));
            }
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);


            var displayName = htmlHelper.DisplayNameFor(expression);
            var labelAddon = metadata.IsRequired
                ? $"<span style = \"font-weight:bold\" class=\"input-group-addon as-label required\"><strong>{displayName}</strong> </span> "
                : $"<span style = \"font-weight:bold\" class=\"required input-group-addon as-label\">{displayName} </span>"; //yıldız olması için required getirildi
                                                                                                                             //style = font-weight:bold / kalın yazması için 
            var textBox = htmlHelper.TextBoxFor(expression, textBoxAttributes);
            var valMessage = htmlHelper.ValidationMessageFor(expression, String.Empty, new { @class = "text-danger" });
            return MvcHtmlString.Create($"<div class=\"input-group input-group-sm\">{labelAddon}{textBox}</div>{valMessage}");
        }

        public static MvcHtmlString TextBoxDecimalBlockFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
                                            Expression<System.Func<TModel, TProperty>> expression, string formatString,
                                             object htmlAttributesTextBox = null)
        {
            IDictionary<string, object> textBoxAttributes = System.Web.WebPages.Html.HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributesTextBox);

            if (textBoxAttributes.ContainsKey("class"))
            {
                textBoxAttributes["class"] = textBoxAttributes["class"] + " form-control";
            }
            else
            {
                textBoxAttributes.Add(new KeyValuePair<string, object>("class", "form-control"));
            }
            if (!textBoxAttributes.ContainsKey("autocomplete"))
            {
                textBoxAttributes.Add(new KeyValuePair<string, object>("autocomplete", "off"));
            }
            if (String.IsNullOrWhiteSpace(formatString)) { formatString = "{0:0.#####}"; }
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

            var label = metadata.IsRequired
                ? htmlHelper.LabelFor(expression, new { @class = "control-label required" })
                : htmlHelper.LabelFor(expression, new { @class = "control-label" });
            var textBox = htmlHelper.TextBoxFor(expression, formatString, textBoxAttributes);
            var valMessage = htmlHelper.ValidationMessageFor(expression, String.Empty, new { @class = "text-danger" });
            return MvcHtmlString.Create($"{label}<div class=\"input-group input-group-sm\">{textBox}</div>{valMessage}");
        }

        public static MvcHtmlString TextBoxDecimalBlockLabelAsAddonFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<System.Func<TModel, TProperty>> expression, string formatString,
            object htmlAttributesTextBox = null)
        {
            IDictionary<string, object> textBoxAttributes = System.Web.WebPages.Html.HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributesTextBox);

            if (textBoxAttributes.ContainsKey("class"))
            {
                textBoxAttributes["class"] = textBoxAttributes["class"] + " form-control";
            }
            else
            {
                textBoxAttributes.Add(new KeyValuePair<string, object>("class", "form-control"));
            }
            if (!textBoxAttributes.ContainsKey("autocomplete"))
            {
                textBoxAttributes.Add(new KeyValuePair<string, object>("autocomplete", "off"));
            }
            if (String.IsNullOrWhiteSpace(formatString)) { formatString = "{0:0.#####}"; }
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);


            var displayName = htmlHelper.DisplayNameFor(expression);
            var labelAddon = metadata.IsRequired
                ? $"<span class=\"input-group-addon as-label required\"><strong>{displayName}</strong> </span>"
                : $"<span class=\"input-group-addon as-label\">{displayName} </span>";
            var textBox = htmlHelper.TextBoxFor(expression, formatString, textBoxAttributes);
            var valMessage = htmlHelper.ValidationMessageFor(expression, String.Empty, new { @class = "text-danger" });
            return MvcHtmlString.Create($"<div class=\"input-group input-group-sm\">{labelAddon}{textBox}</div>{valMessage}");
        }

        public static MvcHtmlString CheckBoxBlockFor<TModel>(this HtmlHelper<TModel> htmlHelper,
                                            Expression<Func<TModel, bool>> expression,
                                             object htmlAttributesCheckBox = null)
        {
            IDictionary<string, object> checkBoxAttributes = System.Web.WebPages.Html.HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributesCheckBox);

            //checkBoxAttributes.Add(new KeyValuePair<string, object>("class", "form-control"));
            checkBoxAttributes.Add(new KeyValuePair<string, object>("data-on-color", "success"));
            checkBoxAttributes.Add(new KeyValuePair<string, object>("data-off-color", "danger"));
            checkBoxAttributes.Add(new KeyValuePair<string, object>("data-size", "mini"));
            checkBoxAttributes.Add(new KeyValuePair<string, object>("data-on-text", "Evet"));
            checkBoxAttributes.Add(new KeyValuePair<string, object>("data-off-text", "Hayır"));

            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var label = metadata.IsRequired
                ? htmlHelper.LabelFor(expression, new { @class = "control-label required" })
                : htmlHelper.LabelFor(expression, new { @class = "control-label" });
            var checkBox = htmlHelper.CheckBoxFor(expression, checkBoxAttributes);
            var valMessage = htmlHelper.ValidationMessageFor(expression, String.Empty, new { @class = "text-danger" });
            return MvcHtmlString.Create(String.Format("{0}<div>{1}{2}</div>", label, checkBox, valMessage));
        }
        public static MvcHtmlString CheckBoxOneLineBlockFor<TModel>(this HtmlHelper<TModel> htmlHelper,
                                            Expression<Func<TModel, bool>> expression,
                                             object htmlAttributesCheckBox = null)
        {
            IDictionary<string, object> checkBoxAttributes = System.Web.WebPages.Html.HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributesCheckBox);

            //checkBoxAttributes.Add(new KeyValuePair<string, object>("class", "form-control"));
            checkBoxAttributes.Add(new KeyValuePair<string, object>("data-on-color", "success"));
            checkBoxAttributes.Add(new KeyValuePair<string, object>("data-off-color", "danger"));
            checkBoxAttributes.Add(new KeyValuePair<string, object>("data-size", "mini"));
            checkBoxAttributes.Add(new KeyValuePair<string, object>("data-on-text", "Evet"));
            checkBoxAttributes.Add(new KeyValuePair<string, object>("data-off-text", "Hayır"));

            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var label = metadata.IsRequired
                ? htmlHelper.LabelFor(expression, new { @class = "control-label left-margin10 required" })
                : htmlHelper.LabelFor(expression, new { @class = "control-label left-margin10" });
            var checkBox = htmlHelper.CheckBoxFor(expression, checkBoxAttributes);
            var valMessage = htmlHelper.ValidationMessageFor(expression, String.Empty, new { @class = "text-danger" });
            return MvcHtmlString.Create(String.Format("{1}{0}{2}", label, checkBox, valMessage));
        }
        public static MvcHtmlString ClientIdFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return MvcHtmlString.Create(
                htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression)));
        }

        public static IDisposable BeginCollectionItem<TModel>(this HtmlHelper<TModel> html, string collectionName)
        {
            if (String.IsNullOrEmpty(collectionName))
                throw new ArgumentException("collectionName is null or empty.", nameof(collectionName));

            string collectionIndexFieldName = $"{collectionName}.Index";
            string itemIndex = GetCollectionItemIndex(collectionIndexFieldName);
            string collectionItemName = $"{collectionName}[{itemIndex}]";


            var indexField = new TagBuilder("input");
            indexField.MergeAttributes(new Dictionary<string, string>() {
                    { "name", $"{collectionName}.Index"},
                    { "value", itemIndex },
                    { "type", "hidden" },
                    { "autocomplete", "off" }
            });

            html.ViewContext.Writer.WriteLine(indexField.ToString(TagRenderMode.SelfClosing));
            return new CollectionItemNamePrefixScope(html.ViewData.TemplateInfo, collectionItemName);
        }

        private class CollectionItemNamePrefixScope : IDisposable
        {
            private readonly TemplateInfo _templateInfo;
            private readonly string _previousPrefix;

            public CollectionItemNamePrefixScope(TemplateInfo templateInfo, string collectionItemName)
            {
                this._templateInfo = templateInfo;

                _previousPrefix = templateInfo.HtmlFieldPrefix;
                templateInfo.HtmlFieldPrefix = collectionItemName;
            }

            public void Dispose()
            {
                _templateInfo.HtmlFieldPrefix = _previousPrefix;
            }
        }

        private static string GetCollectionItemIndex(string collectionIndexFieldName)
        {
            var previousIndices = (Queue<string>)HttpContext.Current.Items[collectionIndexFieldName];
            if (previousIndices == null)
            {
                HttpContext.Current.Items[collectionIndexFieldName] = previousIndices = new Queue<string>();

                string previousIndicesValues = HttpContext.Current.Request[collectionIndexFieldName];
                if (!String.IsNullOrWhiteSpace(previousIndicesValues))
                {
                    foreach (string index in previousIndicesValues.Split(','))
                        previousIndices.Enqueue(index);
                }
            }

            return previousIndices.Count > 0 ? previousIndices.Dequeue() : Guid.NewGuid().ToString();
        }
    }
}