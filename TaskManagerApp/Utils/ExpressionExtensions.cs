using System.Linq.Expressions;

namespace TaskManagerApp.Utils
{
    public static class ExpressionExtensions
    {
        public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> first,Expression<Func<T, bool>> second)
        {
            // Parametreyi almak
            var parameter = Expression.Parameter(typeof(T));

            // İlk ve ikinci koşulları birleştirme
            var body = Expression.AndAlso(
                Expression.Invoke(first, parameter),
                Expression.Invoke(second, parameter)
            );

            // Yeni bir Expression<Func<T, bool>> döndürme
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }
    }
}


/*
Extension Method: Extension method, mevcut bir sınıfa ya da interface'e yeni bir metod eklemenizi sağlar.
Bu metod, sınıfın orijinal koduna dokunmadan kullanılabilir.

Bu durumda, Expression<Func<T, bool>> türüne bir AndAlso metodu ekledik. 
Yani, aslında bu türün bir parçası haline getirdik. 
AndAlso metodu, existing expression'ları birleştirerek yeni bir expression oluşturmanıza olanak tanır.

Expression<Func<T, bool>>: Bu tür, lambda ifadeleri oluşturmak için kullanılan bir yapıdır. 
Bu yapıyı kullanarak, veritabanı sorguları gibi dinamik filtreleme işlemleri yapabiliyoruz. 
Bu tür, bizim AndAlso metodumuzla genişletildi.

Extension Method ile Yeni Bir Metot Üretme: Expression<Func<T, bool>> türüne eklediğimiz AndAlso metodu, aslında bu türün yeni bir özelliği gibi çalışıyor. 
Bu metodu, Expression<Func<T, bool>> üzerinde çağırabiliyoruz.
 */
