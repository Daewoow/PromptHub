// using PromptsProject;
//
// public class User
// {
//     public int Id { get; set; }
//     public string? Name { get; set; }
//     public int Age { get; set; }
// }
//
// public static class Program
// {
//     public static void Main()
//     {
//         using ApplicationContext dp = new ApplicationContext();
//         var tom = new User { Name = "Tom", Age = 33 };
//         var alice = new User { Name = "Alice", Age = 26 };
//         
//         dp.RemoveRange();
//
//         dp.Users.Add(tom);
//         dp.Users.Add(alice);
//
//         dp.SaveChanges();
//             
//         Console.WriteLine("OK");
//
//         var users = dp.Users.ToList();
//         Console.WriteLine("Объекты:");
//         foreach (var user in users)
//         {
//             Console.WriteLine($"{user.Id}.{user.Name} - {user.Age}");
//             dp.Remove(user);
//         }
//     }
// }
