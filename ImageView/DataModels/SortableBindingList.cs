using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ImageView.DataModels
{
    public class SortableBindingList<T> : BindingList<T>
    {
        // reference to the list provided at the time of instantiation
        private static readonly Dictionary<string, Func<List<T>, IEnumerable<T>>>
            cachedOrderByExpressions = new Dictionary<string, Func<List<T>,
                IEnumerable<T>>>();

        private readonly Action<SortableBindingList<T>, List<T>>
            populateBaseList = (a, b) => a.ResetItems(b);

        private List<T> originalList;
        private ListSortDirection sortDirection;
        private PropertyDescriptor sortProperty;

        // function that refereshes the contents
        // of the base classes collection of elements

        public SortableBindingList()
        {
            originalList = new List<T>();
        }

        public SortableBindingList(IEnumerable<T> enumerable)
        {
            originalList = enumerable.ToList();
            populateBaseList(this, originalList);
        }

        public SortableBindingList(List<T> list)
        {
            originalList = list;
            populateBaseList(this, originalList);
        }

        protected override bool SupportsSortingCore
        {
            get { return true; }
        }

        protected override ListSortDirection SortDirectionCore
        {
            get { return sortDirection; }
        }

        protected override PropertyDescriptor SortPropertyCore
        {
            get { return sortProperty; }
        }

        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
        {
            /*
             Look for an appropriate sort method in the cache if not found .
             Call CreateOrderByMethod to create one. 
             Apply it to the original list.
             Notify any bound controls that the sort has been applied.
             */

            sortProperty = prop;

            string orderByMethodName = sortDirection ==
                                       ListSortDirection.Ascending
                ? "OrderBy"
                : "OrderByDescending";
            string cacheKey = typeof (T).GUID + prop.Name + orderByMethodName;

            if (!cachedOrderByExpressions.ContainsKey(cacheKey))
                CreateOrderByMethod(prop, orderByMethodName, cacheKey);

            ResetItems(cachedOrderByExpressions[cacheKey](originalList).ToList());
            ResetBindings();
            sortDirection = sortDirection == ListSortDirection.Ascending
                ? ListSortDirection.Descending
                : ListSortDirection.Ascending;
        }

        private void CreateOrderByMethod(PropertyDescriptor prop, string orderByMethodName, string cacheKey)
        {
            /*
             Create a generic method implementation for IEnumerable<T>.
             Cache it.
            */

            ParameterExpression sourceParameter = Expression.Parameter(typeof (List<T>), "source");
            ParameterExpression lambdaParameter = Expression.Parameter(typeof (T), "lambdaParameter");
            PropertyInfo accesedMember = typeof (T).GetProperty(prop.Name);
            LambdaExpression propertySelectorLambda =
                Expression.Lambda(Expression.MakeMemberAccess(lambdaParameter,
                    accesedMember), lambdaParameter);
            MethodInfo orderByMethod = typeof (Enumerable).GetMethods()
                .Where(a => a.Name == orderByMethodName &&
                            a.GetParameters().Length == 2)
                .Single()
                .MakeGenericMethod(typeof (T), prop.PropertyType);

            var orderByExpression = Expression.Lambda<Func<List<T>, IEnumerable<T>>>(
                Expression.Call(orderByMethod,
                    new Expression[]
                    {
                        sourceParameter,
                        propertySelectorLambda
                    }),
                sourceParameter);

            cachedOrderByExpressions.Add(cacheKey, orderByExpression.Compile());
        }

        protected override void RemoveSortCore()
        {
            ResetItems(originalList);
        }

        private void ResetItems(List<T> items)
        {
            ClearItems();

            for (int i = 0; i < items.Count; i++)
            {
                InsertItem(i, items[i]);
            }
        }

        protected override void OnListChanged(ListChangedEventArgs e)
        {
            originalList = Items.ToList();
        }
    }
}